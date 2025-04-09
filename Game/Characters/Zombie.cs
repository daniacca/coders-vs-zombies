using CodersVsZombies.Game.Terrain;

namespace CodersVsZombies.Game.Characters;

public class Zombie : Unit
{
    public Coord NextPosition { get; init; }
    
    public override double Movement => 400;

    public Zombie Move(Hero player, IList<Human> humans) 
    {
        if(NextPosition is not null) 
            return new Zombie 
            {
                Id = Id,
                Position = NextPosition,
                NextPosition = null
            };

        var units = new List<Unit>(humans.Count + 1);
        units.Add(player);
        units.AddRange(humans);
        
        var nearest = Nearest(units);
        var distance = Position.Distance(nearest.Position);
        var angle = Position.Angle(nearest.Position);
        return new Zombie 
        {
            Id = Id,
            Position = distance <= Movement ? nearest.Position : Position.Translate(Movement, angle),
            NextPosition = null
        };
    }
}