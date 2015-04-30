using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SetVelocityBehaviorComponent : AbstractParticleBehaviorComponent
  {
    private Vector3d desiredVelocity;
    public SetVelocityBehaviorComponent()
      : base("Set Velocity", "Velocity",
          "Sets the particle's velocity to the desired velocity.",
          RS.icon_setVelocityBehavior, "54762f40-4d9d-4275-9ab5-0e8880b647b4")
    {
      
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddVectorParameter("Desired Velocity", "V", "The direction you would like the particle to travel in next.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref desiredVelocity)) return false;
      return true;
    }

    protected override bool Run()
    {
      particle.Velocity = desiredVelocity;
      return true;
    }
  }
}