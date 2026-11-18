// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace UnitTest.Components;

/// <summary>
/// Test for AvatarStack component
/// </summary>
public class AvatarStackTest : BootstrapBlazorTestBase
{
    [Fact]
    public void Render_Ok()
    {
        var cut = Context.Render<AvatarStack>(pb =>
        {
            pb.Add(a => a.ChildContent, builder =>
            {
                builder.OpenComponent<Avatar>(0);
                builder.AddAttribute(1, nameof(Avatar.IsText), true);
                builder.AddAttribute(2, nameof(Avatar.Text), "A");
                builder.CloseComponent();
                builder.OpenComponent<Avatar>(3);
                builder.AddAttribute(4, nameof(Avatar.IsText), true);
                builder.AddAttribute(5, nameof(Avatar.Text), "B");
                builder.CloseComponent();
            });
        });

        var stack = cut.Find("div.avatar-stack");
        Assert.Equal(2, stack.QuerySelectorAll("span.avatar").Length);
    }

    [Fact]
    public void AdditionalAttributes_Ok()
    {
        var cut = Context.Render<AvatarStack>(pb =>
        {
            pb.Add(a => a.AdditionalAttributes, new Dictionary<string, object> { ["class"] = "my-stack" });
        });

        Assert.Contains("avatar-stack my-stack", cut.Find("div.avatar-stack").ClassName);
    }
}
