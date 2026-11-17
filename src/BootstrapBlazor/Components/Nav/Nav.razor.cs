// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">NavMenu 组件基类</para>
/// <para lang="en">NavMenu Component Base Class</para>
/// </summary>
public partial class Nav
{
    /// <summary>
    /// <para lang="zh">获得 组件样式</para>
    /// <para lang="en">Get Component Style</para>
    /// </summary>
    protected string? ClassString => CssBuilder.Default("nav")
        .AddClass("justify-content-center", Alignment == Alignment.Center && !IsVertical)
        .AddClass("justify-content-end", Alignment == Alignment.Right && !IsVertical)
        .AddClass("flex-column", IsVertical)
        .AddClass("nav-pills", IsPills)
        .AddClass("nav-pills-vertical", IsPills && IsVertical)
        .AddClass("nav-fill", IsFill)
        .AddClass("nav-justified", IsJustified)
        .AddClass("text-end", Alignment == Alignment.Right && IsVertical)
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// <para lang="zh">获得/设置 组件数据源</para>
    /// <para lang="en">Gets or sets Component Data Source</para>
    /// </summary>
    [Parameter]
    [NotNull]
    public IEnumerable<NavLink>? Items { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 组件对齐方式</para>
    /// <para lang="en">Gets or sets Component Alignment</para>
    /// </summary>
    [Parameter]
    public Alignment Alignment { get; set; } = Alignment.Left;

    /// <summary>
    /// <para lang="zh">获得/设置 是否垂直分布</para>
    /// <para lang="en">Gets or sets Whether to distribute vertically</para>
    /// </summary>
    [Parameter]
    public bool IsVertical { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否为胶囊</para>
    /// <para lang="en">Gets or sets Whether it is pills</para>
    /// </summary>
    [Parameter]
    public bool IsPills { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否填充</para>
    /// <para lang="en">Gets or sets Whether to fill</para>
    /// </summary>
    [Parameter]
    public bool IsFill { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否等宽</para>
    /// <para lang="en">Gets or sets Whether to be equal width</para>
    /// </summary>
    [Parameter]
    public bool IsJustified { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 空间不足时是否把放不下的导航项收进“更多”菜单，基于 Bootstrap 6 的 <c>NavOverflow</c>。默认 false。导航项在首次渲染后不应再增减</para>
    /// <para lang="en">Gets or sets whether items that do not fit move into a "More" menu, based on the <c>NavOverflow</c> of Bootstrap 6. Default is false. The items should not change after the first render</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public bool IsOverflow { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 “更多”菜单按钮文字，默认 null 使用本地化文字。仅在 <see cref="IsOverflow"/> 为 true 时有效</para>
    /// <para lang="en">Gets or sets the text of the "More" menu button. Default is null, the localized text is used. Only applies when <see cref="IsOverflow"/> is true</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? OverflowText { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Nav>? Localizer { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 组件内容</para>
    /// <para lang="en">Gets or sets Component Content</para>
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Items ??= Enumerable.Empty<NavLink>();
    }

    private bool _overflowEnabled;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override async Task InvokeInitAsync()
    {
        _overflowEnabled = IsOverflow;
        if (IsOverflow)
        {
            await InvokeVoidAsync("init", Id, new { moreText = OverflowText ?? Localizer["More"].Value });
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        // IsOverflow switched on after the first render: the wrapper is new, attach Bootstrap to it
        if (!firstRender && IsOverflow && !_overflowEnabled && Module != null)
        {
            await InvokeInitAsync();
        }
    }
}
