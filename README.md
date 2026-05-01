# MechWar

Blazor WebAssembly prototype for a Battletech-inspired online battlefield map generator.

## What it does

- Renders a hex-tile battlefield in the main view.
- Supports 15 map themes:
  - Lunar
  - Martian
  - Desert
  - Badlands
  - Highlands
  - Lowlands
  - Jungle
  - Tundra
  - Polar
  - Urban / City
  - Alpine / Mountain
  - Grasslands / Savanna
  - Canyon
  - River Delta
  - Moonscape
- Uses internal seeded generation for reproducible map creation logic.
- Builds contiguous terrain regions (hill chains, swamp pockets, river-like strips).
- Adds terrain decals per hex (trees, rocks, craters, urban silhouettes, scrub) with subtle deterministic variation.
- Includes a player mech with turn-based movement: rotate with Left/Right arrows, move forward/backward with Up/Down arrows.
- Supports mech classes (Light/Medium/Heavy) with different movement points per turn.
- Adds movement modes (Walk/Run/Jump) that change per-turn movement budgets and traversal behavior.
- Highlights the player mech front firing arc on the battlefield.
- Runs an enemy AI turn at end of player turn to create a round-based loop.
- Supports basic combat resolution with Battletech-style 2d6 to-hit checks, range/movement modifiers, and armor/internal damage.
- Adds advanced combat effects: front/side/rear hit-location rolls, heat sink management with shutdown checks, and high-heat ammo explosion risk.
- Tracks critical component health (engine, gyro, weapons, jump jets) with destruction rolls and gameplay penalties (movement reduction, to-hit penalties, fire disable).
- Shows retro 1980s "GAME OVER" screen when either mech is destroyed with win/lose messaging and new game option.
- Hover tooltips on each hex now show terrain type, movement/firing effects, and full mech state when a mech occupies that hex.

## How generation works

1. Fill map with a theme base terrain.
2. Apply feature rules as connected regions (with optional linear bias for canyons/rivers/ridges).
3. Run a smoothing pass to remove isolated single tiles.

## Run locally

```powershell
dotnet restore

dotnet run --project .\MechWar.csproj
```

Then open the local URL printed in the console.

## Key files

- `Pages/Home.razor`: UI controls and generated map page.
- `Components/HexMapView.razor`: SVG hex renderer.
- `Services/MapGenerator.cs`: seeded procedural generation.
- `Data/ThemeCatalog.cs`: theme definitions and terrain feature weights.
- `Models/`: map, terrain, and theme data models.

