// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

using Microsoft.Extensions.Localization;

namespace BootstrapBlazor.Components;

/// <summary>
/// <para lang="zh">PasswordStrength 密码强度组件，基于 Bootstrap 6 的 <c>Strength</c></para>
/// <para lang="en">PasswordStrength component, based on the <c>Strength</c> of Bootstrap 6</para>
/// <para><version>11.1.1</version></para>
/// </summary>
public partial class PasswordStrength
{
    /// <summary>
    /// <para lang="zh">获得/设置 密码输入框的 Id，必填。组件在浏览器内读取该输入框的值并显示强度</para>
    /// <para lang="en">Gets or sets the id of the password input, required. The component reads the value of that input in the browser and shows its strength</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? InputId { get; set; }

    /// <summary>
    /// <para lang="zh">获得/设置 最小长度，达到后得第一分。默认 8</para>
    /// <para lang="en">Gets or sets the minimum length that earns the first point. Default is 8</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public int MinLength { get; set; } = 8;

    /// <summary>
    /// <para lang="zh">获得/设置 是否显示强度文字。默认 true</para>
    /// <para lang="en">Gets or sets whether the strength text is shown. Default is true</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public bool ShowText { get; set; } = true;

    /// <summary>
    /// <para lang="zh">获得/设置 强度等级改变回调方法</para>
    /// <para lang="en">Gets or sets the callback invoked when the strength level changes</para>
    /// <para><version>11.1.1</version></para>
    /// </summary>
    [Parameter]
    public Func<PasswordStrengthLevel, Task>? OnStrengthChanged { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<PasswordStrength>? Localizer { get; set; }

    private string? ClassString => CssBuilder.Default("bb-password-strength")
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override Task InvokeInitAsync() => InvokeVoidAsync("init", Id, Interop, nameof(TriggerStrengthChanged), new
    {
        input = $"#{InputId}",
        minLength = MinLength,
        messages = new
        {
            weak = Localizer["Weak"].Value,
            fair = Localizer["Fair"].Value,
            good = Localizer["Good"].Value,
            strong = Localizer["Strong"].Value
        }
    });

    /// <summary>
    /// <para lang="zh">强度等级改变时由 JavaScript 调用</para>
    /// <para lang="en">Called by JavaScript when the strength level changes</para>
    /// </summary>
    /// <param name="level"><para lang="zh">等级 weak fair good strong，空为未输入</para><para lang="en">Level weak, fair, good, strong; null for no input</para></param>
    [JSInvokable]
    public async Task TriggerStrengthChanged(string? level)
    {
        if (OnStrengthChanged != null)
        {
            var value = level switch
            {
                "weak" => PasswordStrengthLevel.Weak,
                "fair" => PasswordStrengthLevel.Fair,
                "good" => PasswordStrengthLevel.Good,
                "strong" => PasswordStrengthLevel.Strong,
                _ => PasswordStrengthLevel.None
            };
            await OnStrengthChanged(value);
        }
    }
}
