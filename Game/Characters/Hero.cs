using CodersVsZombies.MonteCarlo.Interfaces;
using CodersVsZombies.Game.Terrain;

namespace CodersVsZombies.Game.Characters;

public class Hero : Unit, IPlayer
{
    public override double Movement => 1000;

    public double WeaponRange => 2000;

    public Hero Move(Coord target) 
    {
        var distance = Position.Distance(target);
        var angle = Position.Angle(target);
        return new Hero 
        {
            Id = Id,
            Position = distance <= Movement ? target : Position.Translate(Movement, angle),
        };
    }

    public bool CanSave(Human human, IList<Zombie> zombies) 
    {
        var myDistance = Position.Distance(human.Position);
        var myStep = (myDistance - WeaponRange) / Movement;

        var nearestZombie = human.Nearest(zombies.ToArray());
        var zombieStep = human.Position.Distance(nearestZombie.Position) / nearestZombie.Movement;

        return myStep < zombieStep;
    }
}