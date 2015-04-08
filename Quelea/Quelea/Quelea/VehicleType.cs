using System;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
 public class VehicleType : AgentType, IVehicle
 {
   private double wheelDiff;
    public VehicleType(IVehicle v)
     : this(v, v.Orientation, v.WheelRadius)
    {
    }

    public VehicleType(IVehicle v, Point3d emittionPt, Point3d refEmittionPt)
      : base(v, emittionPt, refEmittionPt)
    {
      HalfPi = Math.PI / 2;
      Orientation = v.Orientation;
      WheelRadius = v.WheelRadius;
      WheelLeft = new Wheel(GetPartPosition(BodySize, HalfPi), WheelRadius, 0);
      WheelRight = new Wheel(GetPartPosition(BodySize, -HalfPi), WheelRadius, 0);
    }

   public Point3d GetPartPosition(double gapSize, double rotation)
   {
     Vector3d offsetVec = Velocity;
     offsetVec.Rotate(rotation, Orientation.ZAxis);
     offsetVec.Unitize();
     offsetVec = Vector3d.Multiply(offsetVec, gapSize / 2);
     Point3d partPosition = Position;
     partPosition.Transform(Transform.Translation(offsetVec));
     return partPosition;
   }

   public double HalfPi { get; private set; }

   public VehicleType(IAgent agentSettings, Plane orientation, double wheelRadius)
      : base(agentSettings)
    {
      WheelRadius = wheelRadius;
      Orientation = orientation;
    }

   public Plane Orientation { get; set; }
   public IWheel WheelLeft { get; set; }
   public IWheel WheelRight { get; set; }
   public double WheelRadius { get; set; }

   public override void Run()
   {
     WheelLeft.Run();
     WheelRight.Run();
     wheelDiff = WheelLeft.TangentialVelocity - WheelRight.TangentialVelocity;
     double angle = wheelDiff/BodySize;
     Vector3d velocity = Velocity;
     velocity.Rotate(angle, Orientation.ZAxis);
     Velocity = velocity;
     base.Run();
   }
  }

  public class Wheel : IWheel
  {
    public Wheel(IWheel w)
      : this(w.Position, w.Radius, w.AngularVelocity)
    {
    }

    public Wheel(Point3d position, double radius, double angularVelocity)
    {
      Position = position;
      Radius = radius;
      AngularVelocity = angularVelocity;
      Angle = 0;
      TangentialVelocity = Radius*AngularVelocity;
    }

    public Point3d Position { get; set; }
    public double AngularVelocity { get; set; }
    public double Angle { get; set; }
    public double Radius { get; set; }
    public double TangentialVelocity { get; set; }
    public void SetSpeed(double angularVelocity)
    {
      AngularVelocity = angularVelocity;
      TangentialVelocity = Radius*AngularVelocity;
    }

    public void SetSpeedChange(double increment)
    {
      AngularVelocity += increment;
      TangentialVelocity = AngularVelocity*Radius;
    }

    public void Run()
    {
      Angle += AngularVelocity;
      AngularVelocity = 0;
      if (Angle > Math.PI*2) Angle -= Math.PI*2;
    }
  }
}
