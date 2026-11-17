// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

using System.Text.Json;
using System.Text.RegularExpressions;

namespace UnitTest.Misc;

public class BootstrapAssetsTest
{
    private const string ExpectedVersion = "6.0.0-alpha.1";

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "BootstrapBlazor.slnx")))
        {
            dir = dir.Parent;
        }
        return dir?.FullName ?? throw new DirectoryNotFoundException("Repository root not found");
    }

    private static string GetBootstrapPath(params string[] segments)
    {
        var parts = new[] { GetProjectRoot(), "src", "BootstrapBlazor", "wwwroot", "core", "bootstrap" }.Concat(segments).ToArray();
        return Path.Combine(parts);
    }

    private static string GetBundlerPath() => Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor", "bundler.json");

    [Theory]
    [InlineData("css", "bootstrap.css")]
    [InlineData("js", "bootstrap.bundle.js")]
    [InlineData("js", "bootstrap.bundle.min.js")]
    public void BootstrapAsset_Ok(string folder, string file)
    {
        var path = GetBootstrapPath(folder, file);
        Assert.True(File.Exists(path), $"{path} missing");

        var header = File.ReadLines(path).Take(6).Aggregate(string.Empty, (a, b) => a + b);
        var match = Regex.Match(header, @"Bootstrap\s+v(\d+\.\d+\.\d+(?:-[\w.]+)?)");
        Assert.True(match.Success, $"No Bootstrap version header in {file}");
        Assert.Equal(ExpectedVersion, match.Groups[1].Value);
    }

    [Fact]
    public void BootstrapMinCss_Ok()
    {
        // the minified css of 6.0.0-alpha.1 ships without a version banner
        // offcanvas was replaced by drawer in Bootstrap 6
        var content = File.ReadAllText(GetBootstrapPath("css", "bootstrap.min.css"));
        Assert.Contains(".drawer", content);
        Assert.DoesNotContain(".offcanvas", content);
    }

    [Fact]
    public void BlazorBundleJs_ClassicScript_Ok()
    {
        // Bootstrap 6 ships ESM only. The build converts it to a classic script
        // so <script src="_content/BootstrapBlazor/js/bootstrap.blazor.bundle.min.js"> keeps working
        var path = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor", "wwwroot", "js", "bootstrap.blazor.bundle.min.js");
        Assert.True(File.Exists(path), $"{path} missing");

        var content = File.ReadAllText(path);
        Assert.DoesNotMatch(@"\bexport\s*\{", content);
        Assert.Matches(@"window\.bootstrap\s*=\s*\{[^}]*\bTooltip\b[^}]*\}", content);

        // license banner stays on top followed by the IIFE
        Assert.Matches($@"^/\*!\s*\*\s*Bootstrap v{Regex.Escape(ExpectedVersion)}[\s\S]*?\*/\s*\(function\(\)\{{", content);
        Assert.EndsWith("})();", content.TrimEnd());
    }

    private static HashSet<string> GetBootstrapExports()
    {
        var content = File.ReadAllText(GetBootstrapPath("js", "bootstrap.bundle.js"));
        var match = Regex.Match(content, @"\bexport\s*\{([^}]*)\}");
        Assert.True(match.Success, "No export statement in bootstrap.bundle.js");
        return match.Groups[1].Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
    }

    private static IEnumerable<string> GetComponentScripts()
    {
        var root = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor");
        return Directory.EnumerateFiles(Path.Combine(root, "Components"), "*.js", SearchOption.AllDirectories)
            .Concat(Directory.EnumerateFiles(Path.Combine(root, "wwwroot", "modules"), "*.js", SearchOption.AllDirectories));
    }

    [Fact]
    public void ComponentScripts_BootstrapApi_Ok()
    {
        // every bootstrap.Xxx used by component scripts must be exported by the vendored bundle
        var exports = GetBootstrapExports();
        var violations = GetComponentScripts()
            .SelectMany(file => Regex.Matches(File.ReadAllText(file), @"\bbootstrap\.([A-Z]\w*)")
                .Select(m => m.Groups[1].Value)
                .Where(name => !exports.Contains(name))
                .Distinct()
                .Select(name => $"{Path.GetFileName(file)}: bootstrap.{name}"))
            .ToList();
        Assert.True(violations.Count == 0, string.Join(Environment.NewLine, violations));
    }

    [Fact]
    public void ComponentScripts_BootstrapEvents_Ok()
    {
        // every xxx.bs.<component> event used by component scripts must belong to a component of the vendored bundle
        var components = GetBootstrapExports().Select(i => i.ToLowerInvariant()).ToHashSet();
        var violations = GetComponentScripts()
            .SelectMany(file => Regex.Matches(File.ReadAllText(file), @"\b[a-z]+\.bs\.([a-zA-Z]+)")
                .Where(m => !components.Contains(m.Groups[1].Value.ToLowerInvariant()))
                .Select(m => m.Value)
                .Distinct()
                .Select(name => $"{Path.GetFileName(file)}: {name}"))
            .ToList();
        Assert.True(violations.Count == 0, string.Join(Environment.NewLine, violations));
    }

    [Fact]
    public void ComponentSources_NoBootstrap5Dropdown_Ok()
    {
        // dropdown-menu, dropdown-item, dropdown-divider, dropdown-header, data-bs-toggle="dropdown"
        // and --bs-dropdown-* variables do not exist in Bootstrap 6 (menu, menu-item, ...)
        var regex = new Regex(@"(?<![\w-])dropdown-(menu|item|divider|header)(?!\w)|data-bs-toggle=\\?[""']dropdown|--bs-dropdown-");
        var root = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor");
        var extensions = new[] { ".razor", ".cs", ".js", ".scss" };
        var violations = new[] { "Components", Path.Combine("wwwroot", "modules"), Path.Combine("wwwroot", "scss") }
            .SelectMany(folder => Directory.EnumerateFiles(Path.Combine(root, folder), "*.*", SearchOption.AllDirectories))
            .Where(file => extensions.Contains(Path.GetExtension(file)))
            .SelectMany(file => regex.Matches(File.ReadAllText(file))
                .Select(m => m.Value)
                .Distinct()
                .Select(v => $"{Path.GetRelativePath(root, file)}: {v}"))
            .ToList();
        Assert.True(violations.Count == 0, $"{violations.Count} violations{Environment.NewLine}{string.Join(Environment.NewLine, violations.Take(20))}");
    }

    [Theory]
    // responsive infix col-sm-6 / d-md-none became a prefix in Bootstrap 6: sm:col-6 / md:d-none
    [InlineData("breakpoint", @"(?<![\w:.-])(col|d|offset|order|m[tbsexy]?|p[tbsexy]?|g[xy]?|flex|justify-content|align-items|align-self|align-content|text|float|row-cols|gap)-(sm|md|lg|xl|xxl)-(?=[\w{$])")]
    // btn-primary / btn-outline-danger became btn-solid theme-primary / btn-outline theme-danger
    [InlineData("button", @"(?<![\w-])btn-(outline-)?(primary|secondary|success|danger|warning|info|light|dark)(?![\w-])|btn-(outline-)?\{\w*Color")]
    // text-danger / link-primary became fg-danger, alert-info / text-bg-info became theme-info, text-muted became fg-3
    [InlineData("color", @"(?<![\w-])(text|link|alert|text-bg)-(primary|secondary|success|danger|warning|info|light|dark)(?![\w-])|(?<![\w-])text-muted(?![\w-])|""(text|link|alert|text-bg)-\{[\w.]*Color")]
    // navbar-expand-md became md:navbar-expand, bg-body-tertiary became bg-2
    [InlineData("navbar", @"navbar-expand-(sm|md|lg|xl|xxl|\{)|bg-body-(tertiary|secondary)")]
    // classes and data attributes without a Bootstrap 6 counterpart
    [InlineData("removed", @"(?<![\w-])(accordion-collapse|alert-dismissible|btn-close-white|modal-backdrop)(?![\w-])|data-bs-ride|data-bs-touch")]
    // Bootstrap 6 ships its own .table-card
    [InlineData("table-card", @"(?<![\w-])table-card(?![\w-])")]
    public void ComponentSources_NoBootstrap5Classes_Ok(string family, string pattern)
    {
        var regex = new Regex(pattern);
        var root = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor");
        var extensions = new[] { ".razor", ".cs", ".js" };
        var violations = new[] { "Components", Path.Combine("wwwroot", "modules") }
            .SelectMany(folder => Directory.EnumerateFiles(Path.Combine(root, folder), "*.*", SearchOption.AllDirectories))
            .Where(file => extensions.Contains(Path.GetExtension(file)))
            .SelectMany(file => File.ReadLines(file)
                .Where(line => !line.TrimStart().StartsWith("//"))
                .SelectMany(line => regex.Matches(line).Select(m => m.Value))
                .Distinct()
                .Select(v => $"{Path.GetRelativePath(root, file)}: {v}"))
            .ToList();
        Assert.True(violations.Count == 0, $"{family}: {violations.Count} violations{Environment.NewLine}{string.Join(Environment.NewLine, violations.Take(20))}");
    }

    [Fact]
    public void MotronicCss_Bootstrap6Tokens_Ok()
    {
        // the Motronic theme must set the Bootstrap 6 theme tokens instead of Bootstrap 5 variables and .btn-primary rules
        var content = File.ReadAllText(Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor", "wwwroot", "css", "motronic.min.css"));
        Assert.Matches(@"--bs-primary-bg:\s*#009EF7;", content);
        Assert.Matches(@"--bs-danger-bg:\s*#F1416C;", content);
        Assert.Matches(@"--bs-fg-body:\s*#181C32;", content);
        Assert.DoesNotMatch(@"\.btn-(outline-)?(primary|secondary|success|danger|warning|info|light|dark)\b", content);
        Assert.DoesNotMatch(@"dropdown-menu|--bs-dropdown-|modal-backdrop|modal-content|text-emphasis|border-subtle", content);
        Assert.DoesNotMatch(@"(?m)^\s*--bs-(primary|success|info|warning|danger|body-color|body-bg):", content);
    }

    [Theory]
    [InlineData("bg", "background-color")]
    [InlineData("border", "border-color")]
    [InlineData("fg", "color")]
    public void CompatScss_ColorUtilities_Important(string utility, string property)
    {
        // Bootstrap 5 color utilities were !important, Bootstrap 6 ones are layered and lose against component css
        // components set colors with bg-{color} / border-{color} / fg-{color}, so compat.scss makes them win again
        var content = File.ReadAllText(Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor", "wwwroot", "scss", "compat.scss"));
        Assert.Matches($@"\.{utility}-#\{{\$color\}}\s*\{{\s*{property}:[^;]*!important;", content);
    }

    private static string ReadSource(params string[] segments) => File.ReadAllText(Path.Combine(new[] { GetProjectRoot() }.Concat(segments).ToArray()));

    [Fact]
    public void LayerConflict_RootScss_ElementRules_InRebootLayer()
    {
        // generic element rules must not beat Bootstrap 6 component rules (.nav-link.disabled, .check:disabled ~ label, ...)
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "root.scss");
        Assert.Matches(@"@layer reboot\s*\{[\s\S]*a,\s*\[for\]\s*\{\s*cursor: pointer;", content);
        Assert.Matches(@"@layer reboot\s*\{[\s\S]*a:hover,\s*a:focus\s*\{\s*text-decoration: none;", content);
        Assert.DoesNotMatch(@"(?m)^a,\s*\[for\]\s*\{", content);
        Assert.DoesNotMatch(@"(?m)^a, a:hover, a:focus\s*\{", content);
    }

    [Fact]
    public void LayerConflict_RootScss_InputGroupSizes_Ok()
    {
        // .form-control / .input-group-text get the BB font size, small and large input groups keep their Bootstrap 6 size
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "root.scss");
        Assert.Matches(@"\.input-group-sm > \.form-control,\s*\.input-group-sm > \.input-group-text\s*\{\s*font-size: var\(--bs-btn-input-sm-font-size\);", content);
        Assert.Matches(@"\.input-group-lg > \.form-control,\s*\.input-group-lg > \.input-group-text\s*\{\s*font-size: var\(--bs-btn-input-lg-font-size\);", content);
    }

    [Fact]
    public void LayerConflict_FormControlPadding_InFormsLayer()
    {
        // an unlayered .form-control padding wins against .form-floating > .form-control (floating labels overlap the text)
        // and against .input-group-sm > .form-control, so it lives in the forms layer like the Bootstrap 6 rule it replaces
        var content = ReadSource("src", "BootstrapBlazor", "Components", "ValidateForm", "ValidateForm.razor.scss");
        Assert.Matches(@"@layer forms\s*\{[\s\S]*\.form-control\s*\{\s*padding: var\(--bb-form-control-padding\);", content);
        Assert.DoesNotMatch(@"(?m)^\.form-control\s*\{\s*padding:", content);
    }

    [Fact]
    public void LayerConflict_LinkButton_DisabledBackground_Ok()
    {
        // .btn:disabled of Bootstrap 6 paints a grey background, a disabled link button stays transparent
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Button", "Button.razor.scss");
        Assert.Matches(@"\.link-button\s*\{[^}]*--bs-btn-disabled-bg: transparent;", content);
    }

    [Fact]
    public void LayerConflict_DemoHeadings_InRebootLayer()
    {
        // the demo heading margin must not beat .dialog-title and .popover-header
        var content = ReadSource("src", "BootstrapBlazor.Server", "wwwroot", "css", "site.css");
        Assert.Matches(@"@layer reboot\s*\{\s*h3, h4, h5 \{\s*margin-bottom: var\(--bb-mb-3\);", content);
    }

    [Fact]
    public void CompatScss_ColorUtilities_SameOrderAsBootstrap5()
    {
        // components combine color classes, e.g. Avatar border border-info + border-danger and PopConfirm fg-info + fg-danger
        // with equal specificity the later rule wins: in Bootstrap 5 info came before warning and danger, so danger won
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "compat.scss");
        var map = Regex.Match(content, @"\$compat-colors:\s*\(([\s\S]*?)\);");
        Assert.True(map.Success, "$compat-colors not found");
        var order = Regex.Matches(map.Groups[1].Value, @"""(\w+)"":").Select(m => m.Groups[1].Value);
        Assert.Equal(new[] { "primary", "secondary", "success", "info", "warning", "danger", "light", "dark" }, order);
    }

    [Fact]
    public void ModalScript_EscapeAndBackdrop_HandOverToComponent()
    {
        // a native dialog closes itself on Escape and the browser refuses to let a page cancel a second Escape,
        // so the script takes the key itself (keydown, preventDefault) and hands Escape and the backdrop click
        // (hide.bs.dialog) over to the component, which runs OnClosingAsync like the close button
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Modal", "Modal.razor.js");
        Assert.Matches(@"EventHandler\.on\(document, 'keydown', modal\.handlerEscape\)", content);
        Assert.Matches(@"e\.key !== 'Escape'", content);
        Assert.Matches(@"EventHandler\.on\(el, 'hide\.bs\.dialog'", content);
        Assert.Matches(@"invokeMethodAsync\('RequestCloseCallback'\)", content);
        Assert.Matches(@"EventHandler\.off\(document, 'keydown', modal\.handlerEscape\)", content);
    }

    [Fact]
    public void CompatScss_InputGroup_Wraps()
    {
        // Bootstrap 5 input groups wrapped (flex-wrap: wrap), Bootstrap 6 does not: buttons, segmented controls and
        // addons that do not fit stick out of narrow containers (DateTimePicker, ImageCropper, AutoComplete demo pages)
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "compat.scss");
        Assert.Matches(@"\.input-group\s*\{\s*flex-wrap: wrap;", content);
    }

    [Fact]
    public void CompatScss_FormControl_Wraps()
    {
        // .form-control of Bootstrap 6 is a flex row without wrapping, containers that hold checkboxes or radios
        // (SlideButton demo) stuck out of narrow screens, in Bootstrap 5 their content wrapped
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "compat.scss");
        Assert.Matches(@"\.form-control\s*\{\s*flex-wrap: wrap;", content);
    }

    [Fact]
    public void BaseSelectScript_Enter_HidesMenu()
    {
        // Enter confirms the highlighted item of a Select / Dropdown; the Bootstrap 6 menu is closed by its own instance,
        // removing the 'show' class from the menu element left the menu open (the toggle keeps 'show')
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "modules", "base-select.js");
        Assert.DoesNotContain("menu.classList.remove('show')", content);
        Assert.Matches(@"key === ""Enter""\)\s*\{\s*popover\.hide\(\);", content);
    }

    [Fact]
    public void ModalScript_Shown_FocusesDialog()
    {
        // the native dialog is still invisible while showModal() runs, so the browser cannot focus anything inside
        // and the focus stays on the body: take it into the dialog as soon as it is shown
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Modal", "Modal.razor.js");
        Assert.Matches(@"shown\.bs\.dialog'[^}]*modal\.focusDialog\(\)", content);
        Assert.Matches(@"modal\.focusDialog = \(\) => \{[^}]*\.focus\(", content);
    }

    [Fact]
    public void AlertScss_Block_Ok()
    {
        // Bootstrap 6 alerts are flex rows (icon + text), the BootstrapBlazor Alert holds headings, paragraphs and lists
        // directly: they are laid out side by side and stick out of narrow containers, so the Alert stays a block
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Alert", "Alert.razor.scss");
        Assert.Matches(@"\.alert\s*\{[^}]*\bdisplay: block;", content);
        Assert.Matches(@"\.btn-close\s*\{[^}]*\bfloat: inline-end;", content);
    }

    [Theory]
    [InlineData("primary", "#0095e8", "#0589d3")]
    [InlineData("success", "#47be7d", "#3aab6d")]
    [InlineData("info", "#5014d0", "#612ad6")]
    [InlineData("warning", "#f1bc00", "#ddae08")]
    [InlineData("danger", "#d9214e", "#c41540")]
    [InlineData("dark", "#131628", "#212641")]
    public void MotronicCss_ButtonStates_Ok(string color, string hover, string active)
    {
        // Bootstrap 6 derives hover / active colors from the base color (oklch): for the Motronic palette
        // the result is too light (secondary) or too saturated (success, info, danger), so the theme sets them again
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "css", "motronic.min.css");
        Assert.Matches($@"\.btn-solid\.theme-{color}:hover[^{{]*\{{[^}}]*--bs-btn-hover-bg: {hover};", content);
        Assert.Matches($@"\.btn-solid\.theme-{color}:active[^{{]*\{{[^}}]*--bs-btn-active-bg: {active};", content);
    }

    [Fact]
    public void MotronicCss_SecondaryStates_DarkAware()
    {
        // the secondary color is light-dark(#E4E6EF, #7E8299): the fixed light values (#B5B5C3, #aeaebc) are lighter than the
        // dark base and left the label (white) unreadable on an open dropdown button, so the dark theme keeps the derived colors
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "css", "motronic.min.css");
        Assert.Matches(@"\.btn-solid\.theme-secondary:hover[^{]*\{[^}]*--bs-btn-hover-bg: light-dark\(#B5B5C3, oklch\(from var\(--bs-theme-bg\)", content);
        Assert.Matches(@"\.btn-solid\.theme-secondary:active[^{]*\{[^}]*--bs-btn-active-bg: light-dark\(#aeaebc, oklch\(from var\(--bs-theme-bg\)", content);
    }

    [Fact]
    public void SelectScss_ShownMenu_IsVisible()
    {
        // the .menu of Bootstrap 6 is closed with opacity: 0 and scale(.95) until it has .show itself
        // content moved into a popover (column list of the Table, ...) only has a .show parent
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Select", "Select.razor.scss");
        Assert.Matches(@"\.show > \.menu\s*\{[^}]*\bopacity: 1;[^}]*\btransform: none;", content);
    }

    [Fact]
    public void MenuScss_NavLink_AlignsStart()
    {
        // .nav-link of Bootstrap 6 is a flex row with justify-content: center (Bootstrap 5: block, text at the start)
        // entries of the drop-down lists of the top and bottom menu must keep their text at the start
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Menu", "Menu.razor.scss");
        Assert.Matches(@"\.bb-menu \.nav \.menu \.nav-link[^{]*\{[^}]*\bjustify-content: flex-start;", content);
    }

    [Fact]
    public void CardScss_Header_IsRow()
    {
        // .card-header of Bootstrap 6 is a flex column, Console, Captcha, Card, ... place title and tools in one row
        // (the pixel comparison with the Bootstrap 5 build showed the title centered with the light or icon below it)
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Card", "Card.razor.scss");
        Assert.Matches(@"(?m)^\.card-header \{[^}]*\bflex-direction: row;", content);
    }

    [Fact]
    public void TooltipScss_WrapperDisplay_InComponentsLayer()
    {
        // utilities of Bootstrap 6 are in a layer and not !important: a display rule of the tooltip wrapper outside of a layer
        // wins against d-flex / d-none on a button with a tooltip (the wrapper span carries the classes of the button)
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Tooltip", "Tooltip.razor.scss");
        Assert.Matches(@"@layer components\s*\{\s*:not\(\.is-tips\) > span\[data-bs-toggle=""tooltip""\]\s*\{\s*display: inline-block;", content);
    }

    [Fact]
    public void CheckboxScss_Margin_InComponentsLayer()
    {
        // `class=""ms-3""` on a Checkbox worked in Bootstrap 5 (!important utilities), the unlayered `margin: 0` reset ignores it now
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Checkbox", "Checkbox.razor.scss");
        Assert.Matches(@"@layer components\s*\{\s*\.form-check\s*\{\s*margin: 0;", content);
        Assert.DoesNotMatch(@"(?m)^    margin: 0;\s*$", content);
    }

    [Fact]
    public void TreeViewScss_NodeIcon_VisibleOverrides()
    {
        // .node-icon is visibility: hidden, nodes with children get .visible: in Bootstrap 5 that utility was !important,
        // in Bootstrap 6 it is not and loses against the unlayered component rule (no expand arrow in the tree)
        var content = ReadSource("src", "BootstrapBlazor", "Components", "TreeView", "TreeView.razor.scss");
        Assert.Matches(@"&\.visible\s*\{\s*visibility: visible;", content);
    }

    [Fact]
    public void CollapseScss_OpenHeader_IsHighlighted()
    {
        // Bootstrap 6 marks the open item with details[open], this component keeps div elements and puts .collapsed on the
        // .accordion-button: the header never has .collapsed itself, so `.accordion-header:not(.collapsed)` matched every header
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Collapse", "Collapse.razor.scss");
        Assert.Matches(@"&:has\(> \.accordion-button:not\(\.collapsed\)\)\s*\{[^}]*background-color: var\(--bs-accordion-active-bg\)", content);
        Assert.DoesNotMatch(@"&:not\(\.collapsed\)\s*\{\s*background-color: var\(--bs-accordion-active-bg\)", content);
    }

    [Fact]
    public void CarouselJs_AfterManually_ResumesAfterHover()
    {
        // Bootstrap 6 keeps _playing false when data-bs-autoplay is false, so its own mouseleave handler never
        // restarts the cycle once the carousel was started by the first manual slide
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Carousel", "Carousel.razor.js");
        Assert.Matches(@"EventHandler\.on\(el, 'mouseleave'[\s\S]*?carousel\.started[\s\S]*?\.cycle\(\)", content);
    }

    [Fact]
    public void AntCss_Bootstrap6_Ok()
    {
        // the Ant Design theme sets its own button height, but .btn of Bootstrap 6 has a min-height (2.375rem / 2rem / 2.75rem)
        // that wins against height, so the theme resets it
        var content = ReadSource("src", "BootstrapBlazor.Server", "wwwroot", "css", "ant.css");
        Assert.Matches(@"(?m)^\.btn \{[^}]*\bmin-height: 0;", content);
        Assert.DoesNotMatch(@"\.btn-(outline-)?(primary|secondary|success|danger|warning|info|light|dark)\b", content);
    }

    [Fact]
    public void DevuiCss_ButtonMinHeight_Ok()
    {
        // the DevUI theme makes buttons compact with smaller padding, the min-height of .btn in Bootstrap 6 keeps them tall
        var content = ReadSource("src", "BootstrapBlazor.Server", "wwwroot", "css", "devui.css");
        Assert.Matches(@"(?m)^\.btn \{[^}]*\bmin-height: 0;", content);
    }

    [Fact]
    public void RootScss_DisabledRule_InRebootLayer()
    {
        // Bootstrap 6 puts its css into cascade layers, unlayered rules always win
        // the generic :disabled background must stay below .btn:disabled, so it lives in the reboot layer
        var content = File.ReadAllText(Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor", "wwwroot", "scss", "root.scss"));
        Assert.Matches(@"@layer reboot\s*\{[^}]*:disabled\s*\{\s*background-color:", content);
        Assert.DoesNotMatch(@"(?m)^:disabled\s*\{\s*background-color:|^\.disabled \.range-separator,\s*:disabled\s*\{", content);

        // same for the generic focus outline reset, Bootstrap 6 draws focus rings with outline
        Assert.Matches(@"@layer reboot\s*\{[\s\S]*:focus\s*\{\s*outline: none;", content);
        Assert.DoesNotMatch(@"(?m)^:focus\s*\{\s*outline: none;", content);
    }

    [Fact]
    public void ComponentSources_BootstrapVariables_Ok()
    {
        // every var(--bs-xxx) used by the library must be defined by Bootstrap 6 or by the library itself
        // Bootstrap 6 removed --bs-border-radius, --bs-danger, --bs-body-color-rgb, ... they are provided by compat.scss
        var root = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor");
        var definition = new Regex(@"(--bs-[\w-]+)\s*:");
        var usage = new Regex(@"var\(\s*(--bs-[\w-]+)");

        // set by inline style or never defined by Bootstrap 5 either
        var defined = new HashSet<string>
        {
            "--bs-dial-list-height", "--bs-card-header-bg", "--bs-card-header-color", "--bs-card-footer-bg", "--bs-card-footer-color"
        };
        defined.UnionWith(definition.Matches(File.ReadAllText(GetBootstrapPath("css", "bootstrap.css"))).Select(m => m.Groups[1].Value));

        var extensions = new[] { ".razor", ".cs", ".js", ".scss" };
        var files = new[] { "Components", Path.Combine("wwwroot", "modules"), Path.Combine("wwwroot", "scss") }
            .SelectMany(folder => Directory.EnumerateFiles(Path.Combine(root, folder), "*.*", SearchOption.AllDirectories))
            .Where(file => extensions.Contains(Path.GetExtension(file)))
            .Select(file => (File: file, Content: File.ReadAllText(file)))
            .ToList();
        defined.UnionWith(files.SelectMany(f => definition.Matches(f.Content).Select(m => m.Groups[1].Value)));

        var violations = files
            .SelectMany(f => usage.Matches(f.Content).Select(m => m.Groups[1].Value))
            .Where(v => !defined.Contains(v))
            .GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} ({g.Count()})")
            .ToList();
        Assert.True(violations.Count == 0, $"{violations.Count} undefined variables{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
    }

    [Theory]
    [InlineData("--bs-body-bg", "var(--bs-bg-body)")]
    [InlineData("--bs-body-color", "var(--bs-fg-body)")]
    [InlineData("--bs-border-radius", "var(--bs-radius-4)")]
    [InlineData("--bs-primary", "var(--bs-primary-bg)")]
    [InlineData("--bs-danger", "var(--bs-danger-bg)")]
    public void CompatScss_LegacyVariable_Ok(string name, string value)
    {
        // legacy Bootstrap 5 tokens are mapped to Bootstrap 6 tokens for the library and for user stylesheets
        var root = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor", "wwwroot", "scss");
        var path = Path.Combine(root, "compat.scss");
        Assert.True(File.Exists(path), $"{path} missing");
        Assert.Matches($@"{Regex.Escape(name)}:\s*{Regex.Escape(value)};", File.ReadAllText(path));
        Assert.Contains("@use \"compat.scss\";", File.ReadAllText(Path.Combine(root, "components.scss")));
    }

    [Fact]
    public void RootScss_Breakpoints_FollowBootstrap6()
    {
        // the Responsive component reads the current breakpoint from body:before, it has to switch where the lg:, xl: and 2xl:
        // classes switch: Bootstrap 6 raised lg from 992px to 1024px, xl from 1200px to 1280px and xxl (now 2xl) from 1400px to 1536px
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "root.scss");
        foreach (var (width, name) in new[] { (576, "small"), (768, "medium"), (1024, "large"), (1280, "extraLarge"), (1536, "extraExtraLarge") })
        {
            Assert.Matches($@"@media \(min-width: {width}px\)\s*\{{\s*body:before\s*\{{\s*content: ""{name}"";", content);
        }
        Assert.DoesNotContain("min-width: 992px", content);
        Assert.DoesNotContain("min-width: 1200px", content);
        Assert.DoesNotContain("min-width: 1400px", content);
    }

    [Fact]
    public void BreakpointsDemo_ShowsBootstrap6Thresholds()
    {
        var content = ReadSource("src", "BootstrapBlazor.Server", "Components", "Pages", "Breakpoints.razor");
        Assert.Contains("&gt;= 1024 @Localizer", content);
        Assert.Contains("&gt;= 1280 @Localizer", content);
        Assert.Contains("&gt;= 1536 @Localizer", content);
        Assert.Contains("<code>2xl</code>", content);
        Assert.DoesNotContain("xxl</code>", content.Replace("dialog-xxl</code>", ""));
    }

    [Theory]
    [InlineData("DateTimeRange/DateTimeRange.razor", "flex-fill me-5")]
    [InlineData("QueryBuilder/QueryBuilder.razor", "btn-group me-5")]
    [InlineData("ThemeProvider/ThemeProvider.razor", "class=\"mx-3\"")]
    [InlineData("EditorForm/EditorForm.razor.cs", "\"row g-5\"")]
    [InlineData("SearchForm/SearchForm.razor.cs", "\"row g-5\"")]
    [InlineData("Skeleton/SkeletonTable.razor", "ms-12 me-3")]
    [InlineData("Skeleton/SkeletonEditor.razor", "row g-5")]
    [InlineData("ClockPicker/ClockPicker.razor", "bb-time-text me-12")]
    [InlineData("Toast/Toast.razor.cs", "toast-bar me-3")]
    [InlineData("Dialog/IconDialog.razor", "row g-5")]
    [InlineData("Row/Row.razor.js", "\"g-5\"")]
    [InlineData("Button/ExportPdfButton.cs", "class=\"p-5\"")]
    [InlineData("Table/Table.razor", "class=\"ms-3\"")]
    [InlineData("Table/Table.razor", "class=\"me-3\"")]
    [InlineData("Select/SelectTable.razor.scss", ".g-5 {")]
    [InlineData("SweetAlert/SweetAlertBody.razor", "class=\"ms-5\"")]
    public void Components_UseBootstrap6SpacerScale(string file, string expected)
    {
        // The spacer scale of Bootstrap 6 has 12 steps (1 = .25rem, 5 = 1rem, 12 = 3rem): ms-3 is .5rem now (Bootstrap 5: 1rem).
        // Same look as before: 2 -> 3, 3 -> 5, 4 -> 7, 5 -> 12 (1 stays). Applies to m*, p*, gap-* and g-*
        var content = ReadSource(new[] { "src", "BootstrapBlazor", "Components" }.Concat(file.Split('/')).ToArray());
        Assert.Contains(expected, content);
    }

    [Theory]
    [InlineData("Select", "Select.razor.scss")]
    [InlineData("ValidateForm", "ValidateForm.razor.scss")]
    public void ComponentsScss_NoBootstrap5ValidationClasses(string folder, string file)
    {
        // Bootstrap 6 has no .was-validated and no .has-validation (validation per data-bs-validate and :user-invalid, on the server .is-valid/.is-invalid),
        // BootstrapBlazor never sets them: selectors with them are dead
        var content = ReadSource("src", "BootstrapBlazor", "Components", folder, file);
        Assert.DoesNotContain("was-validated", content);
        Assert.DoesNotContain("has-validation", content);
    }

    [Fact]
    public void IndexDemo_NoBootstrap5Leftovers()
    {
        // --bs-border-radius-lg and .text-body-secondary are gone in Bootstrap 6 (radius scale --bs-radius-*, the masthead does not use the class anyway)
        var css = ReadSource("src", "BootstrapBlazor.Server", "Components", "Pages", "Index.razor.css");
        Assert.DoesNotContain("--bs-border-radius-lg", css);
        Assert.DoesNotContain("text-body-secondary", css);
        Assert.Contains("--bs-btn-border-radius: var(--bs-radius-5)", css);

        // rounded-N has a new scale: Bootstrap 5 rounded-5 (2rem, a pill here) is rounded-pill, rounded-2 (.375rem) is rounded-4
        var index = ReadSource("src", "BootstrapBlazor.Server", "Components", "Pages", "Index.razor");
        Assert.Contains("rounded-pill masthead-notice", index);
    }

    [Theory]
    [InlineData("TwoFactor.razor")]
    [InlineData("TwoFactorApp.razor")]
    public void TwoFactorDemo_UsesBootstrap6RoundedScale(string file)
    {
        var content = ReadSource("src", "BootstrapBlazor.Server", "Components", "Samples", "Tutorials", "MFA", file);
        Assert.Contains("class=\"rounded-4\"", content);
        Assert.DoesNotContain("rounded-2", content);
    }

    [Theory]
    [InlineData("success")]
    [InlineData("warning")]
    public void Motronic_LightColorsHaveDarkText(string name)
    {
        // White text on the Metronic green (#50CD89) has a contrast of 2.0 and on the yellow (#FFC700) of 1.6 (WCAG asks for 4.5): dark text instead
        var css = ReadSource("src", "BootstrapBlazor", "wwwroot", "css", "motronic.min.css");
        var match = Regex.Match(css, $@"--bs-{name}-contrast:\s*([^;}}]+)");
        Assert.True(match.Success, $"--bs-{name}-contrast is not set");
        var value = match.Groups[1].Value.Trim().ToLowerInvariant();
        Assert.NotEqual("#fff", value);
        Assert.NotEqual("#ffffff", value);
        Assert.NotEqual("white", value);
    }

    [Fact]
    public void MenuJs_ExpandAll_ShowsCollapse()
    {
        // Bootstrap 6 removed the toggle option of Collapse: the constructor keeps the state of the markup and opens nothing,
        // so expanding all sub menus (IsExpandAll) needs an explicit show(). _isShown() is private API, the state is in .show
        var content = ReadSource("src", "BootstrapBlazor", "Components", "Menu", "Menu.razor.js");
        Assert.DoesNotMatch(@"toggle:\s*(true|false)", content);
        Assert.Matches(@"new bootstrap\.Collapse\(target\)\.show\(\)", content);
        Assert.DoesNotContain("_isShown", content);
    }

    [Fact]
    public void UtilitiesImportant_CoverBootstrapUtilities()
    {
        // Bootstrap 5 utilities were !important, the ones of Bootstrap 6 are layered and lose against every unlayered rule
        // (components and user css): spacing, display and visibility utilities are generated again with !important
        // (scripts/gen-important-utilities.py), when this fails after an update of Bootstrap run the script again
        var bootstrap = ReadSource("src", "BootstrapBlazor", "wwwroot", "core", "bootstrap", "css", "bootstrap.css");
        var start = bootstrap.IndexOf("@layer helpers {", StringComparison.Ordinal);
        Assert.True(start > 0);
        var utilities = bootstrap[start..];
        var names = Regex.Matches(utilities, @"(?m)^\s*\.((?:(?:sm|md|lg|xl|\\32 xl)\\:)?(?:m[trblxyse]?|p[trblxyse]?|ms|me|ps|pe|d)-[A-Za-z0-9._\\-]*|visible|invisible)\s*\{")
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();
        Assert.True(names.Count > 900, $"only {names.Count} utilities found in Bootstrap");

        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "_utilities-important.scss");
        var missing = names.Where(n => !Regex.IsMatch(content, @"(?m)^\s*\." + Regex.Escape(n) + @" \{[^}]*!important;")).ToList();
        Assert.True(missing.Count == 0, $"{missing.Count} utilities without !important twin, e.g. {string.Join(", ", missing.Take(8))}");
    }

    [Fact]
    public void UtilitiesImportant_UsedByComponents()
    {
        var components = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "components.scss");
        Assert.Contains("@use \"utilities-important.scss\";", components);

        // unlayered, otherwise they would lose against unlayered component rules again
        var content = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "_utilities-important.scss");
        Assert.DoesNotContain("@layer", content);
    }

    [Fact]
    public void Styles_NoRgbTokens()
    {
        // Bootstrap 6 removed every --bs-*-rgb token: translucent colors use color-mix(), no stylesheet may rely on rgba(var(--bs-*-rgb))
        var files = new List<string>();
        foreach (var dir in new[] { new[] { "src", "BootstrapBlazor" }, new[] { "src", "BootstrapBlazor.Server" } })
        {
            var folder = Path.GetFullPath(Path.Combine(new[] { GetProjectRoot() }.Concat(dir).ToArray()));
            files.AddRange(Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                .Where(p => (p.EndsWith(".scss") || p.EndsWith(".razor.css") || p.EndsWith("site.css"))
                    && !p.Contains($"{Path.DirectorySeparatorChar}core{Path.DirectorySeparatorChar}")
                    && !p.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                    && !p.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")));
        }
        var offenders = files.Where(p => Regex.IsMatch(File.ReadAllText(p), @"rgba?\(\s*var\(--bs-(body-color|body-bg|emphasis-color|primary)-rgb\)")).ToList();
        Assert.Empty(offenders);

        var compat = ReadSource("src", "BootstrapBlazor", "wwwroot", "scss", "compat.scss");
        Assert.DoesNotMatch(@"--bs-(body-color|body-bg|emphasis-color|primary)-rgb\s*:", compat);
    }

    [Fact]
    public void DemoHeaderCss_Buttons_Transparent()
    {
        // a bare .btn of Bootstrap 6 has a light neutral background (Bootstrap 5: transparent), the buttons on the colored
        // demo header set color and border only and were unreadable in the light theme
        var content = ReadSource("src", "BootstrapBlazor.Server", "Components", "Components", "Header.razor.css");
        Assert.Matches(@"\.btn-bd-download\s*\{[^}]*background-color: transparent;", content);
        Assert.Matches(@"::deep \.bb-theme-mode \.dropdown-toggle\s*\{[^}]*background-color: transparent;", content);
    }

    [Fact]
    public void Bundler_InputFiles_Exist()
    {
        var root = Path.Combine(GetProjectRoot(), "src", "BootstrapBlazor");
        using var doc = JsonDocument.Parse(File.ReadAllText(GetBundlerPath()));
        foreach (var bundle in doc.RootElement.EnumerateArray())
        {
            foreach (var input in bundle.GetProperty("inputFiles").EnumerateArray())
            {
                var path = Path.Combine(root, input.GetString()!);
                Assert.True(File.Exists(path), $"Bundler input missing: {input}");
            }
        }
    }

    [Fact]
    public void Bundler_NoRtlBundle_Ok()
    {
        // Bootstrap 6 uses logical properties, there is no bootstrap.rtl.css any more
        var content = File.ReadAllText(GetBundlerPath());
        Assert.DoesNotContain("bootstrap.rtl", content);
    }
}
