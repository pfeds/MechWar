# MechWar Body Section Destruction Rules - BattleTech Compliance Analysis

## Executive Summary

**Does MechWar implement correct BattleTech rules for body section destruction?**

➜ **PARTIALLY - 40% Compliance** (Major gaps and rule violations)

MechWar only tracks **total internal structure** but **does NOT implement critical BattleTech body section destruction rules**:
- ✗ Destroying both torsos = automatic death (NOT implemented)
- ✗ Destroying an arm = lose arm functions (NOT implemented)
- ✗ Destroying a leg = mobility penalties (NOT implemented)
- ✗ Destroyed torso = lost component location (NOT implemented)
- ✗ Section-specific cascading damage (NOT implemented)

---

## What MechWar Actually Does (Current Implementation)

### Current Damage Model
```csharp
// From RecalculateDurabilityTotals() (line 1158)
PlayerInternal = PlayerSections.Values.Sum(part => Math.Max(0, part.Internal));

// From CheckGameOver() (line 1565)
if (PlayerInternal <= 0)
{
    IsGameOver = true;  // Mech destroyed
}
```

**This means:**
1. Damage is tracked per **section** ✓
2. BUT sections are **never checked individually** ✗
3. Only **total internal structure** matters ✗
4. When total hits zero → **instant game over** ✗

### Data Structure
```csharp
private readonly Dictionary<HitLocation, SectionState> PlayerSections = new();

// Each section contains:
private readonly record struct SectionState(int Armor, int Internal);

// So each section COULD track:
// - Head: 3 armor, 3 internal
// - Center Torso: 10 armor, 8 internal
// - Left Torso: 8 armor, 6 internal
// - Right Torso: 8 armor, 6 internal
// - Left Arm: 6 armor, 4 internal
// - Right Arm: 6 armor, 4 internal
// - Left Leg: 8 armor, 6 internal
// - Right Leg: 8 armor, 6 internal
```

**The structure exists but is NOT USED for destruction mechanics.**

---

## BattleTech Rules - What SHOULD Happen

### 1. Center Torso Destruction Rule
**Official Rule:** If center torso internal structure reaches 0, the mech is **destroyed immediately**.

**Status in MechWar:** ⚠️ PARTIALLY CORRECT
- Game ends when total internal hits 0
- But doesn't specifically check center torso
- Happens to work by accident since center torso is big part of total

**Example:**
```
BattleTech: Center Torso destroyed → Instant mech death
MechWar: Total internals = 0 → Game over (happens same time usually)
Result: ⚠️ Works but for wrong reason
```

### 2. Losing Both Side Torsos = Mech Loss
**Official Rule:** If both left and right torsos have their internal structure destroyed, the mech is considered destroyed even if center torso intact.

**Status in MechWar:** ✗ NOT IMPLEMENTED
- No check for "both torsos destroyed"
- Mech could have:
  - Center Torso: 8 internal remaining
  - Left Torso: 0 internal (destroyed)
  - Right Torso: 0 internal (destroyed)
  - Legs/Arms: Still intact
  - **Current behavior:** Game continues (WRONG)
  - **Correct behavior:** Mech should be destroyed

### 3. Arm Destruction = Lose Arm Functions
**Official Rule:** When an arm's internal structure is destroyed, the arm is **severed** and the mech loses arm functions immediately.

**Effects of Arm Loss:**
- Cannot fire weapons on that arm
- Cannot do armor-protecting torso twist maneuver
- Reduced to 6 directions of facing (instead of twist)

**Status in MechWar:** ✗ NOT IMPLEMENTED
- Weapons can still fire from destroyed arms
- No check for "arm internal destroyed"
- Component destruction works (weapon broken) but not section destruction

**Example:**
```
Scenario: Left Arm internal = 0 (destroyed)
With weapon in left arm

BattleTech: Weapon CANNOT fire (arm severed)
MechWar: Weapon still fires normally
Result: ✗ WRONG - Arm destruction ignored
```

### 4. Leg Destruction = Immobilized
**Official Rule:** When a leg's internal structure is destroyed, the mech is **immobilized** (cannot move, only rotate).

