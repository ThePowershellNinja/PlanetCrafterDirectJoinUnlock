# Planet Crafter Direct Join Unlock

Planet Crafter Direct Join Unlock is a small **client-side BepInEx plugin** for **The Planet Crafter**. It re-enables the game's direct-address join flow so players can connect to a server by IP or address instead of being forced through the invite-code-only UI.

This repository includes:

- the mod source under `src/PlanetCrafterDirectJoinUnlock/`
- a prebuilt package under `package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/`
- build instructions that use configurable paths instead of hard-coded machine-specific locations

This repo does **not** vendor the full BepInEx distribution or a temporary .NET SDK tree.

## What the mod actually changes

The plugin patches the game's menu flow with Harmony and does three specific things:

1. Forces the main menu's multiplayer/direct-join button to stay visible.
2. Forces the invite-code join menu to stay hidden when the main menu opens or refreshes.
3. Before the game runs the join action, forces the internal saved-data `onlineGame` flag to `false` so the direct join flow uses the offline/UnityTransport path.

It does **not** add a new UI, a server browser, or a hosting tool. It only unlocks the direct join path that the existing game code already contains.

## Repository layout

| Path | Purpose |
| --- | --- |
| `src/PlanetCrafterDirectJoinUnlock/` | C# source and project file |
| `package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/` | Prebuilt plugin files ready to copy into a BepInEx install |

## Prerequisites

To use the mod on Windows, you need:

- a working install of **The Planet Crafter**
- **BepInEx 5 x64** installed into the game folder
- the plugin files from this repository

## Find The Planet Crafter install folder in Steam

The safest way to find the correct folder on Windows is through Steam:

1. Open **Steam**.
2. Open your **Library**.
3. Right-click **The Planet Crafter**.
4. Choose **Manage** -> **Browse local files**.
5. Steam will open the game install folder in File Explorer.

On many systems the default path is:

```text
C:\Program Files (x86)\Steam\steamapps\common\The Planet Crafter\
```

If you use a different Steam library, your path will be different. The correct folder is the one that contains `Planet Crafter.exe`.

## Install BepInEx for The Planet Crafter on Windows

This repository does not bundle BepInEx. Install it separately first.

1. Download the **BepInEx 5 x64** release for Windows.
2. Open the ZIP file.
3. Extract its contents directly into your **The Planet Crafter** install folder.
4. After extraction, the game folder should contain files and folders such as:
   - `BepInEx\`
   - `doorstop_config.ini`
   - `winhttp.dll`
5. Launch the game once, then close it again.

That first launch lets BepInEx finish setting up its folder structure and log file.

After BepInEx is installed, the plugin destination folder will be:

```text
<The Planet Crafter>\BepInEx\plugins\
```

## Install this plugin manually

This repository already includes a packaged build under:

```text
package\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\
```

To install it:

1. Open this repository's `package\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\` folder.
2. Copy `PlanetCrafterDirectJoinUnlock.dll` into your game's BepInEx plugins folder.
3. If you want the packaged files to match the repository package exactly, also copy:
   - `PlanetCrafterDirectJoinUnlock.deps.json`
   - `PlanetCrafterDirectJoinUnlock.pdb`

The **exact destination path** for the plugin DLL is:

```text
<The Planet Crafter>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll
```

If the `PlanetCrafterDirectJoinUnlock` folder does not exist yet under `BepInEx\plugins\`, create it first.

## Launch and test

1. Start **The Planet Crafter** normally.
2. Let BepInEx load the plugin.
3. Open the main menu and look at the multiplayer/join entry points.

Expected behavior:

- the direct multiplayer/direct join button should be visible
- the invite-code join menu should not stay open on the main menu
- using the join action should use the mod's forced offline-direct join mode before the game continues the join flow

If BepInEx loaded the plugin successfully, `BepInEx\LogOutput.log` should contain:

```text
Loaded Planet Crafter Direct Join Unlock.
```

## Join flow behavior to expect

This plugin is intentionally small and specific. The observable behavior should be:

- **Direct IP join availability:** the direct-address multiplayer button is enabled if the game had hidden it.
- **Offline-direct join mode:** when the game's join action runs, the plugin flips the internal saved-data `onlineGame` flag to `false`.
- **Join menu behavior:** the invite-code join screen is hidden whenever the intro/main menu initializes or reopens.

If your workflow depends on a different menu, browser, or host flow, this plugin does not implement that.

## Troubleshooting

### The plugin does not load

- Confirm the DLL is in `BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll`.
- Make sure you installed **BepInEx 5 x64**, not a different platform build.
- Check `BepInEx\LogOutput.log` for load errors.

### The direct join button is still missing

- Make sure BepInEx actually loaded the plugin.
- Test with only this plugin installed to rule out another mod overriding the same UI.
- Reopen the game after copying the DLL; BepInEx plugins are not hot-loaded into a running game session.

### The wrong folder was used

If the game folder does not contain `Planet Crafter.exe`, you are not in the correct install directory yet. Use Steam's **Browse local files** option to jump straight to the right place.

### BepInEx never creates its folders or log

- Recheck that `winhttp.dll` and `doorstop_config.ini` are in the same folder as `Planet Crafter.exe`.
- Launch the game once after extracting BepInEx.
- If antivirus or Windows security blocked a file from the ZIP, re-extract the archive and review any quarantine prompts.

### The game updated and the mod stopped working

This mod relies on Harmony patches against game classes and methods. If an update renames or changes those members, the plugin may need to be updated before it works again.

When reporting the problem, include your game version and relevant lines from `BepInEx\LogOutput.log`.

## Build from source

The project file is set up to be portable: it does not contain hard-coded absolute paths to one workstation.

To build from source, you need:

- a .NET SDK that can build `netstandard2.1`
- a local install of **The Planet Crafter**
- **BepInEx 5** installed into that game directory, or another BepInEx folder you can point the build at

The project accepts these MSBuild properties:

- `GameDir` - your The Planet Crafter install directory
- `BepInExDir` - optional override for the BepInEx directory; defaults to `$(GameDir)/BepInEx`

You can also set the `PLANETCRAFTER_GAME_DIR` environment variable instead of passing `GameDir` on the command line.

### Example build command (PowerShell)

```powershell
$env:PLANETCRAFTER_GAME_DIR = 'C:\Program Files (x86)\Steam\steamapps\common\The Planet Crafter'
dotnet build .\src\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.csproj -c Release
```

### Example build command (explicit properties)

```powershell
dotnet build .\src\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.csproj -c Release `
  -p:GameDir='C:\Program Files (x86)\Steam\steamapps\common\The Planet Crafter'
```

If your BepInEx installation is not under `GameDir\BepInEx`, also pass:

```powershell
-p:BepInExDir='C:\path\to\BepInEx'
```

The same project file also works on other systems as long as you point `GameDir` and `BepInExDir` at your own local game install paths.

Build output is written straight into the packaged plugin layout:

```text
package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/
```

After building, copy the generated files into:

```text
<The Planet Crafter>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\
```
