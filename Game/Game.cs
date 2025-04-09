using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CodersVsZombies.Utils;
using CodersVsZombies.MonteCarlo.Interfaces;
using CodersVsZombies.Game.Characters;
using CodersVsZombies.Game.Actions;
using CodersVsZombies.Game.Terrain;

namespace CodersVsZombies.Game;

public class Game : IState<Hero, Move>
{
    public IList<Human> Humans { get; set; }

    public IList<Zombie> Zombies { get; set; }

    public double Score { get; set; }

    public Game(Hero player, IList<Human> humans, IList<Zombie> zombies, double actualScore)
    {
        CurrentPlayer = player;
        Humans = humans;
        Zombies = zombies;
        Score = actualScore;
    }

    public Hero CurrentPlayer { get; set; }

    public IList<Move> Actions
    {
        get
        {
            var possibleActions = new List<Move>();

            if(Loose || Victory) return possibleActions;

            possibleActions.AddRange(Humans
                .Where(h => CurrentPlayer.CanSave(h, Zombies))
                .Select(h => new Move { Position = h.Position, TargetUnit = UnitsType.Human, TargetId = h.Id }));
            
            possibleActions.AddRange(Zombies
                .Select(z => new Move { Position = z.Position, TargetUnit = UnitsType.Zombie, TargetId = z.Id }));
            
            if(Zombies.Count > 1)
            {
                var zombieCentroid = Coord.Centroid(Zombies.Select(z => z.Position));
                possibleActions.Add(new Move { Position = zombieCentroid });
            }

            return possibleActions.Distinct().ToList();
        }
    }

    public void ApplyAction(Move action)
    {
        // 1. Zombies move towards target
        Zombies = Zombies.Select(z => z.Move(CurrentPlayer, Humans)).ToList();

        // 2. Ash moves towards target
        CurrentPlayer = CurrentPlayer.Move(action.Position);

        // 3. Any Zombie within range from Ash is destroyed, and points are computed
        var toKill = Zombies.Where(z => z.Position.Distance(CurrentPlayer.Position) <= CurrentPlayer.WeaponRange).Count();
        var earnedRoundPoints = ComputePoints(toKill);

        Score += earnedRoundPoints;
        Zombies = Zombies.Where(z => z.Position.Distance(CurrentPlayer.Position) > CurrentPlayer.WeaponRange).ToList();

        // 4. Zombie kill any humans they share position
        Humans = Humans.Where(h => Zombies.All(z => z.Position != h.Position)).ToList();
    }

    private double ComputePoints(int killedNumber) 
    {
        double roundScore = 0;
        
        for(int i = 0; i < killedNumber; i++) 
            roundScore += (Humans.Count * 10) * Fibonacci.Calculate(i+1);
        
        return roundScore;
    }

    public bool Loose => !Humans.Any();

    public bool Victory => !Zombies.Any() && Humans.Any();

    public bool GameOver => Loose || Victory;

    public double GetResult(Hero forPlayer)
    {
        return Victory
            ? 1 
            : Loose 
                ? -1 
                : 0;
    }

    public IState<Hero, Move> Clone() => new Game(CurrentPlayer, Humans, Zombies, Score);
}