// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace UnitTest.Components;

/// <summary>
/// Test for PasswordStrength component
/// </summary>
public class PasswordStrengthTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Render_Ok()
    {
        var cut = Context.Render<PasswordStrength>(pb =>
        {
            pb.Add(a => a.InputId, "pwd");
        });

        // Bootstrap 6 markup: four segments and a text line
        Assert.Single(cut.FindAll(".bb-password-strength"));
        Assert.Single(cut.FindAll(".strength"));
        Assert.Equal(4, cut.FindAll(".strength-segment").Count);
        Assert.Single(cut.FindAll(".strength-text"));
    }

    [Fact]
    public void ShowText_Ok()
    {
        var cut = Context.Render<PasswordStrength>(pb =>
        {
            pb.Add(a => a.InputId, "pwd");
            pb.Add(a => a.ShowText, false);
        });

        Assert.Empty(cut.FindAll(".strength-text"));
    }

    [Fact]
    public void AdditionalAttributes_Ok()
    {
        var cut = Context.Render<PasswordStrength>(pb =>
        {
            pb.Add(a => a.InputId, "pwd");
            pb.Add(a => a.AdditionalAttributes, new Dictionary<string, object> { ["class"] = "my-strength" });
        });

        Assert.Contains("bb-password-strength my-strength", cut.Find(".bb-password-strength").ClassName);
    }

    [Theory]
    [InlineData("weak", PasswordStrengthLevel.Weak)]
    [InlineData("fair", PasswordStrengthLevel.Fair)]
    [InlineData("good", PasswordStrengthLevel.Good)]
    [InlineData("strong", PasswordStrengthLevel.Strong)]
    [InlineData(null, PasswordStrengthLevel.None)]
    [InlineData("unknown", PasswordStrengthLevel.None)]
    public async Task OnStrengthChanged_Ok(string? level, PasswordStrengthLevel expected)
    {
        PasswordStrengthLevel? actual = null;
        var cut = Context.Render<PasswordStrength>(pb =>
        {
            pb.Add(a => a.InputId, "pwd");
            pb.Add(a => a.OnStrengthChanged, v =>
            {
                actual = v;
                return Task.CompletedTask;
            });
        });

        await cut.InvokeAsync(() => cut.Instance.TriggerStrengthChanged(level));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task OnStrengthChanged_NoCallback_Ok()
    {
        var cut = Context.Render<PasswordStrength>(pb =>
        {
            pb.Add(a => a.InputId, "pwd");
        });

        // without a callback the call is ignored
        await cut.InvokeAsync(() => cut.Instance.TriggerStrengthChanged("good"));
    }
}
