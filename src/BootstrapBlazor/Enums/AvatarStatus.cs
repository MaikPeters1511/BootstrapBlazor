// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">头像状态</para>
/// <para lang="en">Avatar status</para>
/// </summary>
public enum AvatarStatus
{
    /// <summary>
    /// <para lang="zh">不显示状态点</para>
    /// <para lang="en">No status dot</para>
    /// </summary>
    None,

    /// <summary>
    /// <para lang="zh">在线</para>
    /// <para lang="en">Online</para>
    /// </summary>
    Online,

    /// <summary>
    /// <para lang="zh">离线</para>
    /// <para lang="en">Offline</para>
    /// </summary>
    Offline,

    /// <summary>
    /// <para lang="zh">忙碌</para>
    /// <para lang="en">Busy</para>
    /// </summary>
    Busy,

    /// <summary>
    /// <para lang="zh">离开</para>
    /// <para lang="en">Away</para>
    /// </summary>
    Away
}
