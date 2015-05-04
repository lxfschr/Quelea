using System;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class ParticleType : IParticle
  {
    public ParticleType()
      : this(new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault), 
             new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault), Vector3d.ZAxis,
             Vector3d.Zero, RS.lifespanDefault, RS.massDefault, RS.bodySizeDefault, RS.historyLenDefault)
    {
    }

    // For Construct Particle Settings
    public ParticleType(Vector3d velocityMin, Vector3d velocityMax, Vector3d up, Vector3d acceleration,
                        int lifespan, double mass, double bodySize,
                        int historyLength)
    {
      VelocityMin = velocityMin;
      VelocityMax = velocityMax;
      Up = up;
      Acceleration = acceleration;
      Lifespan = lifespan;
      Mass = mass;
      BodySize = bodySize;
      HistoryLength = historyLength;
    }

    // Copy Constructor
    public ParticleType(IParticle p)
      : this(p.VelocityMin, p.VelocityMax, p.Up, p.Acceleration, p.Lifespan, p.Mass, p.BodySize, p.HistoryLength)
    {
      InitialVelocitySet = p.InitialVelocitySet;
      Position = p.Position;
      Velocity = p.Velocity;
      PreviousAcceleration3D = p.PreviousAcceleration3D;
      PreviousAcceleration = p.PreviousAcceleration;
      Position3D = p.Position3D;
      Velocity3D = p.Velocity3D;
      Acceleration3D = p.Acceleration3D;
      Position3DHistory = p.Position3DHistory;
    }

    public ParticleType(IParticle p, Point3d emittionPt, AbstractEnvironmentType environment)
      : this(p.VelocityMin, p.VelocityMax, p.Up, p.Acceleration, p.Lifespan,
             p.Mass, p.BodySize, p.HistoryLength)
    {
      Environment = environment;
      Position3D = emittionPt;
      Position = Environment.MapTo2D(emittionPt);
      Velocity3D = Util.Random.RandomVector(VelocityMin, VelocityMax);
      Velocity = MapTo2D(Velocity3D);
      Acceleration = MapTo2D(Acceleration3D);
      Orientation = SetOrientation();
      Position3DHistory = new CircularArray<Point3d>(HistoryLength);
      Position3DHistory.Add(Position3D);
    }

    private Plane SetOrientation()
    {
      Plane orientation;
      if (Velocity.IsZero)
      {
        orientation = new Plane(Position, Up);
      }
      else
      {
        Vector3d y = Vector3d.CrossProduct(Velocity, Up);
        orientation = new Plane(Position, Velocity, y);
      }
      Up = orientation.ZAxis;
      return orientation;
    }

    private Vector3d MapTo2D(Vector3d vector3D)
    {
      Point3d pt2D = Position3D;
      pt2D.Transform(Transform.Translation(vector3D));
      pt2D = Environment.MapTo2D(pt2D);
      //return Util.Vector.Vector2Point(Position, pt2D);
      Vector3d vector2D = Util.Vector.Vector2Point(Position, pt2D);
      vector2D.Unitize();
      vector2D *= vector3D.Length;
      return vector2D;
    }

    public Plane Orientation { get; set; }
    public Vector3d Up { get; set; }

    public Vector3d Forward
    {
      get { return Orientation.ZAxis; }
    }

    public Vector3d Side
    {
      get { return Orientation.YAxis; }
    }

    public double Speed
    {
      get { return Velocity.Length; }
    }

    public double SquareSpeed
    {
      get { return Velocity.SquareLength; }
    }

    public Point3d Position { get; set; }
    public Vector3d Velocity { get; set; }
    public Vector3d VelocityMin { get; set; }
    public Vector3d VelocityMax { get; set; }
    public Vector3d Acceleration { get; set; }
    public Point3d Position3D { get; set; }
    public Vector3d Velocity3D { get; set; }
    public Vector3d Acceleration3D { get; set; }
    public Vector3d PreviousAcceleration3D { get; set; }
    public Vector3d PreviousAcceleration { get; set; }
    public int Lifespan { get; set; }
    public double Mass { get; set; }
    public double BodySize { get; set; }
    public int HistoryLength { get; set; }
    public bool InitialVelocitySet { get; set; }
    public CircularArray<Point3d> Position3DHistory { get; private set; }
    public AbstractEnvironmentType Environment { get; set; }

    public Point3d GetPoint3D()
    {
      return Position;
    }

    virtual public void Run()
    {
      Velocity = Vector3d.Add(Velocity, Acceleration);
      Velocity3D = MapTo3D(Velocity);
      Acceleration3D = MapTo3D(Acceleration);
      Point3d position = Position;
      position.Transform(Transform.Translation(Velocity));
      position = Environment.ClosestPointOnRef(position);
      Position = position;
      Point3d position3D = Position3D;
      position3D.Transform(Transform.Translation(Velocity3D));
      Position3D = position3D;
      Position3DHistory.Add(Position3D);

      PreviousAcceleration3D = Acceleration3D;
      PreviousAcceleration = Acceleration;

      UpdateOrientation();

      Acceleration = Vector3d.Zero;
      Lifespan -= 1;
    }

    private void UpdateOrientation()
    {
      Plane orientation = Orientation;
      orientation.Origin = Position3D;
      if (Util.Number.ApproximatelyEqual(Velocity.SquareLength, 0, Constants.AbsoluteTolerance))
      {
        //orientation.Rotate(-RS.HALF_PI / 2, orientation.ZAxis);
        Orientation = orientation;
        return;
      }
      
      
      double angle = Vector3d.VectorAngle(Velocity, orientation.XAxis, orientation);

      orientation.Rotate(angle, orientation.ZAxis);
      if (!Util.Number.ApproximatelyEqual(Vector3d.VectorAngle(Velocity, orientation.XAxis, orientation), 0, Constants.AbsoluteTolerance))
      {
        orientation.Rotate(-2 * angle, orientation.ZAxis);
      }
      Plane pln = new Plane(orientation.Origin, orientation.XAxis, -orientation.ZAxis);
      angle = Vector3d.VectorAngle(Velocity, orientation.XAxis, pln);
      orientation.Rotate(angle, orientation.YAxis);
      if (!Util.Number.ApproximatelyEqual(Vector3d.VectorAngle(Velocity, orientation.XAxis, pln), 0, Constants.AbsoluteTolerance))
      {
        orientation.Rotate(-2 * angle, orientation.YAxis);
      }
      //if (!Util.Number.ApproximatelyEqual(Vector3d.VectorAngle(Velocity, orientation.XAxis, pln), 0, Constants.AbsoluteTolerance))
      //{
      //  if (!Util.Number.ApproximatelyEqual(Vector3d.VectorAngle(Velocity, orientation.XAxis, pln), RS.TWO_PI, Constants.AbsoluteTolerance))
      //  {
      //    throw new ApplicationException("Plane not aligned.");
      //  }
      //}
      Orientation = orientation;
    }

    private Vector3d MapTo3D(Vector3d vector2D)
    {
      Point3d pt3D = Position;
      pt3D.Transform(Transform.Translation(vector2D));
      pt3D = Environment.MapTo3D(pt3D);
      return Util.Vector.Vector2Point(Position3D, pt3D);
    }

    public Vector3d ApplyForce(Vector3d force, double weightMultiplier)
    {
      force = force / Mass;
      force = force * weightMultiplier;
      Acceleration += force;
      return force;
    }

    public void Die()
    {
      Lifespan = 0;
    }

    public bool IsDead()
    {
      return (Lifespan == -1);
    }

    public override bool Equals(Object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Particle return false.
      return Equals(obj as ParticleType);
    }

    public bool Equals(ParticleType p)
    {
      // If parameter is null return false:
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return Position3D.Equals(p.Position3D) &&
             Position.Equals(p.Position) &&
             Velocity.Equals(p.Velocity) &&
             Acceleration.Equals(p.Acceleration) &&
             Lifespan.Equals(p.Lifespan) &&
             Mass.Equals(p.Mass) &&
             BodySize.Equals(p.BodySize) &&
             HistoryLength.Equals(p.HistoryLength) &&
             InitialVelocitySet.Equals(p.InitialVelocitySet);
    }

    public override int GetHashCode()
    {
      unchecked // disable overflow, for the unlikely possibility that you
      {         // are compiling with overflow-checking enabled
        int hash = 27;
        hash = (13 * hash) + Position3D.GetHashCode();
        hash = (13 * hash) + Position.GetHashCode();
        hash = (13 * hash) + Velocity.GetHashCode();
        hash = (13 * hash) + Acceleration.GetHashCode();
        hash = (13 * hash) + Lifespan.GetHashCode();
        hash = (13 * hash) + Mass.GetHashCode();
        hash = (13 * hash) + BodySize.GetHashCode();
        hash = (13 * hash) + HistoryLength.GetHashCode();
        hash = (13 * hash) + InitialVelocitySet.GetHashCode();
        return hash;
      }
    }

    /**************************** IGH_GOO members ****************************/
    
    public IGH_Goo Duplicate()
    {
      return new ParticleType(this);
    }

    public bool Write(GH_IWriter writer)
    {
      throw new NotImplementedException();
    }

    public bool Read(GH_IReader reader)
    {
      throw new NotImplementedException();
    }

    public IGH_GooProxy EmitProxy()
    {
      throw new NotImplementedException();
    }

    public bool CastFrom(object source)
    {
      throw new NotImplementedException();
    }

    public bool CastTo<T>(out T target)
    {
      throw new NotImplementedException();
    }

    public object ScriptVariable()
    {
      throw new NotImplementedException();
    }

    public bool IsValid
    {
      get
      {
        return (Mass > 0 && BodySize >= 0 && HistoryLength >= 1);
      }
    }
    public string IsValidWhyNot { get { throw new NotImplementedException(); } }
    public string TypeName { get { return RS.agentName; } }
    public string TypeDescription { get { return RS.agentDescription; } }
    /*************************************************************************/
  }
}
