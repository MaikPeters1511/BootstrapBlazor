# Backlog: Bootstrap-6-Migration

Stand: 2026-11-14, `main` = `f7ccac576` (PR #1 bis #3 im Fork gemergt).
Bootstrap-Version: `6.0.0-alpha.1`. Lokaler Teststand: 2811 von 2813 (Release), die zwei Fehlschläge sind `JSModuleExtensionsTest.LoadModule_Ok` und `JSModuleTest.JSModule_JSException` (`#if DEBUG`).

**Legende** – Priorität: P1 vor einem Upstream-PR klären, P2 bald, P3 bei Gelegenheit. Aufwand: S unter 1 h, M einige Stunden, L ein Tag oder mehr.

## Übersicht

| ID | Task | Prio | Aufwand |
| --- | --- | --- | --- |
| A1 | ~~CI im Fork zum Laufen bringen~~ erledigt 2026-11-14 | P1 | S |
| A2 | ~~Testsuite im Debug-Build ausführen~~ erledigt 2026-11-14 (in A1 mit CI) | P1 | S |
| A3 | ~~NuGet-Paket und JS-Konvertierung prüfen~~ erledigt 2026-11-14 | P1 | M |
| B1 | ~~Layer-Konflikte systematisch suchen~~ erledigt 2026-11-14 | P1 | L |
| B2 | ~~Farb-Utilities im Browser prüfen~~ erledigt 2026-11-14 | P1 | M |
| B3 | Theme `ant.css` migrieren | P2 | M |
| B4 | Themes `devui.css` und `blue.css` prüfen | P2 | S |
| B5 | Motronic-Theme: Hover- und Active-Farben | P3 | S |
| C1 | ~~Doku und CHANGELOG nachziehen~~ erledigt 2026-11-15 | P2 | S |
| C2 | Chinesische Texte gegenlesen | P3 | S |
| C3 | CHANGELOG-Link und Upstream-Vorbereitung | P2 | S |
| D1 | Sichtprüfung aller Demo-Seiten (automatisiert) | P2 | L |
| D2 | Drucken von Dialogen prüfen | P3 | S |
| D3 | Schmale Breiten und Touch prüfen | P2 | M |
| D4 | Tastaturbedienung prüfen | P2 | M |
| D5 | Einzelne Komponenten prüfen | P2 | M |
| D6 | Starter-Vorlage `Bs6Test` prüfen und ablegen | P3 | S |
| E1 | ~~Dialog: `OnClosingAsync` bei Escape und Hintergrundklick~~ erledigt 2026-11-14 | P2 | M |
| E2 | Dialog: `dialog-instant` geht beim Re-Render verloren | P3 | S |
| E3 | Carousel: `AutoPlayAfterManually` nach Hover | P3 | S |
| F1 | Wechsel auf neue Bootstrap-6-Alphas und die stabile Version | P3 | M |

---

## A – Absicherung

### A1 – CI im Fork zum Laufen bringen – erledigt 2026-11-14
- **Ergebnis:** Actions waren in der API als aktiv gemeldet, es gab aber nie einen Lauf (auch nicht für PR #1 bis #3). Nach dem erneuten Setzen von `actions/permissions` (`enabled=true`, `allowed_actions=all`) starten die Workflows. Der Probe-PR #4 (inzwischen geschlossen) hat „Auto PR test(bot)“ ausgelöst: 2 Minuten, 2813 von 2813 Tests grün, auf Linux, im Debug-Build.
- **Hinweis:** Ob das erneute Setzen oder etwas anderes den Lauf ausgelöst hat, ist nicht geklärt. Die Workflows `pack`, `docker` und `publish` laufen nur auf den Branches `pack`, `release`, `hotfix`, `docker` und `publish` und brauchen Secrets, die im Fork fehlen.
- **Offen:** `auto-approve.yml` wird übersprungen (nur für den Autor `ArgoZhang`); `build.yml` (Push auf `main`, mit Codecov-Upload) ist noch nicht gelaufen.

(Ursprüngliche Beschreibung:)
- **Warum:** Die drei PRs wurden ohne Checks gemergt (`mergeStateStatus: CLEAN`, aber 0 Checks).
- **Schritte:** Prüfen, warum `.github/workflows/build.yml` und `auto-pull-request-checks.yml` im Fork nicht laufen (Actions im Fork deaktiviert? Trigger nur für das Hauptrepo?). Actions aktivieren oder Trigger anpassen.
- **Fertig, wenn:** Ein neuer PR im Fork startet Build und Tests, und das Ergebnis ist grün.

### A2 – Testsuite im Debug-Build ausführen – erledigt 2026-11-14
- **Ergebnis:** Der CI-Lauf aus A1 nutzt `dotnet test` ohne Konfiguration, also Debug: alle 2813 Tests bestehen, auch die zwei `#if DEBUG`-Tests und die acht Tests, die lokal an der Schreibsperre scheitern.

(Ursprüngliche Beschreibung:)
- **Warum:** Zwei Tests hängen an `#if DEBUG`; im Release-Lauf sind sie nie gelaufen. Debug nutzt ausserdem den Sass-Compiler des Builds statt meines Skripts.
- **Schritte:** `dotnet test test/UnitTest/UnitTest.csproj` ohne `-c Release`; Ergebnis mit dem Release-Lauf vergleichen.
- **Fertig, wenn:** Alle Tests laufen durch, oder jeder Fehlschlag ist erklärt und ein Task angelegt.

### A3 – NuGet-Paket und JS-Konvertierung prüfen – erledigt 2026-11-14
- **Ergebnis:**
  - `dotnet pack src/BootstrapBlazor -c Release` erzeugt `BootstrapBlazor.11.1.1-beta01.nupkg` (7,4 MB) mit allen sechs Zielframeworks (net6.0 bis net11.0).
  - `staticwebassets/js/bootstrap.blazor.bundle.min.js` ist das klassische Script: Bootstrap-6.0.0-alpha.1-Banner, kein `export{`, endet mit `window.bootstrap={Alert,…,Tooltip}`.
  - `staticwebassets/css/bootstrap.blazor.bundle.min.css` und `…rtl.min.css` (je 722 KB) sowie `motronic.min.css` sind enthalten.
  - Eine Kopie der Starter-Vorlage, die das Paket aus einer lokalen Quelle statt des Projektverweises nutzt, baut und läuft: Bundle und CSS werden ausgeliefert, Dropdown, Dialog und Switch funktionieren.
  - Der Lauf von `build.yml` auf `main` (Linux, mit `Bundle=True`, Tests, Codecov-Schritt) ist grün.
- **Beobachtung:** `bootstrap.blazor.bundle.rtl.min.css` ist byte-gleich mit der LTR-Datei, weil Bootstrap 6 keine RTL-Datei mehr hat und beide Bundles dieselben Eingaben bekommen. Das ist gewollt, die Datei bleibt nur aus Kompatibilitätsgründen im Paket.
- **Nicht geprüft:** der Veröffentlichungsweg (`pack.yml` mit NuGet-Login) und das Verhalten unter WebAssembly statt Blazor Server.

(Ursprüngliche Beschreibung:)
- **Warum:** `bootstrap.blazor.bundle.min.js` entsteht über die neue MSBuild-Task `ConvertBootstrapJs` (`Directory.Build.targets`). Der Paketinhalt wurde nie geprüft; die Task lief nur auf macOS.
- **Schritte:**
  1. `dotnet pack src/BootstrapBlazor -c Release`, Paket entpacken, `staticwebassets/js/bootstrap.blazor.bundle.min.js` und `css/*.css` prüfen.
  2. Build auf Linux (CI oder Container) ausführen; die Task nutzt `RoslynCodeTaskFactory`.
  3. Prüfen, dass `bootstrap.blazor.bundle.rtl.min.css` weiterhin im Paket liegt.
- **Fertig, wenn:** Paket enthält Bundle, Skript ist ein klassisches Script mit `window.bootstrap`, Build läuft auf Linux.

---

## B – Migration und Layer-Konflikte

Hintergrund: Bootstrap 6 legt sein CSS in `@layer`. Ungelayertes BB-CSS gewinnt immer. Behoben sind bisher `:disabled`, `:focus` (in `root.scss`) und die Farb-Utilities (in `compat.scss`). Namenskollisionen (`menu`, `drawer`, `switch`, `table-card`) sind per Klassenabgleich abgearbeitet.

### B1 – Layer-Konflikte systematisch suchen – erledigt 2026-11-14
- **Vorgehen:** Ein Skript im Browser (Vergleich je Element und Eigenschaft: Bootstrap-6-Regel im Layer gegen BB-Regel ohne Layer; gemeldet wird, wenn die Bootstrap-Regel die höhere Spezifität hat und die Werte sich unterscheiden, also BB nur wegen der Layer gewinnt) hat 292 von 315 Demo-Seiten durchlaufen. 23 Seiten mit schweren Fremdbibliotheken (PDF, Office, Mermaid, Editoren, Kamera, Sprache, Diagramme, …) habe ich übersprungen, weil sie den Browser blockierten.
- **Befunde und Fixes (PR in der Historie von `main`):**
  - Floating-Label-Eingabefelder: Das Label überlagerte den Text, weil BBs `.form-control { padding }` Bootstrap 6 (`.form-floating > .form-control`) übersteuerte. Die Regel steht jetzt im Layer `forms` (`ValidateForm.razor.scss`).
  - `a` und `[for]` (Cursor, Unterstreichung) im Layer `reboot` (`root.scss`): deaktivierte Nav-Links und Labels deaktivierter Checkboxen und Radios zeigten den falschen Cursor.
  - Kleine und grosse Input-Groups behalten ihre Bootstrap-6-Schriftgrösse (`root.scss`).
  - Deaktivierte `LinkButton`s sind wieder transparent statt grau (`Button.razor.scss`).
  - Demo: `h3, h4, h5 { margin-bottom }` in `site.css` verschob die Titel in `.dialog-title` und `.popover-header`; die Regel steht jetzt im Layer `reboot`.
- **Bewusst nicht geändert:** Übergangszeiten von `.alert` (BB `.fade`) und `.toast` (BB `.toast`) weichen von den Bootstrap-6-Werten ab, die Effekte sind gleichwertig.
- **Grenzen der Methode:** Nur statische Zustände. Regeln für `:hover`, `:focus`, `:active`, `:checked` und Pseudo-Elemente trifft das Skript nicht; auch Container-Queries (z. B. Navbar) und nachträglich geöffnete Popups (Dialog, Dropdown, Toast, Popover) sind nicht abgedeckt. Dafür bleiben D1 und D5.
- **Idee:** Ein Skript, das für jede BB-Komponente die Selektoren im Komponenten-SCSS mit denen von Bootstrap 6 vergleicht und Regeln meldet, die dieselben Elemente und Eigenschaften treffen (Kandidaten: allgemeine Selektoren wie `a`, `button`, `[disabled]`, `.btn`, `.form-control`, `.card`, `.table`, `.nav-link`).
- **Schritte:** Skript schreiben, Trefferliste nach Komponente gruppieren, jeden Treffer im Browser prüfen, echte Konflikte per TDD beheben (Test zuerst, wie bei `root.scss`).
- **Fertig, wenn:** Die Liste abgearbeitet ist; verbleibende Treffer sind begründet als gewollt markiert.

### B2 – Farb-Utilities im Browser prüfen – erledigt 2026-11-14
- **Vorgehen:** Ein Skript hat auf 262 Demo-Seiten (ohne die 23 schweren Seiten aus B1) im dunklen und im hellen Design jedes Element mit `bg-*`, `border-*` oder `fg-*` (primary bis dark) mit einem Prüfelement derselben Klasse verglichen. Zusätzlich habe ich Toast, Message und SweetAlert nach Klicks auf alle Schaltflächen geprüft (Erfolg, Fehler, Info, Warnung).
- **Befund:** Nur ein Fehler, aber ein echter: Bootstrap 6 sortiert die Farben anders (danger vor warning und info). Komponenten, die zwei Farbklassen kombinieren, zeigten dadurch die falsche Farbe: der Rahmen eines fehlgeschlagenen Avatars (`border-info border-danger`) war blau statt rot, das Warn-Icon in `PopConfirmButton` (`fg-info fg-danger`) blau statt rot. `compat.scss` nutzt jetzt die Reihenfolge von Bootstrap 5 (success, info, warning, danger), abgesichert durch `CompatScss_ColorUtilities_SameOrderAsBootstrap5`.
- **Kein Befund:** Switch, Progress, Collapse, Toast, Timeline, FileIcon, CheckboxList und RadioList als Buttons, Rahmenfarben von Input, Select, DateTimePicker und Cascader, Badge, Alert, Tag.
- **Grenzen:** Nur Klassen im Format `bg|border|fg-<Farbe>`, statische Zustände und die geklickten Popups. `theme-*` (Alert, Badge, Buttons) habe ich nur per Augenschein geprüft, Hover- und Fokuszustände nicht.
- **Warum:** Seit PR #2 setzen `bg-*`, `border-*`, `fg-*` wieder `!important`. Geprüft sind Switch, Progress, Collapse.
- **Prüfen:** Toast, Timeline, FileIcon, CheckboxList und RadioList als Buttons, Rahmenfarben der Eingabefelder (`Color` auf Input, Select, DateTimePicker, Cascader), Badge, Alert, Tag.
- **Fertig, wenn:** Alle genannten Komponenten zeigen im hellen und im dunklen Design die richtige `Color`.

### B3 – Theme `ant.css` migrieren
- **Stand:** Nur mechanisch umbenannt (`dropdown-*` → `menu-*`), nie angesehen. Das Theme setzt Bootstrap-5-Klassen (`.btn-primary`, `.btn-danger`, `.btn-background-ghost`) und Variablen.
- **Schritte:** Theme in der Demo aktivieren; Buttons auf `btn-solid theme-*` umschreiben oder durch Bootstrap-6-Tokens ersetzen (siehe `motronic.min.css` als Vorlage); Test nach dem Muster `MotronicCss_Bootstrap6Tokens_Ok` ergänzen.
- **Fertig, wenn:** Buttons, Eingabefelder und Menüs sehen im Theme richtig aus; der Test verhindert die alten Klassen.

### B4 – Themes `devui.css` und `blue.css` prüfen
- **Schritte:** Gleiches Vorgehen wie B3. `devui.css` enthält `bb-switch`-Selektoren (PR #2), die übrigen Regeln sind ungeprüft.

### B5 – Motronic-Theme: Hover- und Active-Farben
- **Stand:** Die von Hand gesetzten Hover-/Active-Farben der Buttons sind entfallen; Bootstrap 6 leitet sie aus der Grundfarbe ab.
- **Schritte:** Entscheiden, ob die Ableitung reicht; falls nicht, `--bs-btn-hover-bg` usw. je Farbe über `.theme-*` im Theme setzen.

---

## C – Doku und Formales

### C1 – Doku und CHANGELOG nachziehen – erledigt 2026-11-15
- **Ergebnis:** Die drei READMEs (`README.md`, `README.zh-CN.md`, Paket-README) haben im Abschnitt „Bootstrap version“ jetzt eine Tabelle der umbenannten Komponentenklassen (`bb-menu`, `bb-drawer`, `bb-switch`, `bb-table-card`) und vier Hinweise für eigenes CSS: Kaskadenebenen (eigenes, ungelayertes CSS gewinnt immer), Farb-Utilities mit `!important`, `theme-light` / `theme-dark` für `Color.Light` und `Color.Dark`, `OnClosingAsync` bei Escape und Hintergrundklick. `CHANGELOG.md` und `CHANGELOG.zh-CN.md` führen diese Punkte unter „Breaking changes“ mit Links auf die PRs.
- **Zur Einordnung:** Die PR-Links zeigen auf den Fork, siehe C3.

(Ursprüngliche Beschreibung:)
- **Inhalt:** `bb-switch`, `bb-switch-core`, `bb-switch-label` und `bb-table-card` in die Breaking-Changes-Liste von `CHANGELOG.md` und `CHANGELOG.zh-CN.md`; in der README-Tabelle „Bootstrap version“ den Hinweis ergänzen, dass `bg-*`/`border-*`/`fg-*` wieder `!important` sind und eigene Überschreibungen es ebenfalls brauchen.
- **Fertig, wenn:** Alle vier Dateien (`README.md`, `README.zh-CN.md`, `CHANGELOG.md`, `CHANGELOG.zh-CN.md`) sind abgeglichen.

### C2 – Chinesische Texte gegenlesen
- **Umfang:** Abschnitt „Bootstrap 版本“ in `README.zh-CN.md` und die Einträge in `CHANGELOG.zh-CN.md`; automatisch übersetzt, nicht von einem Muttersprachler geprüft.

### C3 – CHANGELOG-Link und Upstream-Vorbereitung
- **Warum:** Der Eintrag verlinkt auf `MaikPeters1511/BootstrapBlazor#1`; die CHANGELOG-Dateien sind seit V7.11.1 (2023-10) nicht mehr gepflegt.
- **Schritte:** Klären, ob die Dateien im Hauptrepo gepflegt werden (laut `.agents/skills/release-notes/SKILL.md` kommen Release Notes nach `ChangeLogs/{version}_en.md` und `_zh.md`; den Ordner gibt es im Repo noch nicht); Link auf das Hauptrepo ändern; Issue im Hauptrepo anlegen, weil die Projektregeln ein verknüpftes Issue verlangen; Screenshots für den PR erstellen (hell und dunkel).
- **Entscheidung nötig:** Soll die Änderung überhaupt ins Hauptrepo? Bootstrap 6 ist eine Alpha, das Hauptrepo liefert Bootstrap 5.

---

## D – Prüfung im Browser

Voraussetzung für alle: Demo-App wie in der Memory-Notiz `desktop-write-protection-workarounds` starten (`ASPNETCORE_ENVIRONMENT=Development`, Release-Build, Ordner `wwwroot/llms` und `wwwroot/images/uploader` vorher anlegen).

### D1 – Sichtprüfung aller Demo-Seiten
- **Stand:** Rund 25 von etwa 235 Seiten angesehen.
- **Idee:** Skript, das alle Routen aus dem Menü (`docs.json`/`MenusLocalizerExtensions`) im Browser öffnet, Konsolenfehler sammelt und je Seite einen Screenshot im hellen und dunklen Design ablegt; danach Auffälligkeiten durchsehen.
- **Fertig, wenn:** Eine Liste „Seite → Befund“ liegt vor, echte Fehler sind als Tasks angelegt.

### D2 – Drucken von Dialogen
- **Schritte:** In der Demo „PrintDialog“ öffnen und den Druckdialog des Systems auslösen; prüfen, dass `dialog.dialog` und `.dialog-body` im Druck erscheinen (`PrintButton.razor.js`).

### D3 – Schmale Breiten und Touch
- **Prüfen:** Navbar eingeklappt (Toggler öffnet und schliesst `navbar-collapse`), Tabelle mit `d-none sm:d-inline-block`-Beschriftungen, `sm:`-/`md:`-Raster in EditorForm und SearchForm, `DisableTouchSwiping` beim Carousel (`carousel-no-touch`) auf einem Touch-Gerät oder im Emulator.

### D4 – Tastaturbedienung
- **Prüfen:** Dropdown und Select (Pfeiltasten, Enter, Escape, Tab), Menu (Bootstrap 6 `bootstrap.Menu` hat eine eigene Tastaturlogik), Dialog (Fokus beim Öffnen und Schliessen, Escape bei gestapelten Dialogen).

### D5 – Einzelne Komponenten
- **Prüfen:** TopMenu (`IsBottom`, Placement), Logout, Spaltenliste der Tabelle, SlideButton, Schliessen des Drawers per Klick auf den Hintergrund (bisher nur Unit-Test), Tab-Kontextmenü, ContextMenu, DropdownWidget.

### D6 – Starter-Vorlage prüfen und ablegen
- **Stand:** `~/Desktop/projekte/Bs6Test` baut; nach dem Wiedereinbau des Switch nicht mehr im Browser geöffnet. Sie liegt ausserhalb des Repos und nicht unter Versionsverwaltung.
- **Schritte:** Im Browser prüfen; entscheiden, ob sie als `dotnet new`-Vorlage oder unter `docs/` oder `samples/` ins Repo kommt.

---

## E – Bekannte Verhaltensänderungen

### E1 – Dialog: `OnClosingAsync` bei Escape und Hintergrundklick – erledigt 2026-11-14
- **Ergebnis:** Escape und Klick auf den Hintergrund laufen jetzt über denselben Weg wie die Schaltfläche „Schliessen“ (`Modal.RequestCloseCallback` → `ModalDialog.RequestCloseAsync` → `OnClosingAsync`, bei Ergebnis-Dialogen `DialogResult.Close`). Gibt `OnClosingAsync` `false` zurück, bleibt der Dialog offen.
- **Zur Einordnung:** Unter Bootstrap 5 rief nur der Fall mit gestapelten Dialogen `OnClosingAsync` bei Escape und Hintergrundklick auf; ein einzelner Dialog schloss sich direkt. Das Ergebnis eines `ShowModal`-Dialogs wurde in beiden Fällen nicht gesetzt. Jetzt gilt für alle Fälle dasselbe.
- **Umsetzung:** `Modal.razor.js` fängt Escape selbst ab (`keydown` mit `preventDefault`, weil ein natives `<dialog>` sich sonst selbst schliesst und der Browser das zweite Escape nicht mehr abbrechen lässt) und fängt `hide.bs.dialog` für den Hintergrundklick ab; `forceHide` verhindert eine doppelte Rückfrage, wenn die Komponente selbst schliesst. Tests: `RequestCloseCallback_*` (ModalTest), `ShowModal_RequestClose_ResultClose` (DialogTest), `ModalScript_EscapeAndBackdrop_HandOverToComponent`.
- **Im Browser geprüft** (Starter-App, Test-Seite mit `OnClosingAsync`): mehrfaches Escape bei verweigertem Schliessen, Escape bei erlaubtem Schliessen (Scroll-Sperre wird gelöst), Escape bei zwei gestapelten Dialogen (nur der oberste schliesst), Hintergrundklick verweigert und erlaubt.
- **Offen:** Hat ein Dialog `IsKeyboard=false`, fängt das Skript Escape trotzdem ab und tut nichts (kein „Wackeln“ wie bei Bootstrap).
- **Stand:** Das native `<dialog>` schliesst bei Escape oder Klick auf den Hintergrund selbst; `OnClosingAsync` wird dabei nicht aufgerufen. Bei gestapelten Dialogen ändert sich das Verhalten gegenüber Bootstrap 5.
- **Schritte:** Test schreiben, der Escape und Hintergrundklick simuliert; in `Modal.razor.js` das `cancel`-Ereignis des Dialogs abfangen, `OnClosingAsync` aufrufen und bei Abbruch `preventDefault()`.
- **Fertig, wenn:** `IsKeyboard`, `IsBackdrop` und `OnClosingAsync` verhalten sich wie vorher.

### E2 – Dialog: `dialog-instant` geht beim Re-Render verloren
- **Stand:** Die Klasse wird von `Modal.razor.js` gesetzt, wenn der Container kein `fade` hat; ein Re-Render der Komponente kann sie entfernen.
- **Schritte:** Test mit Re-Render nach dem Öffnen; die Klasse im Razor-Markup statt per Skript setzen.

### E3 – Carousel: `AutoPlayAfterManually` nach Hover
- **Stand:** Der Modus ist nachgebaut (`data-bb-autoplay="after-manually"`, `Carousel.razor.js`). Nach einem Hover läuft das Abspielen nicht von allein weiter.
- **Schritte:** Verhalten in Bootstrap 6 (`_pauseFromInteraction`) lesen; entscheiden, ob `mouseleave` den Lauf wieder starten soll; Test ergänzen.

---

## F – Pflege

### F1 – Wechsel auf neue Bootstrap-6-Alphas und die stabile Version
- **Warum:** Klassennamen und Schnittstellen können sich bis zur stabilen Version ändern.
- **Schritte:** Beim Update `BootstrapAssetsTest` zuerst anpassen (`ExpectedVersion`), neue Assets aus dem npm-Paket (`bootstrap@next`) in `wwwroot/core/bootstrap` ersetzen, den Klassenabgleich (siehe PR #3) und die Wächter-Tests erneut laufen lassen, B1 und D1 wiederholen.
