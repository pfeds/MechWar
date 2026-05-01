# Body Section Destruction - Visual Reference

## BattleTech Rules at a Glance

```
SECTION DESTROYED = WHAT HAPPENS

Head (Cockpit)
├─ Internal = 0
├─ Effect: INSTANT MECH LOSS
├─ Reason: Pilot dead
└─ BattleTech: 💥 Game Over

Center Torso (Engine Room)
├─ Internal = 0
├─ Effect: INSTANT MECH LOSS
├─ Reason: Engine destroyed
└─ BattleTech: 💥 Game Over

Both Side Torsos
├─ Left Torso = 0 AND Right Torso = 0
├─ Effect: INSTANT MECH LOSS
├─ Reason: Structural failure
└─ BattleTech: 💥 Game Over

Left Arm
├─ Internal = 0
├─ Effect: ARM SEVERED
├─ Result: Lose left arm weapons
├─ Also: Can't twist to protect torso
└─ BattleTech: 🔥 Cannot Fire

Right Arm
├─ Internal = 0
├─ Effect: ARM SEVERED
├─ Result: Lose right arm weapons
├─ Also: Can't twist to protect torso
└─ BattleTech: 🔥 Cannot Fire

Left Leg
├─ Internal = 0
├─ Effect: LEG BLOWN OFF
├─ Movement: -2 hexes per turn
├─ Example: Walk 5 → Walk 3
└─ BattleTech: 🚶 Reduced Mobility

Right Leg
├─ Internal = 0
├─ Effect: LEG BLOWN OFF
├─ Movement: -2 hexes per turn
├─ Example: Walk 5 → Walk 3
└─ BattleTech: 🚶 Reduced Mobility

Both Legs
├─ Left Leg = 0 AND Right Leg = 0
├─ Effect: COMPLETELY IMMOBILIZED
├─ Movement: 0 hexes (cannot move)
├─ Can only: Rotate in place
└─ BattleTech: 🛑 IMMOBILIZED
```

---

## Current MechWar Implementation

```
Head (Cockpit)
├─ Status: ❌ NOT SPECIAL
├─ What happens: Nothing (just loses 3 armor/internal)
└─ Should happen: INSTANT LOSS

Center Torso (Engine Room)
├─ Status: ⚠️  MOSTLY WORKS
├─ What happens: Game ends when total internal = 0
├─ Why it works: CT has most internal, so usually dies first
└─ Problem: Doesn't check CENTER TORSO specifically

Both Side Torsos
├─ Status: ❌ WRONG
├─ What happens: Game continues
├─ Should happen: INSTANT LOSS
└─ Problem: Not checked at all

Left Arm
├─ Status: ❌ WRONG
├─ What happens: Still fires
├─ Should happen: Cannot fire (arm gone)
└─ Problem: Section destruction not checked, only component

Right Arm
├─ Status: ❌ WRONG
├─ What happens: Still fires
├─ Should happen: Cannot fire (arm gone)
└─ Problem: Section destruction not checked, only component

Left Leg
├─ Status: ❌ WRONG
├─ What happens: No movement penalty
├─ Should happen: Movement -2
└─ Problem: Leg destruction not checked

Right Leg
├─ Status: ❌ WRONG
├─ What happens: No movement penalty
├─ Should happen: Movement -2
└─ Problem: Leg destruction not checked

Both Legs
├─ Status: ❌ WRONG
├─ What happens: Still walks normally
├─ Should happen: Immobilized (0 movement)
└─ Problem: Leg destruction not checked
```

---

## Real-World Scenarios

### Scenario 1: "The Walking Corpse"

```
═══════════════════════════════════════════════════════════
ENEMY FIRES 50 DAMAGE TO YOUR CENTER TORSO
═══════════════════════════════════════════════════════════

BEFORE DAMAGE:
Center Torso: 10 armor / 8 internal ✓ Healthy
Others:      Various armor/internal ✓ Intact

ATTACK RESOLVES:
Armor absorbed: 10
Remaining damage: 40
Internal damage: 8
Overflow: 32 (cascades to other sections)

AFTER DAMAGE:
Center Torso: 0 armor / 0 internal ❌ DESTROYED
Left Torso: Takes some overflow
Right Torso: Takes some overflow
Head/Legs: Unharmed

══════════════════════════════════════════════════════════

OFFICIAL BATTLETECHIT:
"CENTER TORSO DESTROYED = ENGINE DESTROYED = MECH DESTROYED"
→ GAME OVER IMMEDIATELY

═══════════════════════════════════════════════════════════
WHAT MECH WAR DOES:
- Center Torso shows 0 internal
- BUT game doesn't specifically check center torso
- Checks only if TOTAL internal = 0
- If some damage went to legs, might have total > 0
- Game CONTINUES (WRONG!)

═══════════════════════════════════════════════════════════

EXAMPLE OF VIOLATION:
Center Torso: 0 internal ❌
Left Torso: 2 internal
Right Torso: 2 internal
Head: 3 internal
Both Legs: 6 internal each
──────────────
TOTAL: 19 internal (not zero!)

MechWar result: GAME CONTINUES
Correct result: GAME OVER (center torso destroyed means engine dead)

═══════════════════════════════════════════════════════════
```

