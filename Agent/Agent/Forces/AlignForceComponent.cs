using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AlignForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the AlignForceComponent class.
    /// </summary>
    public AlignForceComponent()
      : base(RS.alignForceName, RS.alignForceNickName,
          RS.alignForceDescription,
          RS.pluginCategoryName, RS.boidForcesSubCategoryName)
    {
      visionRadiusMultiplier = RS.visionRadiusMultiplierDefault;
      componentGuid = new Guid(RS.alignForceGUID);
      icon = RS.icon_alignForce;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(RS.alignForceName, RS.forceNickName, 
                                   RS.alignForceDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override Vector3d CalcForce(AgentType agent, List<AgentType> neighbors)
    {
      Vector3d sum = new Vector3d();
      int count = 0;
      Vector3d steer = new Vector3d();

      foreach (AgentType other in neighbors)
      {
        //Add up all the velocities and divide by the total to calculate
        //the average velocity.
        sum = Vector3d.Add(sum, new Vector3d(other.Velocity));
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        sum = Vector3d.Divide(sum, count);
        sum.Unitize();
        sum = Vector3d.Multiply(sum, agent.MaxSpeed);
        steer = Vector3d.Subtract(sum, agent.Velocity);
        steer = Util.Vector.Limit(steer, agent.MaxForce);
      }
      //Seek the average location of our neighbors.
      return steer;
    }
  }
}