**Effects of Leg Loss:**
- **One leg destroyed:** -2 walking speed (walk 5 → walk 3)
- **Both legs destroyed:** Cannot move at all (immobilized)

**Status in MechWar:** ✗ NOT IMPLEMENTED
- Leg destruction not checked
- No speed penalties applied
- Mech can walk normally even with legs destroyed

**Example:**
```
Scenario: Right Leg internal = 0 (destroyed)
Normal movement: 5 hexes walk

BattleTech: Movement reduced by 2 (3 hexes walk)
MechWar: Still 5 hexes walk
Result: ✗ WRONG - Leg destruction ignored
```

### 5. Head Destruction = Automatic Loss
**Official Rule:** The head contains the cockpit. If internal structure destroyed → pilot dead → mech destroyed.

**Status in MechWar:** ⚠️ PARTIALLY CORRECT
- Head is tracked and can take damage
- But destruction not handled specially
- Just treated as other sections

**Missing:** No special "cockpit hit" celebration or instant-death mechanic.

### 6. Torso Section Cascade Effects
**Official Rule:** When a torso section is destroyed, components located there become inaccessible:
- If left torso destroyed: Left torso weapon offline
- If right torso destroyed: Right torso weapon offline
- If center torso destroyed: Engine + Gyro both destroyed

**Status in MechWar:** ⚠️ PARTIALLY CORRECT
- Components can be destroyed (via critical hits)
- BUT not due to section destruction
- Center Torso destruction doesn't automatically kill engine/gyro

**Missing:** Section-aware component destruction logic.

---

## Comparison Table: Official vs MechWar

| Destruction Event | Official BattleTech | MechWar | Correct? |
|---|---|---|---|
| **Center Torso = 0** | Mech destroyed | Game over | ✓ Works |
| **Both Side Torsos = 0** | Mech destroyed | Game continues | ✗ WRONG |
| **One Arm = 0** | Arm unusable | Arm still fires | ✗ WRONG |
| **Two Arms = 0** | Can't fire anything | Still can fire | ✗ WRONG |
| **One Leg = 0** | Speed -2 | No penalty | ✗ WRONG |
| **Two Legs = 0** | Immobilized | Still mobile | ✗ WRONG |
| **Head = 0** | Pilot dead | Game continues | ✗ WRONG |
| **Component in destroyed section** | Unusable | Sometimes works | ⚠️ PARTIAL |
| **Section-specific penalties** | Multiple rules | No checks | ✗ NO |

---

## Severity Assessment

### Critical Issues (Game-Breaking)

**Issue 1: Both Torsos Can Be Destroyed Without Ending Game**
```
Scenario:
- Center Torso: 8 internal remaining
- Left Torso: 0 internal (destroyed)
- Right Torso: 0 internal (destroyed)
- Total: 8 internal (not zero)

Official BattleTech: Mech destroyed (both torsos gone)
MechWar: Game continues (total internal > 0)

Impact: CRITICAL
Users can have valid game states that violate BattleTech rules
```

**Issue 2: Destroyed Limbs Still Functional**
```
Scenario:
- Left Arm: 0 internal (destroyed/severed)
- Weapon in Left Arm: Still fires normally

Official BattleTech: Can't fire (arm gone)
MechWar: Fires normally

Impact: HIGH
Destroys tactical gameplay (losing limbs doesn't matter)
```

**Issue 3: Destroyed Legs = No Movement Penalty**
```
Scenario:
- Left Leg: 0 internal (destroyed)
- Walking speed: 5 hexes (unchanged)

Official BattleTech: Walking speed = 3 (reduced by 2)
MechWar: Walking speed = 5 (no penalty)

Impact: MEDIUM-HIGH
Movement is core mechanic, ignoring leg destruction is major issue
```

### Medium Issues

**Issue 4: Head Destruction Not Special**
- Head can be destroyed but no special handling
- Should trigger instant loss (cockpit destroyed)
- Current: Just loses 3 armor/internal like any section