---

### Scenario 2: "The Phantom Arm"

```
═══════════════════════════════════════════════════════════
YOUR LEFT ARM IS COMPLETELY DESTROYED (Internal = 0)
═══════════════════════════════════════════════════════════

Left Arm Status:
├─ Armor: 0 (blown away)
├─ Internal: 0 (destroyed)
├─ Weapon: Present and undestroyed
└─ Physical state: SEVERED -- ARM IS GONE

OFFICIAL BATTLETECHIT:
"LEFT ARM HAS NO INTERNAL STRUCTURE = ARM IS GONE"
→ "CANNOT FIRE WEAPONS FROM SEVERED ARM"

═══════════════════════════════════════════════════════════
WHAT MechWar DOES:
- Shows Left Arm internal = 0
- Checks if weapon component destroyed: NO
- Allows firing (WRONG!)

═══════════════════════════════════════════════════════════

VISUAL REPRESENTATION:

Normal Arm:        Destroyed Arm:
  limb             (SEVERED)
  weapon           weapon floating in space
  fires            can't fire               ❌

═══════════════════════════════════════════════════════════
```

---

### Scenario 3: "Running on Stumps"

```
═══════════════════════════════════════════════════════════
BOTH LEGS DESTROYED (Both Legs Internal = 0)
═══════════════════════════════════════════════════════════

Leg Status:
├─ Left Leg: Internal = 0 ❌ DESTROYED
├─ Right Leg: Internal = 0 ❌ DESTROYED
└─ Physical state: BOTH LEGS BLOWN OFF

OFFICIAL BATTLETECHIT:
"BOTH LEGS DESTROYED = IMMOBILIZED"
→ "CANNOT MOVE AT ALL (0 hexes per turn)"
→ "CAN ONLY ROTATE IN PLACE"

═══════════════════════════════════════════════════════════
WHAT MechWar DOES:
- Shows both legs internal = 0
- Doesn't check for leg destruction
- Movement unchanged: 5 hexes (WRONG!)

═══════════════════════════════════════════════════════════

VISUAL MOVEMENT:

║             CURRENT          ║       SHOULD BE
║   Mech walks 5 hexes ...     ║   Mech: "I have no legs!"
║   (Because we don't check)   ║   Movement: 0 hexes
║   (Moves normally) ✗         ║   (Immobilized) ✓
║                              ║

═══════════════════════════════════════════════════════════
```

---

### Scenario 4: "One-Legged Wonder"

```
═══════════════════════════════════════════════════════════
LEFT LEG DESTROYED (Left Leg Internal = 0)
RIGHT LEG INTACT (Right Leg Internal = 6)
═══════════════════════════════════════════════════════════

Leg Status:
├─ Left Leg: Internal = 0 ❌ DESTROYED
└─ Right Leg: Internal = 6 ✓ INTACT

OFFICIAL BATTLETECHIT:
"ONE LEG DESTROYED = MOVEMENT PENALTY -2"
→ "MECH WALKS WITH LIMP"
Example:
  Medium Mech normal walk: 5 hexes
  With one leg destroyed: 3 hexes (-2)

═══════════════════════════════════════════════════════════
WHAT MechWar DOES:
- Shows left leg internal = 0
- Doesn't check for leg destruction
- Movement unchanged: 5 hexes (WRONG!)

═══════════════════════════════════════════════════════════

COMPARISON:

║              REAL            ║      MECHWAR
║  Normal walk: 5 hexes        ║  With destroyed leg: 5 hexes ✗
║  One leg gone: 3 hexes       ║  (No penalty applied)
║  No legs left: 0 hexes       ║

═══════════════════════════════════════════════════════════
```

---

## BattleTech Rules vs MechWar - Decision Tree

