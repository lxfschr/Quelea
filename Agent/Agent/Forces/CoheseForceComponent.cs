using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class CoheseForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the CoheseForceComponent class.
    /// </summary>
    public CoheseForceComponent()
      : base(RS.coheseForceName, RS.coheseForceNickName,
          RS.coheseForceDescription,
          RS.pluginCategoryName, RS.boidForcesSubCategoryName)
    {
      visionRadiusMultiplier = 1.0;
      componentGuid = new Guid(RS.coheseForceGUID);
      icon = RS.icon_coheseForce;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(RS.coheseForceName, RS.forceNickName, 
                                   RS.coheseForceDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override Vector3d CalcForce(AgentType agent, List<AgentType> neighbors)
    {
      Vector3d sum = new Vector3d();
      int count = 0;

      foreach (AgentType neighbor in neighbors)
      {
        //Adding up all the others' location
        sum = Vector3d.Add(sum, new Vector3d(neighbor.RefPosition));
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        //We desire to go in that direction at maximum speed.
        sum = Vector3d.Divide(sum, count);
        sum = Util.Agent.Seek(agent, sum);
      }
      //Seek the average location of our neighbors.
      return sum;
    }
  }
}