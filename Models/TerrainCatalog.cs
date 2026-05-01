namespace MechWar.Models;

public sealed record TerrainInfo(string Label, string ColorHex);

public static class TerrainCatalog
{
    private static readonly IReadOnlyDictionary<TerrainType, TerrainInfo> ByType =
        new Dictionary<TerrainType, TerrainInfo>
        {
            [TerrainType.Plains] = new("Plains", "#87995f"),
            [TerrainType.Hills] = new("Hills", "#8a7450"),
            [TerrainType.Mountains] = new("Mountains", "#666870"),
            [TerrainType.Water] = new("Water", "#356995"),
            [TerrainType.Swamp] = new("Swamp", "#54674a"),
            [TerrainType.Forest] = new("Forest", "#355534"),
            [TerrainType.Jungle] = new("Jungle", "#28472c"),
            [TerrainType.Sand] = new("Sand", "#c8af74"),
            [TerrainType.Rock] = new("Rock", "#7e746b"),
            [TerrainType.Ice] = new("Ice", "#c9dce3"),
            [TerrainType.Urban] = new("Urban", "#777f89"),
            [TerrainType.Canyon] = new("Canyon", "#a15b34"),
            [TerrainType.Crater] = new("Crater", "#6f6970"),
            [TerrainType.Scrub] = new("Scrub", "#989964")
        };

    public static TerrainInfo GetInfo(TerrainType type) => ByType[type];

    public static string GetLabel(TerrainType type) => GetInfo(type).Label;

    public static string GetColor(TerrainType type) => GetInfo(type).ColorHex;
}

