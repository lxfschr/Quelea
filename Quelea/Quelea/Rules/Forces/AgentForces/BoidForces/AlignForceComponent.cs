using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class AlignForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the AlignForceComponent class.
    /// </summary>
    public AlignForceComponent()
      : base(RS.alignForceName, RS.alignForceNickname, RS.alignForceDescription, 
             RS.icon_alignForce, RS.alignForceGuid)
    {
    }

    protected override Vector3d CalculateDesiredVelocity()
    {
      Vector3d desired = new Vector3d();
      int count = 0;

      foreach (IQuelea neighbor in neighbors)
      {
        //Add up all the velocities and divide by the total to calculate
        //the average velocity.
        desired = desired + neighbor.Velocity;
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        desired = desired / count;
        desired.Unitize();
        desired = desired * agent.MaxSpeed;
      }
      //Seek the average location of our neighbors.
      return desired;
    }
  }
}