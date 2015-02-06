using Agent.Util;
using Grasshopper.Kernel;
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
          RS.pluginCategoryName, RS.environmentForcesSubCategory, RS.icon_ptEmitter, RS.containForceGUID)
    {
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.containForceName, RS.forceNickName, RS.containForceDescription, GH_ParamAccess.item);
    }

    protected override Vector3d CalcForce(AgentType agent, AbstractEnvironmentType environment, double visionRadius)
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