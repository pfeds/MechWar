using MechWar.Models;

namespace MechWar.Data;

public static class ThemeCatalog
{
    public static IReadOnlyList<ThemeDefinition> All { get; } =
    [
        new(
            "lunar",
            "Lunar",
            TerrainType.Rock,
            [
                new(TerrainType.Crater, 0.24, 4, 14, false, 2),
                new(TerrainType.Hills, 0.14, 5, 16, true, 3)
            ]),
        new(
            "martian",
            "Martian",
            TerrainType.Sand,
            [
                new(TerrainType.Rock, 0.20, 6, 18, false, 2),
                new(TerrainType.Canyon, 0.12, 7, 20, true, 3),
                new(TerrainType.Crater, 0.08, 4, 12, false, 4)
            ]),
        new(
            "desert",
            "Desert",
            TerrainType.Sand,
            [
                new(TerrainType.Rock, 0.18, 6, 18, false, 2),
                new(TerrainType.Canyon, 0.08, 8, 20, true, 3),
                new(TerrainType.Scrub, 0.10, 5, 15, false, 4)
            ]),
        new(
            "badlands",
            "Badlands",
            TerrainType.Rock,
            [
                new(TerrainType.Canyon, 0.16, 7, 22, true, 2),
                new(TerrainType.Hills, 0.14, 5, 15, false, 3),
                new(TerrainType.Scrub, 0.08, 4, 11, false, 4)
            ]),
        new(
            "highlands",
            "Highlands",
            TerrainType.Hills,
            [
                new(TerrainType.Mountains, 0.20, 7, 22, true, 2),
                new(TerrainType.Forest, 0.12, 6, 18, false, 3),
                new(TerrainType.Water, 0.06, 4, 12, true, 4)
            ]),
        new(
            "lowlands",
            "Lowlands",
            TerrainType.Plains,
            [
                new(TerrainType.Water, 0.10, 8, 24, true, 2),
                new(TerrainType.Swamp, 0.14, 6, 17, false, 3),
                new(TerrainType.Forest, 0.10, 5, 14, false, 4)
            ]),
        new(
            "jungle",
            "Jungle",
            TerrainType.Jungle,
            [
                new(TerrainType.Water, 0.08, 8, 20, true, 2),
                new(TerrainType.Swamp, 0.14, 6, 16, false, 3),
                new(TerrainType.Hills, 0.06, 5, 12, false, 4)
            ]),
        new(
            "tundra",
            "Tundra",
            TerrainType.Scrub,
            [
                new(TerrainType.Ice, 0.16, 6, 16, false, 2),
                new(TerrainType.Water, 0.06, 6, 14, true, 3),
                new(TerrainType.Hills, 0.08, 5, 14, false, 4)
            ]),
        new(
            "polar",
            "Polar",
            TerrainType.Ice,
            [
                new(TerrainType.Rock, 0.12, 5, 13, false, 2),
                new(TerrainType.Water, 0.08, 6, 16, true, 3),
                new(TerrainType.Hills, 0.08, 5, 14, false, 4)
            ]),
        new(
            "urban-city",
            "Urban / City",
            TerrainType.Urban,
            [
                new(TerrainType.Plains, 0.14, 5, 13, false, 2),
                new(TerrainType.Water, 0.06, 8, 18, true, 3),
                new(TerrainType.Rock, 0.07, 4, 10, false, 4)
            ]),
        new(
            "alpine-mountain",
            "Alpine / Mountain",
            TerrainType.Mountains,
            [
                new(TerrainType.Hills, 0.18, 6, 16, false, 2),
                new(TerrainType.Ice, 0.10, 5, 14, false, 3),
                new(TerrainType.Forest, 0.08, 5, 13, false, 4),
                new(TerrainType.Canyon, 0.05, 7, 18, true, 5)
            ]),
        new(
            "grasslands-savanna",
            "Grasslands / Savanna",
            TerrainType.Plains,
            [
                new(TerrainType.Scrub, 0.18, 6, 16, false, 2),
                new(TerrainType.Hills, 0.08, 5, 12, false, 3),
                new(TerrainType.Water, 0.05, 6, 15, true, 4)
            ]),
        new(
            "canyon",
            "Canyon",
            TerrainType.Rock,
            [
                new(TerrainType.Canyon, 0.24, 8, 24, true, 2),
                new(TerrainType.Sand, 0.12, 6, 15, false, 3),
                new(TerrainType.Hills, 0.08, 5, 12, false, 4)
            ]),
        new(
            "river-delta",
            "River Delta",
            TerrainType.Swamp,
            [
                new(TerrainType.Water, 0.24, 8, 24, true, 2),
                new(TerrainType.Plains, 0.12, 6, 14, false, 3),
                new(TerrainType.Forest, 0.10, 5, 13, false, 4)
            ]),
        new(
            "moonscape",
            "Moonscape",
            TerrainType.Crater,
            [
                new(TerrainType.Rock, 0.20, 5, 14, false, 2),
                new(TerrainType.Hills, 0.10, 6, 17, true, 3),
                new(TerrainType.Sand, 0.08, 4, 11, false, 4)
            ])
    ];

    private static readonly IReadOnlyDictionary<string, ThemeDefinition> ByKey =
        All.ToDictionary(theme => theme.Key, StringComparer.OrdinalIgnoreCase);

    public static ThemeDefinition Get(string key) =>
        ByKey.TryGetValue(key, out var theme) ? theme : All[0];
}

