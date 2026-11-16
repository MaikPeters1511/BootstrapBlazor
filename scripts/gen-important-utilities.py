#!/usr/bin/env python3
"""Generates src/BootstrapBlazor/wwwroot/scss/_utilities-important.scss.

The utilities of Bootstrap 5 were !important, the ones of Bootstrap 6 are in a cascade layer and lose against every
unlayered rule (components of BootstrapBlazor and user css). This script reads the spacing, display and visibility
utilities from wwwroot/core/bootstrap/css/bootstrap.css and writes them again with !important, in the original order and
inside the same @media / @container rules.

Run it again after every update of Bootstrap (UtilitiesImportant_CoverBootstrapUtilities in BootstrapAssetsTest fails
when utilities are missing):

    python3 scripts/gen-important-utilities.py
"""
import os
import re
import sys

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', 'src', 'BootstrapBlazor'))
SOURCE = os.path.join(ROOT, 'wwwroot', 'core', 'bootstrap', 'css', 'bootstrap.css')
TARGET = os.path.join(ROOT, 'wwwroot', 'scss', '_utilities-important.scss')

# groups of utilities that are generated again, the color utilities are handled in compat.scss
GROUPS = [
    ('spacing', r'^(m[trblxyse]?|p[trblxyse]?|ms|me|ps|pe)-'),
    ('display', r'^d-'),
    ('visibility', r'^(visible|invisible)$'),
]


def split_selectors(text):
    parts, depth, current = [], 0, ''
    for c in text:
        if c in '([':
            depth += 1
        elif c in ')]':
            depth -= 1
        if c == ',' and depth == 0:
            parts.append(current)
            current = ''
        else:
            current += c
    return parts + [current]


def parse_css(text):
    """Returns [(context, selector, [(property, value, important)])] in source order, context is the list of at-rules."""
    text = re.sub(r'/\*.*?\*/', '', text, flags=re.S)
    out = []
    skip = ('@keyframes', '@font-face', '@property', '@-webkit-keyframes', '@counter-style', '@page')

    def block(i, ctx):
        n = len(text)
        while i < n:
            while i < n and text[i] in ' \t\r\n;':
                i += 1
            if i >= n or text[i] == '}':
                return i + 1
            j, depth, quote = i, 0, None
            while j < n:
                c = text[j]
                if quote:
                    if c == '\\':
                        j += 1
                    elif c == quote:
                        quote = None
                elif c in '"\'':
                    quote = c
                elif c == '{' or (c == ';' and depth == 0):
                    break
                elif c == '(':
                    depth += 1
                elif c == ')':
                    depth -= 1
                j += 1
            head = text[i:j].strip()
            if j >= n or text[j] == ';':
                i = j + 1
                continue
            if head.startswith('@'):
                if head.split()[0] in skip:
                    d, k = 1, j + 1
                    while k < n and d:
                        d += (text[k] == '{') - (text[k] == '}')
                        k += 1
                    i = k
                    continue
                i = block(j + 1, ctx + [head])
            else:
                d, k, quote = 1, j + 1, None
                while k < n and d:
                    c = text[k]
                    if quote:
                        if c == '\\':
                            k += 1
                        elif c == quote:
                            quote = None
                    elif c in '"\'':
                        quote = c
                    else:
                        d += (c == '{') - (c == '}')
                    k += 1
                body = text[j + 1:k - 1]
                decls = []
                for m in re.finditer(r'(?:^|;)\s*([a-zA-Z-]+)\s*:\s*([^;]+)', body):
                    value = m.group(2).strip()
                    decls.append((m.group(1).lower(), value.replace('!important', '').strip(), '!important' in value))
                for selector in split_selectors(head):
                    out.append((tuple(ctx), selector.strip(), decls))
                i = k
        return i

    block(0, [])
    return out


def unescape(name):
    # css escapes: \32 xl\:d-none is 2xl:d-none
    name = re.sub(r'\\([0-9a-fA-F]{1,6}) ?', lambda m: chr(int(m.group(1), 16)), name)
    return re.sub(r'\\(.)', r'\1', name)


def in_group(name):
    name = re.sub(r'^(sm|md|lg|xl|2xl):', '', name)
    return any(re.match(pattern, name) for _, pattern in GROUPS)


def main():
    rules, seen = [], set()
    with open(SOURCE, encoding='utf-8') as f:
        parsed = parse_css(f.read())
    for ctx, selector, decls in parsed:
        match = re.fullmatch(r'\.((?:[a-zA-Z0-9_-]|\\[0-9a-fA-F]{1,6} ?|\\.)+)', selector)
        if not match:
            continue
        layers = ' '.join(ctx)
        if 'utilities' not in layers and 'helpers' not in layers:
            continue
        if any(important for _, _, important in decls):
            continue
        if not in_group(unescape(match.group(1))):
            continue
        props = [(p, v) for p, v, _ in decls if not p.startswith('--')]
        if not props:
            continue
        wrap = tuple(c for c in ctx if c.startswith(('@media', '@container', '@supports')))
        if (wrap, selector) in seen:
            continue
        seen.add((wrap, selector))
        rules.append((wrap, '.' + match.group(1) + ' { ' + ' '.join(f'{p}: {v} !important;' for p, v in props) + ' }'))

    out = ['// GENERATED by scripts/gen-important-utilities.py from wwwroot/core/bootstrap/css/bootstrap.css, do not edit',
           '// Bootstrap 5 utilities were !important, the Bootstrap 6 ones are layered and lose against every unlayered rule',
           '// (components of BootstrapBlazor and user css). These spacing, display and visibility utilities win again, like',
           '// the color utilities in compat.scss. Overriding them in custom css needs !important as well.',
           '']
    i = 0
    while i < len(rules):
        wrap, j = rules[i][0], i
        while j < len(rules) and rules[j][0] == wrap:
            j += 1
        lines = [r[1] for r in rules[i:j]]
        if wrap:
            out += [w + ' {' for w in wrap]
            out += ['    ' + line for line in lines]
            out += ['}'] * len(wrap)
        else:
            out += lines
        i = j
    with open(TARGET, 'w', encoding='utf-8', newline='\n') as f:
        f.write('\n'.join(out) + '\n')
    print(f'{len(rules)} utilities written to {os.path.relpath(TARGET)}')


if __name__ == '__main__':
    sys.exit(main())
