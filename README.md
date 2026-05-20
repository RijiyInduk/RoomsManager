# RoomsManager — Documentation

## Overview

`RoomsManager` is the central Unity manager implemented as a **Singleton** (`DontDestroyOnLoad`) that controls the full lifecycle of rooms (levels) in the game: activating the correct floor/location, procedural object generation, a time-of-day lighting system, and spawning enemies, bosses, chests, and interactive objects.

---

## Architecture: Singleton

```csharp
public static RoomsManager ins;
```

The manager exists as a single instance and is not destroyed on scene load. If a duplicate is created, the old instance is destroyed and replaced by the new one.

---

## Public Fields (Inspector)

### Lighting

| Field | Description |
|-------|-------------|
| `mainDirLight` | Main directional light of the scene |
| `timeDay` | Current time of day: 0=Morning, 1=Day, 2=Evening, 3=Night |
| `inoutDoor` | Location type: `"Indoor"` or `"Outdoor"` |
| `nameRoom` | Current room name (string identifier) |
| `lightsMainRooms[4]` | Main room light sources indexed by time of day |
| `lightsFightRooms[4]` | Combat room light sources indexed by time of day |
| `lightsOtherRooms[4]` | Special lights: Abandoned House, Cave, Fiol, Evening Light |
| `lightsFOtherRooms[4]` | Duplicate special light sources |

### Room Floors (Floor GameObjects)

| Field | Locations |
|-------|-----------|
| `forestFloors[]` | Forest floor variants |
| `caveFloors[]` | Cave floor variants |
| `townFloors[]` | Town floor variants |
| `towerFloors[]` | Tower floor variants |
| `cellarFloors[]` | Cellar floor variants |
| `campFloors[4]` | Camp: 0=Forest, 1=Cave, 2=Town, 3=Tower |
| `otherFloors[10]` | Special: Sawmill, War Camp, Swamp, Graveyard, Abandoned House (×2), Cave Altar, Cave Portal, Town Storage, Lair of Bandits |

### Generation Objects

| Field | Description |
|-------|-------------|
| `positionsSpawn[]` | Spawn point transforms inside the room |
| `forestTiles[] / caveTiles[] / townTiles[] / towerTiles[]` | Decorative tiles placed under objects per biome |
| `eliteTiles[4]` | Elite enemy tiles: 0=Forest, 1=Cave, 2=Town, 3=Tower |
| `bossTiles[]` | Boss arena tiles |
| `enemyElites[14]` | Elite enemies: Rat, Dog, Wolf, Boar, GoblinDagger, GoblinAxe, RobberDagger, RobberMace, Cultist, Skeleton, Zombie, Demon, WaterElem, FireElem |
| `bosses[6]` | Bosses: Forest, Cave, Town, Lich Floor 1/2/3 |
| `campChests[]` | Camp chests |
| `bossChests[]` | Post-boss reward chests |
| `dropBagCasual / dropBagElite` | Drop bags for regular and elite enemies |
| `roomsInteractives[3]` | Interactables: 0=Trader, 1=Skill Altar, 2=Health Altar |
| `obelisks[3] / brokenObelisks[3]` | Obelisks: Forest, Cave, Town |
| `brokenRes[7]` | Breakable resources: Wood (×3), Stone (×3), Leather |

---

## Initialization Methods

### `Start()`
On startup: sets `timeDay` to 0 (Morning), clears lightmaps, deactivates all rooms, disables fog, and turns off the main directional light.

### `ClearLightmaps()`
Fully clears baked lightmaps, light probes, and resets ambient lighting. Called on initialization to prevent lighting artifacts.

### `OffAllSimpleRooms()`
Deactivates all floor GameObjects of every type (forest, cave, town, tower, other, camp) and night walls. Called before activating the target room.

---

## Room Activation

### `CreateSimpleRoom()`
Activates the correct floor GameObject based on the `nameRoom` string value. Supported names:

**Forest:**
- `AmbushForest` — random forest floor (`forestFloors`, Random)
- `SawmillForest` — Sawmill
- `GoblinsCampForest` — Goblins' Camp
- `AbondHouseForest` — Abandoned House (random from 2 variants)
- `SwampForest` — Swamp
- `GraveyardForest` — Graveyard

**Cave:**
- `StoneMineCave`, `GoldMineCave`, `CrystalMineCave`, `AmbushCave` — random cave floor
- `AltarCave` — Cave Altar
- `PortalCave` — Cave Portal

**Town:**
- `AmbushTown` — random town floor
- `CapturedStorageTown` — Captured Storage
- `LairBandits` — Lair of Bandits
- `AbondHouseTown` — Abandoned Town House
- `GraveyardTown` — Town Graveyard

