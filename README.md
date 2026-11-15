<h1 align="center">Bootstrap Blazor Component</h1>

<div align="center">
<h2>Bootstrap Blazor is an enterprise-level UI component library based on Bootstrap and Blazor.</h2>

[![License](https://img.shields.io/github/license/dotnetcore/BootstrapBlazor.svg?logo=git&logoColor=red)](https://github.com/dotnetcore/BootstrapBlazor/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/BootstrapBlazor.svg?color=red&logo=nuget&logoColor=green)](https://www.nuget.org/packages/BootstrapBlazor/)
[![Nuget](https://img.shields.io/nuget/dt/BootstrapBlazor.svg?logo=nuget&logoColor=green)](https://www.nuget.org/packages/BootstrapBlazor/)
[![Repo Size](https://img.shields.io/github/repo-size/dotnetcore/BootstrapBlazor.svg?logo=github&logoColor=green&label=repo)](https://github.com/dotnetcore/BootstrapBlazor)
[![Commit Date](https://img.shields.io/github/last-commit/dotnetcore/BootstrapBlazor/main.svg?logo=github&logoColor=green&label=commit)](https://github.com/dotnetcore/BootstrapBlazor)
[![Github build](https://img.shields.io/github/actions/workflow/status/dotnetcore/BootstrapBlazor/build.yml?branch=main&?label=main&logo=github)](https://github.com/dotnetcore/BootstrapBlazor/actions?query=workflow%3A%22Build+Project%22+branch%3Amain)
[![codecov](https://codecov.io/gh/dotnetcore/BootstrapBlazor/branch/main/graph/badge.svg?token=5SXIWHXZC3)](https://codecov.io/gh/dotnetcore/BootstrapBlazor)
</div>

English | <a href="README.zh-CN.md">中文</a>

---
## .NET Foundation
[<img align="right" src="https://github.com/dotnet-foundation/swag/blob/main/logo/dotnetfoundation_v4.png?raw=true" width="68px" />](https://dotnetfoundation.org/projects/project-detail/bootstrap-blazor)
<p>This project is part of the <a href="https://www.dotnetfoundation.org/">.NET Foundation</a> and operates under their <a href="https://www.dotnetfoundation.org/code-of-conduct">code of conduct</a>. </p>

## Online Examples
[![website](https://img.shields.io/badge/online-https://www.blazor.zone-success.svg?color=&logo=buzzfeed&logoColor=green)](https://www.blazor.zone)

## Environment

- Install .net SDK [Official website](https://dotnet.microsoft.com/download)
- Install Visual Studio latest [Official website](https://visualstudio.microsoft.com/vs)

## Quick Installation Guide

### Install Package
```
dotnet add package BootstrapBlazor
```

### Add the following to `_Imports.razor`
```
@using BootstrapBlazor.Components
```

### Add the following to the `MainLayout.razor`
```html
<BootstrapBlazorRoot>
    @Body
</BootstrapBlazorRoot>
```

### Add the following to your HTML head section
it's either **index.html** or **_Layout.cshtml/_Host.cshtml/App.razor** depending on whether you're running WebAssembly or Server
```html
<link rel="stylesheet" href="_content/BootstrapBlazor/css/bootstrap.blazor.bundle.min.css" />
```

### Add the following script at the end of the body
```html
<script src="_content/BootstrapBlazor/js/bootstrap.blazor.bundle.min.js"></script>
```

### Add the following to the relevant sections of `Program.cs`
```csharp
builder.Services.AddBootstrapBlazor();
```

## Bootstrap version

The components are built on **Bootstrap 6** (currently `6.0.0-alpha.1`). Bootstrap 6 renamed many classes, so markup written for Bootstrap 5 has to be updated:

| Bootstrap 5 | Bootstrap 6 |
| --- | --- |
| `btn-primary`, `btn-outline-danger` | `btn-solid theme-primary`, `btn-outline theme-danger` |
| `text-danger`, `text-muted` | `fg-danger`, `fg-3` |
| `alert-info`, `text-bg-info` | `theme-info` |
| `col-sm-6`, `d-md-none` | `sm:col-6`, `md:d-none` |
| `dropdown-menu`, `dropdown-item` | `menu`, `menu-item` |
| `modal-header`, `modal-body`, `modal-footer` | `dialog-header`, `dialog-body`, `dialog-footer` |
| `navbar-expand-md` | `md:navbar-expand` |

The Bootstrap 5 CSS variables used most often (`--bs-primary`, `--bs-body-bg`, `--bs-border-radius`, ...) are still available and map to the Bootstrap 6 tokens. Bootstrap 6 no longer ships a separate RTL stylesheet.

BootstrapBlazor components that Bootstrap 6 now ships a class of the same name for use the prefix `bb-`. Update custom css and scripts that select the old names:

| Component | Before | Now |
| --- | --- | --- |
| `Menu` | `menu`, `submenu`, `menu-text` | `bb-menu`, `bb-submenu`, `bb-menu-text` |
| `Drawer` | `drawer`, `drawer-body`, `drawer-backdrop`, ... | `bb-drawer`, `bb-drawer-body`, `bb-drawer-backdrop`, ... |
| `Switch`, `NullSwitch` | `switch`, `switch-core`, `switch-label` | `bb-switch`, `bb-switch-core`, `bb-switch-label` |
| `Table` (card view) | `table-card` | `bb-table-card` |

Things to know when you write your own css:

- **Cascade layers**: Bootstrap 6 puts its css into `@layer`, and unlayered css always wins against layered css. Your own rules therefore override Bootstrap rules even with a lower specificity. The generic rules of BootstrapBlazor (`:disabled`, `:focus`, `a`, `[for]`, the padding of `.form-control`) live in the layers `reboot` and `forms` for the same reason.
- **Color utilities**: `bg-*`, `border-*` and `fg-*` for `primary`, `secondary`, `success`, `info`, `warning`, `danger`, `light` and `dark` are `!important` again, like the Bootstrap 5 utilities (components combine them). Overriding one of them in your own css needs `!important` as well.
- **`Color.Light` and `Color.Dark`**: Bootstrap 6 has no such theme, BootstrapBlazor provides `theme-light` and `theme-dark`.
- **Closing a dialog**: `OnClosingAsync` is now also called when a dialog is closed with Escape or a click on the backdrop, and returning `false` keeps the dialog open.

## Usage
```razor
<Display Value="@_text"></Display>
<Button Text="Button" OnClick="@ClickButton"></Button>

@code {
    private string? _text;
    private void ClickButton(MouseEventArgs e)
    {
        _text = DateTime.Now.ToString();
    }
}
```

## Install CLI Template
1. Install the template
```
dotnet new install Bootstrap.Blazor.Templates::*
```

2. Create the Boilerplate project with the template
```
dotnet new bbapp
```

## Contribution
1. Fork
2. Create Feat_xxx branch
3. Commit
4. Create Pull Request

## Code of Conduct
This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## Donate
If this project is helpful to you, please scan the QR code below for a cup of coffee.

<img src="https://raw.githubusercontent.com/ArgoZhang/Images/master/Donate/BarCode%402x.png" width="382px;" />

## Sponsor
Thanks to [JetBrains](https://jb.gg/OpenSourceSupport) for providing free open source license
<img src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg" width="100px" align="right" />
