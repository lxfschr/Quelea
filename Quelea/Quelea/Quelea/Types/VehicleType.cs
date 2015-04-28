using System;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class VehicleType : AgentType, IVehicle
  {
    public enum WheelPositions
    {
      LeftRear,
      RightRear,
      LeftFront,
      RightFront
    };

    //private double wheelDiff;

    public VehicleType(IAgent agentSettings, Plane orientation, double wheelRadius)
      : base(agentSettings)
    {
      WheelRadius = wheelRadius;
      Orientation = new Plane(Position, orientation.ZAxis);
      double angle = Vector3d.VectorAngle(Velocity, orientation.XAxis);
      Orientation.Rotate(angle, Orientation.ZAxis);
    }

    public VehicleType(IVehicle v)
      : this(v, v.Orientation, v.WheelRadius)
    {
    }

    public VehicleType(IVehicle v, Point3d emittionPt, AbstractEnvironmentType environment)
      : base(v, emittionPt, environment)
    {
      Orientation = new Plane(Position, v.Orientation.ZAxis);
      UpdateOrientation();
      WheelRadius = v.WheelRadius;
      Wheels = new IWheel[2];
      Wheels[(int)WheelPositions.LeftRear] = new Wheel(GetPartPosition(BodySize, RS.HALF_PI), WheelRadius, 0);
      Wheels[(int)WheelPositions.RightRear] = new Wheel(GetPartPosition(BodySize, -RS.HALF_PI), WheelRadius, 0);
    }

    private void UpdateOrientation()
    {
      Plane orientation = Orientation;
      orientation.Origin = Position;
      double angle = Vector3d.VectorAngle(Velocity, Orientation.XAxis, Orientation);

      orientation.Rotate(angle, Orientation.ZAxis);
      if (!Util.Number.ApproximatelyEqual(Vector3d.VectorAngle(Velocity, Orientation.XAxis, Orientation), 0, Constants.AbsoluteTolerance))
      {
        orientation.Rotate(-2*angle, Orientation.ZAxis);
      }
      Orientation = orientation;
    }

    public Point3d GetPartPosition(double gapSize, double rotation)
    {
      Vector3d offsetVec = Velocity;
      offsetVec.Rotate(rotation, Orientation.ZAxis);
      offsetVec.Unitize();
      offsetVec = Vector3d.Multiply(offsetVec, gapSize/2);
      Point3d partPosition = RefPosition;
      partPosition.Transform(Transform.Translation(offsetVec));
      return partPosition;
    }

    public void SetSpeedChanges(double leftValue, double rightValue)
    {
      Wheels[(int) WheelPositions.LeftRear].SetSpeedChange(leftValue);
      Wheels[(int) WheelPositions.RightRear].SetSpeedChange(rightValue);
    }

    public Plane Orientation { get; set; }
    public IWheel[] Wheels { get; set; }
    public double WheelRadius { get; set; }

    public override void Run()
    {
      foreach (IWheel wheel in Wheels)
      {
        wheel.Run();
      }
      //wheelDiff = Wheels[(int)WheelPositions.LeftRear].TangentialVelocity -
      //            Wheels[(int)WheelPositions.RightRear].TangentialVelocity;
      //double angle = wheelDiff / BodySize;
      //Vector3d velocity = Velocity;
      //velocity.Rotate(angle, Orientation.ZAxis);
      //Velocity = velocity;
      base.Run();
      UpdateOrientation();
      Wheels[(int)WheelPositions.LeftRear].Position = GetPartPosition(BodySize, RS.HALF_PI);
      Wheels[(int)WheelPositions.RightRear].Position = GetPartPosition(BodySize, -RS.HALF_PI);
    }

    public class Wheel : IWheel, IGH_Goo
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

      public IGH_Goo Duplicate()
      {
        return new Wheel(this);
      }

      public bool IsValid
      {
        get { return (0 <= Angle && Angle <= Math.PI*2) && (Radius > 0); }
      }

      public override string ToString()
      {
        string positionStr = Util.String.ToString("Position", Position);
        string radiusStr = Util.String.ToString("Angle", Angle);
        string angularVelocitStr = Util.String.ToString("Angular Velocity", AngularVelocity);
        string angleStr = Util.String.ToString("Angle", Angle);
        string tangentialVelocityStr = Util.String.ToString("Tangential Velocity", TangentialVelocity);
        return positionStr + radiusStr + angularVelocitStr + angleStr + tangentialVelocityStr;
      }

      public string TypeDescription
      {
        get { return "Wheel"; }
      }

      public string TypeName
      {
        get { return "Wheel"; }
      }

      public bool CastFrom(object source)
      {
        throw new NotImplementedException();
      }

      public bool CastTo<T>(out T target)
      {
        throw new NotImplementedException();
      }

      public IGH_GooProxy EmitProxy()
      {
        throw new NotImplementedException();
      }

      public string IsValidWhyNot
      {
        get { throw new NotImplementedException(); }
      }

      public object ScriptVariable()
      {
        throw new NotImplementedException();
      }

      public bool Read(GH_IO.Serialization.GH_IReader reader)
      {
        throw new NotImplementedException();
      }

      public bool Write(GH_IO.Serialization.GH_IWriter writer)
      {
        throw new NotImplementedException();
      }
    }
  }
}
