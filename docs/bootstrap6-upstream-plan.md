# Bootstrap 6 im Hauptrepo – Stand und Vorschlag (C3)

Stand 2026-11-17 (Zahlen und Entwurf gegenüber 2026-11-16 aktualisiert). Quellen: das Issue `dotnetcore/BootstrapBlazor#8734`, das der Maintainer in die Diskussion
[`#8739`](https://github.com/dotnetcore/BootstrapBlazor/discussions/8739) umgewandelt hat, `CONTRIBUTING.md` und
`docs/agents/pr-guide.md`. Nichts davon wurde im Hauptrepo verändert; es gibt nur lesende Abfragen und einen
lokalen Remote `upstream` (nur zum Holen).

## Was der Maintainer gesagt hat

- Es gibt im Hauptrepo noch keine Arbeit an Bootstrap 6 und es ist keine geplant, **bevor Bootstrap 6 die Beta erreicht**
  (aktuell `6.0.0-alpha.1`; am 2026-11-17 auf npm geprüft: `next` ist weiter `6.0.0-alpha.1`, es gibt keine Beta). Vor einer stabilen API wird nichts in `main` gemergt.
- Vorbereitung in kleinen Schritten ist ausdrücklich willkommen, im Fork oder (laut seiner Ankündigung) mit Branches
  direkt im Hauptrepo. Schreibrechte dort hast du bereits (`push` und `triage` sind gesetzt).
- Gewünscht: **kein** grosser PR, nicht alles in einem Branch. Zuerst die Grundlagen (CSS/Sass/Token-Aufbau,
  Hilfsklassen, einfache Komponenten), dann aufbauend; ein PR pro Komponente mit einer klaren Änderung.
- Wer eine Komponente übernimmt, soll das in der Diskussion melden, damit nichts doppelt gemacht wird.

Unser bisheriger PR #1 (397 Dateien, +29.080/−31.802 Zeilen) ist genau die Art Diff, vor der er warnt. Der Fork ist seitdem mit kleinen PRs weitergewachsen (#34 bis #53); gegen die Basis `9291ace14` sind es jetzt 483 Dateien, +32.835/−32.010 Zeilen.

## Woraus unser Diff besteht (gegen `cfcc339e9`, 429 Dateien)

Der grosse Teil ist mechanisch, der Rest klein:

| Bereich | Dateien | Zeilen |
| --- | ---: | ---: |
| Mitgeliefertes Bootstrap (`wwwroot/core/bootstrap`, 6 Dateien) | 6 | +25.600 / −28.954 |
| 54 Komponenten (`src/BootstrapBlazor/Components/*`) | 143 | +995 / −829 |
| Demo (`BootstrapBlazor.Server`, meist Klassennamen) | 182 | +1.466 / −1.464 |
| Tests | 67 | +1.382 / −325 |
| `wwwroot/scss` und Module (`compat.scss`, Tokens, Hilfsklassen-Teil) | 7 | +1.419 / −36 |
| Doku, Skripte, Bundler, Theme-CSS, Erweiterungen und Sonstiges | 24 | +832 / −498 |

Bei den Komponenten sind 49 von 54 unter 100 geänderten Zeilen. Grösser sind nur: Modal 533, Select 157, Table 138,
Navbar 122, Dropdown 104 (danach Menu 99, Drawer 84, Carousel 68). Ein Zuschnitt in kleine PRs ist also gut machbar.

## Verträglichkeit mit dem heutigen Hauptrepo

Das Hauptrepo ist seit unserer Basis um 15 Commits weitergelaufen (weiter auf Bootstrap 5, `v11.1.0`). Ein
Probe-Merge von `upstream/main` in unseren `main` (2026-11-17, `git merge-tree`, nichts verändert) hat **vier** Konflikte: `Modal.razor.js` (dort wurde „show dialogs
after rapid service calls“ geändert, das Modal ist bei uns ohnehin der grösste Umbau), `Drawer.razor.js` und die Demo-Beispiele `DateTimeRanges.razor` und `Drawers.razor`. Alles andere lässt sich automatisch zusammenführen. Je länger das Hauptrepo auf Bootstrap 5 weiterläuft, desto mehr Konflikte kommen dazu; das ist ein Grund für einen Integrationszweig.

## Vorschlag für den Zuschnitt (Reihenfolge)

Jeder PR braucht laut Projektregeln ein verknüpftes Issue, Tests, bei UI-Änderungen Screenshots (hell und dunkel) und
Conventional-Commits-Titel. Ziel der PRs wäre ein **Integrationszweig** (zum Beispiel `bootstrap6`), nicht `main`;
`main` bleibt bis zur Beta bei Bootstrap 5. Den Zweignamen und ob er im Hauptrepo oder in deinem Fork liegt, soll der
Maintainer entscheiden.

1. **Grundlage A:** Bootstrap-Dateien auf `6.0.0-alpha.1`, Bundler und `Directory.Build.targets`, Laden des ESM-Pakets als
   `window.bootstrap`, `BootstrapAssetsTest` (Version, Dateien). Der Austausch der Bootstrap-Dateien ist gross, aber
   mechanisch und lässt sich mit dem npm-Paket `bootstrap@6.0.0-alpha.1` Datei für Datei nachprüfen.
2. **Grundlage B:** `compat.scss` (BS5-Token auf BS6 abbilden, Farb-Hilfsklassen mit `!important`), Wurzel-Token,
   Motronic-Theme, zugehörige Wächter-Tests.
3. **Grundlage C:** Hilfsklassen-Generator `scripts/gen-important-utilities.py` mit der erzeugten Datei und Test.
4. **Komponenten mit Logik, je ein PR:** Modal, Dropdown, Menu, Select, Table, Navbar, Drawer, Carousel
   (und `Searches`/`SelectGeneric` zusammen mit Select beziehungsweise Table).
5. **Kleine Komponenten in thematischen Gruppen** (meist nur Klassennamen, je unter 50 Zeilen), etwa Formulare
   (Button, Switch, Checkbox, Radio, Input*, AutoComplete, AutoFill, Cascader, DateTimePicker, ValidateForm),
   Navigation und Layout (Tab, Layout, Collapse, Logout, ThemeProvider, ContextMenu, Pagination),
   Rückmeldung und Darstellung (Alert, Toast, Message, Tooltip, Card, Badge, Skeleton, Spinner, Tag, Timeline, TreeView …).
6. **Demo (`BootstrapBlazor.Server`):** 182 Dateien, fast nur Klassennamen, nach Bereichen aufgeteilt, nachdem die
   Komponenten stehen.
7. **Doku zuletzt:** README, Paket-README und CHANGELOG-Einträge (siehe unten).

Die Fehler, die ich beim Vergleich mit der Bootstrap-5-Fassung gefunden habe (Cascader, Card-Header, Collapse,
TreeView, Checkbox, Tooltip, Motronic), gehören jeweils in den PR der betroffenen Komponente.

## CHANGELOG-Link (Teil von C3)

Die Einträge in `CHANGELOG.md` und `CHANGELOG.zh-CN.md` verlinken auf PRs in `MaikPeters1511/BootstrapBlazor`. Sie
sollten erst auf Hauptrepo-PRs umgestellt werden, wenn es diese gibt; bis dahin sind die Fork-Links richtig. Release
Notes des Hauptrepos entstehen laut `.agents/skills/release-notes/SKILL.md` unter `ChangeLogs/{version}_en.md` und
`_zh.md` (den Ordner gibt es im Repo noch nicht), die `CHANGELOG*.md` im Wurzelverzeichnis werden dort seit V7.11.1
nicht mehr gepflegt.

## Entscheidung, die bei dir liegt

- **A – Warten (Empfehlung, solange es keine Beta gibt):** Fork als Referenz behalten, bei Bootstrap 6 Beta (F1) neu bewerten. Kein Aufwand, nichts Öffentliches. Die Bedingung des Maintainers (Beta) ist am 2026-11-17 nicht erfüllt.
- **B – Vorbereiten, aber erst absprechen:** Kurze Notiz in der Diskussion (Entwurf unten), Antwort des Maintainers zu
  Zweig und Reihenfolge abwarten, danach die PRs in kleinen Schritten.
- **C – Sofort PRs öffnen:** nicht empfohlen, weil `main` bis zur Beta nichts annimmt und ein Integrationszweig fehlt.

## Entwurf der Notiz für die Diskussion #8739 (Englisch, nicht gepostet)

> Thanks for the guidance and for the access! A short status and one question, so we don't duplicate work.
>
> **Status:** in my fork (https://github.com/MaikPeters1511/BootstrapBlazor) the library, the demo and the unit tests
> run on Bootstrap 6.0.0-alpha.1 (2904 of 2906 tests pass; the 2 failing ones only exist in Release builds because of
> `#if DEBUG`; CI runs again). I compared the demo page by page with the Bootstrap 5 build (about 330 pages in light and dark, plus
> pop-up, hover and keyboard-focus states and a scan of all utility classes) and fixed the regressions that showed up.
>
> I understand that nothing goes into `main` before the beta and that a single big PR is not wanted. The diff splits
> well: the vendored Bootstrap files are the only large part (6 files, mechanical); the 54 components together are
> about 1,800 changed lines, 49 of them under 100 lines each; a trial merge with today's `main` has two conflicts
> (`Modal.razor.js` and one demo sample).
>
> **Proposal:** the PRs target an integration branch (for example `bootstrap6`), `main` stays on Bootstrap 5. I would
> claim the foundation first (vendored files and build setup, a compat layer, utility classes), then the components
> with real logic one per PR (Modal, Dropdown, Menu, Select, Table, ...), then small components in groups, then the demo.
>
> **Question:** do you prefer the branch in this repository, or should I keep working in my fork until the beta? And is
> this order fine for you?
