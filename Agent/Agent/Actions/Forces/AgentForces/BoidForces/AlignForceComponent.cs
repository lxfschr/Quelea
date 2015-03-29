using Agent.Util;
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
      : base(RS.alignForceName, RS.alignForceNickname, RS.alignForceDescription, 
             RS.flockingForcesSubcategoryName, RS.icon_alignForce, RS.alignForceGuid)
    {
    }

    protected override Vector3d CalcForce()
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
        steer = Vector.Limit(steer, agent.MaxForce);
      }
      //Seek the average location of our neighbors.
      return steer;
    }
  }
}