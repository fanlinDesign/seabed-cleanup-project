# VRpersonalwork

A Unity VR personal project focused on an underwater environmental interaction experience. The project combines XR grab interactions, simple progression gating, scene feedback, and lightweight environmental storytelling around coral restoration and ocean cleanup.

## Project Overview

This project is a small VR experience built in Unity where the player explores an underwater space and completes a sequence of interactive tasks:

- Start the experience from an intro UI and pass the initial barrier.
- Pick up floating debris and place it into a collection bag to clear the seagrass gate.
- Deliver probiotic objects to coral targets to gradually revive bleached coral.
- Pull barnacles off a sea turtle so it can begin swimming along its waypoint path.

The project reads as a compact prototype for VR interaction design, environmental storytelling, and object-based progression.

## Core Interaction Flow

1. **Start sequence**  
   `GameStartUI` controls the opening UI and the start barrier so the player begins in a guided state.

2. **Trash collection**  
   The player grabs debris objects and places them into a socket-based collection bag. `BagCollector` tracks valid trash items and notifies `SeagrassWallGate` to open the next area once the required amount is collected.

3. **Coral revival**  
   `DropZoneTrigger` detects probiotic objects entering the coral zone, then `CoralReviver` updates revival progress and fades the coral from bleached to restored material.

4. **Barnacle removal and turtle movement**  
   `BarnacleXRBridge` and `BarnaclePullOff` handle XR grabbing and pull-off distance checks. When enough barnacles are removed, `TurtleBarnacleLevel` enables `TurtleWaypointSwim`, causing the turtle to follow a waypoint path.

## Technical Stack

- **Engine**: Unity `6000.0.60f1`
- **Render Pipeline**: Universal Render Pipeline (URP)
- **XR**: OpenXR + XR Interaction Toolkit
- **Input**: Unity Input System
- **Target Platform**: Android / standalone VR workflow

### Key Packages

- `com.unity.inputsystem` `1.14.2`
- `com.unity.render-pipelines.universal` `17.0.4`
- `com.unity.xr.interaction.toolkit` `3.0.10`
- `com.unity.xr.openxr` `1.15.1`
- `com.unity.xr.hands` `1.5.1`

## Project Structure

```text
VRpersonalwork/
|- Assets/
|  |- Audio/                   # ambient and interaction audio clips
|  |- Material/                # underwater, coral, and object materials
|  |- Models/                  # imported 3D models
|  |- Prefabs/                 # gameplay and environment prefabs
|  |- Scenes/
|  |  |- SampleScene.unity     # main playable scene
|  |- Scripts/
|  |  |- Gameplay/             # core VR gameplay scripts
|  |- Settings/                # URP settings and rendering assets
|  |- XR/                      # XR/OpenXR settings assets
|- Packages/
|- ProjectSettings/
```

## Main Scripts

- `Assets/Scripts/Gameplay/GameStartUI.cs`  
  Controls the opening UI and unlocks the starting area.

- `Assets/Scripts/Gameplay/BagCollector.cs`  
  Handles socket-based trash collection and spawns visual bottle contents inside the bag.

- `Assets/Scripts/Gameplay/SeagrassWallGate.cs`  
  Opens the gate after enough trash has been collected.

- `Assets/Scripts/Gameplay/DropZoneTrigger.cs`  
  Detects probiotic placement for coral restoration.

- `Assets/Scripts/Gameplay/CoralReviver.cs`  
  Manages coral restore progress and material fade transition.

- `Assets/Scripts/Gameplay/BarnaclePullOff.cs`  
  Checks pull distance and removes barnacles when the interaction succeeds.

- `Assets/Scripts/Gameplay/TurtleWaypointSwim.cs`  
  Moves the turtle along a waypoint path once the barnacle task is complete.

## How To Open And Run

1. Open the project with **Unity Hub** using Unity version `6000.0.60f1`.
2. Open the main scene: `Assets/Scenes/SampleScene.unity`.
3. Make sure XR/OpenXR packages are installed from `Packages/manifest.json`.
4. If you want to build for a headset, switch the build target to **Android** and confirm the XR/OpenXR settings are enabled.
5. Press Play in Editor for interaction testing, or create an APK build for device testing.

## Repository Notes

For GitHub, the important source-controlled folders are:

- `Assets/`
- `Packages/`
- `ProjectSettings/`

Large generated folders and build outputs such as `Library/`, `Logs/`, `obj/`, APK files, and local cache files should stay out of version control.

## Current State

This repository currently contains one playable main scene and a focused set of custom gameplay scripts for a VR prototype. It is suitable as a portfolio or coursework-style project showing:

- XR grab and socket interactions
- object-driven gameplay progression
- simple event chaining between tasks
- visual state change through material transitions
- environmental storytelling in a VR scene

## Future Improvements

- Add a dedicated title screen and end-state summary UI.
- Replace placeholder project metadata such as product name and company name.
- Add screenshots or a short gameplay GIF for GitHub presentation.
- Split reusable gameplay systems into cleaner feature folders if the project grows.

## Author

Personal Unity VR project by Fan Lin.
