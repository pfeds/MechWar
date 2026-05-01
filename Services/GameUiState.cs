namespace MechWar.Services;

public sealed class GameUiState
{
    public event Action? StateChanged;

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

