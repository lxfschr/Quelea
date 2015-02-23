using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ApplyCustomForceComponent : AbstractForceComponent
  {
    private Vector3d vec;
    public ApplyCustomForceComponent()
      : base("Apply Custom Force", "ApplyForce",
          "Applied a user specified force vector to the Agent.",
          RS.forcesSubCategoryName, RS.icon_applyCustomForce, "{a6333370-2246-4fac-afb9-b858a809a414}")
    {
      vec = Vector3d.Zero;
    }

    protected override void RegisterInputParams3(GH_InputParamManager pManager)
    {
      pManager.AddVectorParameter("Force Vector", "V", "The vector to be applied to the Agent position.", GH_ParamAccess.item);
    }

    protected override bool GetInputs3(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref vec)) return false;
      return true;
    }

    protected override Vector3d CalcForce()
    {
      return vec;
    }

    protected override void RegisterOutputParams2(GH_OutputParamManager pManager)
    {
    }
  }
}
