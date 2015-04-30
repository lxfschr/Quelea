using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class KillContainBehaviorComponent : AbstractEnvironmentalBehaviorComponent
  {
    /// <summary>
    /// Initializes a new instance of the EatBehaviorComponent class.
    /// </summary>
    public KillContainBehaviorComponent()
      : base("Kill Contain Behavior", "KillContain",
             "Kills the particle when it leaves the environment boundaries.", RS.icon_killContainBehavior,
             "0aad35e4-5f93-4e99-8ad4-3bc2ede6d903")
    {
      
    }

    protected override bool Run()
    {
      bool behaviorApplied = false;
      if (!environment.Contains(particle.Position))
      {
        particle.Die();
        behaviorApplied = true;
      }
      return behaviorApplied;
    }
  }
}