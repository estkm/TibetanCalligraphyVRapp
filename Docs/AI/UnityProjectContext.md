# Unity Project Context

<!-- unity-onboarding:generated:start -->

## Project Summary

- Project root: `C:/Users/ryanh/Documents/GitHub/TibetanCalligraphyVRapp`
- Purpose: Tibetan calligraphy VR/MR experience targeting Meta Quest 3 and 3S.
- Last analyzed: 2026-08-13
- Last analyzed commit: `85f503733c768551af6682a4629f642f17385110`

## Confirmed Environment

- Unity version: Unity 6.3, `6000.3.13f1` (`8c4f11e4fb20`).
- Render pipeline: Universal Render Pipeline 17.3.0; project and active quality settings reference URP assets under `Assets/Settings/Project Configuration/`.
- Input system: Input System package 1.19.0 with `activeInputHandler: 1` (new Input System), plus Meta/OVR APIs used directly for the MX Ink stylus.
- Target platforms: Android/Meta Quest is the documented and configured primary target. Android uses IL2CPP, minimum API 32, and target API 34. Standalone is also configured for development.

## Important Packages And Frameworks

| Area | Finding | Confidence | Evidence |
| --- | --- | --- | --- |
| Meta XR | Meta XR SDK All 205.0.0, including Core, Interaction, MR Utility Kit, Platform, Haptics, Audio, and Voice dependencies | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json` |
| OpenXR/XR | OpenXR 1.16.1, Meta OpenXR 2.5.1, XR Management 4.5.4, XR Interaction Toolkit 3.3.1 | Confirmed | `Packages/manifest.json` |
| Hands and MR | XR Hands 1.7.3 and AR Foundation 6.6.1; table detection uses `ARPlaneManager` and Meta Scene permission | Confirmed | package manifest; `Assets/_FSVR/Scripts/TableTracking/ScenePermissionGate.cs` |
| Rendering | URP 17.3.0, Shader Graph, and Visual Effect Graph 17.3.0 | Confirmed | package manifest and graphics settings |
| Animation | DOTween is imported under `Assets/Plugins` and used by first-party gameplay code | Confirmed | `Assets/_FSVR/Scripts/ScrabbleManager.cs`, Android/Standalone define symbols |
| Multiplayer | Multiplayer Center is installed, but no first-party networking usage was found; do not classify the app as multiplayer | Confirmed | package manifest and representative code search |

All resolved packages are registry or built-in packages; no Git, local-file, or embedded package source was found in `packages-lock.json`.

## Directory Structure

| Path | Purpose | Confidence | Evidence |
| --- | --- | --- | --- |
| `Assets/_FSVR/` | Primary application scenes, scripts, gameplay art, letter drawing, table tracking, UI, and test/development scenes | Confirmed | directory and source inspection |
| `Assets/MXInk_Resources/` | Logitech MX Ink stylus input, pose, haptics, and drawing integration using OVR APIs | Confirmed | representative scripts |
| `Assets/Settings/` | URP renderer/pipeline assets and project configuration | Confirmed | graphics asset GUID resolution |
| `Assets/XR/`, `Assets/XRI/`, `Assets/MetaXR/`, `Assets/Oculus/` | XR loader/settings and imported Meta/XRI content; ownership is mixed and should be checked before editing | Likely | directory names, build settings, packages |
| `Assets/Samples/`, `Assets/VRTemplateAssets/` | Imported Unity XR samples/template content, not primary application architecture | Confirmed | sample versions, namespaces, asmdefs |
| `Assets/Plugins/`, `Assets/HQP STUDIOS/`, `Assets/Fantasy Skybox FREE/` | Third-party plugins and art | Confirmed | publisher/plugin structure |
| `Assets/_Recovery/` | Recovery/generated scene; not a production scene | Likely | name and exclusion from Build Settings |

## Assembly Boundaries

| Assembly | Responsibility | Key references | Notes |
| --- | --- | --- | --- |
| `Assembly-CSharp` | All first-party runtime gameplay, table tracking, stylus integration, and most template scripts | UnityEngine, XR/AR Foundation, Meta OVR, DOTween | No first-party `.asmdef`; approximately 30 primary scripts currently share this monolithic default assembly |
| Unity XR sample assemblies | Starter Assets, Hands Demo, Hand Visualizer, Interaction Simulator | XRI/XR Hands packages | Imported sample boundaries; editor assemblies are separated where supplied |

There are no first-party editor or test assemblies. Dependency direction is predominantly scene objects -> `MonoBehaviour` managers/components; several managers communicate through static `Instance` fields.

## Scenes And Startup Flow

- Enabled build scenes, in order:
  1. `Assets/_FSVR/Scenes/TableAR.unity`
  2. `Assets/_FSVR/Scenes/ForeverSnowland Stylus.unity`
- Likely startup scene: `TableAR` (build index 0). It requests Meta Scene permission, enables AR plane detection, lets the user place/confirm a table, stores its transform in the static `TableAnchor`, then loads `ForeverSnowland Stylus` by name.
- Gameplay startup: `FSManager.Awake` checks `TableAnchor.HasValue`, aligns the XR origin with the confirmed real table, and enables the base game. A direct launch of the gameplay scene skips recentering.
- Other scenes under `_FSVR/Scenes/test scenes`, `MISC scenes`, `Jyoti`, samples, and recovery are not enabled production build scenes.
- Build Settings contains disabled duplicate/stale entries, including two entries for `ForeverSnowland.unity` and a missing-path-looking `Assets/Scenes/ForeverSnowlandStylusTmp.unity`; review before release changes.

## Architecture

| Pattern | Finding | Confidence | Evidence |
| --- | --- | --- | --- |
| Scene-wired MonoBehaviours | Inspector references and UnityEvents connect gameplay managers, UI, drawing, table tracking, and XR objects | Confirmed | representative scripts and serialized scenes |
| Manager/singleton pattern | `FSManager` and `LetterGameManager` expose static instances; state is mostly held by scene components | Confirmed | manager scripts |
| Cross-scene handoff | `TableAnchor` is a plain static state carrier used during synchronous scene loading | Confirmed | table tracking scripts |
| Coroutine sequencing | Stroke and letter progression uses `IEnumerator` plus `WaitForSeconds` | Confirmed | `LetterGameManager.cs` |
| Feature organization | Primary scripts are grouped by Letters, TableTracking, Table, NPCs, and Debugging, but remain in one assembly | Confirmed | `Assets/_FSVR/Scripts/` |

No dependency-injection container, ECS/DOTS gameplay, event bus, reactive framework, save-data layer, or custom async framework was found in the inspected first-party code.

## Coding Conventions

- Namespace style: mixed. Some core scripts use `namespace FSVR`; many newer/table/letter/stylus scripts use the global namespace.
- Serialized fields: typically `[SerializeField] private`; some scene-facing gameplay state is public. Naming is mixed between camelCase, `_camelCase`, and PascalCase serialized fields.
- Formatting: Allman-style braces dominate, but tabs/spaces and method-parenthesis spacing are inconsistent across files.
- Async: Unity coroutines for timed gameplay; no `async`/`await` convention established.
- Comments/docs: implementation comments are common around XR/table-tracking behavior; XML documentation is not established.
- Preserve serialized field names or use `FormerlySerializedAs` when renaming to avoid breaking scene/prefab references.

## Testing And Validation

- Unity Test Framework 1.6.0 and Code Coverage 1.3.0 are available transitively through the Development feature set.
- No first-party EditMode or PlayMode test source/assemblies were found.
- No repository CI configuration or documented automated build/test command was found.
- Tests were not run during onboarding. The Unity Editor was not connected, so compilation status, Console state, device behavior, and Play Mode behavior remain unverified.

## Available Unity Tooling

| Capability | Status | Evidence |
| --- | --- | --- |
| Repository/package/settings inspection | available | Local workspace access |
| Unity Editor connection and version query | unavailable | No Unity MCP tools exposed in this session |
| Console, scene hierarchy, Build Settings, asset search | unavailable | No Unity MCP tools exposed; serialized repository data used instead |
| Test listing/running, Play Mode, profiler | unavailable | No Unity MCP tools exposed |

No Unity MCP package or client configuration was found in the project. Work can continue using repository evidence; an Editor connection would add live compilation, Console, scene, and test evidence.

## Important Constraints

- Primary deployment is Quest/Android; preserve Meta Scene permission flow, OpenXR configuration, IL2CPP compatibility, and XR interaction bindings.
- Treat imported Meta/Oculus/XRI/sample/template/plugin assets as vendor-controlled until ownership is confirmed.
- Scenes and prefabs carry extensive Inspector wiring; script/field/type renames are high-risk without Editor validation.
- The working tree already had unrelated modifications and untracked XR/settings metadata at onboarding time; preserve them and distinguish future edits carefully.
- `ProjectSettings/ProjectSettings.asset` contains a serialized credential-like platform field. Its value is intentionally not reproduced here; verify whether it is a Unity placeholder before sharing project settings publicly.

## Unknowns And Confidence

- Unknown: current Unity compilation/Console baseline, because no live Editor tooling was available.
- Unknown: actual Quest 3/3S device runtime behavior and whether the Meta Scene permission/table detection flow succeeds on current firmware.
- Unknown: which non-`_FSVR` content folders are intentionally maintained versus legacy/imported assets.
- Unknown: whether direct gameplay-scene launch is a supported production flow or only a development convenience.
- Risk: scene loading depends on the literal string `ForeverSnowland Stylus`; renaming the scene without updating `ConfirmTablePlacement` would break the handoff.
- Risk: mixed namespace/naming/formatting conventions and the single default assembly make broad refactors harder to validate safely.

## Source Files Inspected

- `README.md`
- `ProjectSettings/ProjectVersion.txt`
- `ProjectSettings/EditorBuildSettings.asset`
- `ProjectSettings/GraphicsSettings.asset`
- `ProjectSettings/QualitySettings.asset`
- selected non-secret fields from `ProjectSettings/ProjectSettings.asset`
- `Packages/manifest.json`
- `Packages/packages-lock.json`
- `Assets/_FSVR/Scenes/TableAR.unity` (serialized text only)
- `Assets/_FSVR/Scenes/ForeverSnowland Stylus.unity` (serialized text only)
- representative scripts under `Assets/_FSVR/Scripts/` and `Assets/MXInk_Resources/Scripts/`
- first-party/sample `.asmdef` inventory, scene inventory, test/CI search, and current Git status

<!-- unity-onboarding:generated:end -->