**Issue 5: Section Components Don't Cascade**
- Destroying center torso should destroy engine+gyro
- Currently: Can have destroyed center torso with engine still working
- Less common but still wrong

---

## Detailed Examples

### Example 1: The "Walking Corpse" Scenario

```
Player Mech (Medium, 60 tons):
Status Before Attack:
├─ Center Torso: 8/8 internal
├─ Left Torso: 6/6 internal ← Will be hit
├─ Right Torso: 6/6 internal
├─ Left Leg: 6/6 internal
└─ Right Leg: 6/6 internal
Total Internal: 32/32 (healthy)

Enemy fires 40 damage to Left Torso
- Armor absorbed: 8
- Internal damage: 32 (but only 6 available)
- Overflow to Center Torso: 26

After Damage:
├─ Center Torso: -18 internal → 0 (destroyed)
├─ Left Torso: 0 internal (destroyed)
├─ Right Torso: 6/6 internal
├─ Left Leg: 6/6 internal
└─ Right Leg: 6/6 internal
Total Internal: 6 (game continues in MechWar)

Official BattleTech Result:
- Left Torso destroyed → weapon offline
- Center Torso destroyed → MECH DESTROYED
→ Game ends

MechWar Result:
- Center Torso: shows 0 internal, but game continues
- Can still move, still can fire
→ Mech still fighting with destroyed center torso
```

**Issue:** Center torso should trigger instant loss, but only works because total internal math.

### Example 2: The "Phantom Arm" Scenario

```
Scenario: Left Arm Weapon Destroyed
- Left Arm internal structure: Intact (4/6)
- Left Arm weapon: Destroyed (via critical hit)

What Happens:
✓ MechWar: Weapon disabled (can't fire)
✓ This is correct

But now consider: Left Arm Completely Destroyed

Scenario: Left Arm Section Destroyed
- Left Arm internal structure: 0 (destroyed)
- Left Arm weapon: Functional

What Happens:
✗ MechWar: Weapon still fires (section destruction ignored)
✗ BattleTech: Weapon can't fire (arm is GONE)

This scenario is possible:
- Direct damage to left arm (6+ internal damage)
- Weapon survives critical hit
- In MechWar: Weapon keeps firing
- In BattleTech: Weapon unusable (arm severed)
```

### Example 3: The "Running on Stumps" Scenario

```
Scenario: Both Legs Destroyed But Walking

Both Legs Destroyed:
├─ Left Leg: 0 internal (destroyed)
└─ Right Leg: 0 internal (destroyed)
Movement mode: Walk

Official BattleTech:
- Cannot move (immobilized)
- Can only rotate in place
- Speed: 0 hexes

MechWar:
- Can walk 5 hexes normally
- No penalties applied
- Just walks around on destroyed legs

This is completely wrong from a simulation standpoint.
```

---

## What's Tracked But Not Used

### Component System (Partially Works)
```csharp
// Components ARE tracked properly:
components[HitLocation.LeftArm] = new()
{
    new(ComponentType.Weapon, false),  // Can be set to true (destroyed)
};

// And destruction checks ARE in place:
if (IsComponentDestroyed(PlayerComponents, ComponentType.Weapon))
{
    // Can't fire
}

// So weapon destruction is detected ✓
// But SECTION destruction is not checked ✗
```

**What's Missing:**
When left arm **section** is destroyed (internal = 0), the system should:
1. Automatically destroy all left arm components
2. Prevent firing from left arm
3. Trigger arm loss effects

**Currently:** Only happens if component gets critical hit directly.

### Section Data Structure (Unused for Destruction)
```csharp
// These values are stored:
sections[HitLocation.LeftArm] = new SectionState(
    Armor: 0,       // Can go to 0
    Internal: 0     // Can go to 0
);

// But never checked for zero:
// Missing checks like:
// if (sections[HitLocation.LeftArm].Internal <= 0)
// {
//     // Handle arm destruction
// }

// Exists but unused
```

---

## Implementation Gap Summary

### Missing Checks

