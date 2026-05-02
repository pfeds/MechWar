# Quick Testing Guide - MechWar UI Fixes

## 🧪 Test Checklist

Run through these quick checks to verify the fixes are working:

### ✅ Step 1: Build Verification
```powershell
cd C:\dev\MechWar\MechWar
dotnet build
```
Expected: `Build succeeded. 0 Warning(s), 0 Error(s)`

### ✅ Step 2: Start the Application
```powershell
dotnet run
```
Expected: Application starts on http://localhost:5000

### ✅ Step 3: Home Page Check
Open browser to `http://localhost:5000/`

**Visual Checks:**
- [ ] "MECHWAR" logo and subtitle visible at top
- [ ] White top bar spans full width horizontally
- [ ] Lobby card centered with proper styling
- [ ] Form inputs (Theme, Width, Height) properly styled
- [ ] Mech selection dropdowns functional
- [ ] Buttons styled with proper colors
- [ ] "Deploy & Fight" button visible

**Interaction Checks:**
- [ ] Click "Randomise" button - works without errors
- [ ] Select different theme - updates form
- [ ] Change width/height - updates preview text
- [ ] Click "Deploy & Fight" - navigates to game page

### ✅ Step 4: Game Page Check
After clicking "Deploy & Fight" on home page

**Visual Layout:**
- [ ] **Top Bar**: Spans full page width with logo visible
- [ ] **Top Bar Position**: Horizontal at top, NOT on left side
- [ ] **Main Map Area**: Large area in center-left with hex grid visible
- [ ] **Bottom Control Panel**: Below map with three button rows
- [ ] **Right Console**: Dark panel on right with battle log
- [ ] **Proper Spacing**: No content overlapping, good padding

**Control Panel Check:**
- [ ] **MODE row**: Three buttons (WALK, RUN, JUMP)
  - Active button (green #49e349) = current mode
  - Inactive buttons (blue #7ebcff) = other modes
- [ ] **MOVE row**: Four arrow buttons (↺ ↑ ↓ ↻)
  - Properly arranged in 2x2 grid
  - Enable/disable state changes
- [ ] **FIRE row**: Two buttons (FIRE, END TURN)
  - Fire button green when available
  - End button enabled when in player phase

**Status Display:**
- [ ] **Round/Phase text**: "Round 1 · Your Turn" visible above map
- [ ] **Theme text**: Shows current battlefield theme
- [ ] **Color**: Round text is blue, theme text is green

**Console Panel:**
- [ ] **Dark background**: #040b04 color
- [ ] **Text colors**:
  - [Allied] messages: Blue (#4da3ff)
  - [Enemy] messages: Red (#ff4d4d)
  - Regular messages: Green (#49e349)

### ✅ Step 5: Color Scheme Verification

**Blues (UI Elements):**
- [ ] Mode buttons (inactive): #7ebcff
- [ ] Top bar text: appears in dark color (#1a2740)

**Greens (Game Info):**
- [ ] Theme text in status: #49e349
- [ ] Console output text: #49e349
- [ ] Active mode button: #49e349

**Reds (Enemy/Alerts):**
- [ ] Enemy console messages: #ff4d4d

**Backgrounds:**
- [ ] Game container background: #1a1a1a (dark)
- [ ] Console background: #040b04 (very dark)
- [ ] Top bar background: #ffffff (white)

### ✅ Step 6: Responsive Design Check

Open DevTools (F12) and toggle device toolbar:

**At 1200px width:**
- [ ] Console panel moves below map
- [ ] Layout adapts to single column
- [ ] All elements still visible and usable

**At 800px width:**
- [ ] Move buttons adjust to 2-column grid
- [ ] Control buttons remain functional
- [ ] Text remains readable
- [ ] No horizontal scrolling needed

**On mobile (375px):**
- [ ] All buttons stack properly
- [ ] Console is accessible
- [ ] Touch targets are adequate (44px+ buttons)

### ✅ Step 7: Interaction Testing

**Button Clicks:**
- [ ] Click MODE buttons to change movement mode
- [ ] Buttons properly highlight active state
- [ ] Movement mode changes

**Movement Controls:**
- [ ] Click arrow buttons to move/rotate mech
- [ ] Console logs appear in real-time
- [ ] Buttons disable when not your turn

**Combat:**
- [ ] Click FIRE button during your turn
- [ ] Combat log updates with hit/miss info
- [ ] Hit messages appear in orange (#ff9f1a)
- [ ] Destruction messages appear in red (#ff2c2c)

### ✅ Step 8: No Errors Check

**Browser Console (F12 → Console tab):**
- [ ] No JavaScript errors in red
- [ ] No CSS parsing errors
- [ ] Console is clean

**Application Output:**
- [ ] No exceptions in terminal
- [ ] Application running smoothly
- [ ] Interactions respond quickly

## 🎯 Expected Results

### Before Fixes
❌ "An unhandled error has occurred" message
❌ Top bar displayed on left side
❌ Game page buttons had no styling
❌ Console output had no formatting

### After Fixes
✅ No error messages (CSS loads properly)
✅ Top bar spans horizontally across top
✅ All game controls properly styled
✅ Console output has matrix/terminal styling
✅ Color scheme matches game aesthetic
✅ Responsive layout works on all devices

## 🔧 Troubleshooting

### Issue: Still seeing "unhandled error"
- [ ] Clear browser cache (Ctrl+Shift+Delete)
- [ ] Do a hard refresh (Ctrl+F5)
- [ ] Check browser console for specific error
- [ ] Verify build completed successfully

### Issue: Top bar still on left side
- [ ] Verify MainLayout.razor.css was updated
- [ ] Check file size: should be ~1,474 bytes
- [ ] Look for `flex-direction: column;` on `.page`
- [ ] Clear browser cache and refresh

### Issue: Game buttons have no styling
- [ ] Verify Game.razor.css file size is > 13,000 bytes
- [ ] Check for `.control-btn` class definitions
- [ ] Verify CSS is scoped correctly in component

### Issue: Colors don't match description
- [ ] Check that dark mode is enabled in browser
- [ ] Verify no browser extensions are overriding CSS
- [ ] Compare color values in DevTools computed styles

## 📞 Support Info

If tests don't pass:

1. **Check file modifications:**
   ```powershell
   Get-Item C:\dev\MechWar\MechWar\Pages\Game.razor.css | % {$_.LastWriteTime}
   Get-Item C:\dev\MechWar\MechWar\Layout\MainLayout.razor.css | % {$_.LastWriteTime}
   ```

2. **Rebuild from scratch:**
   ```powershell
   dotnet clean
   dotnet build
   ```

3. **Check for typos:**
   - Search for `.control-btn` in Game.razor.css
   - Search for `flex-direction: column` in MainLayout.razor.css

4. **Verify CSS is loaded:**
   - DevTools → Sources tab
   - Look for App.styles.css file
   - Check console for CSS parse errors

---

## ✨ Success Indicators

When everything is working correctly, you'll see:
1. ✅ Clean build with no errors
2. ✅ Home page with proper styling
3. ✅ Game page with horizontal top bar
4. ✅ Styled control buttons with proper colors
5. ✅ Matrix-style console with syntax highlighting
6. ✅ Responsive layout on different screen sizes
7. ✅ Smooth interactions with no lag
8. ✅ No errors in browser console

**You're all set! Enjoy MechWar! 🎮**

