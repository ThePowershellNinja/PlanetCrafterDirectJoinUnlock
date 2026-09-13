# Planet Crafter Direct Join Unlock

Planet Crafter Direct Join Unlock is a small **client-side BepInEx plugin** for **The Planet Crafter** that keeps the game's **direct-address/direct-IP join path** available **without removing the normal Steam/invite-code join flow**.

On Steam-enabled installs, the base game normally hides the direct-address button and shows only the invite-code join UI. This plugin restores the direct-address option as an **additional** path, so players can choose either:

- the normal **Steam / invite-code** path
- the separate **direct IP / direct address** path

This repository includes:

- the mod source under `src/PlanetCrafterDirectJoinUnlock/`
- a prebuilt package under `package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/`
- portable build instructions with no machine-specific absolute paths
- GitHub Releases for users who just want the packaged plugin ZIP

This repo does **not** bundle the full BepInEx distribution or a temporary .NET SDK tree.

## What the mod actually changes

The plugin patches the game's existing menu flow with Harmony and makes three focused changes:

1. It forces the built-in **direct-address multiplayer button** to remain visible, even when Steam is initialized.
2. It **does not hide or replace** the normal Steam/invite-code join UI.
3. When the player uses the direct-address form, it sets `SavedDataHandler.onlineGame = false` **only for that direct-address join action**, so the game uses the offline/direct UnityTransport path for that join.

It does **not** add a server browser, a host tool, or a custom networking stack. It only unlocks and preserves the game's existing two join paths.

## How the two join paths behave

| Path | How to use it | What the plugin does |
| --- | --- | --- |
| Steam / invite-code join | Use the built-in invite-code join UI on the main menu | Leaves it alone; the game continues to use the online/Steam-style join flow |
| Direct IP / direct address join | Click the separate multiplayer/direct-address button, then enter the server address | Makes that button visible and forces `onlineGame = false` only for that direct join |

In short:

- **Steam join remains available**
- **invite-code join remains available**
- **direct IP join becomes available alongside them**

## Repository layout

| Path | Purpose |
| --- | --- |
| `src/PlanetCrafterDirectJoinUnlock/` | C# source and project file |
| `package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/` | Prebuilt plugin files ready to copy into a BepInEx install |

## Prerequisites

To use the mod on Windows, you need:

- a working install of **The Planet Crafter**
- **BepInEx 5 x64** installed into the game folder
- the packaged plugin release from this repository

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

## Install this plugin from the packaged release

For most users, the easiest install method is the packaged ZIP from the GitHub Releases page.

1. Open the repository's **Releases** page:

   ```text
   https://github.com/ThePowershellNinja/PlanetCrafterDirectJoinUnlock/releases
   ```

2. Download the latest packaged asset:

   ```text
   PlanetCrafterDirectJoinUnlock-v1.0.0.zip
   ```

3. Open the ZIP file.
4. Extract the ZIP contents directly into your **The Planet Crafter** game folder.
5. When prompted, allow the ZIP to merge into the existing `BepInEx\` folder that you installed earlier.

The release ZIP is packaged so that its internal folder structure already matches the game folder layout:

```text
BepInEx\
  plugins\
    PlanetCrafterDirectJoinUnlock\
      PlanetCrafterDirectJoinUnlock.dll
      PlanetCrafterDirectJoinUnlock.deps.json
      PlanetCrafterDirectJoinUnlock.pdb
```

That means you should extract the ZIP into the folder that contains `Planet Crafter.exe`, not into some separate temporary `mods` directory.

## Exact installed file path

After extracting the packaged release, the most important installed file should be here:

```text
<The Planet Crafter>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll
```

If that file is not present in that exact folder, the plugin is not installed correctly.

## Install this plugin from a repository checkout instead

This repository already includes a packaged build under:

```text
package\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\
```

To install it:

1. Open this repository's `package\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\` folder.
2. Copy the files into your game's BepInEx plugins folder.
3. To match the packaged release layout exactly, copy:
   - `PlanetCrafterDirectJoinUnlock.deps.json`
   - `PlanetCrafterDirectJoinUnlock.pdb`
   - `PlanetCrafterDirectJoinUnlock.dll`

The exact destination path for the plugin DLL is:

```text
<The Planet Crafter>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll
```

If the `PlanetCrafterDirectJoinUnlock` folder does not exist yet under `BepInEx\plugins\`, create it first.

## Launch and test

1. Start **The Planet Crafter** normally.
2. Let BepInEx load the plugin.
3. Return to the main menu and look at the multiplayer/join options.

On a Steam-enabled install, the expected result is:

- the normal **invite-code join UI** is still present
- the separate **direct-address multiplayer button** is also visible
- using the invite-code UI should continue the normal online/Steam join path
- using the direct-address button should open the direct join form and use the offline/direct path for that join only

If BepInEx loaded the plugin successfully, `BepInEx\LogOutput.log` should contain:

```text
Loaded Planet Crafter Direct Join Unlock.
```

When the direct-address path is used, the log should also contain:

```text
Prepared the direct-address join path without changing Steam or invite-code joining.
```

## Join flow behavior to expect

This plugin is intentionally small and specific. The observable behavior should be:

- **Join menu behavior:** the Steam/invite-code menu stays visible instead of being replaced.
- **Direct join availability:** the direct-address multiplayer button stays visible so players can open the IP/address join form.
- **Offline-direct join mode:** `onlineGame` is forced to `false` only when the direct-address join button is used.
- **Steam flow preservation:** invite-code joining is left on the game's normal online path.

If your workflow depends on a server browser, a new menu, or a different network backend, this plugin does not implement that.

## Troubleshooting

### The plugin does not load

- Confirm the DLL is in `BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll`.
- Make sure you installed **BepInEx 5 x64**, not a different platform build.
- Check `BepInEx\LogOutput.log` for load errors.

### The direct-address button is still missing

- Make sure BepInEx actually loaded the plugin.
- Check the log for `Enabled the direct-address multiplayer button alongside the standard invite-code flow.`
- Test with only this plugin installed to rule out another mod overriding the same intro UI.
- Reopen the game after copying the DLL; BepInEx plugins are not hot-loaded into a running game session.

### Invite-code joining stopped working

This plugin no longer hides or replaces the invite-code flow. If invite-code joining stops working anyway:

- test with only this plugin enabled
- review `BepInEx\LogOutput.log` for exceptions from other mods
- remove this plugin temporarily and compare behavior against a clean BepInEx install

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

The project file is portable: it does not contain hard-coded absolute paths to one workstation.

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

## License

This project is licensed under the **GNU General Public License v3.0**.

See [LICENSE](LICENSE) for the full license text.
