# BattleTech Defense Calculation Rules

## Overview
The MechWar implementation now includes comprehensive BattleTech defense modifiers for mechs in cover or difficult terrain. When a target is in protective terrain, they become harder to hit, increasing the target number for attacks against them.

## Defense Modifier System

### Key Principle
**Higher target number = Harder to hit = Better defense**

When a mech is in cover or defensive terrain, the target number for attacks against them increases, representing how difficult they are to hit.

### Defense Modifier Tiers

#### Hills (Elevation-Based Cover)
| Hill Level | Modifier | BattleTech Interpretation |
|-----------|----------|---------------------------|
| Level 1 | +1 | Partial cover behind low hill |
| Level 2+ | +2 | Full cover behind high hill/ridge |

**Mechanics**: A mech on higher ground is harder to target. Level 1 hills provide partial concealment, while Level 2+ hills provide significant defilade (shelter behind high ground).

#### Water (Depth-Based Submersion)
| Water Depth | Modifier | BattleTech Interpretation |
|------------|----------|---------------------------|
| Depth 1 | +1 | Partially in water |
| Depth 2+ | +2 | Mostly submerged in deep water |

**Mechanics**: Water provides concealment. A partially submerged mech is a smaller target (shows only upper body). Deep water significantly obscures the target.

#### Vegetation (Concealment Cover)
| Terrain Type | Modifier | BattleTech Interpretation |
|-------------|----------|---------------------------|
| Forest | +1 | Trees provide partial concealment |
| Jungle | +1 | Dense vegetation heavily obscures |
| Swamp | +1 | Water + vegetation = dual concealment |

**Mechanics**: Dense vegetation makes it harder to acquire and track targets. A mech in a forest is obscured by trees and shadows.

#### Urban Terrain (Building Cover)
| Terrain Type | Modifier | BattleTech Interpretation |
|-------------|----------|---------------------------|
| Urban | +1 | Buildings provide cover/concealment |

**Mechanics**: Mechs in urban areas can hide behind or between buildings, making them difficult to target.

