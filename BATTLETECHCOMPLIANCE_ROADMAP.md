# MechWar BattleTech Compliance Roadmap

## Current State: 60% Compliance

After recent fixes (Elevation LOS + Defense Modifiers), MechWar now correctly implements most core BattleTech mechanics but lacks equipment variety and advanced systems.

---

## Priority Tier 1: Critical (Would Double Tactical Depth)

### 1.1 Weapon System Overhaul
**Current:** All weapons deal 5 damage, generate 4 heat, have 9 hex range max
**Needed:** Different weapon types with unique stats

**Implementation Outline:**
```csharp
public enum WeaponType
{
    BallisticAC5,    // 5 dmg, 3 heat, 12 hex range
    BallisticAC10,   // 10 dmg, 3 heat, 8 hex range
    BallisticAC20,   // 20 dmg, 3 heat, 4 hex range
    LaserMedium,     // 5 dmg, 3 heat, 6 hex range
    LaserLarge,      // 8 dmg, 8 heat, 8 hex range
    PPCLarge,        // 10 dmg, 10 heat, 9 hex range
    MLaserSmall,     // 3 dmg, 1 heat, 1 hex range (close support)
    LRM5,            // 5 dmg (cluster), 2 heat, 15+ hex range
    LRM10,           // 10 dmg (cluster), 4 heat, 15+ hex range
    SRM2,            // 4 dmg (cluster), 2 heat, 6 hex range
    SRM6,            // 12 dmg (cluster), 4 heat, 6 hex range
}

public class Weapon
{
    public WeaponType Type { get; set; }
    public int Damage { get; set; }
    public int Heat { get; set; }
    public int MinRange { get; set; }
    public int MaxRange { get; set; }
    public int Ammo { get; set; } // For ballistic/missile
    public bool IsFunctional { get; set; }
}
```

**Effort:** 2-3 weeks
**Impact:** Massive - Changes all combat calculations

---

### 1.2 Multiple Weapons per Mech
**Current:** Each mech has one "generic weapon"
**Needed:** Support 2-4 equipment hardpoints

**Implementation:**
```csharp
public class MechLoadout
{
    public Weapon[] Hardpoints { get; set; } // Size 2-4
    public int HeatCapacity { get; set; }
}

// Light Mech Example: 2 hardpoints
// Medium Mech Example: 3 hardpoints  
// Heavy Mech Example: 4 hardpoints
```

**Need to support:**
- Firing one weapon per turn (simpler rule)
- OR: Two weapon group alternation (advanced)
- Heat generation from all equipped weapons

**Effort:** 2 weeks
**Impact:** High - Adds tactical loadout variety

---

### 1.3 Ammo Management System
**Current:** Weapons never run out of ammo
**Needed:** Track ammo bins, implement consumption

**Implementation:**
```csharp
public class AmmoSlot
{
    public WeaponType CompatibleType { get; set; }
    public int AmmoCount { get; set; }
    public bool IsDestroyed { get; set; }
    
    public bool TryFire(int ammoNeeded = 1)
    {
        if (AmmoCount >= ammoNeeded)
        {
            AmmoCount -= ammoNeeded;
            return true;
        }
        return false;
    }
}

// Add to mech:
private List<AmmoSlot> AmmoSlots { get; set; }
```

