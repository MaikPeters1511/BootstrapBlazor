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
| B3 | ~~Theme `ant.css` migrieren~~ erledigt 2026-11-15 | P2 | M |
| B4 | ~~Themes `devui.css` und `blue.css` prüfen~~ erledigt 2026-11-15 | P2 | S |
| B5 | ~~Motronic-Theme: Hover- und Active-Farben~~ erledigt 2026-11-15 | P3 | S |
| C1 | ~~Doku und CHANGELOG nachziehen~~ erledigt 2026-11-15 | P2 | S |
| C2 | Chinesische Texte gegenlesen (von mir, nicht von einem Muttersprachler; erledigt 2026-11-15) | P3 | S |
| C3 | CHANGELOG-Link und Upstream-Vorbereitung | P2 | S |
| D1 | Sichtprüfung aller Demo-Seiten (automatisierter Teil, Bildvergleich im dunklen und hellen Design und Popup-/Hover-Zustände im dunklen Design erledigt 2026-11-15/16; Rest siehe unten) | P2 | L |
| D2 | ~~Drucken von Dialogen prüfen~~ erledigt 2026-11-15 | P3 | S |
| D3 | Schmale Breiten und Touch prüfen (Überlauf, Navbar, Tabellen-Beschriftungen, `carousel-no-touch`, EditorForm/SearchForm-Raster erledigt 2026-11-15/16; echtes Wischen auf einem Touch-Gerät offen) | P2 | M |
| D4 | ~~Tastaturbedienung prüfen~~ erledigt 2026-11-15 (Dropdown, Select, Dialog, Seitenmenü, gestapelte Dialoge) | P2 | M |
| D5 | ~~Einzelne Komponenten prüfen~~ erledigt 2026-11-15 | P2 | M |
| D6 | ~~Starter-Vorlage `Bs6Test` prüfen und ablegen~~ erledigt 2026-11-15 (bleibt ausserhalb des Repos) | P3 | S |
| E1 | ~~Dialog: `OnClosingAsync` bei Escape und Hintergrundklick~~ erledigt 2026-11-14 | P2 | M |
| E2 | ~~Dialog: `dialog-instant` geht beim Re-Render verloren~~ erledigt 2026-11-15 | P3 | S |
| E3 | ~~Carousel: `AutoPlayAfterManually` nach Hover~~ erledigt 2026-11-15 | P3 | S |
| F1 | Wechsel auf neue Bootstrap-6-Alphas und die stabile Version (am 2026-11-15 geprüft: noch keine neuere Version) | P3 | M |

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
- **Offen / Entscheidung:** Der Scan sieht nur, was die Demo-Seiten rendern. Anwendungscode, der Hilfsklassen an BB-Komponenten hängt, kann weitere Stellen treffen, an denen eine ungeschichtete BB-Regel dasselbe Merkmal setzt. Eine grundsätzliche Lösung wäre, die Hilfsklassen für Abstand und `display` in `compat.scss` wie bei den Farben (`CompatScss_ColorUtilities_Important`) wieder `!important` zu machen (Verhalten von Bootstrap 5); das kostet CSS-Grösse und ändert die Rangfolge für alle Nutzer, deshalb nicht ohne Entscheidung umgesetzt.
- **Reine Designunterschiede (kein Fehler):** unscharfer Hintergrund hinter Modal, Dialog und SweetAlert; grauer quadratischer Schliessen-Knopf in Modal und Toast; dunklere Hover-Hervorhebung in Dropdown- und Select-Listen (`--bs-menu-item-hover-bg` = `bg-1`); Hervorhebung im Tab und in Menüs.
- **Offen:** Popup-/Hover-Zustände im hellen Design; Fokus per Tastatur (`:focus-visible`); Cascader (Selektor fand kein Element); Bereich unterhalb der ersten 2400 px. Aufnahmen und Skripte liegen im Scratchpad, nicht im Repo.

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
- **Noch nicht geprüft:** echtes Wischen auf einem Touch-Gerät (der Emulator liefert nur Mausklicks).
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
