using System;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
 public class VehicleType : AgentType, IVehicle
 {
   private double wheelDiff, wheelAvg;
    public VehicleType(IVehicle v)
     : this(v, v.Orientation, v.WheelRadius)
    {
    }

    public VehicleType(IVehicle v, Point3d emittionPt, Point3d refEmittionPt)
      : base(v, emittionPt, refEmittionPt)
    {
      Orientation = v.Orientation;
      WheelRadius = v.WheelRadius;
      double halfBodySize = BodySize / 2;
      const double halfPi = Math.PI / 2;

      Vector3d wheelLeftVec = Velocity;
      Vector3d wheelRightVec = Velocity;
      wheelLeftVec.Rotate(halfPi, Orientation.ZAxis);
      wheelRightVec.Rotate(-halfPi, Orientation.ZAxis);
      wheelLeftVec.Unitize();
      wheelRightVec.Unitize();
      wheelLeftVec = Vector3d.Multiply(wheelLeftVec, halfBodySize);
      wheelRightVec = Vector3d.Multiply(wheelRightVec, halfBodySize);
      Point3d wheelLeftPos = Position;
      Point3d wheelRightPos = Position;
      wheelLeftPos.Transform(Transform.Translation(wheelLeftVec));
      wheelRightPos.Transform(Transform.Translation(wheelRightVec));
      WheelLeft = new Wheel(wheelLeftPos, WheelRadius, 0);
      WheelRight = new Wheel(wheelRightPos, WheelRadius, 0);
    }

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
     //WheelLeft.Run();
     //WheelRight.Run();
     wheelDiff = WheelLeft.RadialSpeed - WheelRight.RadialSpeed;
     wheelAvg = (WheelLeft.RadialSpeed + WheelRight.RadialSpeed) / 2;
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
      : this(w.Position, w.Radius, w.AngularSpeed)
    {
    }

    public Wheel(Point3d position, double radius, double angularSpeed)
    {
      Position = position;
      Radius = radius;
      AngularSpeed = angularSpeed;
      Angle = 0;
      RadialSpeed = AngularSpeed*Radius;
    }

    public Point3d Position { get; set; }
    public double AngularSpeed { get; set; }
    public double Angle { get; set; }
    public double Radius { get; set; }
    public double RadialSpeed { get; set; }
    public void SetSpeed(double angularSpeed)
    {
      AngularSpeed = angularSpeed;
      RadialSpeed = AngularSpeed*Radius;
    }

    public void SetSpeedChange(double increment)
    {
      AngularSpeed = AngularSpeed + increment;
      RadialSpeed = AngularSpeed*Radius;
    }

    public void Run()
    {
      Angle += AngularSpeed;
      if (Angle > Math.PI*2) Angle -= Math.PI*2;
    }
  }
}
