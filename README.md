# Planet Crafter Direct Join Unlock

`Planet Crafter Direct Join Unlock` is a small BepInEx plugin for **The Planet Crafter** that keeps the direct-address multiplayer path available so players can connect to a server by IP instead of relying on the invite-code join flow.

## What the mod does

This plugin patches the game at runtime to:

- re-enable the multiplayer button on the intro/main menu when it would otherwise be hidden
- disable the invite-code-only join menu so the direct-address join flow is used instead
- force the client into the direct/offline UnityTransport join path before the game runs the normal multiplayer join action

In practical terms, it is meant to make **direct IP joining** available to players connecting to a self-hosted or custom-hosted Planet Crafter server setup.

## Requirements

Before installing this plugin, you need:

1. **A Windows copy of The Planet Crafter**
2. **BepInEx 5 x64** installed into the game
3. The plugin DLL from this repository:
   - `package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/PlanetCrafterDirectJoinUnlock.dll`

This repository does **not** bundle the full BepInEx distribution. Install BepInEx first, then install this plugin.

## Repository layout

- `src/PlanetCrafterDirectJoinUnlock/` - source code and project file
- `package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/` - packaged plugin layout for manual installation

## Step 1: Find your Planet Crafter install folder

If you use Steam:

1. Open **Steam**
2. Open your **Library**
3. Right-click **The Planet Crafter**
4. Select **Manage** -> **Browse local files**

Steam will open the game folder that contains `Planet Crafter.exe`.

You need that exact folder for both BepInEx and this plugin.

## Step 2: Install BepInEx

### Download BepInEx

Download a **BepInEx 5 x64** release suitable for Windows. If you are unsure, use the current BepInEx 5 Windows x64 package from the official BepInEx releases or a trusted mod distribution page.

### Extract BepInEx into the game folder

Open the BepInEx ZIP and extract its contents directly into the same folder as `Planet Crafter.exe`.

After extraction, your game folder should contain items like:

```text
Planet Crafter.exe
winhttp.dll
doorstop_config.ini
BepInEx\
```

Do **not** extract BepInEx into a subfolder like `Planet Crafter\BepInExPack\...`. The files must end up directly beside `Planet Crafter.exe`.

### First launch after BepInEx install

Launch the game once, then close it.

This first run confirms BepInEx is loading and creates the normal plugin/log structure.

## Step 3: Install Planet Crafter Direct Join Unlock

### Manual install from this repository

Copy the plugin DLL from:

```text
package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/PlanetCrafterDirectJoinUnlock.dll
```

into your game folder at:

```text
<Planet Crafter folder>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll
```

If the `PlanetCrafterDirectJoinUnlock` folder does not exist yet under `BepInEx\plugins\`, create it first.

### Final expected path

Example final path:

```text
C:\Games\The Planet Crafter\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.dll
```

## Step 4: Launch and verify

Start **The Planet Crafter** normally.

If the plugin loaded correctly:

- BepInEx should load before the game menu appears
- the plugin should write a startup message to the BepInEx log
- the multiplayer entry on the intro/main menu should remain available
- the invite-code-only join menu behavior should be suppressed in favor of the direct join path

## How to verify the mod loaded

Check the BepInEx log file after launching the game.

Common places to look:

```text
<Planet Crafter folder>\BepInEx\LogOutput.log
```

Look for a line similar to:

```text
Loaded Planet Crafter Direct Join Unlock.
```

## Troubleshooting

### BepInEx does not seem to load at all

Symptoms:

- no `BepInEx` log file
- no BepInEx console/logging output
- plugin appears to do nothing

Checks:

1. Make sure `winhttp.dll`, `doorstop_config.ini`, and the `BepInEx` folder are directly beside `Planet Crafter.exe`
2. Make sure you installed a **Windows x64** BepInEx 5 build
3. Launch the game once after extracting BepInEx

### The plugin DLL is installed but nothing changes in game

Checks:

1. Confirm the DLL is in:

   ```text
   <Planet Crafter folder>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\
   ```

2. Check the BepInEx log for:
   - plugin load success
   - missing dependency errors
   - Harmony patch errors

### The game updated and the mod stopped working

This mod relies on runtime patching of game classes and methods. If the game updates and renames or changes those members, the plugin may stop working until it is updated.

If that happens:

1. keep a copy of the BepInEx log
2. open an issue in this repository
3. include your game version and any relevant log lines

### I want to remove the mod

Delete:

```text
<Planet Crafter folder>\BepInEx\plugins\PlanetCrafterDirectJoinUnlock\
```

Then launch the game again.

## Building from source

### Prerequisites

- .NET SDK 8 or newer
- BepInEx installed somewhere on disk
- a local Planet Crafter install so you have access to the game's managed assemblies

### Required reference folders

You need two local paths when building:

1. **BepInEx core directory**

   Example:

   ```text
   C:\Games\The Planet Crafter\BepInEx\core
   ```

2. **Planet Crafter managed assemblies directory**

   Example:

   ```text
   C:\Games\The Planet Crafter\Planet Crafter_Data\Managed
   ```

### Build command

From the repository root, run:

```powershell
dotnet build .\src\PlanetCrafterDirectJoinUnlock\PlanetCrafterDirectJoinUnlock.csproj `
  -c Release `
  /p:BepInExCoreDir="C:\Games\The Planet Crafter\BepInEx\core" `
  /p:PlanetCrafterManagedDir="C:\Games\The Planet Crafter\Planet Crafter_Data\Managed"
```

### Build output

The project is configured to place the built DLL into the packaged plugin layout:

```text
package/BepInEx/plugins/PlanetCrafterDirectJoinUnlock/
```

That means a successful build gives you a ready-to-copy plugin folder.

## Notes

- This repo only packages the direct-join plugin itself
- BepInEx is still a separate prerequisite
- This project is meant to be manually installable without a mod manager
