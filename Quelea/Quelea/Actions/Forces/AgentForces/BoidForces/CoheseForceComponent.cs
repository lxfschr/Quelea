using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class CoheseForceComponent : AbstractBoidForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the CoheseForceComponent class.
    /// </summary>
    public CoheseForceComponent()
      : base(RS.coheseForceName, RS.coheseForceNickname, RS.coheseForceDescription, 
             RS.icon_coheseForce, RS.coheseForceGuid)
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d desired = new Vector3d();
      int count = 0;

      foreach (IQuelea neighbor in neighbors)
      {
        //Adding up all the others' location
        desired = Vector3d.Add(desired, new Vector3d(neighbor.RefPosition));
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        //We desire to go in that direction at maximum speed.
        desired = Vector3d.Divide(desired, count);
        desired = Util.Agent.Seek(agent, desired);
      }
      //Seek the average location of our neighbors.
      return desired;
    }
  }
}