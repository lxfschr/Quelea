
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SeekForceComponent : AbstractSeekForceComponent
  {
    public SeekForceComponent()
      : base("Seek Force", "Seek",
          "Applies a force to steer the Agent towards the point.",
          RS.icon_seekForce, "c0613c95-7c90-4328-af8c-fcdafe059da9")
    {
    }

    protected override Vector3d CalculateDesiredVelocity()
    {
      Vector3d desired = Util.Agent.Seek(agent, targetPt);
      return desired;
    }
  }
}
