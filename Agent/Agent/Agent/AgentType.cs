using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
 public class AgentType : ParticleType, IAgent
  {
   public AgentType()
     : this(new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault),
            new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault),
            Vector3d.Zero, RS.lifespanDefault, RS.massDefault, RS.bodySizeDefault, 
            RS.historyLenDefault, RS.maxSpeedDefault, RS.maxForceDefault, 
            RS.visionRadiusDefault, RS.visionAngleDefault)
    {
    }

    public AgentType(AgentType a)
     : this(a.VelocityMin, a.VelocityMax, a.Acceleration, a.Lifespan, a.Mass, a.BodySize, a.HistoryLength,
             a.MaxSpeed, a.MaxForce, a.VisionRadius, a.VisionAngle)
    {
    }

    public AgentType(IParticle p, double maxSpeed, double maxForce, double visionRadius, double visionAngle)
      : base(p.VelocityMin, p.VelocityMax, p.Acceleration, p.Lifespan, p.Mass, p.BodySize, p.HistoryLength)
    {
      MaxSpeed = maxSpeed;
      MaxForce = maxForce;
      VisionRadius = visionRadius;
      VisionAngle = visionAngle;
    }

    public AgentType(IAgent a, Point3d emittionPt, Point3d refEmittionPt)
      : base(a.VelocityMin, a.VelocityMax, a.Acceleration, a.Lifespan, a.Mass, a.BodySize, a.HistoryLength)
    {
      MaxSpeed = a.MaxSpeed;
      MaxForce = a.MaxForce;
      VisionRadius = a.VisionRadius;
      VisionAngle = a.VisionAngle;
      Position = emittionPt;
      RefPosition = refEmittionPt;
    }

    public AgentType(Vector3d velocityMin, Vector3d velocityMax, Vector3d acceleration,
                        int lifespan, double mass, double bodySize,
                        int historyLength, double maxSpeed, double maxForce, double visionRadius, double visionAngle)
      : base(velocityMin, velocityMax, acceleration, lifespan, mass, bodySize, historyLength)
    {
      MaxSpeed = maxSpeed;
      MaxForce = maxForce;
      VisionRadius = visionRadius;
      VisionAngle = visionAngle;
    }

   public double MaxSpeed { get; set; }
   public double MaxForce { get; set; }
   public double VisionRadius { get; set; }
   public double VisionAngle { get; set; }
  }
}
