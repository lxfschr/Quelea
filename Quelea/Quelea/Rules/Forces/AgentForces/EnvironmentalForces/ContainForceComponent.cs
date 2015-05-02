using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class ContainForceComponent : AbstractEnvironmentalForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ContainForceComponent class.
    /// </summary>
    public ContainForceComponent()
      : base(RS.containForceName, RS.containForceNickname, RS.containForceDescription,
             RS.icon_containForce, RS.containForceGuid)
    {
    }

    protected override Vector3d CalculateDesiredVelocity()
    {
      Vector3d desired = environment.AvoidEdges(agent, agent.VisionRadius*visionRadiusMultiplier);
      desired.Unitize();
      desired = desired * agent.MaxSpeed;
      return desired;
    }
  }
}