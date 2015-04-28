using System;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class ViewForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    public ViewForceComponent()
      : base(RS.viewForceName, RS.viewForceNickname,
          RS.viewForceDescription, RS.icon_viewForce, RS.viewForceGuid)
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d desired = new Vector3d();
      int count = 0;
      double angle = 0;
      Point3d position = agent.Position;
      Vector3d velocity = agent.Velocity;
      Plane pl = new Plane(position, velocity, Vector3d.ZAxis);
      foreach (IQuelea neighbor in neighbors)
      {
        Point3d neighborPosition2D = agent.Environment.ClosestRefPoint(neighbor.Position);
        Vector3d diff = Util.Vector.Vector2Point(position, neighborPosition2D);
        angle = Vector3d.VectorAngle(velocity, diff, pl);
        //angle = Util.Vector.RadToDeg(angle);
        if (angle > Math.PI) angle = angle - RS.TWO_PI;
        desired = desired + (Vector3d)neighborPosition2D;
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        //We desire to go in that direction at maximum speed.
        desired = desired / count;
        desired = Util.Agent.Seek(agent, new Point3d(desired));
        Plane nrml = new Plane(position, velocity);
        if (angle >= 0) desired.Rotate(Math.PI / 2, nrml.YAxis);
        else desired.Rotate(-Math.PI / 2, nrml.YAxis);
      }
      //Seek the average location of our neighbors.
      return desired;
    }
  }
}