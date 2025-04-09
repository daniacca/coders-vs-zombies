using CodersVsZombies.Game.Characters;
using System.Collections.Generic;

namespace CodersVsZombies.Game;

public class Board 
{
    public IList<Human> Humans { get; set; }

    public IList<Zombie> Zombies { get; set; }

    public Hero CurrentPlayer { get; set; }

    public double Score { get; set; }
}