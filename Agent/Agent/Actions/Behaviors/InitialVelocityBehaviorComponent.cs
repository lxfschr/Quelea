using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class InitialVelocityBehaviorComponent : AbstractBehaviorComponent
  {
    private Vector3d initialVelocity;
    /// <summary>
    /// Initializes a new instance of the InitialVelocityBehaviorComponent class.
    /// </summary>
    public InitialVelocityBehaviorComponent()
      : base("Initial Velocity", "InitialVel",
          "Applys a one-time force to the Agent when it is first emitted.",
          RS.behaviorsSubCategoryName, RS.icon_InitialVelocity, "{e8da8a7b-9d58-4583-ab88-b4c9c8bd7fca}")
    {
      initialVelocity = new Vector3d();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddVectorParameter("Initial Direction", "V", "The direction to travel in initially.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref initialVelocity)) return false;
      return true;
    }

    protected override bool Run()
    {
      if (!agent.InitialVelocitySet)
      {
        agent.Velocity = initialVelocity;
        agent.InitialVelocitySet = true;
        return true;
      }
      return false;
    }
  }
}