using Agent.Util;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ContainForceComponent : AbstractEnvironmentalForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ContainForceComponent class.
    /// </summary>
    public ContainForceComponent()
      : base(RS.containForceName, RS.containForceNickName,
          RS.containForceDescription,
          RS.pluginCategoryName, RS.forcesSubCategoryName, RS.icon_containForce, RS.containForceGUID)
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d steer = new Vector3d();
      if (environment != null)
      {
        steer = environment.AvoidEdges(agent, visionRadius);
        if (!steer.IsZero)
        {
          steer.Unitize();
          steer = Vector3d.Multiply(steer, agent.MaxSpeed);
          steer = Vector3d.Subtract(steer, agent.Velocity);
          steer = Vector.Limit(steer, agent.MaxForce);
        }
      }
      return steer;
    }
  }
}