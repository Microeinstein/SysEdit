# SysEdit

GUI editor for windows environment variables, using winforms. This is an hobby project from 2018 and _although it is finished_, it's not mantained.

## Gallery

<img src="/.repo/main.png" alt="Main" width="98.5%"><br>
<img src="/.repo/unexpanded.png" alt="Unexpanded path" width="49%"> <img src="/.repo/expanded.png" alt="Expanded path" width="49%"><br>

## Features

- [x] 🛡 When launched, ask to be run as administrator
- [x] 👥 Show variables from user, system, process
- [x] 💾 Import, export (all) vars.
- [x] ✏️ Add, edit, delete vars.
- [x] 📋 Treat as raw text or list of entries
- [x] 📁 Detect missing directories
- [x] 🔍 Temporarly expand `%VARS%`
- [x] ⌨️ Shortcuts: <kbd>Alt</kbd> + <kbd>↑</kbd>,<kbd>↓</kbd>,<kbd>PgUp</kbd>,<kbd>PgDown</kbd> to move entries in the list view

## Download

See [releases](https://github.com/Microeinstein/SysEdit/releases).

## How to build

- Dependencies
    - **.NET framework** 4.7.1, winforms
    - **MicroLibrary.WinForms** ([git](https://github.com/Microeinstein/MicroLibrary.WinForms))

- Other requirements
    - Windows 7 or later _(?)_
    - Visual Studio

Being a Visual Studio project, compilation steps will be determined automatically.

## License

[GPLv3](COPYING) ([resources](/Resources) excluded)
