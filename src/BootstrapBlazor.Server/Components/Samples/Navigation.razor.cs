// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

using Microsoft.AspNetCore.Components.Routing;

namespace BootstrapBlazor.Server.Components.Samples;

/// <summary>
/// navigation
/// </summary>
public sealed partial class Navigation
{
    private IEnumerable<NavLink> Items => GetItems();

    // created once: the overflow moves the elements around in the browser, the items must keep their identity between renders
    private readonly List<NavLink> _overflowItems = CreateItems("Home", "Dashboard", "Products", "Services", "Reports", "Help");

    private static List<NavLink> CreateItems(params string[] texts)
    {
        var ret = new List<NavLink>();
        foreach (var text in texts)
        {
            var link = new NavLink();
            var parameters = new Dictionary<string, object?>()
            {
                ["href"] = "#",
                ["class"] = "nav-link",
                ["ChildContent"] = new RenderFragment(builder => builder.AddContent(0, text))
            };
            link.SetParametersAsync(ParameterView.FromDictionary(parameters));
            ret.Add(link);
        }
        return ret;
    }

    private List<NavLink> GetItems()
    {
        var ret = new List<NavLink>();
        var link = new NavLink();
        var parameters = new Dictionary<string, object?>()
        {
            ["href"] = WebsiteOption.Value.AdminUrl,
            ["class"] = "nav-link nav-item",
            ["target"] = "_blank",
            ["ChildContent"] = new RenderFragment(builder =>
            {
                builder.AddContent(0, "BootstrapAdmin");
            })
        };
        link.SetParametersAsync(ParameterView.FromDictionary(parameters));
        ret.Add(link);
        return ret;
    }
}