**Tower / Cellar:**
- `FirstFloorRoomTower`, `SecondFloorRoomTower`, `ThirdFloorRoomTower` — random tower floor
- `FirstFloorCellar`, `SecondFloorCellar` — random cellar floor

**Boss Room:**
- Activates the appropriate `otherFloors` entry based on `mapIndex`

### `CreateBossRoom()`
Activates the boss room floor based on `mapIndex`:
- 1–3 (Forest) → `otherFloors[1]` (War Camp)
- 4–6 (Cave) → `otherFloors[6]` (Altar)
- 7–9 (Town) → `otherFloors[9]` (Lair)
- 10–12 (Tower) / 98–99 (Cellar) → random `towerFloors`

---

## Room Entry Methods

Each room has a dedicated Enter method that:
1. Calls `CreateSimpleRoom()` to activate the floor
2. Delegates object creation to the appropriate location manager

| Method | Delegate |
|--------|----------|
| `EnterSawmill()` | `RForMan.ins.CreateSawmillObjs()` |
| `EnterGoblinsCamp()` | `RForMan.ins.CreateGoblinsCampObjs()` |
| `EnterAmbushForest()` | `RForMan.ins.CreateAmbushForestObjs()` |
| `EnterAbondHouse()` | `RForMan.ins.CreateAbondHouseObjs()` |
| `EnterGraveyard()` | `RForMan.ins.CreateGraveyardObjs()` |
| `EnterSwamp()` | `RForMan.ins.CreateSwampObjs()` |
| `EnterStoneMine()` | `RCaveMan.ins.CreateStoneMineObjs()` |
| `EnterGoldMine()` | `RCaveMan.ins.CreateGoldMineObjs()` |
| `EnterCrystalMineCave()` | `RCaveMan.ins.CreateCrystalMineObjs()` |
| `EnterAmbushCave()` | `RCaveMan.ins.CreateAmbushCaveObjs()` |
| `EnterAltarCave()` | `RCaveMan.ins.CreateAltarCaveObjs()` |
| `EnterPortalCave()` | `RCaveMan.ins.CreatePortalCaveObjs()` |
| `EnterCapturedStorageTown()` | `RTownMan.ins.CreateCapturedStorageTownObjs()` |
| `EnterLairBanditsTown()` | `RTownMan.ins.CreateLairBanditsTownObjs()` |
| `EnterAbondHouseTown()` | `RTownMan.ins.CreateAbondHouseTownObjs()` |
| `EnterAmbushTown()` | `RTownMan.ins.CreateAmbushTownObjs()` |
| `EnterGraveyardTown()` | `RTownMan.ins.CreateGraveyardTownObjs()` |
| `EnterFirstFloorRoomTower()` | `RTowerMan.ins.CreateFirstFloorRoomTowerObjs()` |
| `EnterSecondFloorRoomTower()` | `RTowerMan.ins.CreateSecondFloorRoomTowerObjs()` |
| `EnterThirdFloorRoomTower()` | `RTowerMan.ins.CreateThirdFloorRoomTowerObjs()` |
| `EnterFirstFloorRoomCellar()` | `RTowerMan.ins.CreateFirstFloorRoomCellarObjs()` |
| `EnterSecondFloorRoomCellar()` | `RTowerMan.ins.CreateSecondFloorRoomCellarObjs()` |

---

## Procedural Object Generation

All generation methods place a biome-appropriate **tile** (based on `mapIndex`) at the spawn position, then instantiate the target GameObject above it with a Y offset.

### `GenerateRoomEnemy(int rnd, int i, int iE, enemies[], enemiesE[])`
Generates a regular or elite enemy at `positionsSpawn[i]`:
- Rolls `rr = Random.Range(1, 101)`
- If `rr < rnd` AND no elite has spawned yet (`eliteNumber == 1`) → spawn elite enemy + elite tile
- Otherwise → random regular enemy + biome tile

### `GenerateRoomChest(int i, chests[])`
Places a biome tile and a random chest from the provided array at `positionsSpawn[i]`.

### `GenerateRoomRes(int i, res[])`
Places a biome tile and a random resource object (wood/stone/leather) at `positionsSpawn[i]`.

### `GenerateRoomInteractive(int i)`
Places a biome tile and a random interactive object from the `timeIO` list at `positionsSpawn[i]`. The object is removed from the list after spawning to prevent duplicates.

### `GeneratePresset(int a1, int a2)`
Generates a random preset value `pr` in the range [a1, a2] — used to determine the enemy layout within the room.

### `GenerateTileSupplies()`
Spawns a biome tile at `positionsSpawn[7]` (the camp spawn position).

---

## Boss Spawning

### `BossSpawn()`
Activates the boss room, instantiates a random `bossTile`, and spawns the corresponding boss based on `mapIndex`:

