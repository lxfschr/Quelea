using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
 public class AgentType2 : ParticleType, IAgent
  {
   public AgentType2()
      : this(Point3d.Origin, Vector3d.Zero, Vector3d.Zero, RS.lifespanDefault, 
             RS.massDefault, RS.bodySizeDefault, RS.historyLenDefault, 
             RS.maxSpeedDefault, RS.maxForceDefault, RS.visionRadiusDefault, 
             RS.visionAngleDefault)
    {
    }

    public AgentType2(Point3d position, Vector3d velocity, Vector3d acceleration, 
                      int lifespan, double mass, double bodySize,
                      int historyLength, double maxSpeed, double maxForce, 
                      double visionRadius, double visionAngle)
      : base(position, velocity, acceleration, lifespan, mass, bodySize, historyLength)
    {
      MaxSpeed = maxSpeed;
      MaxForce = maxForce;
      VisionRadius = visionRadius;
      VisionAngle = visionAngle;
    }

    public AgentType2(AgentType2 a)
      : this(a.Position, a.Velocity, a.Acceleration, a.Lifespan, a.Mass, a.BodySize, a.HistoryLength,
             a.MaxSpeed, a.MaxForce, a.VisionRadius, a.VisionAngle)
    {
    }
   public double MaxSpeed { get; set; }
   public double MaxForce { get; set; }
   public double VisionRadius { get; set; }
   public double VisionAngle { get; set; }
  }
}
