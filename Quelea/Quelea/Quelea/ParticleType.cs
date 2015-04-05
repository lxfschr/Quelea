using System;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ParticleType : IParticle
  {
    public ParticleType()
      : this(new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault), 
             new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault), 
             Vector3d.Zero, RS.lifespanDefault, RS.massDefault, RS.bodySizeDefault, RS.historyLenDefault)
    {
    }

    public ParticleType(Vector3d velocityMin, Vector3d velocityMax, Vector3d acceleration,
                        int lifespan, double mass, double bodySize,
                        int historyLength)
    {
      HistoryLength = historyLength;
      PositionHistory = new CircularArray<Point3d>(HistoryLength);
      Position = Point3d.Origin;
      RefPosition = Position;
      Velocity = Util.Random.RandomVector(velocityMin, velocityMax);
      VelocityMin = velocityMin;
      VelocityMax = velocityMax;
      Acceleration = acceleration;
      Lifespan = lifespan;
      Mass = mass;
      BodySize = bodySize;
      InitialVelocitySet = false;
    }

    public ParticleType(IParticle p)
      : this(p.VelocityMin, p.VelocityMax, p.Acceleration, p.Lifespan, p.Mass, p.BodySize, p.HistoryLength)
    {
      InitialVelocitySet = p.InitialVelocitySet;
      Position = p.Position;
      RefPosition = p.RefPosition;
      PositionHistory.Add(Position);
      Velocity = p.Velocity;
    }

    public ParticleType(IParticle p, Point3d emittionPt, Point3d refEmittionPt)
      : this(p.VelocityMin, p.VelocityMax, p.Acceleration, p.Lifespan,
             p.Mass, p.BodySize, p.HistoryLength)
    {
      Position = emittionPt;
      RefPosition = refEmittionPt;
      PositionHistory.Add(Position);
    }

    public Point3d Position { get; set; }
    public Vector3d Velocity { get; set; }
    public Vector3d VelocityMin { get; set; }
    public Vector3d VelocityMax { get; set; }
    public Vector3d Acceleration { get; set; }
    public Point3d RefPosition { get; set; }
    public int Lifespan { get; set; }
    public double Mass { get; set; }
    public double BodySize { get; set; }
    public int HistoryLength { get; set; }
    public bool InitialVelocitySet { get; set; }
    public CircularArray<Point3d> PositionHistory { get; private set; }

    public Point3d GetPoint3D()
    {
      return RefPosition;
    }

    virtual public void Run()
    {
      Velocity = Vector3d.Add(Velocity, Acceleration);
      Point3d refPosition = RefPosition;
      refPosition.Transform(Transform.Translation(Velocity));
      RefPosition = refPosition;
      Point3d position = Position;
      position.Transform(Transform.Translation(Velocity)); //So disconnecting the environment allows the agent to continue from its current position.
      Position = position;
      //PositionHistory.Add(Position);
      Acceleration = Vector3d.Zero;
      Lifespan -= 1;
    }

    public void ApplyForce(Vector3d force)
    {
      force = Vector3d.Divide(force, Mass);
      Acceleration = Vector3d.Add(Acceleration, force);
    }

    public void Die()
    {
      Lifespan = 0;
    }

    public bool IsDead()
    {
      return (Lifespan == 0);
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
      return Position.Equals(p.Position) &&
             RefPosition.Equals(p.RefPosition) &&
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
        hash = (13 * hash) + Position.GetHashCode();
        hash = (13 * hash) + RefPosition.GetHashCode();
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
