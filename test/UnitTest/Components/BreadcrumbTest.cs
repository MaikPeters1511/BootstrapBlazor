// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace UnitTest.Components;

public class BreadcrumbTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Value_Ok()
    {
        // Bootstrap 6 draws the separator with explicit .breadcrumb-divider elements between the items (Bootstrap 5: ::before)
        var cut = Context.Render<Breadcrumb>(pb =>
        {
            pb.Add(a => a.Value, new List<BreadcrumbItem>
            {
                new("Home", "/"),
                new("Library", "/library"),
                new("Data", null, "active")
            });
        });

        var list = cut.Find("ol.breadcrumb");
        Assert.Equal(["breadcrumb-item", "breadcrumb-divider", "breadcrumb-item", "breadcrumb-divider", "breadcrumb-item active"],
            list.Children.Select(c => string.Join(' ', c.ClassList)).ToArray());
    }

    [Fact]
    public void Link_Ok()
    {
        var cut = Context.Render<Breadcrumb>(pb =>
        {
            pb.Add(a => a.Value, new List<BreadcrumbItem>
            {
                new("Home", "/"),
                new("Data", null, "active")
            });
        });

        // links and text carry .breadcrumb-link (padding, color, hover), the current page is .active and has aria-current
        var link = cut.Find("li:first-child > a.breadcrumb-link");
        Assert.Equal("/", link.GetAttribute("href"));
        Assert.Equal("Home", link.TextContent);
        Assert.False(link.ClassList.Contains("active"));

        var current = cut.Find("li.breadcrumb-item.active");
        Assert.Equal("page", current.GetAttribute("aria-current"));
        var text = current.QuerySelector(".breadcrumb-link");
        Assert.NotNull(text);
        Assert.True(text.ClassList.Contains("active"));
        Assert.Equal("Data", text.TextContent);
        Assert.Null(current.QuerySelector("a"));
    }

    [Fact]
    public void SingleItem_NoDivider()
    {
        var cut = Context.Render<Breadcrumb>(pb =>
        {
            pb.Add(a => a.Value, new List<BreadcrumbItem> { new("Home", "/") });
        });
        Assert.Empty(cut.FindAll(".breadcrumb-divider"));
    }

    [Fact]
    public void Empty_Ok()
    {
        var cut = Context.Render<Breadcrumb>();
        Assert.Empty(cut.FindAll("li"));
    }
}
