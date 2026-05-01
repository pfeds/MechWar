# Body Section Destruction - Implementation Guide

## Quick Summary

**Current State:** Section data exists but destruction mechanics are completely missing

**What's Wrong:**
- ✗ Destroying both torsos doesn't end game
- ✗ Destroying arms doesn't disabled weapons
- ✗ Destroying legs doesn't reduce movement
- ✗ Destroying head doesn't cause special loss

**Why It Matters:** Core BattleTech mechanic completely broken

---

## Implementation Roadmap

### Phase 1: Core Mechanics (1 week)

#### Task 1.1: Both Torsos Destruction Check
**What:** Add check for both side torsos destroyed
**Where:** In `CheckGameOver()` method
**Impact:** Fixes major rule violation

```csharp
// Current (line 1565):
private void CheckGameOver()
{
    if (PlayerInternal <= 0)
    {
        IsGameOver = true;
        PlayerWon = false;
    }
    else if (EnemyInternal <= 0)
    {
        IsGameOver = true;
        PlayerWon = true;
    }
}

// Need to add BEFORE the total check:
// 1. Check center torso destroyed → instant loss
// 2. Check both side torsos destroyed → instant loss
// 3. Check head destroyed → instant loss
```

**Code needed:**
```csharp
private void CheckGameOver()
{
    // CHECK 1: Center Torso Destroyed
    if (PlayerSections[HitLocation.CenterTorso].Internal <= 0)
    {
        IsGameOver = true;
        PlayerWon = false;
        LastActionMessage = "Center Torso destroyed! Mech destroyed!";
        return;
    }
    
    if (EnemySections[HitLocation.CenterTorso].Internal <= 0)
    {
        IsGameOver = true;
        PlayerWon = true;
        LastActionMessage = "Enemy Center Torso destroyed! Enemy mech destroyed!";
        return;
    }
    
    // CHECK 2: Both Side Torsos Destroyed
    if (PlayerSections[HitLocation.LeftTorso].Internal <= 0 &&
        PlayerSections[HitLocation.RightTorso].Internal <= 0)
    {
        IsGameOver = true;
        PlayerWon = false;
        LastActionMessage = "Both side torsos destroyed! Mech destroyed!";
        return;
    }
    
    if (EnemySections[HitLocation.LeftTorso].Internal <= 0 &&
        EnemySections[HitLocation.RightTorso].Internal <= 0)
    {
        IsGameOver = true;
        PlayerWon = true;
        LastActionMessage = "Enemy both side torsos destroyed! Enemy mech destroyed!";
        return;
    }
    
    // CHECK 3: Head Destroyed (Cockpit)
    if (PlayerSections[HitLocation.Head].Internal <= 0)
    {
        IsGameOver = true;
        PlayerWon = false;
        LastActionMessage = "Head destroyed! Pilot dead! Mech destroyed!";
        return;
    }
    
    if (EnemySections[HitLocation.Head].Internal <= 0)
    {
        IsGameOver = true;
        PlayerWon = true;
        LastActionMessage = "Enemy head destroyed! Enemy pilot dead!";
        return;
    }
    
    // ORIGINAL CHECK: Total Internal (fallback)
    if (PlayerInternal <= 0)
    {
        IsGameOver = true;
        PlayerWon = false;
        return;
    }
    
    if (EnemyInternal <= 0)
    {
        IsGameOver = true;
        PlayerWon = true;
        return;
    }
}
```

**Effort:** 20 lines of code
**Impact:** HIGH - Fixes critical rule violations

---

#### Task 1.2: Arm Destruction Weapon Disabling
**What:** Check if arm is destroyed before allowing fire
**Where:** In `ResolvePlayerAttack()` and `ExecuteEnemyTurn()`
**Impact:** Fixes limb destruction mechanic

**Current Code (line 738):**
```csharp
private async Task ResolvePlayerAttack()
{
    if (!IsPlayerPhase || !CanPlayerFire || UserMech is null || EnemyMech is null)
    {
        return;
    }
    // ... rest of attack code
}
```

**Need to add arm destruction check to `CanPlayerFire` property:**

```csharp
private bool CanPlayerFire =>
    UserMech is not null &&
    EnemyMech is not null &&
    !HasPlayerFiredThisTurn &&
    !PlayerShutdown &&
    PlayerInternal > 0 &&
    !IsComponentDestroyed(PlayerComponents, ComponentType.Weapon) &&
    EnemyInternal > 0 &&
    // ADD THESE NEW CHECKS:
    !IsArmDestroyed(HitLocation.LeftArm) &&   // Can't fire if arms destroyed
    !IsArmDestroyed(HitLocation.RightArm);

private bool IsArmDestroyed(HitLocation arm)
{
    return arm switch
    {
        HitLocation.LeftArm => PlayerSections[HitLocation.LeftArm].Internal <= 0,
        HitLocation.RightArm => PlayerSections[HitLocation.RightArm].Internal <= 0,
        _ => false
    };
}
```

