namespace MechWar.Models;

public sealed record TerrainFeatureRule(
    TerrainType Terrain,
    double Coverage,
    int MinRegionSize,
    int MaxRegionSize,
    bool Linear = false,
    int Priority = 1);

public sealed record ThemeDefinition(
    string Key,
    string DisplayName,
    TerrainType BaseTerrain,
    IReadOnlyList<TerrainFeatureRule> FeatureRules);

