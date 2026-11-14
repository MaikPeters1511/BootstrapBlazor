// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">Color 枚举扩展方法</para>
/// <para lang="en">Color enum extension methods</para>
/// </summary>
internal static class ColorExtensions
{
    /// <summary>
    /// <para lang="zh">获得 Bootstrap 6 按钮样式 如 btn-solid theme-primary</para>
    /// <para lang="en">Gets the Bootstrap 6 button classes e.g. btn-solid theme-primary</para>
    /// </summary>
    /// <param name="color"></param>
    /// <param name="isOutline"></param>
    public static string? ToButtonClass(this Color color, bool isOutline = false)
    {
        if (color == Color.Link)
        {
            return "btn-link";
        }

        var variant = isOutline ? "btn-outline" : "btn-solid";
        return color == Color.None ? (isOutline ? variant : null) : $"{variant} theme-{color.ToDescriptionString()}";
    }
}
