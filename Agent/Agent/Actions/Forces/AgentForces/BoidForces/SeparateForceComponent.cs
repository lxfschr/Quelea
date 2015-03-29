using Agent.Util;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SeparateForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    public SeparateForceComponent()
      : base(RS.separateForceName, RS.separateForceNickname,
          RS.separateForceDescription, RS.flockingForcesSubcategoryName, 
          RS.icon_separateForce, RS.separateForceGuid)
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d sum = new Vector3d();
      Vector3d diff;
      int count = 0;

      foreach (AgentType other in neighbors)
      {
        double d = agent.RefPosition.DistanceTo(other.RefPosition);
        if (!(d > 0)) continue;
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