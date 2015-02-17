using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SeekForceComponent : AbstractAttractionForceComponent
  {
    public SeekForceComponent()
      : base("Seek Force", "Seek",
          "Applies a force to steer the Agent towards the point.",
          RS.pluginCategoryName, RS.forcesSubCategoryName, null, "{c0613c95-7c90-4328-af8c-fcdafe059da9}")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter("Arrival Radius", "AR", "The radius within which Agents will start to slow down to eventually stop at the target point. Set this to 0 if you do not want the Agent to stop at the target point.",
        GH_ParamAccess.item, 0);

    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddVectorParameter("Seek Force", RS.forceNickName, "", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess da)
    {
      base.SolveInstance(da);
      double arrivalRadius = 0;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(4, ref arrivalRadius)) return;


      Vector3d force = Run(arrivalRadius);

      // Finally assign the output parameter.
      da.SetData(0, force);
    }

    protected Vector3d Run(double arrivalRadius)
    {
      Vector3d force = CalcForce(arrivalRadius);
      force = Vector3d.Multiply(force, weightMultiplier);
      agent.ApplyForce(force);
      return force;
    }

    private Vector3d CalcForce(double arrivalRadius)
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
        double m = Util.Number.Map(d, 0, arrivalRadius, 0, agent.MaxSpeed);
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
      desired = Util.Vector.Limit(desired, agent.MaxForce);
      return desired;
    }
  }
}
