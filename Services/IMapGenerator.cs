using MechWar.Models;

namespace MechWar.Services;

public interface IMapGenerator
{
    HexMap Generate(ThemeDefinition theme, int width, int height, int seed);
}

