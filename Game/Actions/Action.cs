using CodersVsZombies.MonteCarlo.Interfaces;

namespace CodersVsZombies.Game.Actions;

public abstract class GameAction : IAction
{
    public abstract string Name { get; }
}