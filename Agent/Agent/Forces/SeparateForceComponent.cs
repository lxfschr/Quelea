using System;
using System.Collections.Generic;
using Agent.Util;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SeparateForceComponent : BoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the CoheseForceComponent class.
    /// </summary>
    public SeparateForceComponent()
      : base(RS.separateForceName, RS.separateForceNickName,
          RS.separateForceDescription,
          RS.pluginCategoryName, RS.boidForcesSubCategoryName)
    {
      visionRadiusMultiplier = 1.0 / 3.0;
      icon = RS.icon_separateForce;
      componentGuid = new Guid(RS.separateForceGUID);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(RS.separateForceName, RS.forceNickName, 
                                   RS.separateForceDescription, 
                                   GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override Vector3d CalcForce(AgentType agent, List<AgentType> neighbors)
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
        sum = Vector.Limit(sum, agent.MaxForce);
      }
      //Seek the average position of our neighbors.
      return sum;
    }
  }
}