namespace GraviSharp;

public class Vector
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    
    public Vector(double X, double Y, double Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    public Vector Add(Vector vector)
    {
        X += vector.X;
        Y += vector.Y;
        Z += vector.Z;

        return this;
    }

    public void Set(double X, double Y, double Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    public override string ToString()
    {
        return $"{X}, {Y}, {Z}";
    }

}