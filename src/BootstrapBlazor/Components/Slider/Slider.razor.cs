// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">滑块组件</para>
/// <para lang="en">Slider Component</para>
/// </summary>
public partial class Slider<TValue>
{
    // Bootstrap 6: .form-range is the wrapper (track, fill, bubble, ticks), the input inside is .form-range-input
    // the inherited Color gives the fill and the thumb the theme color
    private string? WrapperClassString => CssBuilder.Default("form-range")
        .AddClass($"theme-{Color.ToDescriptionString()}", Color != Color.None)
        .Build();

    private string? ClassString => CssBuilder.Default("form-range-input")
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// <para lang="zh">获得/设置 最小值 默认为 null 未设置</para>
    /// <para lang="en">Gets or sets Min Value. Default null</para>
    /// </summary>
    [Parameter]
    [NotNull]
    public TValue? Min { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 最大值 默认为 null 未设置</para>
    /// <para lang="en">Gets or sets Max Value. Default null</para>
    /// </summary>
    [Parameter]
    [NotNull]
    public TValue? Max { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 步长 默认为 null 未设置</para>
    /// <para lang="en">Gets or sets Step. Default null</para>
    /// </summary>
    [Parameter]
    [NotNull]
    public TValue? Step { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否在滑块上方显示当前值气泡，基于 Bootstrap 6 的 <c>Range</c>。默认 false，首次渲染后不应再改变</para>
    /// <para lang="en">Gets or sets whether a bubble with the current value is shown above the thumb, based on the <c>Range</c> of Bootstrap 6. Default is false, it should not change after the first render</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public bool IsShowBubble { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 是否按步长显示刻度。未设置时按浏览器默认值 0、100、1 计算，刻度超过 100 个时不显示。默认 false</para>
    /// <para lang="en">Gets or sets whether tick marks are shown at every step. Not set values count as the defaults of the browser (0, 100 and 1), more than 100 ticks are not shown. Default is false</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public bool IsShowTicks { get; set; }

    private string? MinString => Min.ToString() == "0" ? GetRangeMinString : Min.ToString();

    private string? GetRangeMinString => _range?.Minimum.ToString();

    private string? MaxString => Max.ToString() == "0" ? GetRangeMaxString : Max.ToString();

    private string? GetRangeMaxString => _range?.Maximum.ToString();

    private string? StepString => Step.ToString() == "0" ? null : Step.ToString();

    private RangeAttribute? _range = null;

    private const int MaxTicks = 100;

    private List<string> _ticks = [];

    private bool HasTicks => _ticks.Count > 0;

    private string? ListId => HasTicks ? $"{Id}_ticks" : null;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (FieldIdentifier.HasValue)
        {
            _range = FieldIdentifier.Value.GetRange();
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _ticks = IsShowTicks ? BuildTicks() : [];
    }

    private List<string> BuildTicks()
    {
        // a value of 0 or nothing means "not set": the browser uses 0, 100 and 1
        var ret = new List<string>();
        // the overload without NumberStyles needs .NET 7, the library also targets older versions
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        var style = System.Globalization.NumberStyles.Number;
        var min = decimal.TryParse(MinString, style, culture, out var minValue) ? minValue : 0;
        var max = decimal.TryParse(MaxString, style, culture, out var maxValue) ? maxValue : 100;
        var step = decimal.TryParse(StepString, style, culture, out var stepValue) ? stepValue : 1;
        if (step > 0 && max > min && (max - min) / step <= MaxTicks)
        {
            for (var value = min; value <= max; value += step)
            {
                ret.Add(value.ToString(culture));
            }
        }
        return ret;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override Task InvokeInitAsync() => InvokeVoidAsync("init", Id, new { bubble = IsShowBubble });

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        // the value may have been set from the program: let Bootstrap draw the fill again
        if (!firstRender && Module != null)
        {
            await InvokeVoidAsync("update", Id);
        }
    }
}