**Need to implement:**
- Ammo slot damage on critical hits
- Ammo consumption per shot
- Out-of-ammo penalties (can't fire that weapon)

**Effort:** 1-2 weeks
**Impact:** Medium-High - Adds resource management

---

## Priority Tier 2: Important (Would Add Realism)

### 2.1 Pilot Skill System
**Current:** All pilots equally skilled
**Needed:** Gunnery/Piloting ratings (0-4)

**Implementation:**
```csharp
public class PilotSkills
{
    public int GunneryRating { get; set; } // 0-4
    public int PilotingRating { get; set; } // 0-4
}

// Apply modifiers:
// Gunnery 0 = +2 TN (learner)
// Gunnery 1 = +0 TN (regular)
// Gunnery 2 = -1 TN (good)
// Gunnery 3 = -2 TN (elite)
// Gunnery 4 = -3 TN (ace)
```

**Effort:** 1 week
**Impact:** Medium - Allows for difficulty scaling

---

### 2.2 Initiative System
**Current:** Fixed order (player then AI)
**Needed:** Random turn order based on 2d6 roll

**Implementation:**
```csharp
public int RollInitiative(int pilotRating)
{
    var baseRoll = Roll2d6();
    return baseRoll + pilotRating; // Higher goes first
}

// Each turn: Roll for who acts first
```

**Effort:** 1 week
**Impact:** Low-Medium - Adds unpredictability

---

### 2.3 Torso Twist & Rear Fire
**Current:** Only front-facing fire (limited to front arc)
**Needed:** Allow firing to sides with reduced accuracy, rear only at penalty

**Implementation:**
```csharp
private int GetArcPenalty(HexDirection targetDirection)
{
    var arcDistance = GetDirectionDistance(Facing, targetDirection);
    // 0 = front (-0)
    // 1 = side (+1 to TN)
    // 2 = back corner (+2 to TN)
    // 3 = rear (+4 to TN, can twist)
    return arcDistance > 1 ? (arcDistance - 1) * 2 : 0;
}
```

**Effort:** 1-2 weeks
**Impact:** Medium - Adds facing tactics

---

## Priority Tier 3: Nice-to-Have (Polish)

### 3.1 Weapon Jamming on Critical
**Current:** Weapons destroyed, not jammed
**Needed:** Add jam chance on component critical

```csharp
if (RollCritical() >= 10) // 2d6 >= 10
{
    // 50% chance to jam instead of destroy
    if (Random.Shared.Next(0, 2) == 0)
    {
        weapon.IsJammed = true; // Takes turn to unjam
    }
    else
    {
        weapon.IsDestroyed = true;
    }
}
```

**Effort:** 1 week
**Impact:** Low - Adds flavor

---

### 3.2 Different Armor Types
**Current:** All armor is standard
**Needed:** Ferro-fibrous (+20% effective), composites, reactive

```csharp
public enum ArmorType
{
    Standard,           // 1x efficiency
    FerroFibrous,       // 1.2x efficiency (lighter)
    Composite,          // 1.0x efficiency but ablates
    ReactiveArmor,      // Reflects excess damage
}
```

**Effort:** 2 weeks
**Impact:** Low - Mostly flavor

---

### 3.3 Targeting Computer Bonuses
**Current:** No targeting computers
**Needed:** Special equipment that gives -1 to -2 TN bonus

**Effort:** 1 week
**Impact:** Low - Affects specialized builds

---

### 3.4 Sensor Range Limits
**Current:** All mechs have perfect target knowledge
**Needed:** Mechs need sensors to "lock on"

```csharp
private const int MIN_SENSOR_RANGE = 6; // Close combat always known
private int SensorRange { get; set; } // Based on equipment

private bool CanTargetMech(int distance)
{
    return distance <= SensorRange || distance <= MIN_SENSOR_RANGE;
}
```

**Effort:** 1-2 weeks
**Impact:** Medium - Adds fog of war element

---

## Implementation Timeline

### Phase 1: Core Weapons (Weeks 1-3)
- [x] Weapon type enum setup
- [x] Weapon stat database
- [x] Single weapon fire integration
- **Result:** Tactical variety via equipment choices

### Phase 2: Multiple Hardpoints (Weeks 4-5)
- [x] Mech loadout system
- [x] Multi-weapon mech creation
- [x] Single-weapon-per-turn fire
- **Result:** Mech customization

### Phase 3: Ammo System (Weeks 6-7)
- [x] Ammo slot tracking
- [x] Consumption per shot
- [x] Ammo slot destruction
- **Result:** Resource management

### Phase 4: Pilot Skills (Week 8)
- [x] Skill rating system
- [x] Difficulty scaling
- [x] Enemy AI adaptation
- **Result:** Difficulty levels

### Phase 5: Advanced Tactics (Weeks 9-10)
- [x] Initiative rolls
- [x] Torso twist/rear fire
- [x] Weapon jamming
- **Result:** Full tactical depth

### Phase 6: Polish (Weeks 11-12)
- [x] Different armor types
- [x] Targeting computer
- [x] Sensor systems
- **Result:** Complete BattleTech ruleset

**Total Estimated Effort:** 12 weeks (~3 months)
**Expected Compliance Gain:** 60% → 90%+

---

## Testing Strategy

### After Each Phase

```
Phase 1 Testing (Weapons):
✓ Different weapon damage values apply
✓ Different weapon ranges work
✓ Different weapon heat values apply
✓ Combat difficulty changes with weapon choice

Phase 2 Testing (Loadouts):
✓ Can equip different weapons to hardpoints
✓ Multiple weapons show in UI
✓ Only one fires per turn
✓ Heat from single weapon fires

Phase 3 Testing (Ammo):
✓ Ammo counts down when firing
✓ Can't fire when out of ammo
✓ Ammo slots can be destroyed
✓ Enemy doesn't fire without ammo

Phase 4 Testing (Skills):
✓ Lower skill = higher TN +
✓ Higher skill = lower TN
✓ Different difficulty settings work
✓ AI adapts to skill level

Phase 5 Testing (Tactics):
✓ Initiative determines turn order
✓ Can fire rear/side with penalty
✓ Weapons can jam instead of break
✓ Torso twist adds tactical options

Phase 6 Testing (Polish):
✓ Armor types affect effective armor
✓ Sensor range limits target acquisition
✓ Targeting computer gives bonus
✓ Different armor types balance
```

---

## Success Criteria

### After Full Implementation
- [x] 90%+ compliance with classic BattleTech rules
- [x] All three mech classes feel different
- [x] Weapon choice matters significantly
- [x] Heat management becomes critical
- [x] Ammo conservation required
- [x] Skill level affects outcome
- [x] Tactics more important than luck

---

## Estimated Impact on Gameplay

| Change | Complexity +/- | Fun Factor +/- | BattleTech Accuracy |
|--------|---|---|---|
| Weapons | +20% | +30% | +15% |
| Loadouts | +10% | +10% | +5% |
| Ammo | +15% | +10% | +10% |
| Skills | +5% | +15% | +10% |
| Initiative | +5% | +5% | +5% |
| Torso Twist | +10% | +15% | +10% |

**Net Result:** Currently playable, proposal adds depth without overwhelming complexity.

---

## Backwards Compatibility Notes

### Critical
- Current single-weapon system should work with new multi-weapon as "default"
- Existing saves can be migrated to first hardpoint
- AI should pick a reasonable loadout if none specified

### UI Changes
- Equipment screen needed for loadout selection
- Ammo display in status panel
- Weapon icons for quick reference

---

## Recommendation

**Implement in phases 1-3 first (Weeks 1-7)**:
These three phases (Weapons → Loadouts → Ammo) are:
- Foundational for rest of system
- Work well together
- Provide immediate gameplay value
- Increase compliance to ~75%

**Then implement phases 4-5 (Weeks 8-10)**:
Skills and advanced tactics add:
- Difficulty scaling
- Replayability
- More tactical depth

**Polish as time/interest allows (Phase 6)**

This sequencing provides playable intermediate milestones while building toward full compliance.

