using Agent.Util;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SeekForceComponent : AbstractSeekForceComponent
  {
    public SeekForceComponent()
      : base("Seek Force", "Seek",
          "Applies a force to steer the Agent towards the point.",
          RS.forcesSubCategoryName, RS.icon_seekForce, "c0613c95-7c90-4328-af8c-fcdafe059da9")
    {
    }

    protected override Vector3d CalcForce()
    {
      Vector3d desired = Vector3d.Subtract((Vector3d)targetPt, new Vector3d(agent.RefPosition));
      desired.Unitize();
      // The agent desires to move towards the target at maximum speed.
      // Instead of teleporting to the target, the agent will move incrementally.
      desired = Vector3d.Multiply(desired, agent.MaxSpeed);

      // The actual force that is applied to the agent is the difference 
      // between its current heading and the desired heading.
      desired /*steer*/ = Vector3d.Subtract(desired, agent.Velocity);
      // Optimumization so we don't need to create a new Vector3d called steer

      // Steering ability can be controlled by limiting the magnitude of the steering force.
      desired = Vector.Limit(desired, agent.MaxForce);
      return desired;
    }
  }
}
