using System;
using Agent.Util;
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
          RS.viewForceDescription, RS.boidForcesSubCategoryName, 
          RS.icon_viewForce, RS.viewForceGuid)
    {
    }

    protected override Vector3d CalcForce()
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
        steer = Vector3d.Subtract(sum, velocity);
        steer = Vector.Limit(steer, agent.MaxForce);
      }
      //Seek the average location of our neighbors.
      return steer;
    }
  }
}