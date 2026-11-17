<h1 align="center">Bootstrap Blazor Component UI Lib</h1>

<div align="center">
<h2>A set of enterprise-class UI components based on Bootstrap and Blazor.</h2>


[![Nuget](https://img.shields.io/nuget/v/BootstrapBlazor.svg?color=red&logo=nuget&logoColor=green)](https://www.nuget.org/packages/BootstrapBlazor/)
[![Nuget](https://img.shields.io/nuget/dt/BootstrapBlazor.svg?logo=nuget&logoColor=green)](https://www.nuget.org/packages/BootstrapBlazor/)
[![Commit Date](https://img.shields.io/github/last-commit/ArgoZhang/BootstrapBlazor/main.svg?logo=github&logoColor=green&label=commit)](https://github.com/ArgoZhang/BootstrapBlazor)

</div>

---

### 发布周期

- Major version: consistent with Microsoft .NET
- Minor version: increase by 1 every month
- patch version: official version will be released every Monday

--- 

## Unreleased

### Features
* feat(Bootstrap): upgrade to v6.0.0-alpha.1 by @MaikPeters1511 in https://github.com/MaikPeters1511/BootstrapBlazor/pull/1
* feat(PasswordStrength): new `PasswordStrength` component based on `bootstrap.Strength`: four segments and a text show the strength of a password field (`InputId`, `MinLength`, `ShowText`, `OnStrengthChanged`)
* feat(Nav): new parameters `IsOverflow` and `OverflowText`: items that do not fit move into a "More" menu (based on the `NavOverflow` of Bootstrap 6)
* feat(ScrollSpy): new `ScrollSpy` component based on `bootstrap.ScrollSpy`: highlights the matching link of a navigation while the content scrolls (`Target`, `TopMargin`, `RootMargin`, `IsSmoothScroll`, `OnActivate`, `RefreshAsync`)
* feat(Input): `BootstrapInput` (and its subclasses) get `PrefixIcon`, `PrefixText`, `SuffixIcon` and `SuffixText`: an icon or text before or after the input, based on the `form-adorn` of Bootstrap 6

### Breaking changes
* The thresholds of `BreakPoint` and of the `Responsive` component follow Bootstrap 6: `Large` from 1024px (was 992px), `ExtraLarge` from 1280px (was 1200px) and `ExtraExtraLarge` from 1536px (was 1400px); the widths of `dialog-xxl` are unchanged
* The spacing classes of the components follow the new 12-step spacer scale of Bootstrap 6 (`ms-3` is 0.5rem now, was 1rem); the library maps its own usages with the official table `2` -> `3`, `3` -> `5`, `4` -> `7`, `5` -> `12` (`1` stays), so the components look as before. Your own markup with `m*`, `p*`, `gap-*` and `g-*` has to be mapped the same way
* Bootstrap 6 needs Chrome and Edge 130, Firefox 132 and Safari 18 or newer (macOS, iOS and iPadOS) and ships no fallbacks, prefixes or polyfills; older browsers do not resolve the colors and render the pages incorrectly
* Bootstrap 6 renamed many classes; markup written for Bootstrap 5 has to be updated (`btn-primary` -> `btn-solid theme-primary`, `text-danger` -> `fg-danger`, `col-sm-6` -> `sm:col-6`, `dropdown-menu` -> `menu`, `modal-body` -> `dialog-body`). See the "Bootstrap version" section in `README.md`
* `Modal` renders a native `<dialog>` element
* `Menu`, `Drawer`, `Switch` / `NullSwitch` and the card view of `Table` use the classes `bb-menu`, `bb-drawer`, `bb-switch` (`bb-switch-core`, `bb-switch-label`) and `bb-table-card`, because Bootstrap 6 ships its own classes with the old names ([#1](https://github.com/MaikPeters1511/BootstrapBlazor/pull/1), [#2](https://github.com/MaikPeters1511/BootstrapBlazor/pull/2), [#3](https://github.com/MaikPeters1511/BootstrapBlazor/pull/3))
* `InputUpload`: the defaults of `BrowserButtonClass` and `DeleteButtonClass` are now `btn-solid theme-primary` and `btn-solid theme-danger`
* there is no separate Bootstrap RTL stylesheet any more
* Bootstrap 6 uses cascade layers, unlayered css always wins against it; the color utilities `bg-*`, `border-*` and `fg-*` and the spacing, display and visibility utilities (`m*-*`, `p*-*`, `d-*`, `visible`, `invisible`) are `!important` again like in Bootstrap 5 and overriding them in custom css needs `!important` ([#2](https://github.com/MaikPeters1511/BootstrapBlazor/pull/2), [#5](https://github.com/MaikPeters1511/BootstrapBlazor/pull/5), [#6](https://github.com/MaikPeters1511/BootstrapBlazor/pull/6))
* `OnClosingAsync` is also called when a dialog is closed with Escape or a click on the backdrop, and returning `false` keeps the dialog open ([#7](https://github.com/MaikPeters1511/BootstrapBlazor/pull/7))

## Release 2023-10-16 V7.11.1

### Bugs
* fix(MenuLink): always active if href equal "/" by @ray-gilbert in https://github.com/dotnetcore/BootstrapBlazor/pull/2204
* fix(CountUp): not show the Value when set value to 0 by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2236
* fix(Upload): preview function not work after reupload by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2240
* bug(Table): can't resize the last column when fixed header by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2266

### Features

* feat(QueryPageOptions): add IsVirtualScroll parameter by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2211
* feat(Responsive): add ExtraExtraSmall breakpoint by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2215
* feat(Bootstrap): upgrade to v5.3.2 by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2217
* feat(DockView): add Reset instance method by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2220
* feat(Dock): add GetLayoutConfig instance method by @densen2014 in https://github.com/dotnetcore/BootstrapBlazor/pull/2221
* feat(Table): order of edit pop-up consisitent with table column by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2223
* feat(ColorPicker): update css adapte InputGroup by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2226
* feat(Marquee): add Marquee component by @azlis in https://github.com/dotnetcore/BootstrapBlazor/pull/2225
* feat(Layout): add AllowDragTab on Layout component by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2230
* feat(MaterialDesign): add TableAdvancedSort icon by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2238
* feat(BootstrapInputGroupLabel): add ShowRequiredMark parameter by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2242
* feat(Carousel): add PlayMode parameter by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2252
* feat(Chart): support change grid line color by @azlis in https://github.com/dotnetcore/BootstrapBlazor/pull/2248
* feat(InputGroup): compatible Tooltip inside InputGroup by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2254
* feat(InputGroup): compatible Popover inside InputGroup by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2256
* feat(InputGroup): compatible Swtich inside InputGroup by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2259
* feat(Select): add GroupItemTemplate parameter by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2264
* feat: add stack component by @Vision-Zhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2250
* feat(InputNumber): add global setting for step by @ArgoZhang in https://github.com/dotnetcore/BootstrapBlazor/pull/2268

## New Contributors
* @ray-gilbert made their first contribution in https://github.com/dotnetcore/BootstrapBlazor/pull/2204

**Full Changelog**: https://github.com/dotnetcore/BootstrapBlazor/compare/v7.11.0...v7.11.1