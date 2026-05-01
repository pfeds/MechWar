# BattleTech Line-of-Sight Elevation Fix

## Issue
Previously, mechs behind Level 1 hills could still be targeted, which violated BattleTech line-of-sight (LOS) rules.

## Root Cause
The blocking threshold calculation was incorrect:
```csharp
// OLD (WRONG)
var blockingThreshold = Math.Max(attackerLevel, targetLevel) + 1;
```

This meant:
- If attacker is at level 0 and target is at level 1, threshold = 2
- A level 1 intervening hill would NOT block (since 1 < 2)
- This allowed mechs behind level 1 hills to be targeted

## BattleTech Rules Implemented
In BattleTech:
- **Each elevation level = half a mech's height**
- A mech has an effective height of 2 elevation levels (since each level is half mech height)
- An intervening hex blocks LOS if it is taller than what BOTH the attacker and target can see over
- You can see over terrain lower than yourself

## Solution
```csharp
// NEW (CORRECT)
var attackerSightLevel = attackerLevel + 2;  // Mech's eye level
var targetSightLevel = targetLevel + 2;      // Target's top
var blockingThreshold = Math.Min(attackerSightLevel, targetSightLevel);
```

### How It Works
1. Each mech has a sight line that extends 2 elevation levels above its current terrain
2. The blocking threshold is the **minimum** of the two sight lines
3. Intervening terrain blocks if it meets or exceeds this threshold

### Examples
| Scenario | Attacker | Target | Intervening | Blocks? | Reason |
|----------|----------|--------|-------------|---------|--------|
| Level 1 hill blocking | Level 0 | Level 1 | Level 1 | YES | min(0+2, 1+2) = 2; hill ≥ 2 |
| Level 0 intervening | Level 0 | Level 0 | Level 0 | NO | min(0+2, 0+2) = 2; 0 < 2 |
| Mixed terrain | Level 0 | Level 2 | Level 1 | NO | min(0+2, 2+2) = 2; 1 < 2 |
| High ground shooting | Level 2 | Level -1 (water) | Level 0 | NO | min(2+2, -1+2) = 1; 0 < 1 |

## Files Modified
- `Pages/Home.razor` - `IsLineBlockedByInterveningTerrain()` method (lines 1742-1783)

## Testing Recommendations
1. Place a mech on flat ground (level 0)
2. Place another mech on a Level 1 hill
3. Try to fire between them through an intervening Level 1 hill
4. Verify LOS is now blocked (previously would have worked)
5. Try firing with direct line (no intervening terrain) - should work
6. Try firing through Level 0 terrain - should work

## Related Code
- `GetTileLevel()` (lines 1785-1803) - Calculates terrain level from tile data
- `CanFireBetween()` (lines 1713-1748) - Main LOS check that calls this method
- `HexTile` model - Contains `HillLevel` and `WaterDepth` properties

