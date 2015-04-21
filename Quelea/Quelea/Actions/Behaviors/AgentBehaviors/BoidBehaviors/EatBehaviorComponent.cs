using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class EatBehaviorComponent : AbstractBoidBehaviorComponent
  {
    /// <summary>
    /// Initializes a new instance of the BounceContainBehaviorComponent class.
    /// </summary>
    public EatBehaviorComponent()
      : base("Eat Behavior", "Eat",
          "Kills particles that are within its neighborhood. Try setting the neighborhood radius to the Predator's Body Size and the angle to be low, mimicing a mouth on the front of the Predator.",
          RS.icon_EatBehavior, "1453af23-ec0e-42d9-b108-d74b00ad4594")
    {
    }

    protected override bool Run()
    {
      bool ate = false;
      foreach (IParticle neighbor in neighbors)
      {
        neighbor.Die();
        ate = true;
      }
      return ate;
    }
  }
}