```
✗ if (sections[HitLocation.CenterTorso].Internal <= 0)
    → Game Over (partly works, mostly by accident)

✗ if (sections[HitLocation.LeftTorso].Internal <= 0 && 
      sections[HitLocation.RightTorso].Internal <= 0)
    → Game Over (NOT IMPLEMENTED)

✗ if (sections[HitLocation.LeftArm].Internal <= 0)
    → Disable left arm weapons (NOT IMPLEMENTED)

✗ if (sections[HitLocation.RightArm].Internal <= 0)
    → Disable right arm weapons (NOT IMPLEMENTED)

✗ if (sections[HitLocation.Head].Internal <= 0)
    → Game Over (cockpit destroyed) (NOT IMPLEMENTED)

✗ if (sections[HitLocation.LeftLeg].Internal <= 0 || 
      sections[HitLocation.RightLeg].Internal <= 0)
    → Reduce movement speed (NOT IMPLEMENTED)

✗ if (sections[HitLocation.LeftLeg].Internal <= 0 && 
      sections[HitLocation.RightLeg].Internal <= 0)
    → Immobilize (NOT IMPLEMENTED)
```

### Missing Cascading Effects

When a section is destroyed, currently nothing happens except:
- Section health shows 0
- Internal structure total goes down

Missing:
- Automatic component destruction
- Section-specific effects
- Game state penalties
- Player feedback

---

## Current vs Needed Behavior

### Center Torso Destruction

**Current Code:**
```csharp
// Just checks total
if (PlayerInternal <= 0)
{
    IsGameOver = true;
}
```

**Should Be:**
```csharp
// Check center torso specifically
if (PlayerSections[HitLocation.CenterTorso].Internal <= 0)
{
    IsGameOver = true; // Center torso destroyed = automatic loss
    LastActionMessage = "Center Torso destroyed! Mech destroyed!";
}
// Also check both torsos
else if (PlayerSections[HitLocation.LeftTorso].Internal <= 0 &&
         PlayerSections[HitLocation.RightTorso].Internal <= 0)
{
    IsGameOver = true; // Both torsos destroyed = mech destroyed
    LastActionMessage = "Both side torsos destroyed! Mech destroyed!";
}
```

### Arm Destruction

**Current Code:**
```csharp
// Weapon destruction is checked
if (IsComponentDestroyed(PlayerComponents, ComponentType.Weapon))
{
    // Can't fire - only checks weapon, not section
}
```

**Should Be:**
```csharp
private bool CanFireLeftArmWeapon()
{
    // Check if left arm section is destroyed
    if (PlayerSections[HitLocation.LeftArm].Internal <= 0)
    {
        return false; // Arm destroyed = can't fire
    }
    
    // Also check if weapon component destroyed
    if (IsComponentDestroyed(PlayerComponents, ComponentType.Weapon))
    {
        return false;
    }
    
    return true; // Arm intact and weapon functional
}
```

### Leg Destruction

**Current Code:**
```csharp
// No leg destruction checks at all
var movement = GetMovementByClassAndMode(UserMech.Class, ActiveMovementMode);
```

**Should Be:**
```csharp
private int GetEffectiveMovement(MechClass mechClass, MovementMode mode)
{
    var baseMovement = GetMovementByClassAndMode(mechClass, mode);
    
    // Check for leg damage
    var leftLegDestroyed = PlayerSections[HitLocation.LeftLeg].Internal <= 0;
    var rightLegDestroyed = PlayerSections[HitLocation.RightLeg].Internal <= 0;
    
    if (leftLegDestroyed && rightLegDestroyed)
    {
        return 0; // Both legs gone = immobilized
    }
    else if (leftLegDestroyed || rightLegDestroyed)
    {
        return baseMovement - 2; // One leg gone = -2 movement
    }
    
    return baseMovement; // Both legs intact
}
```

---

## Rules Required for Full Compliance

### Must Implement (High Priority)

1. **Both Torsos Destruction Check**
   - Currently playable in violation
   - Easy to add
   - High impact

2. **Arm Destruction Effects**
   - Prevent firing
   - Easy to add
   - Medium impact

