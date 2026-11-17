// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace UnitTest.Components;

/// <summary>
/// Test for the adornments (Bootstrap 6 form-adorn) of BootstrapInput
/// </summary>
public class InputAdornTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Default_NoAdorn()
    {
        var cut = Context.Render<BootstrapInput<string>>();

        Assert.Empty(cut.FindAll(".form-adorn"));
        Assert.Contains("form-control", cut.Find("input").ClassName);
        Assert.DoesNotContain("form-ghost", cut.Markup);
    }

    [Fact]
    public void PrefixIcon_Ok()
    {
        var cut = Context.Render<BootstrapInput<string>>(pb =>
        {
            pb.Add(a => a.PrefixIcon, "fa-solid fa-user");
        });

        // Bootstrap 6: the label is the control, the input inside is a ghost
        var label = cut.Find("label.form-control.form-adorn");
        var icon = label.QuerySelector("div.form-adorn-icon[aria-hidden=\"true\"] > i.fa-user");
        Assert.NotNull(icon);
        var input = label.QuerySelector("input.form-ghost");
        Assert.NotNull(input);
        Assert.DoesNotContain("form-control", input.ClassName);
        // the icon comes before the input
        Assert.True(label.InnerHtml.IndexOf("form-adorn-icon", StringComparison.Ordinal) < label.InnerHtml.IndexOf("<input", StringComparison.Ordinal));
    }

    [Fact]
    public void SuffixText_Ok()
    {
        var cut = Context.Render<BootstrapInput<string>>(pb =>
        {
            pb.Add(a => a.SuffixText, "@example.com");
        });

        var label = cut.Find("label.form-control.form-adorn");
        var text = label.QuerySelector("span.form-adorn-text[aria-hidden=\"true\"]");
        Assert.NotNull(text);
        Assert.Equal("@example.com", text.TextContent);
        // the text follows the input
        Assert.True(label.InnerHtml.IndexOf("<input", StringComparison.Ordinal) < label.InnerHtml.IndexOf("form-adorn-text", StringComparison.Ordinal));
    }

    [Fact]
    public void PrefixAndSuffix_Ok()
    {
        var cut = Context.Render<BootstrapInput<string>>(pb =>
        {
            pb.Add(a => a.PrefixText, "https://");
            pb.Add(a => a.SuffixIcon, "fa-solid fa-globe");
        });

        var label = cut.Find("label.form-adorn");
        var html = label.InnerHtml;
        var prefix = html.IndexOf("https://", StringComparison.Ordinal);
        var input = html.IndexOf("<input", StringComparison.Ordinal);
        var suffix = html.IndexOf("fa-globe", StringComparison.Ordinal);
        Assert.True(prefix >= 0 && prefix < input && input < suffix);
    }

    [Fact]
    public void Color_BorderOnLabel()
    {
        var cut = Context.Render<BootstrapInput<string>>(pb =>
        {
            pb.Add(a => a.PrefixIcon, "fa-solid fa-user");
            pb.Add(a => a.Color, Color.Danger);
        });

        Assert.Contains("border-danger", cut.Find("label.form-adorn").ClassName);
        Assert.DoesNotContain("border-danger", cut.Find("input").ClassName);
    }

    [Fact]
    public void Clearable_InsideLabel()
    {
        var cut = Context.Render<BootstrapInput<string>>(pb =>
        {
            pb.Add(a => a.PrefixIcon, "fa-solid fa-user");
            pb.Add(a => a.IsClearable, true);
            pb.Add(a => a.Value, "abc");
        });

        Assert.Empty(cut.FindAll(".bb-clearable-input"));
        Assert.Single(cut.FindAll("label.form-adorn > .form-control-clear-icon"));
    }

    [Fact]
    public void Disabled_NoClearIcon()
    {
        var cut = Context.Render<BootstrapInput<string>>(pb =>
        {
            pb.Add(a => a.PrefixIcon, "fa-solid fa-user");
            pb.Add(a => a.IsClearable, true);
            pb.Add(a => a.IsDisabled, true);
        });

        Assert.Empty(cut.FindAll(".form-control-clear-icon"));
    }

    [Fact]
    public void Password_Ok()
    {
        // the subclasses get the adornments too
        var cut = Context.Render<BootstrapPassword>(pb =>
        {
            pb.Add(a => a.PrefixIcon, "fa-solid fa-lock");
        });

        Assert.Equal("password", cut.Find("input.form-ghost").GetAttribute("type"));
    }
}
