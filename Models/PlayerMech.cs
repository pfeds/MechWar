namespace MechWar.Models;

public sealed record PlayerMech(
    MechClass Class,
    int Column,
    int Row,
    HexDirection Facing,
    int RemainingMovement,
    int MaxMovement);

