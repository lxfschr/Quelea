using Agent.Util;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SeekForceComponent : AbstractAttractionForceComponent
  {
    private double arrivalRadius;
    public SeekForceComponent()
      : base("Seek Force", "Seek",
          "Applies a force to steer the Agent towards the point.",
          RS.forcesSubCategoryName, null, "{c0613c95-7c90-4328-af8c-fcdafe059da9}")
    {
      arrivalRadius = 0;
    }

    protected override void RegisterInputParams3(GH_InputParamManager pManager)
    {
      pManager.AddNumberParameter("Arrival Radius", "AR", "The radius within which Agents will start to slow down to eventually stop at the target point. Set this to 0 if you do not want the Agent to stop at the target point.",
        GH_ParamAccess.item, 0);
    }

    protected override bool GetInputs3(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref arrivalRadius)) return false;

      return true;
    }

    protected override void RegisterOutputParams2(GH_OutputParamManager pManager)
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d desired = Vector3d.Subtract((Vector3d)targetPt, new Vector3d(agent.RefPosition));
      double d = desired.Length;
      desired.Unitize();
      // The agent desires to move towards the target at maximum speed.
      // Instead of teleporting to the target, the agent will move incrementally.
      if (d > radius && radius > 0)
      {
        return new Vector3d();
      }
      if (d < arrivalRadius)
      {
        double m = Number.Map(d, 0, arrivalRadius, 0, agent.MaxSpeed);
        desired = Vector3d.Multiply(desired, m);
      }
      else
      {
        desired = Vector3d.Multiply(desired, agent.MaxSpeed);
      }

      //Seek the average position of our neighbors.
      desired /*steer*/ = Vector3d.Subtract(desired, agent.Velocity);
      // Optimumization so we don't need to create a new Vector3d called steer

      // Steering ability can be controlled by limiting the magnitude of the steering force.
      desired = Vector.Limit(desired, agent.MaxForce);
      return desired;
    }
  }
}
