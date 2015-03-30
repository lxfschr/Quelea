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
      : base(RS.coheseForceName, RS.coheseForceNickname, RS.coheseForceDescription, 
             RS.icon_coheseForce, RS.coheseForceGuid)
    {
    }

    protected override Vector3d CalcForce()
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