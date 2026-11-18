// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">AvatarStack 头像堆叠组件，基于 Bootstrap 6 的 <c>avatar-stack</c>：多个 <see cref="Avatar"/> 部分重叠，鼠标移上的头像抬起</para>
/// <para lang="en">AvatarStack component, based on the <c>avatar-stack</c> of Bootstrap 6: several <see cref="Avatar"/> overlap, the one under the mouse lifts</para>
/// <para><version>11.1.1</version></para>
/// </summary>
public partial class AvatarStack
{
    /// <summary>
    /// <para lang="zh">获得/设置 头像内容，放入多个 <see cref="Avatar"/>。第一个头像显示在最右侧，最后一个头像显示在最左侧并位于最上层</para>
    /// <para lang="en">Gets or sets the content, put several <see cref="Avatar"/> in it. The first avatar is shown on the right, the last one on the left and on top</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? ClassString => CssBuilder.Default("avatar-stack")
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();
}
