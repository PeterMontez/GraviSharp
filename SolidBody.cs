namespace GraviSharp;

using _3dSharp;

public class SolidBody
{
    public double mass { get; set; }
    public Point3d position { get; set; }
    public Vector speed { get; set; }
    public Vector acceleration { get; set; }
    public Vector airResistance { get; set; }
    public DateTime time { get; set; }
    public Angle angle { get; set; }
    public bool colision { get; set; }
    
    public SolidBody(Point3d position, bool colision, Vector? speed = null, Vector? acceleration = null, Angle? angle = null)
    {
        this.speed  = speed == null ? new Vector(0, 0, 0) : speed;
        this.acceleration = acceleration == null ? new Vector(0, -9.8, 0) : acceleration;
        this.angle = angle == null ? new Angle(0, 0, 0) : angle;
        this.colision = colision;
        this.position = position;
    }

    public Point3d UpdatePosition()
    {
        position.X = position.X + speed.X;
        position.Y = position.Y + speed.Y;
        position.Z = position.Z + speed.Z;

        return position;
    }

    public Vector UpdateSpeed()
    {
        double timePassed = (DateTime.Now - time).TotalSeconds;
        time = DateTime.Now;

        speed.X += acceleration.X * timePassed;
        speed.Y += acceleration.Y * timePassed;
        speed.Z += acceleration.Z * timePassed;

        return speed;
    }

    public Vector AddForce(Vector vector)
    {
        acceleration.X = vector.X / mass;
        acceleration.Y = vector.Y / mass;
        acceleration.Z = vector.Z / mass;

        return acceleration;
    }

    public Vector AddAirResistance()
    {

        return acceleration;
    }

}