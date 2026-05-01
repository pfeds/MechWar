# Defense Calculation Test Cases

## Quick Reference Table

| Terrain Type | Modifier | Coverage | BattleTech Class |
|---|---|---|---|
| Plains | +0 | None | Open ground |
| Rock | +0 | None | Open ground |
| Sand | +0 | None | Open ground |
| Ice | +0 | None | Open ground |
| Scrub | +0 | Minimal | Light brush |
| **Hills Level 1** | **+1** | **Partial** | **Defilade** |
| **Hills Level 2+** | **+2** | **Full** | **Strong Defilade** |
| **Forest** | **+1** | **Partial** | **Concealment** |
| **Jungle** | **+1** | **Partial** | **Concealment** |
| **Swamp** | **+1** | **Partial** | **Concealment** |
| **Urban** | **+1** | **Partial** | **Building Cover** |
| **Canyon** | **+1** | **Partial** | **Terrain Cover** |
| **Crater** | **+1** | **Partial** | **Terrain Cover** |
| **Water Depth 1** | **+1** | **Partial** | **Submersion** |
| **Water Depth 2+** | **+2** | **Full** | **Deep Water** |
| Mountains | +2 | Full | (Impassable) |

## Scenario-Based Test Cases

### Test 1: Basic Hill Defense
**Setup:**
- Attacker: Plains (Level 0), 6 hexes away
- Defender: Level 1 Hill
- Attacker mode: Run
- Defender movement: 0

**Calculation:**
- Base TN: 4
- Range (6 hexes): +2
- Attacker (run): +2
- Defender (stationary): +0
- Heat: +0
- **Defense (Level 1 Hill): +1**
- **Final TN: 10** (vs 9 without hill)

**Verification:** Defender gets +1 difficulty modifier ✓

---

### Test 2: Strong Hill Defense
**Setup:**
- Attacker: Plains (Level 0)
- Defender: Level 2+ Hill
- Attacker mode: Walk
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range (3 hexes): +0
- Attacker (walk): +1
- Defender: +0
- Heat: +0
- **Defense (Level 2+ Hill): +2**
- **Final TN: 7** (vs 5 without hill)

**Verification:** Defender gets +2 difficulty modifier ✓

---

### Test 3: Water Defense (Shallow)
**Setup:**
- Attacker: Plains
- Defender: Shallow Water (Depth 1)
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Water Depth 1): +1**
- **Final TN: 6**

**Verification:** Shallow water adds +1 difficulty ✓

---

### Test 4: Water Defense (Deep)
**Setup:**
- Attacker: Plains
- Defender: Deep Water (Depth 2+)
- Range: 6 hexes

**Calculation:**
- Base TN: 4
- Range (6 hexes): +2
- Attacker (run): +2
- Defender: +0
- Heat: +0
- **Defense (Water Depth 2+): +2**
- **Final TN: 10**

**Verification:** Deep water adds +2 difficulty ✓

---

### Test 5: Forest Concealment
**Setup:**
- Attacker: Plains
- Defender: Forest (no elevation)
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- Heat: +0
- **Defense (Forest): +1**
- **Final TN: 6**

**Verification:** Forest provides +1 concealment ✓

---

### Test 6: Urban Cover
**Setup:**
- Attacker: Plains
- Defender: Urban (buildings)
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- Heat: +0
- **Defense (Urban): +1**
- **Final TN: 6**

**Verification:** Urban terrain provides +1 building cover ✓

---

### Test 7: Canyon Cover
**Setup:**
- Attacker: Plains
- Defender: Canyon
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- Heat: +0
- **Defense (Canyon): +1**
- **Final TN: 6**

**Verification:** Canyon walls provide +1 cover ✓

---

### Test 8: Priority - Hill Overrides Forest
**Setup:**
- Attacker: Plains
- Defender: Level 2 Hill WITH Forest terrain
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- Heat: +0
- **Defense (Level 2 Hill): +2** (forest adds +1 but hill +2 is primary)
- **Final TN: 7**

**Verification:** Hill level 2 = +2 takes precedence ✓
**Note:** The implementation correctly prioritizes elevation over terrain type.

---

### Test 9: Stacking - Hill 1 + Water Depth 1
**Setup:**
- Attacker: Plains
- Defender: Level 1 Hill + Shallow Water
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Hit Level 1): +1**
- **Defense (Depth 1): +1** (combined = +2)
- **Final TN: 7**

**Verification:** Multiple defensive sources stack correctly ✓

---

### Test 10: Maximum Stacking - Hill 2 + Deep Water
**Setup:**
- Attacker: Plains
- Defender: Level 2 Hill + Deep Water
- Range: 6 hexes

