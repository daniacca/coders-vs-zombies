using CodersVsZombies.MonteCarlo.Interfaces;
using CodersVsZombies.Game.Terrain;

namespace CodersVsZombies.Game.Actions;

public enum UnitsType {
    Human,
    Zombie
}

public class Move : GameAction 
{
    public override string Name => "Move to";

    public Coord Position { get; init; }

    public UnitsType TargetUnit { get; init; }

    public int TargetId { get; init; }

    public override string ToString() => $"Move to {TargetUnit} ID: {TargetId}, coord: ({Position.X}, {Position.Y})";
}
