using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class BounceContainBehaviorComponent : AbstractEnvironmentalBehaviorComponent
  {
    /// <summary>
    /// Initializes a new instance of the EatBehaviorComponent class.
    /// </summary>
    public BounceContainBehaviorComponent()
      : base(RS.bounceContainBehaviorName, RS.bounceContainBehaviorNickname,
             RS.bounceContainBehaviorDescription, RS.icon_bounceContainBehavior, 
             RS.bounceContainBehaviorGuid)
    {
      
    }

    protected override bool Run()
    {
      return environment.BounceContain(particle);
    }
  }
}