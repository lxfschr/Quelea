using Quelea.Util;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SeparateForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    public SeparateForceComponent()
      : base(RS.separateForceName, RS.separateForceNickname,
          RS.separateForceDescription, RS.icon_separateForce, RS.separateForceGuid)
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d desired = new Vector3d();
      int count = 0;

      foreach (IQuelea neighbor in neighbors)
      {
        double d = agent.RefPosition.DistanceTo(neighbor.RefPosition);
        if (!(d > 0)) continue;
        //double d = Vector3d.Subtract(agent.RefPosition, other.RefPosition).Length;
        //if we are not comparing the seeker to iteself and it is at least
        //desired separation away:
        Vector3d diff = Point3d.Subtract(agent.RefPosition, neighbor.RefPosition);
        diff.Unitize();

        //Weight the magnitude by distance to other
        diff = Vector3d.Divide(diff, d);

        desired = Vector3d.Add(desired, diff);

        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        desired = Vector3d.Divide(desired, count);
        desired.Unitize();
        desired = Vector3d.Multiply(desired, agent.MaxSpeed);
      }
      //Seek the average position of our neighbors.
      return desired;
    }
  }
}