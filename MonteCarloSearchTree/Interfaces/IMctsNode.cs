namespace CodersVsZombies.MonteCarlo.Interfaces;

public interface IMctsNode<TAction> where TAction : IAction
{
    TAction Action { get; }

    int NumRuns { get; }

    double NumWins { get; }
}