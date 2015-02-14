using System;
using System.Collections.Generic;
using Agent.Util;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ViewForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    public ViewForceComponent()
      : base(RS.viewForceName, RS.viewForceComponentNickName,
          RS.viewForceDescription,
          RS.pluginCategoryName, RS.boidForcesSubCategoryName, RS.icon_viewForce, RS.viewForceGuid)
    {
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(RS.viewForceName, RS.forceNickName, 
                                   RS.viewForceDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override Vector3d CalcForce(AgentType agent, List<AgentType> neighbors)
    {
      Vector3d sum = new Vector3d();
      int count = 0;
      Vector3d steer = new Vector3d();
      double angle = 0;
      Point3d position = agent.Position;
      Vector3d velocity = agent.Velocity;
      Plane pl = new Plane(position, velocity, Vector3d.ZAxis);
      foreach (AgentType neighbor in neighbors)
      {
        Vector3d diff = Vector3d.Subtract(new Vector3d(neighbor.Position), new Vector3d(position));
        angle = Vector3d.VectorAngle(velocity, diff, pl);
        angle = Vector.RadToDeg(angle);
        if (angle > 180) angle = angle - 360;
        sum = Vector3d.Add(sum, new Vector3d(neighbor.Position));
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        //We desire to go in that direction at maximum speed.
        sum = Vector3d.Divide(sum, count);
        Plane nrml = new Plane(new Point3d(position), velocity);
        if (angle >= 0) sum.Rotate(Math.PI / 2, nrml.YAxis);
        else sum.Rotate(-Math.PI / 2, nrml.YAxis);
        //sum.PerpendicularTo(sum);
        steer = Vector3d.Subtract(sum, velocity);
        steer = Vector.Limit(steer, agent.MaxForce);
      }
      //Seek the average location of our neighbors.
      return steer;
    }
  }
}