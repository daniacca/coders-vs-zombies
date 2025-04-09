namespace CodersVsZombies.Game.Terrain;

public class Coord 
{
    private static int MIN_X = 0;
    private static int MIN_Y = 0;

    private static int MAX_X = 16000;
    private static int MAX_Y = 9000;

    private int _x;
    public int X
    { 
        get => _x; 
        set 
        {
            if(value < MIN_X) value = MIN_X;

            if(value > MAX_X) value = MAX_X;
            
            _x = value;
        } 
    }

    private int _y;
    public int Y 
    { 
        get => _y; 
        set 
        {
            if(value < MIN_Y) value = MIN_Y;

            if(value > MAX_Y) value = MAX_Y;

            _y = value;
        } 
    }

    public override string ToString() => $"{_x} {_y}";

    public static bool operator ==(Coord p1, Coord p2) => p1.X == p2.X && p1.Y == p2.Y;

    public static bool operator !=(Coord p1, Coord p2) => p1.X != p2.X || p1.Y != p2.Y;

    public override bool Equals(object? obj) => (obj is Coord p2) ? this == p2 : false;

    public override int GetHashCode() => HashCode.Combine(_x, _y);    

    public double Distance(Coord p) => Math.Sqrt(Math.Pow(p.X - _x, 2) + Math.Pow(p.Y - _y, 2));

    public double Angle(Coord p) => (Math.Atan(((p.Y - _y) / (p.X - _x == 0 ? 1 : p.X - _x))));

    public Coord Translate(double distance, double angle)
    {
        return new Coord {
            X = _x + (int)Math.Round(distance * Math.Cos(angle)),
            Y = _y + (int)Math.Round(distance * Math.Sin(angle)),
        };
    }

    public static Coord Centroid(IEnumerable<Coord> points) => new Coord {
        X = points.Sum(p => p.X) / points.Count(),
        Y = points.Sum(p => p.Y) / points.Count(),
    };
}