**Additional:** Add arm-specific weapone disabling

```csharp
// Helper method to check if specific arm weapon can fire
private bool CanFireLeftArmWeapon()
{
    return PlayerSections[HitLocation.LeftArm].Internal > 0 &&
           !IsComponentDestroyed(PlayerComponents, ComponentType.Weapon);
}

private bool CanFireRightArmWeapon()
{
    return PlayerSections[HitLocation.RightArm].Internal > 0 &&
           !IsComponentDestroyed(PlayerComponents, ComponentType.Weapon);
}
```

**Effort:** 15 lines of code
**Impact:** MEDIUM - Fixes arm destruction effect

**Note:** Since current code has single generic weapon, just disable fire. In future multi-weapon system, would check arm-specific weapons.

---

#### Task 1.3: Leg Destruction Movement Penalty
**What:** Reduce movement when legs destroyed
**Where:** In `GetMovementByClassAndMode()` and movement cost calculation
**Impact:** Fixes leg destruction effect

**Current Code (line 1095):**
```csharp
private static int GetMovementByClassAndMode(MechClass mechClass, MovementMode movementMode) => movementMode switch
{
    MovementMode.Walk => mechClass switch
    {
        MechClass.Light => 6,
        MechClass.Medium => 5,
        MechClass.Heavy => 4,
        _ => 5
    },
    // ...
};
```

**This is static, needs to become instance method:**

```csharp
private int GetEffectiveMovement(MechClass mechClass, MovementMode mode, 
    Dictionary<HitLocation, SectionState> sections, string playerOrEnemy)
{
    var baseMovement = GetBaseMovement(mechClass, mode);
    
    var leftLegDestroyed = sections[HitLocation.LeftLeg].Internal <= 0;
    var rightLegDestroyed = sections[HitLocation.RightLeg].Internal <= 0;
    
    // Both legs destroyed = immobilized
    if (leftLegDestroyed && rightLegDestroyed)
    {
        return 0;  // Cannot move
    }
    
    // One leg destroyed = -2 movement penalty
    if (leftLegDestroyed || rightLegDestroyed)
    {
        return Math.Max(0, baseMovement - 2);
    }
    
    return baseMovement;  // Both legs intact
}

private static int GetBaseMovement(MechClass mechClass, MovementMode mode) 
    => mode switch
{
    MovementMode.Walk => mechClass switch
    {
        MechClass.Light => 6,
        MechClass.Medium => 5,
        MechClass.Heavy => 4,
        _ => 5
    },
    // ... (rest unchanged)
};
```

**Need to update calls to movement:**

1. **In `DeployUserMech()` (around line 335):**
```csharp
// Instead of:
// var playerMovement = GetMovementByClassAndMode(SelectedMechClass, ActiveMovementMode);

// Use:
var playerMovement = GetEffectiveMovement(SelectedMechClass, ActiveMovementMode, 
    PlayerSections, "player");
```

2. **In `EndPlayerTurn()` (around line 867):**
```csharp
// Instead of:
// var refreshedMovement = PlayerShutdown ? 0 : GetMovementByClassAndMode(UserMech.Class, ActiveMovementMode);

// Use:
var refreshedMovement = PlayerShutdown ? 0 : 
    GetEffectiveMovement(UserMech.Class, ActiveMovementMode, PlayerSections, "player");
```

3. **In `ExecuteEnemyTurn()` (around line 897):**
```csharp
// Instead of:
// RemainingMovement = GetMovementByClassAndMode(EnemyMech.Class, MovementMode.Walk)

// Use:
RemainingMovement = GetEffectiveMovement(EnemyMech.Class, MovementMode.Walk, 
    EnemySections, "enemy")
```

**Effort:** 30 lines of code
**Impact:** MEDIUM-HIGH - Fixes leg destruction effect

---

### Phase 1 Summary
```
Tasks: 3
Lines of Code: ~65
Estimated Time: 3-4 days
Complexity: Low
Testing Time: 1 day
Total: 1 week

Critical fixes:
✓ Both torsos destruction
✓ Arm destruction disabling
✓ Leg destruction penalty
```

---

## Phase 2: Polish (Optional, 1 week)

### Task 2.1: Destruction Messages
Add specific messages when sections destroyed:
- "Your left arm has been blown off!"
- "You've lost your right leg!"
- "Engine critical - mech shutdown!"

### Task 2.2: Visual Feedback
- Gray out destroyed sections in mech display
- Show "DESTROYED" label on sections with 0 internal

