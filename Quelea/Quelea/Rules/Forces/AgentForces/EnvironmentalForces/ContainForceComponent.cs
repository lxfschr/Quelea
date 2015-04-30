using Quelea.Util;
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

    protected override Vector3d CalcForce()
    {
      Vector3d desired = new Vector3d();
      if (environment != null)
      {
        desired = environment.AvoidEdges(agent, visionRadius);
        if (!desired.IsZero)
        {
          desired.Unitize();
          desired = desired * agent.MaxSpeed;
        }
      }
      return desired;
    }
  }
}