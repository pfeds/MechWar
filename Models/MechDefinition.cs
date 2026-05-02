namespace MechWar.Models;

public sealed record Weapon(
    string Name,
    int Damage,
    int Heat,
    int MaxRange,
    HitLocation Location
);

public sealed record MechDefinition(
    string Name,
    int Tonnage,
    MechClass Class,
    int WalkSpeed,
    int RunSpeed,
    int JumpSpeed,
    int HeatSinks,
    int TotalArmor,
    IReadOnlyList<Weapon> Weapons)
{
    public int GetWalkMp() => WalkSpeed;
    public int GetRunMp() => RunSpeed;
    public int GetJumpMp() => JumpSpeed;
}

public static class MechCatalog
{
    private static readonly Dictionary<string, MechDefinition> _mechs = new(StringComparer.OrdinalIgnoreCase)
    {
        // Light Mechs
        ["Locust LCT-1V"] = new(
            "Locust LCT-1V", 20, MechClass.Light, 8, 12, 0, 10, 64,
            new[] {
                new Weapon("MG", 2, 0, 3, HitLocation.RightArm),
                new Weapon("MG", 2, 0, 3, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Stinger STG-3R"] = new(
            "Stinger STG-3R", 20, MechClass.Light, 6, 9, 6, 10, 80,
            new[] {
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.RightArm),
                new Weapon("MG", 2, 0, 3, HitLocation.LeftArm)
            }
        ),
        ["Wasp WSP-1A"] = new(
            "Wasp WSP-1A", 20, MechClass.Light, 6, 9, 6, 10, 80,
            new[] {
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("SRM-2", 4, 2, 6, HitLocation.CenterTorso)
            }
        ),

        // Medium Mechs
        ["Commando COM-2D"] = new(
            "Commando COM-2D", 25, MechClass.Medium, 6, 9, 0, 10, 88,
            new[] {
                new Weapon("SRM-6", 12, 4, 6, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm)
            }
        ),
        ["Jenner JR7-D"] = new(
            "Jenner JR7-D", 35, MechClass.Medium, 7, 11, 5, 10, 112,
            new[] {
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("SRM-4", 8, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Panther PNT-9R"] = new(
            "Panther PNT-9R", 35, MechClass.Medium, 4, 6, 4, 10, 104,
            new[] {
                new Weapon("PPC", 10, 10, 9, HitLocation.RightArm),
                new Weapon("SRM-4", 8, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Firestarter FS9-H"] = new(
            "Firestarter FS9-H", 35, MechClass.Medium, 6, 9, 6, 10, 112,
            new[] {
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.Head),
                new Weapon("Flamer", 3, 3, 3, HitLocation.RightArm),
                new Weapon("Flamer", 3, 3, 3, HitLocation.RightArm),
                new Weapon("Flamer", 3, 3, 3, HitLocation.LeftArm),
                new Weapon("Flamer", 3, 3, 3, HitLocation.LeftArm),
                new Weapon("Flamer", 3, 3, 3, HitLocation.CenterTorso),
                new Weapon("Flamer", 3, 3, 3, HitLocation.CenterTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.RightArm),
                new Weapon("MG", 2, 0, 3, HitLocation.LeftArm)
            }
        ),
        ["Wolfhound WLF-1"] = new(
            "Wolfhound WLF-1", 35, MechClass.Medium, 6, 9, 0, 10, 112,
            new[] {
                new Weapon("Large Laser", 8, 8, 8, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.Head)
            }
        ),
        ["Phoenix Hawk PXH-1"] = new(
            "Phoenix Hawk PXH-1", 45, MechClass.Medium, 6, 9, 6, 10, 144,
            new[] {
                new Weapon("Large Laser", 8, 8, 8, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.RightArm),
                new Weapon("MG", 2, 0, 3, HitLocation.LeftArm)
            }
        ),

        // Heavy Mechs
        ["Clint CLNT-2-3T"] = new(
            "Clint CLNT-2-3T", 40, MechClass.Heavy, 6, 9, 0, 10, 136,
            new[] {
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftTorso)
            }
        ),
        ["Assassin ASN-21"] = new(
            "Assassin ASN-21", 40, MechClass.Heavy, 7, 11, 0, 10, 128,
            new[] {
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightArm),
                new Weapon("SRM-2", 4, 2, 6, HitLocation.LeftTorso),
                new Weapon("LRM-5", 5, 2, 15, HitLocation.RightTorso)
            }
        ),
        ["Vulcan VL-2T"] = new(
            "Vulcan VL-2T", 40, MechClass.Heavy, 6, 9, 0, 10, 128,
            new[] {
                new Weapon("AC/2", 2, 1, 14, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Flamer", 3, 3, 3, HitLocation.CenterTorso)
            }
        ),
        ["Hunchback HBK-4G"] = new(
            "Hunchback HBK-4G", 50, MechClass.Heavy, 4, 6, 0, 10, 176,
            new[] {
                new Weapon("AC/20", 20, 7, 6, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Small Laser", 3, 2, 3, HitLocation.CenterTorso),
                new Weapon("Small Laser", 3, 2, 3, HitLocation.CenterTorso)
            }
        ),
        ["Centurion CN9-A"] = new(
            "Centurion CN9-A", 50, MechClass.Heavy, 4, 6, 0, 10, 168,
            new[] {
                new Weapon("AC/10", 10, 3, 8, HitLocation.RightArm),
                new Weapon("LRM-10", 10, 4, 20, HitLocation.LeftTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftTorso)
            }
        ),
        ["Trebuchet TBT-5N"] = new(
            "Trebuchet TBT-5N", 50, MechClass.Heavy, 5, 8, 0, 10, 152,
            new[] {
                new Weapon("LRM-15", 15, 5, 20, HitLocation.LeftTorso),
                new Weapon("LRM-15", 15, 5, 20, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Griffin GRF-1N"] = new(
            "Griffin GRF-1N", 55, MechClass.Heavy, 5, 8, 5, 10, 176,
            new[] {
                new Weapon("PPC", 10, 10, 9, HitLocation.RightArm),
                new Weapon("LRM-10", 10, 4, 20, HitLocation.LeftTorso)
            }
        ),
        ["Shadow Hawk SHD-2H"] = new(
            "Shadow Hawk SHD-2H", 55, MechClass.Heavy, 5, 8, 3, 10, 176,
            new[] {
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightArm),
                new Weapon("LRM-5", 5, 2, 15, HitLocation.LeftTorso),
                new Weapon("SRM-2", 4, 2, 6, HitLocation.Head),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Wolverine WVR-6R"] = new(
            "Wolverine WVR-6R", 55, MechClass.Heavy, 5, 8, 5, 10, 184,
            new[] {
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightArm),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.LeftTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Rifleman RFL-3N"] = new(
            "Rifleman RFL-3N", 60, MechClass.Heavy, 4, 6, 0, 10, 184,
            new[] {
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightArm),
                new Weapon("AC/5", 5, 1, 10, HitLocation.LeftArm),
                new Weapon("Large Laser", 8, 8, 8, HitLocation.RightArm),
                new Weapon("Large Laser", 8, 8, 8, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["JagerMech JM6-S"] = new(
            "JagerMech JM6-S", 65, MechClass.Heavy, 4, 6, 0, 10, 152,
            new[] {
                new Weapon("AC/2", 2, 1, 14, HitLocation.RightArm),
                new Weapon("AC/2", 2, 1, 14, HitLocation.LeftArm),
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightArm),
                new Weapon("AC/5", 5, 1, 10, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Thunderbolt TDR-5S"] = new(
            "Thunderbolt TDR-5S", 65, MechClass.Heavy, 4, 6, 0, 10, 208,
            new[] {
                new Weapon("Large Laser", 8, 8, 8, HitLocation.RightArm),
                new Weapon("LRM-15", 15, 5, 20, HitLocation.LeftTorso),
                new Weapon("SRM-2", 4, 2, 6, HitLocation.Head),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightTorso),
                new Weapon("Small Laser", 3, 2, 3, HitLocation.LeftLeg),
                new Weapon("Small Laser", 3, 2, 3, HitLocation.RightLeg)
            }
        ),
        ["Catapult CPLT-C1"] = new(
            "Catapult CPLT-C1", 65, MechClass.Heavy, 4, 6, 4, 10, 184,
            new[] {
                new Weapon("LRM-15", 15, 5, 20, HitLocation.RightArm),
                new Weapon("LRM-15", 15, 5, 20, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.Head)
            }
        ),
        ["Archer ARC-2R"] = new(
            "Archer ARC-2R", 70, MechClass.Heavy, 4, 6, 0, 10, 216,
            new[] {
                new Weapon("LRM-20", 20, 6, 24, HitLocation.LeftTorso),
                new Weapon("LRM-20", 20, 6, 24, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm)
            }
        ),
        ["Warhammer WHM-6R"] = new(
            "Warhammer WHM-6R", 70, MechClass.Heavy, 4, 6, 0, 18, 224,
            new[] {
                new Weapon("PPC", 10, 10, 9, HitLocation.RightArm),
                new Weapon("PPC", 10, 10, 9, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.LeftTorso),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.RightTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.CenterTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.CenterTorso)
            }
        ),
        ["Marauder MAD-3R"] = new(
            "Marauder MAD-3R", 75, MechClass.Heavy, 4, 6, 0, 16, 216,
            new[] {
                new Weapon("PPC", 10, 10, 9, HitLocation.RightArm),
                new Weapon("PPC", 10, 10, 9, HitLocation.LeftArm),
                new Weapon("AC/5", 5, 1, 10, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.Head),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso)
            }
        ),
        ["Orion ON1-K"] = new(
            "Orion ON1-K", 75, MechClass.Heavy, 4, 6, 0, 10, 240,
            new[] {
                new Weapon("AC/10", 10, 3, 8, HitLocation.RightArm),
                new Weapon("LRM-15", 15, 5, 20, HitLocation.LeftTorso),
                new Weapon("SRM-4", 8, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm)
            }
        ),
        ["Battlemaster BLR-1G"] = new(
            "Battlemaster BLR-1G", 85, MechClass.Heavy, 4, 6, 0, 18, 272,
            new[] {
                new Weapon("PPC", 10, 10, 9, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.Head),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.LeftTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.CenterTorso),
                new Weapon("MG", 2, 0, 3, HitLocation.CenterTorso)
            }
        ),
        ["Stalker STK-3F"] = new(
            "Stalker STK-3F", 85, MechClass.Heavy, 3, 5, 0, 10, 280,
            new[] {
                new Weapon("LRM-10", 10, 4, 20, HitLocation.LeftTorso),
                new Weapon("LRM-10", 10, 4, 20, HitLocation.RightTorso),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.LeftArm),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.RightArm),
                new Weapon("Large Laser", 8, 8, 8, HitLocation.LeftTorso),
                new Weapon("Large Laser", 8, 8, 8, HitLocation.RightTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm)
            }
        ),
        ["Awesome AWS-8Q"] = new(
            "Awesome AWS-8Q", 80, MechClass.Heavy, 3, 5, 0, 28, 240,
            new[] {
                new Weapon("PPC", 10, 10, 9, HitLocation.RightArm),
                new Weapon("PPC", 10, 10, 9, HitLocation.LeftArm),
                new Weapon("PPC", 10, 10, 9, HitLocation.RightTorso)
            }
        ),
        ["Atlas AS7-D"] = new(
            "Atlas AS7-D", 100, MechClass.Heavy, 3, 5, 0, 10, 304,
            new[] {
                new Weapon("AC/20", 20, 7, 6, HitLocation.RightTorso),
                new Weapon("LRM-20", 20, 6, 24, HitLocation.LeftTorso),
                new Weapon("SRM-6", 12, 4, 6, HitLocation.LeftTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.RightArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.LeftArm),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.CenterTorso),
                new Weapon("Medium Laser", 5, 3, 6, HitLocation.Head)
            }
        )
    };

    public static IReadOnlyList<string> AllMechNames => _mechs.Keys.ToList();

    public static MechDefinition? Get(string name) =>
        _mechs.TryGetValue(name, out var mech) ? mech : null;

    public static MechDefinition GetRandom() =>
        _mechs.Values.ElementAt(Random.Shared.Next(_mechs.Count));
}