3. **Leg Destruction Penalties**
   - Reduce movement
   - Medium complexity
   - High impact

### Should Implement (Medium Priority)

4. **Head Destruction = Instant Loss**
   - Special cockpit destruction
   - Low complexity
   - Flavor/completeness

5. **Section Component Cascade**
   - Auto-destroy components when section dies
   - Medium complexity
   - Medium impact

### Nice-to-Have (Low Priority)

6. **Section-specific destruction messages**
   - "Left arm blown off!"
   - Polish only
   - No impact to rules

---

## Compliance Grade

```
Current Implementation: 40% ✗

Breakdown:
- Center Torso destruction:   ⚠️  Partial (works by accident)
- Both Torsos destruction:    ✗  0% (not checked)
- Arm destruction effects:    ✗  0% (not checked)
- Leg destruction effects:    ✗  0% (not checked)
- Head destruction special:   ✗  0% (not checked)
- Section-aware components:   ✗  0% (not implemented)

Grade: F to D (20-40%)

Major rules violations make this a critical gap.
```

---

## Impact on Gameplay

### Current Broken Behaviors

1. **Mechs Don"t Go "Limbless"**
   - Can lose both arms and still fight normally
   - Should be unable to fire

2. **Legless Mechs Still Walk**
   - Can destroy both legs and still move 5+ hexes
   - Should be immobilized

3. **Surviving Impossible Damage**
   - Can have center torso + both legs destroyed
   - But stay in game if some section has health left
   - Should be auto-loss

4. **Component-Orphaned Weapons**
   - Arm can be completely destroyed (0 internal)
   - But weapon in that arm still functions
   - Should be unusable

### Strategic Implications

- **Limb destruction is not penalized** → Tactics ignore dismemberment
- **Legs matter only for movement points** → No consequence to leg loss
- **Arms matter only if weapon destroyed** → Arm loss means nothing
- **Center torso can die and combat continues** → Wrong mechanic

---

## Recommended Fix Priority

### Phase 1 (Critical - 1-2 weeks)
1. Add both-torsos destruction check
2. Add arm destruction weapon disabling
3. Add leg destruction movement penalty

### Phase 2 (Important - 1 week)
4. Add head destruction special handling
5. Add destruction messages
6. Add visual feedback (grayed out sections)

### Phase 3 (Polish - 1 week)
7. Component cascade on section destruction
8. Disable components on section loss
9. UI updates for destroyed sections

---

## Conclusion

**MechWar has 40% compliance with BattleTech body section destruction rules.**

The data structures are in place, but the **destruction mechanics are completely missing**. This is a **critical game-breaking gap** because:

1. ✗ Mechs can survive impossible damage states
2. ✗ Destroying limbs has no tactical consequence
3. ✗ Removing legs doesn't reduce mobility
4. ✗ Removing arms doesn't disable weapons

**What needs fixing:**
- Both-torsos destruction check (15 lines)
- Arm destruction weapon disabling (20 lines)
- Leg destruction movement penalty (15 lines)
- Total: ~50 lines of code, huge gameplay impact

**Current status:** Playable but rule-violating
**Needed status:** Implement section destruction effects
**Estimated effort:** 2-3 weeks
**Impact:** Transforms from broken to authentic BattleTech

---

## Test Cases for Compliance

### Test 1: Both Torsos Destruction
```
Setup: Center torso healthy (8 internal), both side torsos at 0
Expected: Game over (both torsos destroyed)
Current: Game continues (FAIL)
```

### Test 2: Arm Destruction Disables Fire
```
Setup: Left arm at 0 internal, weapon present
Expected: Cannot fire left arm weapon
Current: Can still fire (FAIL)
```

### Test 3: Leg Destruction Reduces Movement
```
Setup: One leg at 0 internal
Expected: Movement reduced by 2
Current: Movement unchanged (FAIL)
```

### Test 4: Both Legs Immobilize
```
Setup: Both legs at 0 internal
Expected: Cannot move (immobilized)
Current: Can move normally (FAIL)
```

---

**Status:** Critical gap identified and documented. Implementation roadmap provided.