### Task 2.3: Component Cascade
When section destroyed automatically destroy components:
```csharp
private void OnSectionDestroyed(HitLocation location)
{
    if (location == HitLocation.CenterTorso)
    {
        // Auto-destroy engine and gyro
        DestroyComponent(location, ComponentType.Engine);
        DestroyComponent(location, ComponentType.Gyro);
    }
    
    if (location == HitLocation.LeftArm || location == HitLocation.RightArm)
    {
        // Auto-destroy arm weapon
        DestroyComponent(location, ComponentType.Weapon);
    }
    
    if (location == HitLocation.LeftLeg || location == HitLocation.RightLeg)
    {
        // Auto-destroy jump jet
        DestroyComponent(location, ComponentType.JumpJet);
        DestroyComponent(location, ComponentType.JumpJet);  // Two per leg
    }
}
```

---

## Implementation Checklist

### Phase 1 - Critical

- [ ] Add `CheckGameOver()` enhancements
  - [ ] Center torso instant loss
  - [ ] Both torsos instant loss  
  - [ ] Head instant loss
- [ ] Add `IsArmDestroyed()` method
- [ ] Add arm checks to `CanPlayerFire`
- [ ] Add enemy arm check to fire logic
- [ ] Add `GetEffectiveMovement()` method
- [ ] Update all movement calls
- [ ] Test armor section destruction behavior
- [ ] Test weapon disabling on arm loss
- [ ] Test movement penalty on leg loss

### Phase 2 - Polish (Optional)

- [ ] Add destruction messages
- [ ] Add UI visual feedback
- [ ] Add component cascade logic
- [ ] Test all combinations

---

## Testing Strategy

### Test Case 1: Both Torsos Destruction
```
Setup: Create scenario where both torsos get destroyed
1. Damage left torso internal to 0
2. Damage right torso internal to 0
3. Keep center torso > 0

Expected: Game over (both torsos destroyed)
Verify: LastActionMessage shows correct reason
```

### Test Case 2: Center Torso Instant Loss
```
Setup: Damage center torso to 0
Expected: Instant game over (before checking other sections)
Verify: Game ends immediately
```

### Test Case 3: Head Destruction
```
Setup: Damage head to 0
Expected: Game over (cockpit destroyed)
Verify: Correct message and game state
```

### Test Case 4: Arm Weapon Disabled
```
Setup: Destroy left arm (internal = 0)
1. Click fire button
Expected: Cannot fire / message says "Cannot fire"
Verify: No shot is actually made
```

### Test Case 5: Leg Movement Penalty
```
Setup: Destroy one leg
1. Check movement display
Expected: Movement reduced by 2
Example: Light walk 6 → 4
Verify: Mech can only move 4 hexes
```

### Test Case 6: Both Legs Immobilized
```
Setup: Destroy both legs
Expected: Total movement = 0
Verify: Cannot move at all
Note: Should still be able to rotate
```

---

## Code Locations

All changes in: `C:\dev\MechWar\MechWar\Pages\Home.razor`

### Methods to Modify
1. `CheckGameOver()` - Line 1565
2. `DeployUserMech()` - Line 324
3. `EndPlayerTurn()` - Line 841
4. `ExecuteEnemyTurn()` - Line 883
5. `CanPlayerFire` property - Line 729

### Methods to Add
1. `IsArmDestroyed()` - New
2. `GetEffectiveMovement()` - New
3. `GetBaseMovement()` - Convert existing static method

### Methods to Keep (No change)
- `ApplyDamage()` - Already works
- `RecalculateDurabilityTotals()` - Already works
- All component destruction - Already works

---

## Risk Assessment

### Low Risk Changes
- Adding condition checks to existing methods
- Converting static method to instance method
- Adding new helper methods

### Medium Risk Changes
- Changing movement calculation (affects many places)
- Adding game-over checks (timing dependent)

### Mitigation
- Extensive testing required
- Verify game-over logic doesn't trigger unexpectedly
- Ensure movement penalties apply correctly in all scenarios

---

## Build Verification

After implementing Phase 1:

```powershell
cd "C:\dev\MechWar\MechWar"
dotnet build

# Should compile with 0 errors
# Check for warnings about unused methods
```

---

## Timeline Estimate

| Task | Estimated Time |
|------|---|
| Code changes | 2 days |
| Unit testing | 1 day |
| Integration testing | 1 day |
| Bug fixes | 1 day |
| Documentation | 1 day |
| **Total** | **1 week** |

---

## Success Criteria

All of these must pass:

```
✓ Both destroyed torsos = game over
✓ Destroyed center torso = game over
✓ Destroyed head = game over
✓ Destroyed arm = weapon disabled
✓ One destroyed leg = movement -2
✓ Both destroyed legs = immobilized
✓ Game still playable
✓ No crashes
✓ Correct messages shown
✓ All existing tests pass
```

---

## References

- **BattleTech Section Destruction:** Official Technical Readout
- **MechWar Code:** Pages/Home.razor
- **Related Docs:**
  - BODY_DESTRUCTION_COMPLIANCE.md
  - BATTLETECHCOMPLIANCE_ASSESSMENT.md

