// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">Direction 枚举扩展方法</para>
/// <para lang="en">Direction enum extension methods</para>
/// </summary>
internal static class DirectionExtensions
{
    /// <summary>
    /// <para lang="zh">获得 Bootstrap Menu 组件 data-bs-placement 值</para>
    /// <para lang="en">Gets the data-bs-placement value used by the Bootstrap Menu component</para>
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="alignment"></param>
    public static string ToMenuPlacement(this Direction direction, Alignment alignment) => direction switch
    {
        Direction.Dropup => alignment switch
        {
            Alignment.Center => "top",
            Alignment.Right => "top-end",
            _ => "top-start"
        },
        Direction.Dropleft => "start-start",
        Direction.Dropright => "end-start",
        _ => alignment switch
        {
            Alignment.Center => "bottom",
            Alignment.Right => "bottom-end",
            _ => "bottom-start"
        }
    };
}
