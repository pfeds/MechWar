namespace MechWar.Services;

public sealed class GameUiState
{
    public event Action? StateChanged;

    // Game setup params (set on Home page, read on Game page)
    public string SelectedThemeKey { get; private set; } = string.Empty;
    public int MapWidth { get; private set; } = 28;
    public int MapHeight { get; private set; } = 20;
    public string PlayerMechName { get; private set; } = "Jenner JR7-D";
    public string EnemyMechName { get; private set; } = "Hunchback HBK-4G";
    public bool HasPendingGame { get; private set; }

    public void RequestNewGame(string themeKey, int width, int height, string playerMech, string enemyMech)
    {
        SelectedThemeKey = themeKey;
        MapWidth = width;
        MapHeight = height;
        PlayerMechName = playerMech;
        EnemyMechName = enemyMech;
        HasPendingGame = true;
        StateChanged?.Invoke();
    }

    public void ConsumeGame()
    {
        HasPendingGame = false;
    }

    // Legacy popup support (kept for NavMenu compat)
    public bool ShowCreateGamePopup { get; private set; }

    public void OpenCreateGamePopup()
    {
        ShowCreateGamePopup = true;
        StateChanged?.Invoke();
    }

    public void CloseCreateGamePopup()
    {
        ShowCreateGamePopup = false;
        StateChanged?.Invoke();
    }
}
