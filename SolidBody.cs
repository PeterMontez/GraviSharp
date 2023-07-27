namespace GraviSharp;

using _3dSharp;

public class SolidBody
{
    public double Mass { get; set; } = 1;
    public Point3d Position { get; set; }
    public Vector Speed { get; set; }
    public double AirSpeed { get; set; }
    public Vector Acceleration { get; set; }
    // public Vector AirResistance { get; set; }
    public DateTime Time { get; set; }
    public Angle Angle { get; set; }
    public bool Colision { get; set; }
    public bool Gravity { get; set; }

    public SolidBody(Point3d position, bool colision, bool gravity, Vector? speed = null, Vector? acceleration = null, Angle? angle = null)
    {
        Speed = speed == null ? new Vector(0, 0, 0) : speed;
        Acceleration = acceleration == null ? new Vector(0, (gravity ? -9.8 * 100 : 0), 0) : acceleration;
        Angle = angle == null ? new Angle(0, 0, 0) : angle;
        Colision = colision;
        Gravity = gravity;
        Position = position;
        Time = DateTime.Now;
    }

    public void Update(bool[] moves, double[] moveParams)
    {
        double ElapsedTime = (DateTime.Now - Time).TotalSeconds;
        Time = DateTime.Now;

        Acceleration.Set(0, 0, 0);
        if (Gravity) AddGravity(ElapsedTime);
        Move(moves, moveParams);
        UpdateSpeed(ElapsedTime);
        UpdatePosition(ElapsedTime);
    }

    public Point3d UpdatePosition(double ElapsedTime)
    {
        Position.X = Position.X + Speed.X * ElapsedTime;
        Position.Y = Position.Y + Speed.Y * ElapsedTime;
        Position.Z = Position.Z + Speed.Z * ElapsedTime;

        return Position;
    }

    public Vector UpdateSpeed(double ElapsedTime)
    {
        Speed.X += Acceleration.X * ElapsedTime;
        Speed.Y += Acceleration.Y * ElapsedTime;
        Speed.Z += Acceleration.Z * ElapsedTime;

        AirSpeed = GetAirSpeed(Speed);

        return Speed;
    }

    public Vector AddForce(Vector vector)
    {
        Acceleration.X = vector.X / Mass;
        Acceleration.Y = vector.Y / Mass;
        Acceleration.Z = vector.Z / Mass;

        return Acceleration;
    }

    public void Thrust(double force)
    {
        Vector crr = new Vector(force, 0, 0);

        Angle hRotation = new Angle(0, 0, Angle.pitch);

        double X = AMath.DgCos(hRotation.roll) * AMath.DgCos(hRotation.yaw) * crr.X +
        (-AMath.DgSin(hRotation.roll)) * crr.Y +
        AMath.DgCos(hRotation.roll) * AMath.DgSin(hRotation.yaw) * crr.Z;

        double Y = (AMath.DgCos(hRotation.pitch) * AMath.DgSin(hRotation.roll) * AMath.DgCos(hRotation.yaw) + AMath.DgSin(hRotation.pitch) * AMath.DgSin(hRotation.yaw)) * crr.X +
        AMath.DgCos(hRotation.pitch) * AMath.DgCos(hRotation.roll) * crr.Y +
        (AMath.DgCos(hRotation.pitch) * AMath.DgSin(hRotation.roll) * AMath.DgSin(hRotation.yaw) - AMath.DgSin(hRotation.pitch) * AMath.DgCos(hRotation.yaw)) * crr.Z;

        double Z = (AMath.DgSin(hRotation.pitch) * AMath.DgSin(hRotation.roll) * AMath.DgCos(hRotation.yaw) + AMath.DgCos(hRotation.pitch) * AMath.DgSin(hRotation.yaw)) * crr.X +
        AMath.DgSin(hRotation.pitch) * AMath.DgCos(hRotation.roll) * crr.Y +
        (AMath.DgSin(hRotation.pitch) * AMath.DgSin(hRotation.roll) * AMath.DgSin(hRotation.yaw) - AMath.DgCos(hRotation.pitch) * AMath.DgCos(hRotation.yaw)) * crr.Z;

        crr = new Vector(X, Y, Z);

        Angle vRotation = new Angle(Angle.yaw, 0, 0);

        X = AMath.DgCos(vRotation.roll) * AMath.DgCos(vRotation.yaw) * crr.X +
        (-AMath.DgSin(vRotation.roll)) * crr.Y +
        AMath.DgCos(vRotation.roll) * AMath.DgSin(vRotation.yaw) * crr.Z;

        Y = (AMath.DgCos(vRotation.pitch) * AMath.DgSin(vRotation.roll) * AMath.DgCos(vRotation.yaw) + AMath.DgSin(vRotation.pitch) * AMath.DgSin(vRotation.yaw)) * crr.X +
        AMath.DgCos(vRotation.pitch) * AMath.DgCos(vRotation.roll) * crr.Y +
        (AMath.DgCos(vRotation.pitch) * AMath.DgSin(vRotation.roll) * AMath.DgSin(vRotation.yaw) - AMath.DgSin(vRotation.pitch) * AMath.DgCos(vRotation.yaw)) * crr.Z;

        Z = (AMath.DgSin(vRotation.pitch) * AMath.DgSin(vRotation.roll) * AMath.DgCos(vRotation.yaw) + AMath.DgCos(vRotation.pitch) * AMath.DgSin(vRotation.yaw)) * crr.X +
        AMath.DgSin(vRotation.pitch) * AMath.DgCos(vRotation.roll) * crr.Y +
        (AMath.DgSin(vRotation.pitch) * AMath.DgSin(vRotation.roll) * AMath.DgSin(vRotation.yaw) - AMath.DgCos(vRotation.pitch) * AMath.DgCos(vRotation.yaw)) * crr.Z;

        crr = new Vector(X, Y, Z);

        AddForce(new Vector(X, Y, Z));
    }

