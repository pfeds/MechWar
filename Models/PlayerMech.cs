namespace MechWar.Models;

public sealed record PlayerMech(
    string MechName,
    MechClass Class,
    int Column,
    int Row,
    HexDirection Facing,
    int RemainingMovement,
    int MaxMovement,
    IReadOnlyList<Weapon>? Weapons = null,
    int WalkMp = 4,
    int RunMp = 6,
    int JumpMp = 0);

