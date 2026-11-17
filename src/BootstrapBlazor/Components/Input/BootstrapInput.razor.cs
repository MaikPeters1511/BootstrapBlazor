// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">BootstrapInput 组件</para>
/// <para lang="en">BootstrapInput Component</para>
/// </summary>
public partial class BootstrapInput<TValue>
{
    /// <summary>
    /// <para lang="zh">获得/设置 是否为只读，默认为 false</para>
    /// <para lang="en">Gets or sets whether readonly. Default is false</para>
    /// </summary>
    [Parameter]
    public bool Readonly { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 用户删除后是否自动更改为默认值 0，默认为 false</para>
    /// <para lang="en">Gets or sets whether to automatically set default value when user deletes. Default is false</para>
    /// </summary>
    [Parameter]
    public bool AutoSetDefaultWhenNull { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否显示清空小按钮，默认为 false</para>
    /// <para lang="en">Gets or sets whether to show clear button. Default is false</para>
    /// </summary>
    [Parameter]
    [Obsolete("已弃用，请使用 IsClearable 参数；Deprecated, use the IsClearable parameter")]
    [ExcludeFromCodeCoverage]
    public bool Clearable { get => IsClearable; set => IsClearable = value; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否显示清空小按钮，默认为 false</para>
    /// <para lang="en">Gets or sets whether to show clear button. Default is false</para>
    /// </summary>
    [Parameter]
    public bool IsClearable { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 清空文本框时的回调方法，默认为 null</para>
    /// <para lang="en">Gets or sets the callback method when clearing text box. Default is null</para>
    /// </summary>
    [Parameter]
    public Func<TValue, Task>? OnClear { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 清空小按钮图标，默认为 null</para>
    /// <para lang="en">Gets or sets the clear button icon. Default is null</para>
    /// </summary>
    [Parameter]
    [Obsolete("已弃用，请使用 ClearIcon 参数；Deprecated, use the ClearIcon parameter")]
    [ExcludeFromCodeCoverage]
    public string? ClearableIcon { get => ClearIcon; set => ClearIcon = value; }

    /// <summary>
    /// <para lang="zh">获得/设置 清空小按钮图标，默认为 null</para>
    /// <para lang="en">Gets or sets the clear button icon. Default is null</para>
    /// </summary>
    [Parameter]
    public string? ClearIcon { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 输入框前面的图标，基于 Bootstrap 6 的 <c>form-adorn</c>。默认 null</para>
    /// <para lang="en">Gets or sets the icon before the input, based on the <c>form-adorn</c> of Bootstrap 6. Default is null</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? PrefixIcon { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 输入框前面的文字，如 <c>https://</c>。默认 null</para>
    /// <para lang="en">Gets or sets the text before the input such as <c>https://</c>. Default is null</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? PrefixText { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 输入框后面的图标。默认 null</para>
    /// <para lang="en">Gets or sets the icon after the input. Default is null</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? SuffixIcon { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 输入框后面的文字，如 <c>@example.com</c>。默认 null</para>
    /// <para lang="en">Gets or sets the text after the input such as <c>@example.com</c>. Default is null</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public string? SuffixText { get; set; }

    private bool HasPrefix => !string.IsNullOrEmpty(PrefixIcon) || !string.IsNullOrEmpty(PrefixText);

    private bool HasSuffix => !string.IsNullOrEmpty(SuffixIcon) || !string.IsNullOrEmpty(SuffixText);

    private bool HasAdorn => HasPrefix || HasSuffix;

    // with adornments the label is the control (border, focus ring, size) and the input inside is a ghost
    private string? AdornClassString => CssBuilder.Default("form-control form-adorn")
        .AddClass($"border-{Color.ToDescriptionString()}", Color != Color.None && !IsDisabled && !IsValid.HasValue)
        .Build();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override string? ClassName => HasAdorn
        ? CssBuilder.Default("form-ghost").AddClass(CssClass).AddClass(ValidCss).Build()
        : base.ClassName;

    [Inject]
    [NotNull]
    private IIconTheme? IconTheme { get; set; }

    private string? ReadonlyString => Readonly ? "true" : null;

    private string? ClearableIconString => CssBuilder.Default("form-control-clear-icon")
        .AddClass(ClearIcon)
        .Build();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        ClearIcon ??= IconTheme.GetIconByKey(ComponentIcons.InputClearIcon);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <param name="validationErrorMessage"></param>
    protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, out string? validationErrorMessage)
    {
        bool ret;
        var v = IsTrim ? value.Trim() : value;
        if (AutoSetDefaultWhenNull && string.IsNullOrEmpty(v))
        {
            result = default!;
            validationErrorMessage = null;
            ret = true;
        }
        else
        {
            ret = base.TryParseValueFromString(v, out result, out validationErrorMessage);
        }
        return ret;
    }

    private async Task OnClickClear()
    {
        if (OnClear != null)
        {
            await OnClear(Value);
        }
        CurrentValueAsString = "";
    }
}
