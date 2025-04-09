using CodersVsZombies.Game.Terrain;

namespace CodersVsZombies.Game.Characters;

public abstract class Unit 
{
    public int Id { get; init; }

    public Coord Position { get; init; }
    
    public abstract double Movement { get; }
    
    public bool CanMove => Movement > 0;

    public Unit Nearest(IList<Unit> characters) 
    {
        Unit Nearest = null;
        double lowerDistance = double.MaxValue;

        foreach(var c in characters) 
        {
            var dist = Position.Distance(c.Position);
            if(dist < lowerDistance) 
            {
                lowerDistance = dist;
                Nearest = c;
            }
        }

        return Nearest;
    }
}