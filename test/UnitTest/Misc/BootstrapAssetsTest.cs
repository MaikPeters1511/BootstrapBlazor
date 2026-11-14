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
