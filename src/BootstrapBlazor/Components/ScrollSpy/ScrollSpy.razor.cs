// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">ScrollSpy 滚动监听组件，基于 Bootstrap 6 的 <c>ScrollSpy</c>：内容滚动时高亮导航中对应的链接</para>
/// <para lang="en">ScrollSpy component, based on the <c>ScrollSpy</c> of Bootstrap 6: highlights the matching link of a navigation while the content scrolls</para>
/// <para><version>11.1.1</version></para>
/// </summary>
public partial class ScrollSpy
{
    /// <summary>
    /// <para lang="zh">获得/设置 导航的 CSS 选择器，必填。导航里的链接 <c>href</c> 指向内容中元素的 Id，滚动时对应链接获得 <c>active</c></para>
    /// <para lang="en">Gets or sets the CSS selector of the navigation, required. The <c>href</c> of its links points to the id of an element in the content; the matching link gets <c>active</c> while scrolling</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? Target { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 激活线距滚动容器顶部的位置，百分比或像素，如 <c>12%</c>、<c>96px</c>。默认 null 使用 Bootstrap 默认值 12%。设置 <see cref="RootMargin"/> 后忽略</para>
    /// <para lang="en">Gets or sets the position of the activation line from the top of the scroll container, percent or pixels such as <c>12%</c> or <c>96px</c>. Default is null, Bootstrap's default of 12% is used. Ignored when <see cref="RootMargin"/> is set</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? TopMargin { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 IntersectionObserver 的 rootMargin，如 <c>0px 0px -40%</c>。默认 null，优先于 <see cref="TopMargin"/></para>
    /// <para lang="en">Gets or sets the rootMargin of the IntersectionObserver such as <c>0px 0px -40%</c>. Default is null, takes precedence over <see cref="TopMargin"/></para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? RootMargin { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 点击导航链接时是否平滑滚动。默认 false</para>
    /// <para lang="en">Gets or sets whether a click on a link of the navigation scrolls smoothly. Default is false</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public bool IsSmoothScroll { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 激活的链接改变回调方法，参数为链接的 <c>href</c></para>
    /// <para lang="en">Gets or sets the callback invoked when the active link changes, the argument is the <c>href</c> of the link</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public Func<string?, Task>? OnActivate { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 滚动区域内容，需设置高度并允许滚动，如 <c>style="height: 300px; overflow-y: auto"</c></para>
    /// <para lang="en">Gets or sets the content of the scroll area; give it a height and let it scroll, for example <c>style="height: 300px; overflow-y: auto"</c></para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? ClassString => CssBuilder.Default("bb-scroll-spy")
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override Task InvokeInitAsync() => InvokeVoidAsync("init", Id, Interop, nameof(TriggerActivate), new
    {
        target = Target,
        topMargin = TopMargin,
        rootMargin = RootMargin,
        smoothScroll = IsSmoothScroll
    });

    /// <summary>
    /// <para lang="zh">重新计算位置，内容的高度或可见性改变后调用</para>
    /// <para lang="en">Recalculates the positions; call it after the height or the visibility of the content changed</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    public Task RefreshAsync() => InvokeVoidAsync("refresh", Id);

    /// <summary>
    /// <para lang="zh">激活的链接改变时由 JavaScript 调用</para>
    /// <para lang="en">Called by JavaScript when the active link changes</para>
    /// </summary>
    /// <param name="href"><para lang="zh">链接的 href</para><para lang="en">The href of the link</para></param>
    [JSInvokable]
    public async Task TriggerActivate(string? href)
    {
        if (OnActivate != null)
        {
            await OnActivate(href);
        }
    }
}