    public void AddGravity(double ElapsedTime)
    {
        AddForce(new Vector(0, -9.8 * 100 * ElapsedTime, 0));
    }

    public Vector AddAirResistance()
    {
        double hAngle = AMath.RadToDeg(Math.Atan(Speed.Z / Speed.X));
        double hSpeed = Math.Sqrt((Speed.Z * Speed.Z) + (Speed.X * Speed.X));
        double vAngle = AMath.RadToDeg(Math.Atan(Speed.Y / hSpeed));

        double hRelativeAngle = hAngle - Angle.yaw;
        double vRelativeAngle = vAngle - Angle.pitch;

        // double hDrag = 
        // double vDrag = 

        // -------------------- VERTICAL RESISTANCE --------------------

        double Vcomponent = AirSpeed * AMath.DgTan(vRelativeAngle);
        double Hratio = AirSpeed / (AirSpeed + Vcomponent);
        double Vratio = 1 - Hratio;

        // -------------------------------------------------------------



        double[] xRes = new double[3];
        double[] yRes = new double[3];
        double[] zRes = new double[3];

        // xRes[0] = 

        return Acceleration;
    }

    public void Move(bool[] moves, double[] moveParams)
    {
        for (int i = 0; i < moves.Length; i++)
        {
            switch (i)
            {
                case 0: if (moves[i]) Thrust(moveParams[0]); break;
                case 1: if (moves[i]) Thrust(-moveParams[0]); break;
                case 2: if (moves[i]) YawCorrection(moveParams[1]); break;
                case 3: if (moves[i]) YawCorrection(-moveParams[1]); break;
                case 4: if (moves[i]) PitchCorrection(moveParams[2]); break;
                case 5: if (moves[i]) PitchCorrection(-moveParams[2]); break;
                case 6: if (moves[i]) Angle.RollAdd(moveParams[3]); break;
                case 7: if (moves[i]) Angle.RollAdd(-moveParams[3]); break;

                default: break;
            }
        }
    }

    public void PitchCorrection(double value)
    {
        if (Angle.roll == 0)
        {
            Angle.PitchAdd(value);
            return;
        }

        if (Angle.roll < 90)
        {
            double Vcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.PitchAdd(Vcomponent);
            Angle.YawAdd(-(value - Vcomponent));
            return;
        }

        if (Angle.roll == 90)
        {
            Angle.YawAdd(-value);
            return;
        }

        if (Angle.roll < 180)
        {
            double Vcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.PitchAdd(Vcomponent);
            Angle.YawAdd(-(value + Vcomponent));
            return;
        }

        if (Angle.roll == 180)
        {
            Angle.PitchAdd(-value);
            return;
        }

        if (Angle.roll < 270)
        {
            double Vcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.PitchAdd(Vcomponent);
            Angle.YawAdd((value + Vcomponent));
            return;
        }

        if (Angle.roll == 270)
        {
            Angle.YawAdd(value);
            return;
        }

        if (Angle.roll < 360)
        {
            double Vcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.PitchAdd(Vcomponent);
            Angle.YawAdd((value - Vcomponent));
            return;
        }

        // double Vcomponent = AMath.DgCos(Angle.roll) * value;

        // Angle.PitchAdd(Vcomponent);
        // Angle.YawAdd(value - Vcomponent);
    }

    public void YawCorrection(double value)
    {
        if (Angle.roll == 0)
        {
            Angle.YawAdd(value);
            return;
        }

        if (Angle.roll < 90)
        {
            double Hcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.YawAdd(Hcomponent);
            Angle.PitchAdd(-(value - Hcomponent));
            return;
        }

        if (Angle.roll == 90)
        {
            Angle.PitchAdd(-value);
            return;
        }

        if (Angle.roll < 180)
        {
            double Hcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.YawAdd(Hcomponent);
            Angle.PitchAdd(-(value + Hcomponent));
            return;
        }

        if (Angle.roll == 180)
        {
            Angle.YawAdd(-value);
            return;
        }

        if (Angle.roll < 270)
        {
            double Hcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.YawAdd(Hcomponent);
            Angle.PitchAdd(value + Hcomponent);
            return;
        }

        if (Angle.roll == 270)
        {
            Angle.PitchAdd(value);
            return;
        }

        if (Angle.roll < 360)
        {
            double Hcomponent = AMath.DgCos(Angle.roll) * value;
            Angle.YawAdd(Hcomponent);
            Angle.PitchAdd(-(value - Hcomponent));
            return;
        }
    }

    public double GetAirSpeed(Vector speed)
    {
        return Math.Sqrt((speed.X * speed.X) + (speed.Y * speed.Y) + (speed.Z * speed.Z));
    }

}