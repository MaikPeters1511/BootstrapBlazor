// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace UnitTest.Components;

/// <summary>
/// Test for ScrollSpy component
/// </summary>
public class ScrollSpyTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Render_Ok()
    {
        var cut = Context.Render<ScrollSpy>(pb =>
        {
            pb.Add(a => a.Target, "#nav");
            pb.Add(a => a.ChildContent, builder => builder.AddMarkupContent(0, "<h4 id=\"s1\">First</h4>"));
        });

        var el = cut.Find("div.bb-scroll-spy");
        Assert.False(string.IsNullOrEmpty(el.Id));
        // a scroll container without focusable children needs tabindex 0
        Assert.Equal("0", el.GetAttribute("tabindex"));
        Assert.Contains("<h4 id=\"s1\">First</h4>", el.InnerHtml);
    }

    [Fact]
    public void AdditionalAttributes_Ok()
    {
        var cut = Context.Render<ScrollSpy>(pb =>
        {
            pb.Add(a => a.Target, "#nav");
            pb.Add(a => a.AdditionalAttributes, new Dictionary<string, object> { ["class"] = "my-spy", ["style"] = "height: 200px" });
        });

        var el = cut.Find("div.bb-scroll-spy");
        Assert.Contains("bb-scroll-spy my-spy", el.ClassName);
        Assert.Equal("height: 200px", el.GetAttribute("style"));
    }

    [Theory]
    [InlineData("#section-2", "#section-2")]
    [InlineData(null, null)]
    public async Task OnActivate_Ok(string? href, string? expected)
    {
        string? actual = "not called";
        var cut = Context.Render<ScrollSpy>(pb =>
        {
            pb.Add(a => a.Target, "#nav");
            pb.Add(a => a.OnActivate, v =>
            {
                actual = v;
                return Task.CompletedTask;
            });
        });

        await cut.InvokeAsync(() => cut.Instance.TriggerActivate(href));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task OnActivate_NoCallback_Ok()
    {
        var cut = Context.Render<ScrollSpy>(pb =>
        {
            pb.Add(a => a.Target, "#nav");
        });

        // without a callback the call is ignored
        await cut.InvokeAsync(() => cut.Instance.TriggerActivate("#s1"));
    }

    [Fact]
    public async Task Refresh_Ok()
    {
        var cut = Context.Render<ScrollSpy>(pb =>
        {
            pb.Add(a => a.Target, "#nav");
        });

        // runs against the loose JSInterop of the tests
        await cut.InvokeAsync(() => cut.Instance.RefreshAsync());
    }
}
