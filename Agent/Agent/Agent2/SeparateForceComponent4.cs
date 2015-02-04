using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent.Agent2
{
  public class SeparateForceComponent4 : BoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the CoheseForceComponent class.
    /// </summary>
    public SeparateForceComponent4()
      : base("Separate Force", "Separate4",
          "Separation Force",
          "Agent", "Agent2")
    {
      this.visionRadiusMultiplier = 1.0 / 3.0;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Separation Force", "F", "Separation Force", GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override Vector3d calcForce(AgentType agent, List<AgentType> neighbors)
    {
      Vector3d sum = new Vector3d();
      Vector3d diff;
      int count = 0;

      foreach (AgentType other in neighbors)
      {
        double d = agent.RefPosition.DistanceTo(other.RefPosition);
        if (d > 0)
        {
          //double d = Vector3d.Subtract(agent.RefPosition, other.RefPosition).Length;
          //if we are not comparing the seeker to iteself and it is at least
          //desired separation away:
          diff = Point3d.Subtract(agent.RefPosition, other.RefPosition);
          diff.Unitize();

          //Weight the magnitude by distance to other
          diff = Vector3d.Divide(diff, d);

          sum = Vector3d.Add(sum, diff);

          //For an average, we need to keep track of how many boids
          //are in our vision.
          count++;
        }
      }

      if (count > 0)
      {
        sum = Vector3d.Divide(sum, count);
        sum.Unitize();
        sum = Vector3d.Multiply(sum, agent.MaxSpeed);
        sum = Vector3d.Subtract(sum, agent.Velocity);
        sum = Util.Vector.limit(sum, agent.MaxForce);
      }
      //Seek the average position of our neighbors.
      return sum;
    }

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override System.Drawing.Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return Resources.IconForThisComponent;
        return Properties.Resources.icon_separateForce;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{1ba32976-9427-425e-aa14-fe86ee64a50a}"); }
    }
  }
}