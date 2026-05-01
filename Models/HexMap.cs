namespace MechWar.Models;

public sealed record HexMap(
    int Width,
    int Height,
    ThemeDefinition Theme,
    int Seed,
    IReadOnlyList<HexTile> Tiles);

