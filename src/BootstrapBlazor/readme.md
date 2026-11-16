# Bootstrap Blazor Component

## an enterprise-level UI component library based on Bootstrap and Blazor.

This project is part of the [.NET Foundation](https://www.dotnetfoundation.org/) and operates under their [Code Of Conduct](https://www.dotnetfoundation.org/code-of-conduct)

[![License](https://img.shields.io/github/license/dotnetcore/bootstrapblazor.svg?logo=git&logoColor=red&color=)](https://github.com/dotnetcore/BootstrapBlazor/blob/main/LICENSE)
[![Github build](https://img.shields.io/github/actions/workflow/status/dotnetcore/BootstrapBlazor/build.yml?branch=main&?label=main&logo=github)](https://github.com/dotnetcore/BootstrapBlazor/actions?query=workflow%3A%22Build+Project%22+branch%3Amain)
[![Repo Size](https://img.shields.io/github/repo-size/dotnetcore/BootstrapBlazor.svg?logo=github&logoColor=green&label=repo)](https://github.com/dotnetcore/BootstrapBlazor)
[![Commit Date](https://img.shields.io/github/last-commit/dotnetcore/BootstrapBlazor/main.svg?logo=github&logoColor=green&label=commit)](https://github.com/dotnetcore/BootstrapBlazor)
[![codecov](https://codecov.io/gh/dotnetcore/BootstrapBlazor/branch/main/graph/badge.svg?token=5SXIWHXZC3)](https://codecov.io/gh/dotnetcore/BootstrapBlazor)

---

## Online Examples
[![website](https://img.shields.io/badge/online-https://www.blazor.zone-success.svg?&logo=buzzfeed&logoColor=green)](https://www.blazor.zone)

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
- **Spacing, display and visibility utilities**: `m*-*`, `p*-*`, `d-*`, `visible` and `invisible` (all breakpoints) are `!important` again as well, so `class="ms-3"`, `d-none` or `d-flex` on a BootstrapBlazor component keeps working even where the component sets the same property. The rules are generated from the Bootstrap css by `scripts/gen-important-utilities.py`. Overriding one of them in your own css needs `!important`.
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

## Code of conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