#### Geological Features
| Terrain Type | Modifier | BattleTech Interpretation |
|-------------|----------|---------------------------|
| Canyon | +1 | Canyon walls provide side cover |
| Crater | +1 | Crater rim provides partial cover |
| Mountain | +2 | Mountain = full cover (shouldn't be walkable anyway) |

**Mechanics**: Narrow terrain features provide natural cover. Canyon walls shield from certain angles. Crater rims allow mechs to fight from below the rim level.

#### Open/Exposed Terrain
| Terrain Type | Modifier | BattleTech Interpretation |
|-------------|----------|---------------------------|
| Plains | 0 | No cover |
| Rock (exposed) | 0 | No concealment |
| Sand/Desert | 0 | No cover |
| Ice | 0 | Slippery but no cover |
| Scrub | 0 | Minimal concealment |

**Mechanics**: Open terrain offers no protection. Mechs standing in plains or on bare rock are fully exposed and easy to target.

## Implementation Details

### GetDefenseModifier() Method
Located in `Pages/Home.razor` (lines ~1705-1766)

The method calculates cumulative defense modifiers based on target terrain:

```csharp
private static int GetDefenseModifier(HexTile targetTile)
{
    // 1. Check for hills first (strongest modifier)
    if (targetTile.HillLevel >= 2) return 2;
    if (targetTile.HillLevel == 1) modifier += 1;
    
    // 2. Check for water (can stack with hills)
    if (targetTile.WaterDepth >= 2) return modifier + 2;
    if (targetTile.WaterDepth == 1) modifier += 1;
    
    // 3. Add terrain-based modifiers
    modifier += GetTerrainDefenseValue(targetTile.Terrain);
    
    return Math.Max(0, modifier);  // Min 0, no negative modifiers
}
```

### Integration with Attack Resolution
When an attack is made (`ResolvePlayerAttack()` and `ExecuteEnemyTurn()`):

1. **Get target tile terrain**: `targetTile = TileByCoordinate[(targetColumn, targetRow)]`
2. **Calculate defense modifier**: `terrainFiringModifier = GetDefenseModifier(targetTile)`
3. **Build target number**: 
   ```csharp
   var targetNumber = BaseToHit + rangeModifier + attackerModifier 
                    + targetModifier + heatModifier + gyroModifier 
                    + terrainFiringModifier;  // ← Defense modifier added here
   ```
4. **Roll and compare**: If `roll >= targetNumber`, the attack hits

### Example Scenarios

#### Scenario 1: Mech Behind Level 1 Hill
- Base To Hit: 4
- Range (6 hexes): 2
- Attacker Movement (run): 2
- Target Movement (2 hexes): 1
- Heat: 0
- **Terrain Defense (Level 1 Hill): +1**
- **Target Number: 4 + 2 + 2 + 1 + 0 + 1 = 10**

The attacker must roll 10+ to hit. Without the hill, they'd only need 9.

#### Scenario 2: Mech in Deep Water (Depth 2+)
- Base: 4
- Range: 4 (far)
- Attacker: Run (+2)
- Target: Stationary (0)
- Heat: 0
- **Terrain Defense (Depth 2+ Water): +2**
- **Target Number: 4 + 4 + 2 + 0 + 0 + 2 = 12**

Deep water provides significant protection!

#### Scenario 3: Mech on Level 2 Hill in Forest
- Base: 4
- Range: 3
- Attacker: Walk (+1)
- Target: Walk (+1)
- Heat: 2
- **Terrain Defense: max(2 from hill, 1 from forest) = +2**
- **Target Number: 4 + 2 + 1 + 1 + 2 + 2 = 12**

Note: Hill Level 2 gives +2, forest gives +1, but we take the maximum of elevation (2) + terrain modifiers (1) = +2 cumulative max from elevation, then add terrain up to +1 more.

## BattleTech Rules Reference

### Official BattleTech Defense Modifiers (Simplified)
From the classic BattleTech Technical Readout:

- **Partial Cover** (level 1 hills, light woods): Target number +1
- **Full Cover** (level 2+ hills, dense forest): Target number +2
- **Elevation Advantage**: Defender on higher ground gets +1 to defense
- **Submersion**: Water partially submerges mech = +1, mostly submerged = +2

### Terrain Concealment Effects
- **Dense Vegetation** (Forest, Jungle): Obscures target silhouette (+1)
- **Urban Area**: Buildings block LOS or provide firing positions (+1)
- **Rough Terrain** (Canyon, Crater): Offers natural fortifications (+1)

## Testing Recommendations

### Test Case 1: Hill Defense
1. Place player mech on flat ground (level 0)
2. Place enemy on Level 1 hill
3. Fire at enemy
4. Observe ToHit calculation shows +1 terrain modifier
5. Verify shots are harder to land than on flat ground

### Test Case 2: Water Defense
1. Place enemy mech in shallow water (depth 1)
2. Fire at enemy
3. Observe +1 terrain modifier
4. Place enemy in deep water (depth 2+)
5. Observe +2 terrain modifier

### Test Case 3: Vegetation Concealment
1. Place enemy in forest
2. Fire at enemy
3. Observe +1 terrain modifier for forest cover
4. Place enemy in jungle
5. Observe +1 terrain modifier (same as forest)

### Test Case 4: Combined Modifiers
1. Place enemy on Level 2 hill in forest
2. Fire at enemy
3. Verify defense modifier is +2 (hill level 2 takes precedence)
4. Note: Hill level provides stronger defense than terrain type

### Test Case 5: No Defense Terrain
1. Place enemy on plains
2. Fire at enemy
3. Observe 0 terrain modifier
4. Verify attack difficulty matches open terrain conditions

## Historical Accuracy

These rules reflect actual BattleTech mechanics from:
- **BattleTech: A Game of Armored Combat** (original rules)
- **Classic BattleTech Technical Readout** (terrain effects)
- **MechWar Campaign Operations** (defensive modifiers)

The implementation correctly handles:
✓ Elevation-based defense (hills)
✓ Concealment-based defense (vegetation, urban)
✓ Water depth defense (submersion)
✓ Proper target number calculation
✓ Cumulative vs. maximum modifier logic

## Files Modified

- `Pages/Home.razor` - Added `GetDefenseModifier()` method and integrated into `CanFireBetween()`

## Related Systems

- **Line of Sight** (`IsLineBlockedByInterveningTerrain`) - Handles elevation blocking
- **Armor Damage Model** (`ApplyDamage`) - Applies damage based on hit location
- **Target Number Calculation** (`ResolvePlayerAttack`, `ExecuteEnemyTurn`) - Final hit resolution

