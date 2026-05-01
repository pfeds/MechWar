using MechWar.Models;

namespace MechWar.Services;

public sealed class MapGenerator : IMapGenerator
{
    public HexMap Generate(ThemeDefinition theme, int width, int height, int seed)
    {
        width = Math.Clamp(width, 8, 80);
        height = Math.Clamp(height, 8, 60);

        var random = new Random(seed);
        var terrain = new TerrainType[width, height];
        var priorities = new int[width, height];

        FillBaseTerrain(terrain, priorities, theme.BaseTerrain);

        foreach (var rule in theme.FeatureRules.OrderBy(rule => rule.Priority))
        {
            PaintFeatureRegions(rule, terrain, priorities, width, height, random);
        }

        SmoothIsolatedTiles(terrain, width, height);

        var tiles = new List<HexTile>(width * height);
        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                var terrainType = terrain[column, row];
                int? hillLevel = terrainType == TerrainType.Hills ? RollWeightedLevel(random) : null;
                int? waterDepth = terrainType == TerrainType.Water ? RollWeightedLevel(random) : null;
                tiles.Add(new HexTile(column, row, terrainType, hillLevel, waterDepth));
            }
        }

        return new HexMap(width, height, theme, seed, tiles);
    }

    private static void FillBaseTerrain(TerrainType[,] terrain, int[,] priorities, TerrainType baseTerrain)
    {
        for (var row = 0; row < terrain.GetLength(1); row++)
        {
            for (var column = 0; column < terrain.GetLength(0); column++)
            {
                terrain[column, row] = baseTerrain;
                priorities[column, row] = 0;
            }
        }
    }

    private static void PaintFeatureRegions(
        TerrainFeatureRule rule,
        TerrainType[,] terrain,
        int[,] priorities,
        int width,
        int height,
        Random random)
    {
        var area = width * height;
        var targetCells = (int)Math.Round(area * rule.Coverage);
        var painted = 0;
        var safetyCounter = 0;

        while (painted < targetCells && safetyCounter < area * 3)
        {
            safetyCounter++;
            var seedColumn = random.Next(0, width);
            var seedRow = random.Next(0, height);

            if (priorities[seedColumn, seedRow] > rule.Priority)
            {
                continue;
            }

            var regionSizeTarget = random.Next(rule.MinRegionSize, rule.MaxRegionSize + 1);
            var actualSize = PaintRegion(
                seedColumn,
                seedRow,
                Math.Min(regionSizeTarget, targetCells - painted),
                rule,
                terrain,
                priorities,
                width,
                height,
                random);

            painted += actualSize;
        }
    }

    private static int PaintRegion(
        int startColumn,
        int startRow,
        int targetSize,
        TerrainFeatureRule rule,
        TerrainType[,] terrain,
        int[,] priorities,
        int width,
        int height,
        Random random)
    {
        var paintedCount = 0;
        var frontier = new List<(int Column, int Row)>();
        var region = new HashSet<int>();

        if (!TryPaint(startColumn, startRow, rule, terrain, priorities, width, height))
        {
            return 0;
        }

        frontier.Add((startColumn, startRow));
        region.Add(ToKey(startColumn, startRow, width));
        paintedCount++;

        var current = (Column: startColumn, Row: startRow);

        while (paintedCount < targetSize && frontier.Count > 0)
        {
            var source = rule.Linear && random.NextDouble() < 0.7
                ? current
                : frontier[random.Next(0, frontier.Count)];

            var candidateNeighbors = GetNeighbors(source.Column, source.Row, width, height)
                .Where(neighbor =>
                    priorities[neighbor.Column, neighbor.Row] <= rule.Priority &&
                    !region.Contains(ToKey(neighbor.Column, neighbor.Row, width)))
                .ToList();

            if (candidateNeighbors.Count == 0)
            {
                frontier.Remove(source);
                continue;
            }

            var chosen = candidateNeighbors[random.Next(0, candidateNeighbors.Count)];

            if (!TryPaint(chosen.Column, chosen.Row, rule, terrain, priorities, width, height))
            {
                continue;
            }

            frontier.Add(chosen);
            region.Add(ToKey(chosen.Column, chosen.Row, width));
            current = (Column: chosen.Column, Row: chosen.Row);
            paintedCount++;

            if (!HasExpandableNeighbor(source.Column, source.Row, priorities, rule.Priority, region, width, height))
            {
                frontier.Remove(source);
            }
        }

        return paintedCount;
    }

    private static bool TryPaint(
        int column,
        int row,
        TerrainFeatureRule rule,
        TerrainType[,] terrain,
        int[,] priorities,
        int width,
        int height)
    {
        if (column < 0 || row < 0 || column >= width || row >= height)
        {
            return false;
        }

        if (priorities[column, row] > rule.Priority)
        {
            return false;
        }

        terrain[column, row] = rule.Terrain;
        priorities[column, row] = rule.Priority;
        return true;
    }

    private static bool HasExpandableNeighbor(
        int column,
        int row,
        int[,] priorities,
        int maxPriority,
        HashSet<int> region,
        int width,
        int height)
    {
        foreach (var neighbor in GetNeighbors(column, row, width, height))
        {
            if (priorities[neighbor.Column, neighbor.Row] <= maxPriority &&
                !region.Contains(ToKey(neighbor.Column, neighbor.Row, width)))
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<(int Column, int Row)> GetNeighbors(int column, int row, int width, int height)
    {
        int[][] offsets =
            row % 2 == 0
                ?
                [
                    [-1, 0],
                    [1, 0],
                    [0, -1],
                    [-1, -1],
                    [0, 1],
                    [-1, 1]
                ]
                :
                [
                    [-1, 0],
                    [1, 0],
                    [1, -1],
                    [0, -1],
                    [1, 1],
                    [0, 1]
                ];

        foreach (var offset in offsets)
        {
            var neighborColumn = column + offset[0];
            var neighborRow = row + offset[1];

            if (neighborColumn >= 0 && neighborRow >= 0 && neighborColumn < width && neighborRow < height)
            {
                yield return (neighborColumn, neighborRow);
            }
        }
    }

    private static void SmoothIsolatedTiles(TerrainType[,] terrain, int width, int height)
    {
        var copy = (TerrainType[,])terrain.Clone();

        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                var counts = new Dictionary<TerrainType, int>();

                foreach (var neighbor in GetNeighbors(column, row, width, height))
                {
                    var terrainType = copy[neighbor.Column, neighbor.Row];
                    counts[terrainType] = counts.GetValueOrDefault(terrainType) + 1;
                }

                if (counts.Count == 0)
                {
                    continue;
                }

                var current = copy[column, row];
                var sameCount = counts.GetValueOrDefault(current);
                var (majorityTerrain, majorityCount) = counts.MaxBy(pair => pair.Value);

                if (sameCount <= 1 && majorityCount >= 3)
                {
                    terrain[column, row] = majorityTerrain;
                }
            }
        }
    }

    private static int ToKey(int column, int row, int width) => row * width + column;

    // Weighted Battletech-style tiers: mostly 1, fewer 2, rare 3.
    private static int RollWeightedLevel(Random random)
    {
        var roll = random.Next(0, 100);
        if (roll < 70)
        {
            return 1;
        }

        if (roll < 92)
        {
            return 2;
        }

        return 3;
    }
}

