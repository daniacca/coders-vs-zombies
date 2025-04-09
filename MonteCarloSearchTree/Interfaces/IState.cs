namespace CodersVsZombies.MonteCarlo.Interfaces;

public interface IState<TPlayer, TAction> where TPlayer : IPlayer where TAction: IAction
{
    TPlayer CurrentPlayer { get; }

    IList<TAction> Actions { get; }

    void ApplyAction(TAction action);

    double GetResult(TPlayer forPlayer);

    IState<TPlayer, TAction> Clone();
}