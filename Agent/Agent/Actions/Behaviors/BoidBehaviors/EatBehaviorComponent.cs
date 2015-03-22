using RS = Agent.Properties.Resources;

namespace Agent
{
  public class EatBehaviorComponent : AbstractBoidBehaviorComponent
  {
    /// <summary>
    /// Initializes a new instance of the BounceContainBehaviorComponent class.
    /// </summary>
    public EatBehaviorComponent()
      : base("Eat Behavior", "Eat",
          "Kills Agents that are within its neighborhood. Try setting the neighborhood radius to the Predator's Body Size and the angle to be low, mimicing a mouth on the front of the Predator.",
          RS.behaviorsSubCategoryName, RS.icon_EatBehavior, "1453af23-ec0e-42d9-b108-d74b00ad4594")
    {
    }

    protected override bool Run()
    {
      bool ate = false;
      foreach (AgentType neighbor in neighbors)
      {
        neighbor.Die();
        ate = true;
      }
      return ate;
    }
  }
}