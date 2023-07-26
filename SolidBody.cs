namespace GraviSharp;

using _3dSharp;

public class SolidBody
{
    public double Mass { get; set; }
    public Point3d Position { get; set; }
    public Vector Speed { get; set; }
    public double TotalSpeed { get; set; }
    public Vector Acceleration { get; set; }
    public Vector AirResistance { get; set; }
    public DateTime Time { get; set; }
    public Angle Angle { get; set; }
    public bool Colision { get; set; }
    
    public SolidBody(Point3d position, bool colision, Vector? speed = null, Vector? acceleration = null, Angle? angle = null)
    {
        Speed  = speed == null ? new Vector(0, 0, 0) : speed;
        Acceleration = acceleration == null ? new Vector(0, -9.8, 0) : acceleration;
        Angle = angle == null ? new Angle(0, 0, 0) : angle;
        Colision = colision;
        Position = position;
    }

    public Point3d UpdatePosition()
    {
        Position.X = Position.X + Speed.X;
        Position.Y = Position.Y + Speed.Y;
        Position.Z = Position.Z + Speed.Z;

        return Position;
    }

    public Vector UpdateSpeed()
    {
        double timePassed = (DateTime.Now - Time).TotalSeconds;
        Time = DateTime.Now;

        Speed.X += Acceleration.X * timePassed;
        Speed.Y += Acceleration.Y * timePassed;
        Speed.Z += Acceleration.Z * timePassed;

        return Speed;
    }

    public Vector AddForce(Vector vector)
    {
        Acceleration.X = vector.X / Mass;
        Acceleration.Y = vector.Y / Mass;
        Acceleration.Z = vector.Z / Mass;

        return Acceleration;
    }

    public Vector AddAirResistance()
    {
        double hAngle = AMath.RadToDeg(Math.Atan(Speed.Z/Speed.X));
        double hSpeed = Math.Sqrt((Speed.Z*Speed.Z) + (Speed.X*Speed.X));
        double vAngle = AMath.RadToDeg(Math.Atan(Speed.Y/hSpeed));

        double hRelativeAngle = Angle.yaw - hAngle;
        double vRelativeAngle = Angle.pitch - vAngle;

        double hDrag = 
        double vDrag =

        double[] xRes = new double[3];
        double[] yRes = new double[3];
        double[] zRes = new double[3];

        xRes[0] = 

        return Acceleration;
    }

}