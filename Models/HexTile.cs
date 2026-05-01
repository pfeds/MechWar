namespace MechWar.Models;

public sealed record HexTile(
	int Column,
	int Row,
	TerrainType Terrain,
	int? HillLevel = null,
	int? WaterDepth = null);

