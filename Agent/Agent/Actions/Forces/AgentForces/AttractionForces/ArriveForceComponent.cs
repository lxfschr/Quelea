using Agent.Util;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ArriveForceComponent : AbstractSeekForceComponent
  {
    private double arrivalRadius;
    public ArriveForceComponent()
      : base("Arrive Force", "Arrive",
          "Applies a force to steer the Agent towards a target point and slow down to a stop is it approaches the target point.",
          RS.forcesSubCategoryName, RS.icon_arriveForce, "052be3a2-59a3-419f-a9d4-6b31ff991b26")
    {
      arrivalRadius = RS.visionRadiusDefault;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter("Arrival Radius", "AR", "The radius within which agents will start to slow down to eventually stop at the target point. Set this to 0 if you do not want the Agent to stop at the target point.",
        GH_ParamAccess.item, RS.visionRadiusDefault);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref arrivalRadius)) return false;

      return true;
    }

    protected override Vector3d CalcForce()
    {
      Vector3d desired = Vector3d.Subtract((Vector3d)targetPt, new Vector3d(agent.RefPosition));
      double d = desired.Length;
      desired.Unitize();
      // The agent desires to move towards the target at maximum speed.
      // Instead of teleporting to the target, the agent will move incrementally.
      if (d < arrivalRadius)
      {
        double m = Number.Map(d, 0, arrivalRadius, 0, agent.MaxSpeed);
        desired = Vector3d.Multiply(desired, m);
      }
      else
      {
        desired = Vector3d.Multiply(desired, agent.MaxSpeed);
      }

      // The actual force that is applied to the agent is the difference 
      // between its current heading and the desired heading.
      desired /*steer*/ = Vector3d.Subtract(desired, agent.Velocity);
      // Optimumization so we don't need to create a new Vector3d called steer

      // Steering ability can be controlled by limiting the magnitude of the steering force.
      desired = Vector.Limit(desired, agent.MaxForce);
      return desired;
    }
  }
}