| mapIndex | Boss |
|----------|------|
| 1–3 | `bosses[0]` — Forest Boss |
| 4–6 | `bosses[1]` — Cave Boss |
| 7–9 | `bosses[2]` — Town Boss |
| 10 | `bosses[3]` — Lich, Floor 1 |
| 11 | `bosses[4]` — Lich, Floor 2 |
| 12 | `bosses[5]` — Lich, Floor 3 |
| 98–99 | Random from first 3 bosses |

### `SpawnBossTreasure()`
After defeating a boss: destroys all existing chests and spawns the reward:
- Standard maps: `eliteTile` + `bossChests[0]`
- Tower / Cellar (10–12, 98–99): 3 tiles + `bossChests[1]` + Skill Altar + Health Altar
- Forest with active quest (`mapIndex == 2`, forest boss kill quest): `smallMonolythForest` instead of a chest
- Tower Floor 12 (finale): `finalBook` instead of a chest

---

## Camp

### `CreateSupplies()`
Activates the camp floor and background (`campFloors`, `campBG`) based on `mapIndex`, calls `GenerateTileSupplies()`, and instantiates `campChests[0]` at position 7.

### `ButNewGear()`
"New Gear" button handler: destroys all chests and enemy models, resets inventory, and rebuilds the camp.

---

## Time of Day System

### `CheckTimeDay()`
Randomly selects the time of day: `timeDay = Random.Range(0, 4)`.

### `SelectTimeDay()`
Applies lighting based on `inoutDoor` and `timeDay`:

**Indoor:**
- Abandoned houses, Storage, Lair, Tower Floor 1 → `lightsOtherRooms[0]` (warm light)
- Mines, Caves, Tower Floor 2, Cellar Floor 1 → `lightsOtherRooms[1]` (cold cave light)
- Altars, Portals, Tower Floor 3, Cellar Floor 2, BossRoom → `lightsOtherRooms[1]` + `lightsOtherRooms[2]` (mystic)
- Main directional light is always disabled for Indoor

**Outdoor:**

| timeDay | Light Source | mainDirLight Rotation | Color |
|---------|-------------|----------------------|-------|
| 0 — Morning | `lightsMainRooms[0]` | X=10° | White |
| 1 — Day | `lightsMainRooms[1]` | X=60° | Warm yellow |
| 2 — Evening | `lightsMainRooms[2]` | X=160° | Orange |
| 3 — Night | `lightsMainRooms[3]` | X=200° | Light disabled |

### `ChangeColorMainLight()`
Sets the directional light color:
- Morning: `RGB(255,255,255)` — neutral white
- Day: `RGB(255,235,185)` — warm
- Evening: `RGB(255,160,50)` — orange
- Night: `RGB(255,255,255)` — light is disabled

### `ChangeFog()`
Configures `RenderSettings` fog:

| Condition | Fog Color | Density |
|-----------|-----------|---------|
| Indoor — houses/storage | Orange | 0.001 |
| Indoor — mines | Mint | 0.001 |
| Indoor — altars/portals | Purple | 0.003 |
| Outdoor — morning | Golden | 0.002 |
| Outdoor — day | Sky blue | 0.001 |
| Outdoor — evening | Dark brown | 0.001 |
| Outdoor — night | Dark blue | 0.002 |

### `OnWalls()`
Activates night walls (`nightWallsMR`, `frWallsNight`) only when `timeDay == 3`.

---

## Object Destruction

| Method | Description |
|--------|-------------|
| `Destr()` | Destroys objects a1–a6 (bosses, chests, interactives) |
| `Destr0()` | Destroys all objects in the `aa[]` array (procedurally generated) |

---

## Map Index Reference (mapIndex)

| mapIndex | Biome | Difficulty |
|----------|-------|------------|
| 0 | Start scene | — |
| 1–3 | Forest | Easy / Normal / Hard |
| 4–6 | Cave | Easy / Normal / Hard |
| 7–9 | Town | Easy / Normal / Hard |
| 10–12 | Tower | Easy / Normal / Hard |
| 98–99 | Cellar | — |

---

## Dependencies on Other Managers

| Manager | Role |
|---------|------|
| `GumeManager` | Provides `mapIndex`, manages chests and enemies |
| `EnemyManager` | Controls `eliteNumber`, enemy names |
| `InventoryManager` | Resets inventory on camp restart |
| `UIManager` | Closes UI panels |
| `QuestsManager` | Checks quest state when spawning rewards |
| `StoryManager` | Story objects: monolith, final book |
| `RForMan` | Creates objects in forest rooms |
| `RCaveMan` | Creates objects in cave rooms |
| `RTownMan` | Creates objects in town rooms |
| `RTowerMan` | Creates objects in tower and cellar rooms |