```
WHEN A SECTION IS HIT AND DESTROYED:

┌─ Is it CENTER TORSO?
│  ├─ BattleTech: YES → Engine destroyed → Mech destroyed → END GAME
│  └─ MechWar: Check only total internal (hits zero later probably)
│
├─ Are BOTH SIDE TORSOS destroyed?
│  ├─ BattleTech: YES → Structural failure → Mech destroyed → END GAME
│  └─ MechWar: No check → Game continues (WRONG!)
│
├─ Is it HEAD?
│  ├─ BattleTech: YES → Cockpit destroyed → Mech destroyed → END GAME
│  └─ MechWar: No special handling → Game continues (WRONG!)
│
├─ Is it ARM (Left or Right)?
│  ├─ BattleTech: YES → Arm severed → Can't fire from that arm
│  └─ MechWar: No check → Still fires (WRONG!)
│
├─ Is it LEG (Left or Right)?
│  ├─ BattleTech: YES → Movement penalty -2
│  └─ MechWar: No check → No penalty (WRONG!)
│
└─ Are BOTH LEGS destroyed?
   ├─ BattleTech: YES → Immobilized → Movement = 0
   └─ MechWar: No check → Still walks (WRONG!)
```

---

## Compliance Matrix

```
SECTION          BattleTech Rule          MechWar Status     Compliant?
─────────────────────────────────────────────────────────────────────────
Center Torso     Instant loss             Partial (by luck)  ⚠️  50%
Both Side Torso  Instant loss             Not checked        ❌ 0%
Head             Instant loss             Not checked        ❌ 0%
Left Arm         Disable weapon           Not checked        ❌ 0%
Right Arm        Disable weapon           Not checked        ❌ 0%
One Leg          Movement -2              Not checked        ❌ 0%
Both Legs        Immobilized (0 move)     Not checked        ❌ 0%
Any Section      Track damage             ✓ Tracked          ✓ 100%

OVERALL COMPLIANCE:                                           ⚠️  40%
```

---

## Fix Impact Visualization

```
BEFORE FIXES:
                                    ┌─ Different sections
                                    │  but not used
  Total Internal ←───────────────────┤
                                    │
                                    └─ No destruction effects

  Flaws: Mechs survive impossible damage

AFTER FIXES:
                                    ┌─ Center Torso = 0 → END
                                    │
  Section Checks ←─────────────────┼─ Both Torsos = 0 → END
                                    │
                                    ├─ Head = 0 → END
                                    │
                                    ├─ Arm = 0 → No weapon fire
                                    │
                                    └─ Leg = 0 → Movement penalty

  Rules: Authentic BattleTech destruction mechanics
```

---

## Testing Checklist

```
BOTH TORSOS DESTRUCTION:
□ Manually damage left torso to 0
□ Manually damage right torso to 0
□ Keep center torso > 0
□ Verify game-over message appears
□ Verify correct message about torso destruction

CENTER TORSO DESTRUCTION:
□ Damage center torso to 0
□ Verify game ends immediately
□ Verify "Center Torso destroyed" message

HEAD DESTRUCTION:
□ Damage head to 0
□ Verify game ends immediately
□ Verify pilot death message

ARM DESTRUCTION:
□ Damage left arm to 0 (or right)
□ Try to fire
□ Verify fire action blocked
□ Verify "Cannot fire - arm destroyed" message

ONE LEG DESTRUCTION:
□ Damage one leg to 0
□ Check movement display
□ Verify movement reduced by 2
□ Example: 5 →  3 hexes

BOTH LEGS DESTRUCTION:
□ Damage both legs to 0
□ Check movement display
□ Verify movement = 0
□ Verify cannot move at all
□ Verify can still rotate
```

---

## Summary

```
╔════════════════════════════════════════════════════════════╗
║        BODY SECTION DESTRUCTION - BattleTech vs MechWar     ║
╠════════════════════════════════════════════════════════════╣
║                                                             ║
║  CURRENT COMPLIANCE:              40% ❌                   ║
║  Most destruction effects missing                           ║
║                                                             ║
║  CRITICAL FIXES NEEDED:                                     ║
║  ✓ Both torsos instant loss                                ║
║  ✓ Arm destruction disables weapons                        ║
║  ✓ Leg destruction reduces movement                        ║
║  ✓ Head destruction = pilot death                          ║
║                                                             ║
║  IMPLEMENTATION TIME:             1 week                   ║
║  CODE CHANGES:                    ~65 lines                ║
║  IMPACT:                          HIGH (core mechanic)     ║
║                                                             ║
║  STATUS:                          ❌ NOT IMPLEMENTED       ║
║  PRIORITY:                        🔴 CRITICAL              ║
║                                                             ║
╚════════════════════════════════════════════════════════════╝
```

---

See detailed docs:
- **BODY_DESTRUCTION_COMPLIANCE.md** - Full analysis
- **BODY_DESTRUCTION_IMPLEMENTATION.md** - Step-by-step code guide

