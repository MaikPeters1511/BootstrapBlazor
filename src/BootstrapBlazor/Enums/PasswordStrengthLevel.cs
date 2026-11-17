// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">密码强度等级</para>
/// <para lang="en">Password strength level</para>
/// </summary>
public enum PasswordStrengthLevel
{
    /// <summary>
    /// <para lang="zh">未输入密码</para>
    /// <para lang="en">No password entered</para>
    /// </summary>
    None,

    /// <summary>
    /// <para lang="zh">弱</para>
    /// <para lang="en">Weak</para>
    /// </summary>
    Weak,

    /// <summary>
    /// <para lang="zh">一般</para>
    /// <para lang="en">Fair</para>
    /// </summary>
    Fair,

    /// <summary>
    /// <para lang="zh">良好</para>
    /// <para lang="en">Good</para>
    /// </summary>
    Good,

    /// <summary>
    /// <para lang="zh">强</para>
    /// <para lang="en">Strong</para>
    /// </summary>
    Strong
}
