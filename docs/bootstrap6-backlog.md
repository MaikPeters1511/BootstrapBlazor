# Backlog: Bootstrap-6-Migration

Stand: 2026-11-14, `main` = `f7ccac576` (PR #1 bis #3 im Fork gemergt).
Bootstrap-Version: `6.0.0-alpha.1`. Lokaler Teststand: 2811 von 2813 (Release), die zwei Fehlschläge sind `JSModuleExtensionsTest.LoadModule_Ok` und `JSModuleTest.JSModule_JSException` (`#if DEBUG`).

**Legende** – Priorität: P1 vor einem Upstream-PR klären, P2 bald, P3 bei Gelegenheit. Aufwand: S unter 1 h, M einige Stunden, L ein Tag oder mehr.

## Übersicht

| ID | Task | Prio | Aufwand |
| --- | --- | --- | --- |
| A1 | ~~CI im Fork zum Laufen bringen~~ erledigt 2026-11-14 | P1 | S |
| A1b | ~~CI im Fork läuft seit 2026-11-16 20:35 UTC nicht mehr: GitHub hat Workflows für den Fork gesperrt, ein Maintainer muss sie auf der Actions-Seite wieder aktivieren~~ erledigt 2026-11-17 | P1 | S |
| A2 | ~~Testsuite im Debug-Build ausführen~~ erledigt 2026-11-14 (in A1 mit CI) | P1 | S |
| A3 | ~~NuGet-Paket und JS-Konvertierung prüfen~~ erledigt 2026-11-14 | P1 | M |
| B1 | ~~Layer-Konflikte systematisch suchen~~ erledigt 2026-11-14 | P1 | L |
| B2 | ~~Farb-Utilities im Browser prüfen~~ erledigt 2026-11-14 | P1 | M |
| B3 | ~~Theme `ant.css` migrieren~~ erledigt 2026-11-15 | P2 | M |
| B4 | ~~Themes `devui.css` und `blue.css` prüfen~~ erledigt 2026-11-15 | P2 | S |
| B5 | ~~Motronic-Theme: Hover- und Active-Farben~~ erledigt 2026-11-15 | P3 | S |
| C1 | ~~Doku und CHANGELOG nachziehen~~ erledigt 2026-11-15 | P2 | S |
| C2 | Chinesische Texte gegenlesen (von mir, nicht von einem Muttersprachler; erledigt 2026-11-15) | P3 | S |
| C3 | CHANGELOG-Link und Upstream-Vorbereitung | P2 | S |
| D1 | Sichtprüfung aller Demo-Seiten (automatisierter Teil, Bildvergleich im dunklen und hellen Design sowie Popup-, Hover- und Fokus-Zustände erledigt 2026-11-15/16; Rest siehe unten) | P2 | L |
| D2 | ~~Drucken von Dialogen prüfen~~ erledigt 2026-11-15 | P3 | S |
| D3 | Schmale Breiten und Touch prüfen (Überlauf, Navbar, Tabellen-Beschriftungen, `carousel-no-touch`, EditorForm/SearchForm-Raster erledigt 2026-11-15/16; Wischen: Bootstrap 6 hat kein JavaScript dafür, siehe unten; nur der Test auf einem echten Gerät steht aus) | P2 | M |
| D4 | ~~Tastaturbedienung prüfen~~ erledigt 2026-11-15 (Dropdown, Select, Dialog, Seitenmenü, gestapelte Dialoge) | P2 | M |
| D5 | ~~Einzelne Komponenten prüfen~~ erledigt 2026-11-15 | P2 | M |
| D6 | ~~Starter-Vorlage `Bs6Test` prüfen und ablegen~~ erledigt 2026-11-15 (bleibt ausserhalb des Repos) | P3 | S |
| E1 | ~~Dialog: `OnClosingAsync` bei Escape und Hintergrundklick~~ erledigt 2026-11-14 | P2 | M |
| E2 | ~~Dialog: `dialog-instant` geht beim Re-Render verloren~~ erledigt 2026-11-15 | P3 | S |
| E3 | ~~Carousel: `AutoPlayAfterManually` nach Hover~~ erledigt 2026-11-15 | P3 | S |
| F1 | Wechsel auf neue Bootstrap-6-Alphas und die stabile Version (am 2026-11-15 geprüft: noch keine neuere Version) | P3 | M |
| G1 | ~~Breadcrumb ohne Trenner~~ erledigt 2026-11-16 | P1 | S |
| G2 | ~~Menü „alle aufklappen“ (`IsExpandAll`) wirkungslos~~ erledigt 2026-11-16 | P1 | S |
| G3 | ~~Browser-Mindestversionen von Bootstrap 6 in README und CHANGELOG nennen~~ erledigt 2026-11-16 | P1 | S |
| G4 | ~~Breakpoints in `root.scss`, `Responsive` und Demo auf die Werte von Bootstrap 6 anpassen~~ erledigt 2026-11-16 | P2 | S |
| G5 | ~~Spacer-Skala: `ms-3` & Co. haben in Bootstrap 6 andere Werte~~ Variante B (Bibliothek) erledigt 2026-11-16 | P2 | M |
| G6 | ~~Komponentenabgleich BB-Markup gegen die Bootstrap-6-Doku~~ erledigt 2026-11-17 (FloatingLabel, ToggleButton, Nav behoben) | P2 | M |
| G7 | ~~Split-Dropdown (`dropdown-toggle-split`) prüfen~~ geprüft, kein Fehler 2026-11-16 | P3 | S |
| G8 | ~~Tote Selektoren aus Bootstrap 5 aufräumen (`.was-validated`, `.has-validation`)~~ erledigt 2026-11-16 | P3 | S |
| G9 | ~~`rgba(var(--bs-*-rgb))` durch `color-mix()` ersetzen, Compat-Variablen entfernen~~ erledigt 2026-11-17 | P3 | M |
| G10 | ~~Reste von Bootstrap 5 in der Demo (`lead`, `text-body-secondary`, `--bs-border-radius-lg`, `xxl`)~~ erledigt 2026-11-16 | P3 | S |
| G11 | ~~Fokusringe über die Tokens `--bs-focus-ring*` vereinheitlichen~~ erledigt 2026-11-17 | P3 | M |
| G12 | ~~Kontrast der Motronic-Farben (weisse Schrift auf `primary`, `success`, `warning`)~~ success/warning erledigt 2026-11-17 | P3 | M |
| G12b | ~~Komponenten mit `bg-success`/`bg-warning` und Text darauf: Collapse, FileIcon, Toggle (Kontrast 1,2 bis 2,0)~~ erledigt 2026-11-17 | P3 | M |
| G13 | ~~`iframe` und `color-scheme` (Reboot setzt `color-scheme: light dark`) prüfen~~ geprüft, kein Fehler 2026-11-17 | P3 | S |
| G14 | ~~Abhängigkeit vom Global `window.bootstrap` (34 Stellen in 17 Dateien)~~ erledigt 2026-11-17 | P3 | M |
| G15 | ~~Neue Bootstrap-6-Bausteine als Chance bewerten (Combobox, Datepicker, Stepper, Avatar, …)~~ bewertet 2026-11-17, Avatar-Fehler behoben | P3 | M |
| G15a | ~~Neue Komponente `PasswordStrength` auf Basis von `bootstrap.Strength` (BB hat keine)~~ erledigt 2026-11-17 | P3 | M |
| G15b | ~~`NavOverflow` für `Nav` und Tabs prüfen (Überlauf mit „Mehr“-Menü)~~ erledigt 2026-11-17 (`Nav` ja, `Tab` bewusst nein) | P3 | M |

---

## A – Absicherung

### A1 – CI im Fork zum Laufen bringen – erledigt 2026-11-14
- **Ergebnis:** Actions waren in der API als aktiv gemeldet, es gab aber nie einen Lauf (auch nicht für PR #1 bis #3). Nach dem erneuten Setzen von `actions/permissions` (`enabled=true`, `allowed_actions=all`) starten die Workflows. Der Probe-PR #4 (inzwischen geschlossen) hat „Auto PR test(bot)“ ausgelöst: 2 Minuten, 2813 von 2813 Tests grün, auf Linux, im Debug-Build.
- **Hinweis:** Ob das erneute Setzen oder etwas anderes den Lauf ausgelöst hat, ist nicht geklärt. Die Workflows `pack`, `docker` und `publish` laufen nur auf den Branches `pack`, `release`, `hotfix`, `docker` und `publish` und brauchen Secrets, die im Fork fehlen.
- **Offen:** `auto-approve.yml` wird übersprungen (nur für den Autor `ArgoZhang`); `build.yml` (Push auf `main`, mit Codecov-Upload) ist noch nicht gelaufen.

### A1b – CI im Fork gesperrt – erledigt 2026-11-17
- **Ergebnis:** Der Eigentümer hat die Workflows auf der Actions-Seite wieder aktiviert (Hinweis „re-enable“). Danach startete beim Probe-PR #52 (`main` plus leerer Commit) sofort `Auto PR test(bot)`; `run test` lief 2 Minuten und war grün. Damit hat die CI den gesamten ohne CI gemergten Stand (#39 bis #51) bestanden. `Auto Approve PR(bot)` wird wie zuvor übersprungen (gilt nur für den Autor `ArgoZhang`). Das Wieder-Aktivieren kann GitHub bei hoher Last erneut verlangen: Bleiben Checks wieder aus, zuerst die Actions-Seite des Forks auf den Hinweis prüfen.
- **Befund:** Seit dem Push auf `main` am 2026-11-16 20:35 UTC (Lauf `Build project #36`) startet im Fork kein Workflow mehr, weder für Pull Requests noch für Pushes auf `main`. Die PRs #39 bis #49 haben deshalb nie Checks bekommen und wurden mit den lokalen Tests gemergt. Die API meldet alles aktiv (`actions/permissions`: `enabled=true`, `allowed_actions=all`; sechs Workflows `active`; githubstatus „Actions operational“), das Neusetzen der Rechte (wie in A1) und ein Probe-PR (#50, geschlossen) änderten nichts.
- **Ursache:** Auf der Seite `https://github.com/MaikPeters1511/BootstrapBlazor/actions` steht ein Hinweis von GitHub: „Workflows are not currently being run on this fork due to the scale of GitHub Actions usage. A repository maintainer can choose to re-enable these.“ GitHub hat die Workflows des Forks wegen der Gesamtlast durch Forks gestoppt. Die API zeigt das nicht an.
- **Behebung (nur mit Anmeldung als Eigentümer möglich):** Auf der Actions-Seite im Hinweis auf „re-enable“ klicken. Danach einen PR öffnen und prüfen, ob `Auto PR test(bot)` startet. Das konnte ich nicht selbst tun: Das Browser-Pane war nicht angemeldet, und ich melde mich nicht in fremde Konten an.
- **Folge:** Alle ohne CI gemergten PRs (#39 bis #49) sollten nach der Aktivierung einmal gegen den aktuellen `main` laufen (ein Push auf `main` löst `Build project` aus); lokal waren die Tests jeweils grün (2885 bis 2904 von 2887 bis 2906, nur die 2 bekannten `#if DEBUG`-Fehler).

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

### B3 – Theme `ant.css` migrieren – erledigt 2026-11-15
- **Ergebnis:** `ant.css` war nach dem mechanischen Umbenennen fast richtig: Primary und Danger (`.btn-solid.theme-primary`, `.btn-solid.theme-danger`) werden blau und rot, alle anderen Farben bleiben wie bei Bootstrap 5 weiss mit grauem Rand (das Theme kennt nur diese beiden). Einen Fehler gab es: `.btn` von Bootstrap 6 hat `min-height` (2,375 rem; klein 2 rem, gross 2,75 rem), das gegen die `height` des Themes gewinnt. Die Schaltflächen waren 38 px statt 32 px hoch (klein 32 statt 24, gross 44 statt 40). Die `.btn`-Regel setzt jetzt `min-height: 0`; gemessen: normal 32 px, klein 24 px, gross 40 px, in der Tabellen-Werkzeugleiste durchgehend 32 px. Test: `AntCss_Bootstrap6_Ok` (prüft auch, dass keine Bootstrap-5-Farbklassen mehr vorkommen).
- **Nicht geprüft:** Das Theme ist laut Name „in Arbeit“ und enthält nur Schaltflächen. Eingabefelder, Tabellen und Dialoge sehen darin wie das Standard-Design aus.

(Ursprüngliche Beschreibung:)
- **Stand:** Nur mechanisch umbenannt (`dropdown-*` → `menu-*`), nie angesehen. Das Theme setzt Bootstrap-5-Klassen (`.btn-primary`, `.btn-danger`, `.btn-background-ghost`) und Variablen.
- **Schritte:** Theme in der Demo aktivieren; Buttons auf `btn-solid theme-*` umschreiben oder durch Bootstrap-6-Tokens ersetzen (siehe `motronic.min.css` als Vorlage); Test nach dem Muster `MotronicCss_Bootstrap6Tokens_Ok` ergänzen.
- **Fertig, wenn:** Buttons, Eingabefelder und Menüs sehen im Theme richtig aus; der Test verhindert die alten Klassen.

### B4 – Themes `devui.css` und `blue.css` prüfen – erledigt 2026-11-15
- **Ergebnis:** `devui.css` hatte denselben Fehler wie `ant.css`: Das Theme macht Schaltflächen kompakt (Innenabstand 3 px 8 px), das `min-height` von Bootstrap 6 hielt sie bei 38 px. Mit `min-height: 0` in der `.btn`-Regel sind sie wieder 26 px hoch (Test: `DevuiCss_ButtonMinHeight_Ok`). `blue.css` setzt nur zwei Variablen (`--bb-primary-color`, …) und braucht nichts, `layui.css` ist leer, `fluent.css` der Demo setzt nur Variablen der Seitenleiste.
- **Nicht in diesem Repo:** Das Fluent-Theme der Demo (`_content/BootstrapBlazor.Theme.Fluent/fluent.css`) kommt aus einem NuGet-Paket und zielt auf Bootstrap 5. Ob es mit dem Bootstrap-6-Stand funktioniert, ist ungeprüft; es müsste im eigenen Repo des Pakets angepasst werden.

(Ursprüngliche Beschreibung:)
- **Schritte:** Gleiches Vorgehen wie B3. `devui.css` enthält `bb-switch`-Selektoren (PR #2), die übrigen Regeln sind ungeprüft.

### B5 – Motronic-Theme: Hover- und Active-Farben – erledigt 2026-11-15
- **Vergleich:** Bootstrap 6 leitet Hover mit `oklch(l*.95, c*1.1)` und Active mit `oklch(l*.9, c*1.15)` aus der Grundfarbe ab. Gegen die alten Motronic-Werte: Primary, Dark und Warning liegen nah dran; Secondary ist beim Hover viel heller (213 statt 181 im Rotkanal), Success, Info und Danger werden im Active-Zustand deutlich greller (z. B. Success `0,184,107` statt `58,171,109`).
- **Entscheidung:** Die Ableitung reicht nicht. `motronic.min.css` setzt Hover- und Active-Farben (Hintergrund und Rahmen) für `.btn-solid` und `.btn-outline` je Farbe wieder auf die alten Werte, ungelayert und mit höherer Spezifität (`.btn-solid.theme-<Farbe>:hover` usw.). Abgesichert durch `MotronicCss_ButtonStates_Ok` (sieben Farben).
- **Geprüft:** Die ausgelieferte CSS liefert für alle sieben Farben, solid und outline, die erwarteten Werte in `--bs-btn-hover-bg` und `--bs-btn-active-bg`.
- **Nicht geprüft:** Den echten Hover mit der Maus; das Browser-Fenster löst `:hover` nicht aus. Dunkles Design, `theme-light`, Subtle und Text-Buttons.

---

## C – Doku und Formales

### C1 – Doku und CHANGELOG nachziehen – erledigt 2026-11-15
- **Ergebnis:** Die drei READMEs (`README.md`, `README.zh-CN.md`, Paket-README) haben im Abschnitt „Bootstrap version“ jetzt eine Tabelle der umbenannten Komponentenklassen (`bb-menu`, `bb-drawer`, `bb-switch`, `bb-table-card`) und vier Hinweise für eigenes CSS: Kaskadenebenen (eigenes, ungelayertes CSS gewinnt immer), Farb-Utilities mit `!important`, `theme-light` / `theme-dark` für `Color.Light` und `Color.Dark`, `OnClosingAsync` bei Escape und Hintergrundklick. `CHANGELOG.md` und `CHANGELOG.zh-CN.md` führen diese Punkte unter „Breaking changes“ mit Links auf die PRs.
- **Zur Einordnung:** Die PR-Links zeigen auf den Fork, siehe C3.

(Ursprüngliche Beschreibung:)
- **Inhalt:** `bb-switch`, `bb-switch-core`, `bb-switch-label` und `bb-table-card` in die Breaking-Changes-Liste von `CHANGELOG.md` und `CHANGELOG.zh-CN.md`; in der README-Tabelle „Bootstrap version“ den Hinweis ergänzen, dass `bg-*`/`border-*`/`fg-*` wieder `!important` sind und eigene Überschreibungen es ebenfalls brauchen.
- **Fertig, wenn:** Alle vier Dateien (`README.md`, `README.zh-CN.md`, `CHANGELOG.md`, `CHANGELOG.zh-CN.md`) sind abgeglichen.

### C2 – Chinesische Texte gegenlesen – erledigt 2026-11-15
- **Umfang:** Abschnitt „Bootstrap 版本“ in `README.zh-CN.md`, die Einträge in `CHANGELOG.zh-CN.md` und die geänderten Zeilen in `Locales/zh-CN.json`.
- **Ergebnis:** Inhaltlich deckt sich die chinesische Fassung mit der englischen. In `zh-CN.json` wurden nur Klassennamen ersetzt (`text-danger` -> `fg-danger`, `modal-body` -> `dialog-body`, `d-md-block` -> `md:d-block`), der übrige Text ist unverändert. Geglättet habe ich vier Formulierungen: „重新带有 `!important`“ -> „仍带有 `!important`“ (README und CHANGELOG), den Satz zu den `bb-`-Klassen (README) und den Eintrag zu `Menu`/`Drawer`/`Switch`/`Table` (CHANGELOG).
- **Grenze:** Die Prüfung stammt von mir, nicht von einem Muttersprachler. Wer das Projekt im Hauptrepo einreicht (C3), sollte die Texte von jemandem mit Chinesisch als Muttersprache gegenlesen lassen.

### C3 – CHANGELOG-Link und Upstream-Vorbereitung
- **Warum:** Der Eintrag verlinkt auf `MaikPeters1511/BootstrapBlazor#1`; die CHANGELOG-Dateien sind seit V7.11.1 (2023-10) nicht mehr gepflegt.
- **Schritte:** Klären, ob die Dateien im Hauptrepo gepflegt werden (laut `.agents/skills/release-notes/SKILL.md` kommen Release Notes nach `ChangeLogs/{version}_en.md` und `_zh.md`; den Ordner gibt es im Repo noch nicht); Link auf das Hauptrepo ändern; Issue im Hauptrepo anlegen, weil die Projektregeln ein verknüpftes Issue verlangen; Screenshots für den PR erstellen (hell und dunkel).
- **Entscheidung nötig:** Soll die Änderung überhaupt ins Hauptrepo? Bootstrap 6 ist eine Alpha, das Hauptrepo liefert Bootstrap 5.

---

## D – Prüfung im Browser

Voraussetzung für alle: Demo-App wie in der Memory-Notiz `desktop-write-protection-workarounds` starten (`ASPNETCORE_ENVIRONMENT=Development`, Release-Build, Ordner `wwwroot/llms` und `wwwroot/images/uploader` vorher anlegen).

### D1 – Sichtprüfung aller Demo-Seiten – automatisierter Teil und Bildvergleich erledigt 2026-11-15
- **Vorgehen:** Ein Skript hat 262 Demo-Seiten (ohne die 53 schweren Seiten aus B1, dazu Kamera, Video und Audio) in iframes im dunklen und im hellen Design geladen und geprüft: waagerechter Überlauf der Seite, kaputte Bilder, und danach die Konsole nach JavaScript-Fehlern (`Uncaught`, `TypeError`, `ReferenceError`, `is not a function`, `bootstrap`).
- **Befund und Fix:** Drei Seiten liefen über den rechten Rand hinaus (`/datetime-picker` 183 px, `/image-cropper` 259 px, `/auto-complete` 3 px). Ursache war dieselbe: Eine Bootstrap-5-`.input-group` brach um (`flex-wrap: wrap`), die von Bootstrap 6 nicht mehr; Schaltflächen, Segmented-Steuerelemente und Beschriftungen ragten aus schmalen Containern. `compat.scss` setzt `flex-wrap: wrap` wieder (`CompatScss_InputGroup_Wraps`). Danach läuft keine der Seiten mehr über.
- **Keine JavaScript-Fehler** auf den geprüften Seiten. Die einzigen Konsolenmeldungen waren HTTP-500-Antworten der Demo-Seite „Waterfall“ (Bilder werden aus `bin/…/wwwroot/images/waterfall` gelesen, das ich in dieser Umgebung nicht hatte) und der absichtliche Fehler der Seite `/error-page`; beides hat mit Bootstrap nichts zu tun.
- **Verworfen:** Ein Kontrast-Scan (Textfarbe gegen Hintergrund) liefert zu viele Fehlalarme (Verläufe, Übergänge, absichtlich helle Farben) und taugt nicht, um Regressionen von Designentscheidungen zu trennen.
- **Bildvergleich (dunkles Design, 328 von 330 Routen, 1280 px, obere 2400 px je Seite):** Die Bootstrap-5-Fassung (`cfcc339e9`, eigener Worktree, `components.scss` mit dart-sass vorab kompiliert) und der aktuelle `main` wurden mit Headless-Chrome über das DevTools-Protokoll aufgenommen (Animationen aus, warten bis das DOM stabil ist) und je Seite verglichen. Die Prozentwerte taugen nicht zum Ranking: Der Designwechsel verschiebt fast alles um ein paar Pixel (alle Seiten 10–40 %), und feste Elemente (Kopfzeile, Hinweisband unten) schwanken zwischen zwei Läufen derselben Fassung um 6–9 %. Deshalb wurden die auffälligsten und eine Auswahl typischer Seiten als Nebeneinander-Bilder angesehen (Alert, Meet, Menu, ListView, Dropdown, Table Search, Card, Captcha, Console, Collapse, Timeline u. a.).
- **Befunde und Fixes:** (1) `.card-header` ist in Bootstrap 6 eine Flex-Spalte, BB setzte nur `align-items: center` – Titel und Symbol von Console und Captcha standen zentriert untereinander statt in einer Zeile. `Card.razor.scss` setzt `flex-direction: row` (`CardScss_Header_IsRow`). (2) Collapse: Bootstrap 6 markiert den offenen Eintrag mit `details[open]`, BB nutzt `div`s mit `.collapsed` am Button; die Regel `.accordion-header:not(.collapsed)` traf jeden Header (er trägt die Klasse nie), ein offener Eintrag war nicht hervorgehoben. Jetzt `.accordion-header:has(> .accordion-button:not(.collapsed))` mit Farbe und Hintergrund (`CollapseScss_OpenHeader_IsHighlighted`). Beide Fixes im Browser geprüft.
- **Reine Designunterschiede (kein Fehler):** etwas mehr Abstand in Beschreibungen und Karten, neue Tab-Leiste der Demo, andere Hervorhebung im oberen Menü, grössere Schrift im Collapse-Inhalt.
- **Bildvergleich im hellen Design (gleicher Ablauf, 328 Seiten):** Befunde: (1) Die Demo-Kopfzeile: Ein nacktes `.btn` hat in Bootstrap 6 einen hellen neutralen Hintergrund (Bootstrap 5: transparent). Der „Download“-Knopf und der Theme-Schalter auf der farbigen Kopfzeile setzten nur Farbe und Rahmen und waren im hellen Design kaum lesbar; `Header.razor.css` der Demo setzt jetzt `background-color: transparent` (`DemoHeaderCss_Buttons_Transparent`). (2) Der Farbverlauf des Heros der Startseite fehlte, weil `--bs-body-bg-rgb` in Bootstrap 6 entfällt; `compat.scss` definiert es jetzt wie `--bs-body-color-rgb` für hell (`255, 255, 255`) und dunkel (`33, 37, 41`) (`CompatScss_BodyBgRgb_Ok`). Die Bibliothek selbst nutzt nur `--bs-body-color-rgb`, `--bs-emphasis-color-rgb` und `--bs-primary-rgb`, die `compat.scss` schon liefert. Beide Fixes im hellen und dunklen Design geprüft.
- **Bekannt, kein Fehler:** `Color.None` (nacktes `.btn`) zeigt in Bootstrap 6 den neutralen hellen Hintergrund statt „nur Text“ wie in Bootstrap 5; das ist das Standardaussehen von Bootstrap 6. Der geöffnete Collapse-Eintrag ist im hellen Design jetzt neutral grau hervorgehoben (Bootstrap 5: blau).
- **Popup-, Hover- und Fokus-Zustände (dunkles Design, 2026-11-16):** Ein Skript hat mit echten Mausereignissen (DevTools-Protokoll, Animationen aus) 35 Zustände in beiden Fassungen aufgenommen: Dropdown (offen, Eintrag-Hover), Select, MultiSelect, SelectTable, AutoComplete, DateTimePicker, ColorPicker, Modal, Dialog, Drawer, Toast, Message, SweetAlert, PopConfirm, Tooltip, Popover, oberes und seitliches Menü, Kontextmenü, Button-Hover, Eingabe-Fokus, Tabellen-Zeile, Spaltenliste, Card, Collapse, Pagination, Tab, Switch, TreeView. Befunde: (1) TreeView: Die Pfeile zum Auf- und Zuklappen fehlten. `.node-icon` ist `visibility: hidden`, Knoten mit Kindern bekommen `.visible`; in Bootstrap 5 war das Hilfsklassen-Utility `!important`, in Bootstrap 6 nicht, die BB-Regel gewinnt. `TreeView.razor.scss` setzt `&.visible { visibility: visible; }` (`TreeViewScss_NodeIcon_VisibleOverrides`). (2) Motronic: Die festen Hover-/Active-Werte von `secondary` (`#B5B5C3`, `#aeaebc`) gelten nur für das helle Design; die Farbe ist `light-dark(#E4E6EF, #7E8299)`, im dunklen Design war ein geöffneter Dropdown-Knopf heller als der geschlossene, weisse Schrift darauf (Kontrast ca. 1,9). Jetzt `light-dark(<hell>, oklch(from var(--bs-theme-bg) …))` mit der Formel von Bootstrap 6 für das dunkle Design (Kontrast offen 3,6, geschlossen 3,8; hell unverändert) (`MotronicCss_SecondaryStates_DarkAware`).
- **Muster für künftige Funde:** Hilfsklassen von Bootstrap 5 (`.visible`, `.d-*`, `.text-*` …) waren `!important`, die von Bootstrap 6 nicht und liegen in einer Schicht; eine ungeschichtete BB-Regel für dasselbe Merkmal gewinnt immer. Nach `CompatScss_ColorUtilities_Important` ist das TreeView der nächste Fall; weitere Stellen, an denen BB ein Merkmal setzt und per Hilfsklasse überstimmt, sind möglich.
- **Hilfsklassen-Scan (2026-11-16):** Ein statischer Textscan (BB-Markup gegen die kompilierte BB-CSS) fand nur Fehlalarme und hätte den TreeView-Fall verpasst (`visible` und `node-icon` stehen in verschiedenen Zeichenketten). Brauchbar war ein Laufzeit-Scan: Für jedes Element der 309 Demo-Seiten mit einer Bootstrap-6-Hilfsklasse (899 mit stichwortartigen Eigenschaften wie `display`, `visibility`, `margin-*`, `padding-*`, `text-align`, `flex-*`) wird der berechnete Wert mit dem verglichen, den die Hilfsklasse allein auf einem Probe-Element erzeugt; weicht er ab, wird sie überstimmt (bei 1280 und 375 px; Gegenprobe: eine eingeschleuste `!important`-Regel wurde an 88 Knoten erkannt). Fehlalarme: `float` und `position` (Probe ist absolut positioniert), `*-auto`-Ränder, inaktive responsive Klassen, Hilfsklassen, die eine andere Hilfsklasse überstimmt. Befunde in der Bibliothek: (1) `Checkbox.razor.scss` setzt `.form-check { margin: 0 }` ohne Schicht; `class="ms-3"` oder `me-3` an einem `<Checkbox>` (so nutzt es auch die Demo in `/web-serial`) wurde ignoriert. Der Reset liegt jetzt in `@layer components`, damit die Hilfsklassen aus der Schicht `utilities` gewinnen (`CheckboxScss_Margin_InComponentsLayer`). (2) `Tooltip.razor.scss`: `:not(.is-tips) > span[data-bs-toggle="tooltip"] { display: inline-block }` überstimmte `d-flex` und `d-none` an einem Button mit Tooltip (der Wrapper-`span` trägt die Klassen des Buttons; sichtbar im DialButton der Demo). Ebenfalls in `@layer components` (`TooltipScss_WrapperDisplay_InComponentsLayer`). Nach beiden Fixes meldet der Scan sie nicht mehr; normale Checkboxen behalten Rand 0.
- **Nicht aus diesem Repo (kein Fix):** `div.bb-univer-sheet-backdrop` mit `d-none`, das auf `display: flex` steht (Paket `BootstrapBlazor.UniverSheet`, wie `BootstrapBlazor.Region` bei D3); in der Demo `code { margin: 0 }` in `site.css` (überstimmt `ms-1` in `/display`), `img` mit `rounded-2` (50 %) und die Seitenleiste mit `md:d-block`.
- **Entscheidung zu `!important` (2026-11-16, PR #29):** Spacing-, `display`- und Sichtbarkeits-Hilfsklassen (`m*-*`, `p*-*`, `d-*`, `visible`, `invisible`, alle Breakpoints, 1240 Regeln) sind wieder `!important`, wie in Bootstrap 5 und wie die Farb-Hilfsklassen. Erzeugt von `scripts/gen-important-utilities.py` aus `bootstrap.css` nach `wwwroot/scss/_utilities-important.scss` (eingebunden in `components.scss`, ohne Schicht); nach einem Update von Bootstrap das Skript erneut ausführen, `UtilitiesImportant_CoverBootstrapUtilities` schlägt sonst fehl. Kosten: Bundle 723 KB -> 777 KB (+54 KB, +7,5 %), gzip 102 KB -> 108 KB (+5,9 KB, +5,8 %). Gewählt aus vier Varianten (nur Abstand/display/Sichtbarkeit, Layout-Satz +69 KB, alle Gruppen +79 KB, nicht umsetzen).
- **Wirkung (Geometrie-Scan, 309 Seiten, 1280 und 375 px, vorher/nachher):** Es ändern sich genau vier Dinge, alle erwünscht: `code.ms-1` in `/display` bekommt seinen Rand (die Demo setzt `code { margin: 0 }`), `div.bb-univer-sheet-backdrop` mit `d-none` ist jetzt wirklich versteckt (externes Paket `BootstrapBlazor.UniverSheet`), die Seitenleiste der Demo (`md:d-block`) wird `block` statt `flex` (sieht unverändert aus). Auf 375 px bleiben nur die ersten beiden. Keine Seite änderte sich sonst; Aufnahmen vorher/nachher von Card, Dropdown und TreeView sehen gleich aus.
- **Folge für eigenes CSS:** Wer eine dieser Hilfsklassen in eigenem CSS überschreibt, braucht jetzt `!important` (README, Paket-README und beide CHANGELOGs nennen das). Die übrigen Gruppen (Grösse, Rundung, Schrift, Schatten, Flex, Gap) sind nicht umgesetzt; bei einem Fund lässt sich `GROUPS` im Skript erweitern.
- **Reine Designunterschiede (kein Fehler):** unscharfer Hintergrund hinter Modal, Dialog und SweetAlert; grauer quadratischer Schliessen-Knopf in Modal und Toast; dunklere Hover-Hervorhebung in Dropdown- und Select-Listen (`--bs-menu-item-hover-bg` = `bg-1`); Hervorhebung im Tab und in Menüs.
- **Popup-/Hover-Zustände im hellen Design und Tastaturfokus (2026-11-16):** Dieselben 35 Zustände wie im dunklen Design (jetzt 47 mit Tastaturfokus: Tab-Taste per echtem Tastaturereignis auf Button, Outline-Button, Eingabe, Checkbox, Radio, Switch, Dropdown, Select, Tab, Pagination, Menü, Modal-Auslöser; Fokus im dunklen Design) in beiden Fassungen aufgenommen. Hell: keine neuen Abweichungen ausser Design (neutrale Hervorhebung im Collapse, unscharfer Dialog-Hintergrund, grauer Schliessen-Knopf); die TreeView-Pfeile sind da. Fokus: Bootstrap 6 zeichnet den Fokusring an Button, Eingabe, Checkbox und Select sichtbar, kein verlorener Indikator. Button-Kontrast bei echtem Hover (CDP, beide Designs): alle Farben im Hover-Zustand wie von Motronic vorgegeben (`#0095e8`, `#47be7d`, …); der Wert im gedrückten Zustand ist im Test gleich dem Hover-Wert (Regelreihenfolge im Bootstrap-6-Kern, nicht weiter verfolgt). Niedrige Kontraste (weisse Schrift auf `primary` 2,9, `success` 2,0, `warning` 1,6) sind die Metronic-Farben und waren in der Bootstrap-5-Fassung gleich.
- **Befund 3 (Cascader):** `/cascader` zeigte keine Auswahlfelder. Das Wurzelelement hatte die Klasse `menu` (`"select cascade menu dropdown"` in `Cascader.razor.cs`, schon im Bootstrap-5-Code vorhanden); in Bootstrap 6 ist `.menu` das Popup-Menü (`display: none`, solange es kein `.show` hat), der ganze Cascader war unsichtbar (Grösse 0). Die Klasse ist entfernt (`CascaderTest.RootElement_IsNotMenu`); das innere Popup bleibt das `.menu`. Im Browser: Feld 200 × 38 px, Popup mit drei Einträgen, Auswahl setzt den Wert. Ein Scan aller 309 Seiten nach weiteren `.menu`-Elementen: alle übrigen sind Popups in `dropdown`-Containern, keine weiteren Wurzelelemente.
- **Offen:** Bereich unterhalb der ersten 2400 px; zweite Ebene des Cascaders (Kinder) nicht geprüft (in der Demo hat „item1“ keine anklickbare Unterebene); Aufnahmen und Skripte liegen im Scratchpad, nicht im Repo.

(Ursprüngliche Beschreibung:)
- **Stand:** Rund 25 von etwa 235 Seiten angesehen.
- **Idee:** Skript, das alle Routen aus dem Menü (`docs.json`/`MenusLocalizerExtensions`) im Browser öffnet, Konsolenfehler sammelt und je Seite einen Screenshot im hellen und dunklen Design ablegt; danach Auffälligkeiten durchsehen.
- **Fertig, wenn:** Eine Liste „Seite → Befund“ liegt vor, echte Fehler sind als Tasks angelegt.

### D2 – Drucken von Dialogen – erledigt 2026-11-15
- **Vorgehen:** In `/print` (Demo „PrintDialog“) den Dialog geöffnet, `window.print` abgefangen und im Moment des Druckens den Zustand gemessen.
- **Befund:** Kein Fehler. `PrintButton.razor.js` findet `dialog.dialog` und `.dialog-body`; das native `<dialog>` ist beim Druck ausgeblendet (`d-none` wirkt, `display: none`), der Druckinhalt `.bb-printview` hängt direkt am `<body>` und enthält den Inhalt des Dialogs, `bb-printview-open` ist gesetzt. Nach dem Druck sind Klasse und Hilfselement wieder entfernt, der Dialog ist sichtbar.
- **Nicht geprüft:** Der echte Druckdialog des Systems und die Seitenvorschau. Die `@media print`-Regeln (`.bb-printview-open > *:not(.bb-printview) { display: none !important }` in `Modal.razor.scss`) habe ich nur gelesen, nicht im Druck gesehen.

### D3 – Schmale Breiten und Touch
- **Erledigt (375 px, Vergleich mit Bootstrap 5):** Alert war ein Flex-Block (`display: block` in `Alert.razor.scss`, `btn-close` rechts) – behob den Überlauf auf `/table`, `/table/column`, `/tree-view`, `/anchor-link`, `/tab`, `/datetime-picker`; `.form-control` bricht um (`compat.scss`) – behob `/slide-button`.
- **Bleibt:** `/select-city` (Paket `BootstrapBlazor.Region` nutzt noch `dropdown-menu`, nicht in diesem Repo), `/circle` und `/query-builder` (4–6 px), schmalere Schaltflächen in `/button`, `/sweet-alert` (Demo-Raster, ähnlich wie in Bootstrap 5).
- **Geprüft 2026-11-15, ohne Befund (Browser-Pane, 375 px und 800 px):** Navbar-Toggler (`/navbar`: öffnet auf 245 px Höhe, schliesst mit `display: none`); Tabellen-Toolbar (`/table/toolbar`: bei 375 px alle `d-none sm:d-inline-block`-Beschriftungen weg, Zahnrad `sm:d-none` sichtbar und sein Menü geht auf, kein Überlauf; bei 800 px Beschriftungen sichtbar, Zahnrad weg); `carousel-no-touch` (per Skript gesetzt: `overflow-x: hidden`, `touch-action: pan-y`, die Pfeile schalten weiter). Die `sm:`-Klassen von Bootstrap 6 haben denselben Breakpoint (576 px) wie die der alten Klassen.
- **Geprüft 2026-11-16, ohne Befund (Browser-Pane, 375/800/1280 px):** `sm:`-/`md:`-Raster in EditorForm (`/editor-form`) und SearchForm (`/table/search`). Die Klassen `sm:col-N` und `md:col-N` gibt es in Bootstrap 6 (Media-Query ab 576 px bzw. 768 px, wie in Bootstrap 5). Bei 375 px steht jedes Element in voller Breite (kein Überlauf), ab 576 px zu 50 %, `ItemsPerRow="3"` ergibt bei 1280 px 33 %. Beachten: Die Breakpoints richten sich nach dem Viewport, nicht nach der Breite des Formulars, daher sind die Felder bei 800 px neben dem Seitenmenü der Demo (Formular ca. 350 px) schmal, genau wie in Bootstrap 5.
- **Wischen, geklärt 2026-11-17 (Quelltext von Bootstrap 6 und Browser-Pane mit Touch-Emulation 375 px, `maxTouchPoints` 5):** Das Carousel von Bootstrap 6 hat kein JavaScript fürs Wischen mehr. `.carousel-inner` ist ein Scroll-Container (`overflow-x: auto`, `scroll-snap-type: x mandatory`, `scroll-snap-align: start` an den Einträgen), das Wischen erledigt der Browser. Das `Swipe`-Hilfsobjekt (Zeiger vom Typ `touch` oder `pen`, Schwelle 40 px) nutzt in Bootstrap nur der Drawer; BB benutzt dafür den eigenen `bb-drawer`, also läuft diese Wischgeste hier nicht. Das BB-Stück ist `carousel-no-touch` (`overflow-x: hidden`, `touch-action: pan-y`), das am 2026-11-15 geprüft wurde. Mit Touch-Emulation: In 3 von 6 Carousels der Demo stehen die Einträge in einem Scroll-Container mit `overflow-x: auto`, ein Scrollen per Skript bewegte den Inhalt (280 px); ob Bootstrap dabei das Attribut `active` und die Anzeige nachzieht, kam in der Vorschau nicht zuverlässig an (der Bereich läuft gedrosselt, `scrollend` kam nicht). Ein echter Fingerwisch lässt sich im Pane nicht erzeugen; das bleibt ein Test auf einem Gerät, ist aber kein Code von BB.
- **Prüfen:** Navbar eingeklappt (Toggler öffnet und schliesst `navbar-collapse`), Tabelle mit `d-none sm:d-inline-block`-Beschriftungen, `sm:`-/`md:`-Raster in EditorForm und SearchForm, `DisableTouchSwiping` beim Carousel (`carousel-no-touch`) auf einem Touch-Gerät oder im Emulator.

### D4 – Tastaturbedienung
- **Erledigt:** Select: Enter bestätigte den Eintrag, schloss das Menü aber nicht mehr (`base-select.js` ruft jetzt `popover.hide()`); Dialog: der Fokus blieb beim Öffnen auf `body` (`Modal.razor.js` fokussiert den Dialog bei `shown.bs.dialog`). Ohne Befund: Dropdown öffnen mit Enter, Escape (Fokus zurück auf den Auslöser), Select Escape und Pfeiltasten, Dialog Tab-Falle und Fokus-Rückgabe.
- **Unverändert gegenüber Bootstrap 5:** Dropdown-Einträge sind `<div class="menu-item">` ohne `tabindex`, die Pfeiltasten des Bootstrap-6-Menus fokussieren sie nicht und Enter wählt nichts aus; Select und Dropdown lassen sich nicht per Tastatur öffnen (Bootstrap ignoriert Pfeiltasten in Eingabefeldern). Verbesserung möglich: Einträge als `<button class="menu-item">` rendern.
- **Geprüft 2026-11-15, ohne Befund (echte Tastendrücke im Browser-Pane):** Seitenmenü (`/menu`): es nutzt `bootstrap.Collapse` und `NavLink`, nicht `bootstrap.Menu`; Einträge haben `href` und `tabindex` 0, Enter klappt ein Untermenü auf (`aria-expanded=true`) und wieder zu, Tab überspringt die Kinder eines geschlossenen Untermenüs. Escape bei gestapelten Dialogen (`/dialog`, „MultipleDialog“): der erste Escape schliesst nur den obersten Dialog, der Fokus bleibt im unteren, der zweite schliesst auch diesen.
- **Beobachtung:** Nach dem Schliessen des letzten per `DialogService` geöffneten Dialogs steht der Fokus auf `body`, nicht auf der Schaltfläche, die ihn geöffnet hat. Nicht mit Bootstrap 5 verglichen, daher keine Änderung.
- **Prüfen:** Dropdown und Select (Pfeiltasten, Enter, Escape, Tab), Menu (Bootstrap 6 `bootstrap.Menu` hat eine eigene Tastaturlogik), Dialog (Fokus beim Öffnen und Schliessen, Escape bei gestapelten Dialogen).

### D5 – Einzelne Komponenten – erledigt 2026-11-15
- **Geprüft im Browser (dunkles Design):** ContextMenu, DropdownWidget, SlideButton, Logout, Drawer (Klick auf den Hintergrund bei `IsBackdrop="true"`), Tab-Kontextmenü, TopMenu (oben und mit `IsBottom`), Spaltenliste der Tabelle.
- **Fehler 1 (Tabelle, Spaltenliste):** Das Popover öffnete sich, der Inhalt blieb unsichtbar. Bootstrap 6 schliesst `.menu` mit `opacity: 0` und `scale(.95)`, bis das Menü selbst `.show` trägt; im Popover trägt nur das Elternelement `.show`. `Select.razor.scss` setzt jetzt für `.show > .menu` auch `opacity: 1` und `transform: none` (`SelectScss_ShownMenu_IsVisible`). Betrifft jedes Popover-Menü ohne eigenes `.show`.
- **Fehler 2 (TopMenu):** `.nav-link` ist in Bootstrap 6 eine Flexbox mit `justify-content: center`; Einträge mit Pfeil (`SubMenu1`, `SubMenu2`) standen mittig bis rechts statt links. `Menu.razor.scss` setzt `justify-content: flex-start` in den Aufklapplisten (`MenuScss_NavLink_AlignsStart`).
- **Kein Befund:** ContextMenu, DropdownWidget, Logout, Drawer, Tab-Kontextmenü, `IsBottom` (Liste öffnet nach oben).
- **Bekannt, aber kein Bootstrap-6-Fehler:** Die Liste des SlideButton hat im dunklen Design einen weissen Hintergrund (`$bb-slide-list-bg: #fff`) mit hellem Text, schlecht lesbar. Das war in der Bootstrap-5-Basis (`cfcc339e9`) genauso. Idee: `var(--bs-body-bg)` verwenden.
- **Nicht geprüft:** Placement-Varianten von SlideButton und TopMenu im Einzelnen, Hell-Design.

### D6 – Starter-Vorlage prüfen – erledigt 2026-11-15
- **Geprüft im Browser (`~/Desktop/projekte/Bs6Test`):** Startseite (Buttons, Dropdown, Select, Dialog, Toast, Checkbox, Switch, Texteingabe, Alert), Counter, Weather; keine Überläufe, keine neuen Konsolenfehler.
- **Entscheidung:** Die Vorlage kommt nicht ins Repo und wird nicht als `dotnet new`-Vorlage abgelegt; sie bleibt ein lokales Prüfprojekt ausserhalb der Versionsverwaltung. Die nötigen Schritte (Einbindung von CSS und Skript, Klassennamen) stehen im README-Abschnitt „Bootstrap version“.
- **Aufgeräumt:** Die temporäre Seite `Closing.razor` (Test für `OnClosingAsync`, E1) ist aus der Vorlage entfernt; das Verhalten ist durch Unit-Tests abgesichert.

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

### E2 – Dialog: `dialog-instant` geht beim Re-Render verloren – erledigt 2026-11-15
- **Ergebnis:** `ModalDialog` setzt `dialog-instant` jetzt im Markup (`ClassName`), wenn `Modal` ohne Fade läuft (`Modal.UseFade`, vorher `Options.GetIsFadeValue`). `Modal.razor.js` setzt die Klasse beim Öffnen nicht mehr; nur beim Aufräumen (`dispose`) bleibt sie, damit der Dialog ohne Animation schliesst. Abgesichert durch `DialogInstant_Ok` (mit und ohne Fade).
- **Geprüft im Browser:** Mit `ModalSettings.IsFade = false` im lokalen Starter trägt das `<dialog>` die Klasse, nach einem Re-Render der Seite (Toast) bleibt sie, `animation-name` ist `none`.
- **Nicht geprüft:** Ein Re-Render, das die Klassenliste selbst ändert (z. B. Maximieren), nur im Unit-Test über `Size`.

### E3 – Carousel: `AutoPlayAfterManually` nach Hover – erledigt 2026-11-15
- **Ergebnis:** Bootstrap 6 setzt `_playing` bei `data-bs-autoplay="false"` auf `false`; sein `mouseleave`-Handler (`_maybeEnableCycle`) startet deshalb nie neu. `Carousel.razor.js` merkt sich jetzt, dass der erste manuelle Wechsel das Abspielen gestartet hat (`carousel.started`), und ruft bei `mouseleave` selbst `cycle()` auf (nur bei `HoverPause`). Wächter-Test `CarouselJs_AfterManually_ResumesAfterHover` (war rot). Im Browser (Demo mit `PlayMode=AutoPlayAfterManually`, Hover per `mouseover`/`mouseout`): während Hover 7 s Stillstand, nach dem Verlassen Wechsel nach ca. 5 s.
- **Nicht geprüft:** echter Mauszeiger; Touch-Wischen (siehe D3).
- **Ursprünglicher Stand:** Der Modus ist nachgebaut (`data-bb-autoplay="after-manually"`, `Carousel.razor.js`). Nach einem Hover läuft das Abspielen nicht von allein weiter.
- **Schritte:** Verhalten in Bootstrap 6 (`_pauseFromInteraction`) lesen; entscheiden, ob `mouseleave` den Lauf wieder starten soll; Test ergänzen.

---

## F – Pflege

### F1 – Wechsel auf neue Bootstrap-6-Alphas und die stabile Version
- **Stand 2026-11-15:** Auf npm gibt es nur `6.0.0-alpha.1` (veröffentlicht 2026-10-08); der Tag `next` zeigt darauf, `latest` ist `5.3.9`. Es gibt nichts zu aktualisieren. Punkt bleibt offen, bis eine neue Version erscheint (dann mit `npm view bootstrap dist-tags` prüfen).
- **Warum:** Klassennamen und Schnittstellen können sich bis zur stabilen Version ändern.
- **Schritte:** Beim Update `BootstrapAssetsTest` zuerst anpassen (`ExpectedVersion`), neue Assets aus dem npm-Paket (`bootstrap@next`) in `wwwroot/core/bootstrap` ersetzen, den Klassenabgleich (siehe PR #3) und die Wächter-Tests erneut laufen lassen, B1 und D1 wiederholen.


---

## G – Offene Punkte aus der Bootstrap-6-Dokumentation

Quelle: <https://getbootstrap.com/docs/6.0/>, gelesen am 2026-11-16: der Migrationsleitfaden (`guides/migration`), der Migrations-Leitfaden für Agenten (`skills/bootstrap-v5-v6-migration/SKILL.md` im Bootstrap-Repository), „Browsers & devices“, „JavaScript“, „Transitions“, „Accessibility“, „Color modes“ und die Gliederung aller rund 200 Seiten der Version 6.0 (Sitemap). Die Seiten werden serverseitig geliefert, mit `curl` abrufbar. Jeder Punkt wurde gegen unseren Code geprüft; was im Browser oder am Quelltext bestätigt ist, steht als „bestätigt“.

**Bestätigte Fehler**

### G1 – Breadcrumb ohne Trenner – erledigt 2026-11-16
- **Befund:** `/breadcrumb` zeigt „HomeLibraryData“ ohne Trenner und Abstand. `Breadcrumb.razor` rendert das Markup von Bootstrap 5 (`li.breadcrumb-item` mit blossem `<a>`), Bootstrap 6 zeichnet den Trenner nicht mehr per `::before` am Eintrag (`::before` ist `none`), sondern über ein eigenes Element. Der Klassen-Abgleich (G6) und der Bildvergleich (D1) haben das nicht gefunden, weil `.breadcrumb-item` in BB-CSS vorkommt.
- **Soll (laut Migrationsleitfaden):** Links bekommen `.breadcrumb-link` (der aktive Eintrag zusätzlich `.active`), zwischen den Einträgen steht ein leeres `<li class="breadcrumb-divider"></li>` (Chevron per CSS-Maske); der Standardabstand nach unten (`$spacer`) entfällt.
- **Ergebnis:** `Breadcrumb.razor` rendert zwischen den Einträgen `<li class="breadcrumb-divider">`, Links und Texte tragen `.breadcrumb-link` (Eintrag ohne `Url` als `<span>`), ein Eintrag mit `CssClass="active"` bekommt `aria-current="page"` am `li` und `.active` am Link. Die Demo markiert den letzten Eintrag jetzt als aktiv. Neue Tests in `BreadcrumbTest` (Trenner, Link-Klassen, einzelner Eintrag, leere Liste; zwei davon waren vorher rot). Im Browser hell und dunkel: „Home › Library › Data“ mit Chevrons, der aktuelle Eintrag kräftiger. Der Standardabstand nach unten entfällt wie in Bootstrap 6, die Demo braucht ihn nicht.

### G2 – Menü „alle aufklappen“ wirkungslos – erledigt 2026-11-16
- **Befund:** Der Schalter „Expand all“ der Demo-Seitenleiste (`IsExpandAll` am `Menu`) setzt `data-bb-expand="true"`, aber nur 1 von 18 Untermenüs öffnet sich. Ursache in `Menu.razor.js` (`update()`): Ohne vorhandene Collapse-Instanz wird `new bootstrap.Collapse(target, { toggle: true })` aufgerufen. Bootstrap 6 hat die Option `toggle` entfernt, der Konstruktor behält den im Markup deklarierten Zustand und öffnet nichts. Beim Ausschalten werden die Instanzen entsorgt, beim erneuten Einschalten passiert dasselbe. Gegenprobe: Ein manueller Aufruf von `update(id)` bei vorhandenen Instanzen (`collapse.show()`) öffnet alle 18.
- **Soll:** Laut Leitfaden `new bootstrap.Collapse(el).show()` statt `toggle: true`; zusätzlich ist `.collapsed` am Auslöser kein Zustandssignal mehr (`aria-expanded` verwenden).
- **Ergebnis:** `Menu.razor.js` öffnet ohne vorhandene Instanz jetzt mit `new bootstrap.Collapse(target).show()`; die private Methode `_isShown()` ist durch die Klasse `.show` am Ziel ersetzt (`update()` und `disposeCollapse()`). Wächter-Test `MenuJs_ExpandAll_ShowsCollapse` (war rot). Im Browser mit echten Klicks: Schalter an -> 18 von 18 Untermenüs offen (vorher 1), aus -> 0, wieder an -> 18; im Akkordeon-Modus bleibt nach dem Öffnen dreier Einträge nur einer offen, „Expand all“ ist dann deaktiviert.

### G3 – Browser-Mindestversionen nennen – erledigt 2026-11-16
- **Befund:** README, Paket-README und CHANGELOG sagen nichts zu Browsern. Bootstrap 6 setzt Chrome und Edge ab 130, Firefox ab 132, Safari und iOS ab 18 voraus, ohne Fallbacks, Präfixe oder Polyfills (`light-dark()`, `oklch()`, `color-mix()`, `:has()`, Container Queries, natives `<dialog>`, `content-visibility`); in älteren Browsern werden Farben nicht aufgelöst. Dazu: Collapse und Menü-Untermenüs animieren die Grösse mit `interpolate-size`, Browser ohne diese Funktion (Firefox ist genannt) öffnen und schliessen sofort.
- **Ergebnis:** `README.md`, `src/BootstrapBlazor/readme.md` (Paket-README) und `README.zh-CN.md` haben einen Abschnitt „Browser support“ am Ende von „Bootstrap version“ (Tabelle Chrome/Edge 130, Firefox 132, Safari 18, keine Fallbacks, Hinweis auf `interpolate-size` und Firefox); `CHANGELOG.md` und `CHANGELOG.zh-CN.md` nennen es als erste Zeile unter „Breaking changes“. Offen und bewusst nicht gemacht: ein Hinweis auf der Demo-Startseite oder eine Laufzeit-Warnung für ältere Browser.

**Anpassungen und Entscheidungen**

### G4 – Breakpoints angleichen – erledigt 2026-11-16
- **Befund:** `wwwroot/scss/root.scss` (Zeilen 192–206) meldet BB per `body:before` die Schwellen 768/992/1200/1400 px (`medium`, `large`, `extraLarge`, `extraExtraLarge`); die Komponente `Responsive` (`OnBreakPointChanged`, Enum `BreakPoint`) liest sie. Bootstrap 6 hat `lg` auf 1024, `xl` auf 1280 und `2xl` (vorher `xxl`) auf 1536 px angehoben (`md` 768 und `sm` 576 unverändert; Container 960/1200/1440). Zwischen 992 und 1023 px meldet BB „large“, die Klassen `lg:*` greifen noch nicht. Die Demo-Seite `/breakpoint` nennt 992/1200/1400 und `xxl`.
- **Ergebnis:** `root.scss` meldet jetzt `large` ab 1024 px, `extraLarge` ab 1280 px und `extraExtraLarge` ab 1536 px (`small` 576, `medium` 768 und `extraSmall` 375 unverändert). Doku-Kommentare von `BreakPoint`, `Size` und `FullScreenSize` angepasst (dort hiess es `xxl`, jetzt `2xl`), die Demo-Seite `/breakpoint` zeigt 1024/1280/1536 und `2xl`, die Spaltenüberschrift „Klassen-Suffix“ heisst „Klassen-Präfix“. Wächter-Tests `RootScss_Breakpoints_FollowBootstrap6` und `BreakpointsDemo_ShowsBootstrap6Thresholds` (beide waren rot); Eintrag unter „Breaking changes“ in beiden CHANGELOGs. Im Browser bei 1000, 1030, 1270, 1290, 1530 und 1540 px: `medium`, `large`, `large`, `extraLarge`, `extraLarge`, `extraExtraLarge`, jeweils passend zu `lg:`, `xl:` und `2xl:`.
- **Bewusst nicht geändert:** Die Breiten der eigenen Dialoggrösse `dialog-xxl` in `Modal.razor.scss` (Stufen bei 992, 1200, 1400 und 1600 px mit 800/1140/1280/1440 px). Das sind eigene Werte von BB, keine Bootstrap-Breakpoints; bei Bedarf als eigener Punkt prüfen.

### G5 – Spacer-Skala – Variante B erledigt 2026-11-16
- **Befund:** Die Abstandsklassen haben in Bootstrap 6 andere Werte: `3` ist `0.5rem` (vorher `1rem`), `4` `0.75rem` (vorher `1.5rem`), `5` `1rem` (vorher `3rem`), `2` `0.375rem` (vorher `0.5rem`); neu sind die Stufen 6 bis 12. Die offizielle Abbildung für dieselbe Optik: `2` -> `3`, `3` -> `5`, `4` -> `7`, `5` -> `12` (`1` bleibt). Das gilt gleichermassen für `m*`, `p*`, `gap-*`, `g-*`. Unser Markup behält die Bootstrap-5-Zahlen und rendert dadurch deutlich engere Abstände: Bibliothek 22 Stellen in 17 Dateien, Demo etwa 996 Stellen in 269 Dateien (zusätzlich `rounded-2` in drei Demo-Dateien; die Rundungsskala `rounded-1`..`rounded-5` wurde ebenfalls neu gemappt).
- **Optionen:** (A) lassen, engere Optik akzeptieren; (B) nur die Bibliothek umstellen, damit Komponenten ihre bisherige Optik behalten (22 Stellen, überschaubar); (C) Bibliothek und Demo per Skript umstellen (grosser, rein mechanischer Diff). Anwendungscode von Nutzern bleibt in jedem Fall ihre Sache (in CHANGELOG/README als Breaking Change nennen, mit der offiziellen Abbildung).
- **Empfehlung:** B, die Demo dann bereichsweise bei Bedarf; vorher mit einem Vergleich der Bibliotheks-Seiten (vor/nach) prüfen, ob die Abstände stören.
- **Ergebnis (B):** Entschieden: nur die Bibliothek. 15 Dateien umgestellt (`DateTimeRange`, `QueryBuilder`, `ThemeProvider`, `EditorForm`, `SearchForm`, `SkeletonTable`, `SkeletonEditor`, `ClockPicker`, `Toast`, `IconDialog`, `Row.razor.js`, `ExportPdfButton`, `Table`, `SelectTable.razor.scss` (Selektor `.g-5`), `SweetAlertBody`) mit der offiziellen Abbildung 2->3, 3->5, 4->7, 5->12; Wächter-Test `Components_UseBootstrap6SpacerScale` (16 Fälle, alle waren rot), Eintrag unter „Breaking changes“ in beiden CHANGELOGs. Die Demo bleibt bewusst auf den alten Zahlen (G10 für `rounded-2`); bei Bedarf bereichsweise.

### G6 – Komponentenabgleich BB-Markup gegen Bootstrap-6-Markup – erledigt 2026-11-17
- **Anlass:** Der Breadcrumb (G1) zeigt, dass weder der Klassenabgleich (Klassen ohne jede CSS-Regel: nur drei harmlose Treffer) noch die Bildvergleiche aus D1 Fälle finden, in denen Bootstrap 6 Dekoration (Trenner, Icons, Pseudo-Elemente) auf neues Markup umgestellt hat, das BB nicht rendert.
- **Geprüft, ohne Befund:** Badge, Progress, Pagination, Alert, Card, Carousel, Collapse (nach D1), Slider (`.form-range` direkt am `<input>`; Bootstrap 6 erwartet einen Wrapper, die Darstellung ist aber in Ordnung), Dropdown-Richtungen (`dropup`, `dropstart`, `dropend`, Center; BB setzt `data-bs-placement`).
- **Zu prüfen (Markup in Bootstrap 6 umgebaut):** Spinner, Tag, Nav/Tab/RibbonTab, ListGroup, Skeleton (Placeholder `glow` -> `wave`/`pulse`), ButtonGroup und Split-Dropdown (G7), Table-Varianten (`table-*` -> `theme-*`), FloatingLabel, InputGroup, Toast (Klassen `showing`/`hide` entfallen), Tooltip und Popover (Pfeil-Klassen, `.fade` wird nicht mehr gesetzt), Close-Button (kein Kind-`<svg>`, kein `btn-close-white`), Toggle-Button (Bootstrap setzt `aria-pressed` nur beim Laden der Seite, `ToggleButton.razor` rendert es nicht).
- **Vorgehen:** Je Komponente die Seite der Bootstrap-6-Doku neben die BB-Demo legen (Markup und Bild), Befunde als eigene Punkte anlegen.
- **Stand 2026-11-17 (erste Runde):**
  - **Befund und Fix, FloatingLabel:** Bootstrap 6 hebt das Label mit `label:has(~ .form-control:focus)` und Verwandten an, das Label muss also *vor* dem Eingabefeld stehen (Bootstrap 5: dahinter, Selektor `~ label`). BB rendert `<input>` dann `<label>`: das Label hob sich nie, der Platzhalter überdeckte es (sichtbar in der Demo `/floating-label`, Beispiel „DisplayText“). `FloatingLabel.razor` rendert jetzt Label zuerst, `FloatingLabel.razor.scss` (Variante `is-group`) nutzt `label:has(~ …)`; Wächter-Test `FloatingLabel_LabelBeforeInput` (war rot); im Browser hebt sich das Label mit Wert und bei Fokus.
  - **Geprüft, ohne Befund:** Spinner (BB hat eigene Grössenklassen `spinner-border-xs/sm/lg/…` mit eigenem Stil, Farbe über `fg-*`), Toast (Einblenden, Fortschrittsbalken, automatisches Ausblenden nach 4 s, Schliessen-Knopf), Close-Button (`btn-close` leer, wie in der Doku), Skeleton (eigene Klassen, nutzt `placeholder` von Bootstrap nicht), Nav (Pills sehen jetzt aus wie in Bootstrap 6: Container mit Rundung).
  - **Klassenabgleich im gerenderten DOM** (Seiten Tab, ListGroup, Toast, Tooltip, Skeleton): keine weiteren Klassen ohne Regel ausser JS-Haken und Inhalten fremder Skripte (`d-md-flex`, `text-bg-warning` stammen nicht aus unseren Quellen).
  - **Zweite Runde (2026-11-17):**
    - **Befund und Fix, ToggleButton:** Bootstrap 6 setzt `aria-pressed` nur beim Initialisieren; `ToggleButton.razor` rendert es jetzt aus `IsActive` (`true`/`false`). Wächter-Test `ToggleButton_AriaPressed` (war rot); im Browser (`/button`): vor dem Klick `false`, danach Klasse `active` und `aria-pressed="true"`.
    - **Befund und Fix, Nav:** `IsPills` zusammen mit `IsVertical` setzt zusätzlich `nav-pills-vertical` (in Bootstrap 6 ist `.nav-pills` ein Inline-Container, die vertikale Variante streckt die Links auf volle Breite). Wächter-Test `IsPillsVertical_UsesPillsVerticalClass` (war rot).
    - **Geprüft, ohne Befund:** ListGroup (Rand, Abstände, Tokens `--bs-list-group-*` sind in Bootstrap 6 vorhanden), Tag (`btn-close`, `theme-*`), InputGroup (23 Gruppen im Browser: Rundungen aussen, Mitte eckig; Wrapper wie `.select` tragen den Radius im Inneren; die gedrängte Checkbox-Zeile in der Demo entsteht durch Platzhaltertexte in schmalen Spalten), Tooltip (Klick-Auslöser, Pfeil, Farben), Popover (bei echtem Hover Deckkraft 1, Kopf, Körper, Pfeil), Table (`table-sm`, `table-bordered`, `table-striped`, `table-hover` existieren in Bootstrap 6; keine `table-*`-Farbvarianten im Einsatz), Tab und RibbonTab (eigenes Markup und eigene Stile, Demo sieht richtig aus).
    - **G6 damit abgeschlossen.** Weitere Funde würden als eigene Punkte angelegt.

**Aufräumen und Pflege**

### G7 – Split-Dropdown – geprüft, kein Fehler 2026-11-16
- `Dropdown.razor.cs` setzt `dropdown-toggle-split` und `Dropdown.razor.scss` (Zeilen 76–81) stylt es selbst; Bootstrap 6 hat die Klasse entfernt und behandelt den Rundungsrand von Split-Menüs über `:has(+ .menu)`. Prüfen, ob Radius, Rand und Pfeil doppelt oder gar nicht greifen (Demo `/dropdown`, Abschnitt „Outline style“ mit Split).
- **Ergebnis:** Im Browser (Demo `/dropdown`, Outline und gefüllte Varianten) greifen die Regeln von Bootstrap 6 richtig: Der Hauptknopf hat rechts Radius 0 (`.btn-group > [class*=btn-]:not(:last-child, :has(+ .menu))`), der Pfeilknopf behält seinen Rundungsrand rechts (`:has(+ .menu)`, direkt darauf folgt `.menu shadow`), beide überlappen um 1 px Rand. Die BB-Regeln (`padding-inline: .5625rem`, `margin-inline-start: 0` am Pfeil) sind harmlos: der Pfeil ist 8x4 px ohne Aussenabstand, der Pfeilknopf 28 px breit. Klick auf den Pfeil öffnet das Menü. Keine Codeänderung nötig; die Klasse `dropdown-toggle-split` ist nur noch BB-eigen.

### G8 – Tote Selektoren aus Bootstrap 5 – erledigt 2026-11-16
- `Select.razor.scss` und `ValidateForm.razor.scss` enthalten Regeln für `.was-validated` und `.has-validation` (10 Stellen); Bootstrap 6 kennt beides nicht mehr (Validierung per `data-bs-validate` und `:user-invalid`, serverseitig `.is-invalid`/`.is-valid`). Prüfen, ob BB die Klassen noch selbst setzt; sonst entfernen.
- **Ergebnis:** BB setzt `.was-validated` nirgends (weder C#, JS noch Razor), `.has-validation` kommt in der Bibliothek nicht mehr vor. Die 10 toten Selektoren sind aus `Select.razor.scss` und `ValidateForm.razor.scss` entfernt; die `.is-valid`/`.is-invalid`-Regeln bleiben. Wächter-Test `ComponentsScss_NoBootstrap5ValidationClasses` (2 Fälle, beide waren rot). Im Browser (`/validate-form`, ungültiges Feld im Fokus): Rand `--bs-danger`, Schatten `none`, wie vorher. Nicht angefasst: `.custom-select` (Bootstrap-4-Klasse) in `ValidateForm.razor.scss`, ebenfalls tot, aber ein eigener Aufräumpunkt.

### G9 – `rgba(var(--bs-*-rgb))` – erledigt 2026-11-17
- **Ergebnis:** 64 Stellen in 16 Dateien (Bibliothek: `variables.scss` 26 und 7 Komponenten-SCSS; Demo: 7 Dateien) auf `color-mix(in oklab, <Farbe> N%, transparent)` umgestellt: `body-color` -> `var(--bs-fg-body)`, `body-bg` -> `var(--bs-bg-body)`, `emphasis-color` -> `light-dark(var(--bs-black), var(--bs-white))`, `primary` -> `var(--bs-primary-bg)`. In `compat.scss` entfallen `--bs-body-bg-rgb`, `--bs-body-color-rgb`, `--bs-emphasis-color-rgb`, `--bs-primary-rgb` samt dem Dunkel-Block; in `Index.razor.css` die ungenutzten `--bs-primary-rgb`/`--bs-secondary-rgb`. Der Wächter `CompatScss_BodyBgRgb_Ok` ist durch `Styles_NoRgbTokens` ersetzt (sucht in allen SCSS/CSS der Bibliothek und der Demo). Im Browser geprüft: `--bb-segmented-bg` 6 % der Textfarbe, folgt dem Thema (hell `#181C32`, dunkel `#dee2e6`); Tabelle: Hover 10 %, Loader 20 %.
- **Hinweis:** Der Verlauf der Startseite nutzt jetzt die Primärfarbe des Themes statt des festen Blaus `#0d6efd`. Die eigenen `--bd-*-rgb` der Startseite bleiben. Anwendungscode mit `rgba(var(--bs-body-color-rgb), x)` verliert den Wert (wie in Bootstrap 6 selbst).
- Bootstrap 6 hat alle `--bs-*-rgb`-Variablen entfernt und empfiehlt `color-mix(in oklab, var(--bs-primary), transparent 50%)`. Wir stützen uns auf `compat.scss` (`--bs-body-color-rgb`, `--bs-emphasis-color-rgb`, `--bs-primary-rgb`, `--bs-body-bg-rgb`): Bibliothek 26 Stellen in 8 Dateien, `variables.scss` 26 Stellen, Demo 12. Funktioniert, ist aber ein Zwischenzustand; Umstellung in einem Schritt, danach die Compat-Variablen und den Wächter-Test streichen.

### G10 – Reste von Bootstrap 5 in der Demo – erledigt 2026-11-16
- `Index.razor.css`: `.lead`, `.text-body-secondary` (beide entfallen), `--bs-border-radius-lg` (nicht definiert, Ersatz `--bs-radius-5`); `Breakpoints.razor` (siehe G4); `rounded-2` in `TwoFactor*.razor` (neue Skala, G5).
- **Ergebnis:** `Index.razor.css`: `--bs-border-radius-lg` durch `--bs-radius-5` ersetzt (Button-Radius 8 px wie vorher), die ungenutzten Regeln für `.text-body-secondary` entfernt. `Index.razor`: die Hinweis-Pille nutzte `rounded-5` (Bootstrap 5: 2rem, also rund), jetzt `rounded-pill`. `TwoFactor.razor` und `TwoFactorApp.razor`: `rounded-2` (Bootstrap 5: .375rem) -> `rounded-4`. `.lead` bleibt: die Klasse hat in Bootstrap 6 keinen eigenen Stil mehr, die Seite definiert sie selbst (`.bd-masthead .lead`). `xxl` war bereits in G4 erledigt. Wächter-Tests `IndexDemo_NoBootstrap5Leftovers` und `TwoFactorDemo_UsesBootstrap6RoundedScale` (3 Fälle, alle waren rot). **Beobachtung:** der Untertitel `.lead` rendert 23 px gross, obwohl die Regel `font-size: 1rem` setzt; nicht untersucht, hat mit diesem Punkt nichts zu tun.

### G11 – Fokusringe – erledigt 2026-11-17
- **Ergebnis:** Mit echter Tastatur (Klick auf die Seite, ein Tab, dann Fokus per Skript je Elementtyp) auf `/multi-select`, `/datetime-picker`, `/calendar`, `/tab`, `/dropdown` und `/switch` geprüft, ob der Fokus sichtbar wird (Outline, Schatten, Rand-, Hintergrund- oder Textfarbe). Button, Eingabe, Select, Checkbox, Radio, Dropdown, Calendar-Schaltflächen und Datumseingabe zeigen den Bootstrap-6-Ring. Zwei Lücken: das Feld von `MultiSelect` (`.dropdown-toggle` samt `.multi-select-input`) und die Pfeiltasten `.picker-panel-icon-btn` im `DateTimePicker` setzten `outline: none` und hatten keinen Ersatz. Beide nutzen jetzt `outline: var(--bs-focus-ring)` und `outline-offset: var(--bs-focus-ring-offset)` bei `:focus-visible` (`Components_FocusVisible_UsesFocusRingTokens`, beide Fälle waren rot).
- **Bewusst gelassen:** Die Reste von Bootstrap 5 (`box-shadow: none` bei `:focus` in `Checkbox`, `ValidateForm`, `Select`, `CheckboxList`, Token `$bb-select-focus-shadow: none`) wirken nicht mehr, weil Bootstrap 6 den Fokus über `outline` zeichnet; sie schaden nicht. Der Schalter `Switch` (`span` ohne `tabindex`) und der Tooltip-Auslöser sind per Tastatur nicht erreichbar bzw. ohne Ring; das war schon in Bootstrap 5 so und ist eine Frage der Bedienbarkeit, nicht der Umstellung.
- Ursprüngliche Beschreibung:
- Bootstrap 6 vereinheitlicht den Fokus über `--focus-ring`, `--focus-ring-width`, `--focus-ring-offset`, `--focus-ring-color` (Mixin `focus-ring`). BB setzt eigene Fokus-Schatten (`$bb-select-focus-shadow` und andere). Tastaturfokus ist an Button, Eingabe, Checkbox und Select sichtbar (D1); eine Vereinheitlichung wäre optisch und für Anpassungen sauberer, ist aber kein Fehler.

### G12 – Kontrast der Motronic-Farben – success und warning erledigt 2026-11-17
- Die Doku weist auf WCAG 2.2 hin (4,5:1 für Text, 3:1 für Nicht-Text). Gemessen im Hover-Lauf: weisse Schrift auf `primary` 2,9, `success` 2,0, `warning` 1,6 (Metronic-Palette, in der Bootstrap-5-Fassung gleich). Bootstrap 6 bringt je Farbe ein `--bs-*-contrast`-Token mit; mit dunkler Schrift auf `success`/`warning` wären die Werte lesbar. Gestaltungsentscheidung, keine Regression.
- **Entscheidung (2026-11-17):** Dunkle Schrift auf `success` und `warning`; `primary` (2,9), `danger` (3,7) und `info` (6,0) bleiben weiss, das Metronic-Blau bleibt wie gewohnt. Gemessen (WCAG, weiss gegen dunkel): `success` 2,0 -> 7,7, `warning` 1,6 -> 9,9.
- **Ergebnis:** `motronic.min.css`: `--bs-success-contrast` neu und `--bs-warning-contrast` auf `var(--bs-gray-900)` (Motronic: `#181C32`). Alle gefüllten Buttons, Badges und Alerts mit `theme-success`/`theme-warning` bekommen so dunkle Schrift (Buttons im Browser: 8,3 und 10,7). `Progress` setzt jetzt `theme-{Farbe}` statt `bg-{Farbe}` (wie in der Bootstrap-6-Doku; `bg-*` setzt nur den Hintergrund, die weisse Schrift blieb). Wächter-Tests `Motronic_LightColorsHaveDarkText` (2 Fälle) und `Color_UsesTheme`, alle waren rot.

### G12b – Komponenten mit `bg-success`/`bg-warning` und Text – erledigt 2026-11-17
- **Ergebnis:** `Collapse` (`TitleColor`, Kopfzeile und Button), `FileIcon` (Badge) und `Toggle` (Zustand „an“) setzen `theme-{Farbe}` statt `bg-{Farbe}`. Das Badge nimmt Hintergrund und Schrift aus `--bs-theme-bg`/`--bs-theme-contrast`; für Collapse und Toggle legen `Collapse.razor.scss` und `Toggle.razor.scss` beides aus denselben Tokens fest. Im Browser (Motronic) gemessen: success 8,3, warning 10,7 (vorher 1,2 bis 2,0). Tests in `CollapseTest`, `FileIconTest` und `ToggleTest` prüfen `theme-*` und dass kein `bg-*` mehr gesetzt wird.
- **Rest:** Die weissen Schriften auf den Motronic-Farben primary (2,9) und danger (3,7) bleiben unter 4,5; das sind die Farben des Themes selbst und gehören nicht zu dieser Änderung.
- **Befund:** Diese Komponenten färben ein Element mit `bg-{Farbe}` und legen Text darauf, die Schrift bleibt weiss (gemessen im Browser, Motronic): `Collapse` (Kopfzeile mit `TitleColor`, 1,6 auf success, 1,3 auf warning), `FileIcon` (Badge, 1,6 und 1,2), `Toggle` (Zustand „an“, 2,0 und 1,6). `Switch` und `NullSwitch` setzen ebenfalls `bg-{Farbe}`; dort steht kein Text auf der Farbe (Seite ohne Treffer). `Toast` (`toast-progress`) und `SelectTable` (Zähler) tragen keinen oder keinen auffälligen Text.
- **Vorgehen:** Je Komponente auf `theme-{Farbe}` umstellen (Hintergrund und passende Schriftfarbe aus einem Token), wo die Komponente Bootstrap-Stile nutzt (`badge`, `accordion-button`); bei `Toggle` die eigene Schriftfarbe in `Toggle.razor.scss` auf `--bs-theme-contrast` legen. Bestehende Tests prüfen die Klassen (`FileIconTest` `badge bg-primary`, `ToggleTest` `bg-success`, `CollapseTest` `bg-secondary`, `SelectTableTest`, `RadioListGenericTest`, `CheckboxListGenericTest`): mitziehen, die `bg-*`-Utilities bleiben für Anwendungscode erhalten.


### G13 – `iframe` und `color-scheme` – geprüft, kein Fehler 2026-11-17
- Reboot setzt `color-scheme: light dark` auf jedes `iframe`, damit eingebettete Seiten bei erzwungenem Thema durchsichtig bleiben; für Einbettungen ohne dunkles Design gibt es `.color-scheme-light`. BB hat die Komponente `IFrame` und Demo-Seiten mit eingebetteten Fremdseiten (`/iframe`, `/meet`, `/office-viewer`, `/pdf-viewer`). Im dunklen und im hellen Design prüfen, ob der Hintergrund stimmt.
- **Ergebnis:** Die Demo-Seiten betten Fremdseiten ein, die offline nicht laden; deshalb mit lokalen `srcdoc`-Iframes im Demo-Browser geprüft. Dunkles Design: eine Seite ohne eigenes Farbschema bekommt eine weisse Fläche (Text bleibt lesbar), Seiten mit `color-scheme: dark` oder `light dark` bleiben durchsichtig und passen sich an; mit `.color-scheme-light` bleibt auch die Seite ohne Schema durchsichtig, ihr schwarzer Text läge dann auf dem dunklen Hintergrund. Helles Design: weisse Fläche, lesbar. Das entspricht dem Verhalten von Bootstrap 5, keine Codeänderung. Hinweis für Nutzer: `.color-scheme-light` ist nur für Seiten gedacht, die selbst einen hellen Hintergrund malen.


### G14 – Abhängigkeit vom Global `window.bootstrap` – erledigt 2026-11-17
- **Ergebnis:** Neues Modul `wwwroot/modules/bootstrap.js` ist die einzige Stelle, die `window.bootstrap` liest (Proxy, der erst beim Zugriff auflöst, nicht beim Laden). Die 20 Skripte der Bibliothek (17 Komponenten, `base-popover.js`, `utility.js`, `validate.js`) importieren `bootstrap` von dort; ihre Aufrufe (`bootstrap.Tooltip` usw.) bleiben unverändert. Beim Bundle-Update (F1) oder bei einem späteren Wechsel auf einen echten Import ist nur diese Datei betroffen. Wächter `ComponentScripts_BootstrapGlobal_OnlyInModule` (war rot), `ComponentScripts_BootstrapApi_Ok` prüft weiter die Namen gegen die Exporte des Bundles.
- **Bewusst kein `import` des ES-Moduls:** Das Bundle läuft als klassisches Skript mit `window.bootstrap` (Data-API und Komponenten teilen sich eine Kopie). Ein zweiter `import` des ES-Moduls hätte eine zweite Kopie mit eigener Instanzen-Map; `getInstance` fände die Instanzen der anderen nicht.
- **Im Browser geprüft:** Tooltip (Hover), Popover (Hover), Dialog (öffnen, Escape schließt; Instanz vorhanden), Tabelle mit Tooltips, keine JavaScript-Fehler.
- Ursprüngliche Beschreibung:
- Bootstrap 6 liefert nur ES-Module und kein Global; unser Bundle stellt `window.bootstrap` selbst bereit (`BootstrapAssetsTest` sichert das). 34 Stellen in 17 Dateien der Bibliothek nutzen `bootstrap.*`. Kein Fehler, aber bei einem Update des Bundles (F1) die erste Stelle, die brechen kann; langfristig `import` aus einem Modul.

### G15 – Neue Bausteine von Bootstrap 6 – bewertet 2026-11-17
- **Vorgehen:** Das mitgelieferte Bundle (`6.0.0-alpha.1`) enthält die Klassen `Combobox`, `Datepicker`, `Chips`, `OtpInput`, `Strength`, `NavOverflow`, `Range`, `Toggler`. Mit dem Markup der offiziellen Dokumentation habe ich sie in die geladene Demo-Seite eingesetzt (mit allen BB-Stilen, dunkles Design) und bedient: Tippen, Klicken, Tastatur, `show()`. Danach das CSS auf Namenskollisionen mit BB-Klassen durchsucht.
- **Ergebnis je Baustein:**

| Baustein | Befund im Browser | Vergleich mit BB | Entscheidung |
|---|---|---|---|
| `Strength` (Passwortstärke) | Vier Segmente und Text („Good“) reagieren beim Tippen; Optionen `minLength`, `messages`, `scorer`, Ereignis `strengthChange.bs.strength` | BB hat nichts Vergleichbares | Chance: G15a |
| `OtpInput` | Schlitze, Fokusring, Zeichen rücken weiter; Ereignis `complete.bs.otpInput` | BB hat `OtpInput` (Blazor-gebunden, eigene Validierung) | BB behalten |
| `Stepper` | Waagerecht mit `theme-success`/aktiv, sieht stimmig aus | BB hat `Step` | BB behalten; optional später angleichen |
| `Combobox` | Öffnet, wählt (`change.bs.combobox`), Menü passt zum BB-Stil | BB `Select`/`AutoComplete`/`MultiSelect` binden an Blazor, validieren, virtualisieren | BB behalten (Combobox ist reines JavaScript mit verstecktem Eingabefeld) |
| `Datepicker` | Mit `locale: 'de-DE'` Wochentage „Mo, Di, Mi …“, Monat korrekt, öffnet über `show()` (über Daten-API nur mit `data-bs-toggle="datepicker"`); bringt `vanilla-calendar-pro` 3.4.0 mit | BB `DateTimePicker`/`DateTimeRange` können Zeit, Bereiche, Wochen/Monate/Jahre | BB behalten (nur Datum) |
| `NavOverflow` | Überzählige Einträge wandern hinter „Mehr“ (`moreText` einstellbar), 4 von 6 sichtbar bei 360 px | `Nav` und `Tab` haben keinen eigenen Überlauf mit Menü | Chance: G15b |
| `Chips` | Chip mit Schließen-Knopf und `theme-*` | BB `Tag`/`TagsInput` | BB behalten |
| `Avatar` (CSS) | Das `.avatar` von Bootstrap 6 und das von BB teilen den Klassennamen und die Größenstufen `xs`..`xl` | BB `Avatar` | **Fehler gefunden und behoben**, siehe unten |
| Drawer-Wischgeste | Nicht im Browser prüfbar (kein Touch) | BB `Drawer` | in D3 |
| `Form-adorn`, `.form-field` | Mein Markup nach Vermutung brach das Layout; Dokumentation nicht gelesen | – | nicht bewertet |

- **Namenskollisionen:** Von den neuen Klassen (`stepper*`, `avatar*`, `chip*`, `combobox*`, `otp*`, `strength*`, `nav-overflow*`, `form-adorn*`, `form-field*`, `drawer*`) nutzt BB nur `.avatar` und `.avatar-xs/-sm/-lg/-xl`. Alle anderen Namen sind frei.
- **Fehler (behoben):** Weil BB-CSS nicht in einer Ebene liegt, gewinnen Breite, Höhe, Rundung und Farben von BB; das `.avatar` von Bootstrap 6 lieferte aber weiter `text-transform: uppercase` und `font-weight: 500`. Der Text-Avatar „User“ der Demo erschien als „USER“ (in Bootstrap 5: „User“). `Avatar.razor.scss` setzt jetzt `text-transform: none` und `font-weight: inherit` (`AvatarScss_ResetsBootstrap6TextStyle`, war rot; im Browser: „User“, Gewicht 400). Das `.avatar` von Bootstrap 6 bringt außerdem `position: relative` und `font-size` aus `--bs-avatar-size`; beides verändert die BB-Darstellung nicht.
- **Eigene Punkte:** G15a (PasswordStrength), G15b (NavOverflow).
- **G15a erledigt 2026-11-17:** Neue Komponente `PasswordStrength` (`Components/PasswordStrength/`, Enum `PasswordStrengthLevel`). Sie rendert `.strength` mit vier Segmenten und `.strength-text`, hängt `bootstrap.Strength` über `modules/bootstrap.js` an das Feld mit `InputId` (der Wert wird im Browser bei jeder Eingabe bewertet, kein Rundlauf zum Server) und meldet den Wechsel der Stufe über `OnStrengthChanged`. Parameter `InputId` (Pflicht), `MinLength` (8), `ShowText` (true); Texte aus `IStringLocalizer` (`Locales/en.json`, `zh.json`). Bootstrap 6 färbt in alpha.1 den Text nicht mit der Stufe (die Regel liest `--bs-strength-color`, das nur auf `.strength` liegt, und das Skript setzt `--strength-color`); `PasswordStrength.razor.scss` färbt den Text mit `--bs-danger-fg`/`warning`/`info`/`success`. Demo `/password-strength`, Menü, `docs.json`, Changelog. Tests: `PasswordStrengthTest` (11), `PasswordStrength_Locales_HaveLevels` (2). Im Browser geprüft: „abc“ zeigt „Weak“ mit einem Segment, ein längeres Passwort „Strong“ mit vier, der Blazor-Rückruf aktualisiert die Anzeige. Die übrigen Sprachdateien in `localization/` wurden nicht ergänzt (Übersetzungen werden dort gesammelt). Datepicker, Combobox und Chips sind bewusst kein Ersatz.
- **G15b erledigt 2026-11-17:** `Nav` hat die Parameter `IsOverflow` und `OverflowText` (Standard: lokalisiertes „More“/„更多“, `Locales/en.json`, `zh.json`). Mit `IsOverflow` rendert `Nav` einen Wrapper `div.nav-overflow` um das `nav` und jeden Eintrag in ein `div.nav-item`; `Nav.razor.js` legt `bootstrap.NavOverflow` an (über `modules/bootstrap.js`), das ResizeObserver von Bootstrap schiebt zu breite Einträge hinter einen „Mehr“-Knopf. Ohne `IsOverflow` bleibt das Markup unverändert. `Nav` erbt dafür von `BootstrapModuleComponentBase`; die JavaScript-Funktion startet nur bei `IsOverflow`. Im Browser (Demo `/navigation`, Block „Overflow“): bei 260 px sind 2 von 6 Einträgen sichtbar, 4 im Menü (Products, Services, Reports, Help); manuelles `update()` ergibt bei 100/200/330/500 px 6/5/4/1 Einträge im Menü. Das automatische Nachziehen beim Verkleinern kam im Vorschau-Browser einen Schritt zu spät (der Bereich läuft im Hintergrund, Bilder werden gedrosselt); in einem normalen Fenster nicht geprüft. **Grenze:** Bootstrap misst und verschiebt die Elemente im Browser; die Einträge dürfen nach dem ersten Rendern nicht mehr wechseln (die Demo hält sie deshalb in einem Feld). Nur die Einträge aus `Items` werden zu `.nav-item`, Inhalt aus `ChildContent` bleibt unverändert.
- **`Tab` bewusst unverändert:** `Tab` verwaltet seine Einträge selbst (`.tabs-item-wrap` mit Schließen-Knopf, Ziehen zum Umsortieren, aktivem Eintrag per Blazor, Scrollbereich `tabs-nav-scroll` mit Scrollen zum aktiven Eintrag). Bootstrap würde diese Elemente verschieben und klonen; das widerspricht dem Umsortieren und den Schließen-Knöpfen.

(Ursprüngliche Beschreibung:)
- **Neu in Bootstrap 6:** Combobox, Datepicker (Peer-Abhängigkeit `vanilla-calendar-pro`), Stepper, Avatar, Chip und Chip-Input, OTP-Input, Password-Strength, Range (JavaScript), Toggler, Nav-Overflow, Submenü im Menü, Form-Adorn, `.form-field`, Prose, Drawer mit Wisch-Geste, `.btn-icon`/`.btn-text`/`.btn-subtle`/`.btn-styled`, `.card-translucent`/`.card-subtle`, `.accordion-gap`, `.pagination.theme-*`, `hover-lift`, Motion-Hilfsklassen, Container-Query- und `dvh`-Hilfsklassen, `color-scheme`-Hilfsklassen, geschichtete Schatten.
- **Einordnung:** BB hat eigene Komponenten für die meisten (Select/AutoComplete/MultiSelect, DateTimePicker, Step, Avatar, Tag/TagsInput, OtpInput, Slider, Drawer, Tab-Überlauf). Keine Pflicht, sie zu ersetzen; Chancen sind Password-Strength (fehlt in BB), Nav-Overflow für Tabs und Menüs und die Wisch-Geste für Drawer. Bei einer Entscheidung pro Baustein einen eigenen Punkt anlegen.

**Geprüft ohne Handlungsbedarf (zur Nachvollziehbarkeit)**
- `bootstrap.Tab` löst bei fehlendem Tab-Panel jetzt einen Fehler aus: BB nutzt das Plugin nicht (0 Stellen). ScrollSpy: nicht genutzt.
- Entfernte oder umbenannte Klassen und Optionen (`text-muted`, `text-bg-*`, `bg-light`/`bg-dark`, `link-*`, `d-print-*`, `lead`, `display-*`, `mh-*`/`mw-*`, `fs-1`..`fs-6`, `lh-base`, `carousel-dark`, `btn-close-white`, `navbar-light`/`navbar-dark`, `alert-dismissible`, `offcanvas`, Infix-Breakpoints wie `d-md-*`, Ereignisnamen `.bs.dropdown`/`.bs.modal`, `popperConfig`): in der Bibliothek keine Treffer; in der Demo nur die unter G10 genannten.
- `.form-select` und `.form-check`: BB nutzt sie als eigene Klassen mit eigenem CSS, kein natives `<select>` in der Bibliothek. `.collapsed` wird nur in `Collapse.razor.scss` ausgewertet (BB setzt die Klasse selbst).
- Die Navbar nutzt weiter Collapse statt Drawer (Bootstrap 6 empfiehlt Drawer); der Umschalter funktioniert (D3).
- Sass-Umbauten (`@use`, `$breakpoints`, entfernte Variablen, PostCSS-Präfix): BB kompiliert nur eigenes SCSS, nicht das Sass von Bootstrap; das mitgelieferte CSS ist fertig.
- `show()`/`hide()` liefern jetzt ein Promise: optional lassen sich die Zeitüberbrückungen (`getTransitionDelayDurationFromElement` in `Menu.razor.js`) damit ersetzen; kein Fehler.