**Calculation:**
- Base TN: 4
- Range (6 hexes): +2
- Attacker (run): +2
- Defender: +0
- Heat: +0
- **Defense (Hill Level 2): +2**
- **Defense (Depth 2+): +2** (combined = +4)
- **Final TN: 12**

**Verification:** 
- Maximum stacking creates very difficult target ✓
- Defender is nearly impossible to hit ✓

---

### Test 11: No Defense - Open Plains
**Setup:**
- Attacker: Plains
- Defender: Plains
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Plains): +0**
- **Final TN: 5**

**Verification:** Open terrain provides no defense as expected ✓

---

### Test 12: No Defense - Exposed Rock
**Setup:**
- Attacker: Plains
- Defender: Rock (exposed, no cover)
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Rock): +0**
- **Final TN: 5**

**Verification:** Exposed rock provides no cover ✓

---

### Test 13: Minimal Defense - Scrub
**Setup:**
- Attacker: Plains
- Defender: Scrub (light vegetation)
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Scrub): +0**
- **Final TN: 5**

**Verification:** Scrub provides minimal/no effective cover ✓

---

### Test 14: Swamp Concealment
**Setup:**
- Attacker: Plains
- Defender: Swamp
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Swamp): +1** (water + vegetation combination)
- **Final TN: 6**

**Verification:** Swamp provides dual concealment benefit ✓

---

### Test 15: Crater Defense
**Setup:**
- Attacker: Plains
- Defender: Crater
- Range: 3 hexes

**Calculation:**
- Base TN: 4
- Range: +0
- Attacker: +1
- Defender: +0
- **Defense (Crater): +1**
- **Final TN: 6**

**Verification:** Crater rim provides +1 partial cover ✓

---

## Implementation Verification Checklist

- [x] Hills Level 1 = +1 modifier
- [x] Hills Level 2+ = +2 modifier
- [x] Water Depth 1 = +1 modifier
- [x] Water Depth 2+ = +2 modifier
- [x] Forest = +1 modifier
- [x] Jungle = +1 modifier
- [x] Urban = +1 modifier
- [x] Canyon = +1 modifier
- [x] Crater = +1 modifier
- [x] Swamp = +1 modifier
- [x] Plains = +0 modifier
- [x] Rock = +0 modifier
- [x] Sand = +0 modifier
- [x] Ice = +0 modifier
- [x] Scrub = +0 modifier
- [x] Mountains = +2 modifier
- [x] Hill level takes precedence over terrain type
- [x] Modifiers properly stack
- [x] Code compiles without errors
- [x] Integrated into CanFireBetween()
- [x] Used in both player and enemy attack calculations

## In-Game Testing Steps

### Step 1: Deploy and Position Mechs
1. Start new game
2. Deploy player mech on plains
3. Deploy (or let AI place) enemy on different terrain

### Step 2: Observe ToHit Display
1. Look at status panel which shows ToHit calculation
2. Verify terrain modifier appears in breakdown
3. Compare different terrain types

### Step 3: Manual Test Sequence
- [ ] Enemy on Level 1 Hill: Verify +1 modifier appears
- [ ] Enemy in Forest: Verify +1 modifier appears
- [ ] Enemy in Plains: Verify +0 modifier (no modifier shown)
- [ ] Enemy in Urban: Verify +1 modifier appears
- [ ] Enemy in Canyon: Verify +1 modifier appears
- [ ] Enemy in Water (shallow): Verify +1 modifier appears
- [ ] Enemy in Water (deep): Verify +2 modifier appears
- [ ] Fire at various terrains: Observe if hits are more/less frequent

### Step 4: Combat Log Verification
1. Load game with enemy in defensive terrain
2. Fire at enemy
3. Check combat log messages for accurate modifier calculations
4. Confirm target numbers match what's displayed

## Expected Behavior after Fix

### Hitting Targets Should Be:
- **Easier** in open terrain (plains, rock, sand)
- **Moderate** in light concealment (forest, urban, canyon)
- **Harder** in hills or water
- **Very Hard** in combination terrain (hill in forest, hill in water)

### Attack Success Rates Should:
- Increase for targets in open ground
- Decrease for targets in cover/elevated positions
- Vary based on terrain composition
- Reflect BattleTech tactical emphasis on terrain use

## Related Bug Fixes

This fix complements the **ELEVATION_LOS_FIX.md** which handles:
- Line-of-sight blocking by intervening terrain
- Elevation level calculations
- Mech sight lines

Together these systems provide complete BattleTech terrain interactions:
1. **LOS System**: Can you see them? (blocking/elevation)
2. **Defense System**: How hard are they to hit? (cover/terrain)

