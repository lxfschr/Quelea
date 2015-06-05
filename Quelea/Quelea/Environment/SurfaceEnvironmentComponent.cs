using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SurfaceEnvironmentComponent : AbstractEnvironmentComponent
  {
    private Surface srf;
    private bool wrap;
    /// <summary>
    /// Initializes a new instance of the WorldBoxEnvironmentComponent class.
    /// </summary>
    public SurfaceEnvironmentComponent()
      : base(RS.surfaceEnvironmentName, RS.surfaceEnvironmentComponentNickname,
          RS.surfaceEnvironmentDescription, RS.icon_srfEnvironment, RS.surfaceEnvironmentGuid)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddSurfaceParameter(RS.surfaceName, RS.surfaceNickname, RS.surfaceForEnvironmentDescription, GH_ParamAccess.item);
      pManager.AddBooleanParameter(RS.wrapName, RS.wrapNickname, RS.wrapDescription, GH_ParamAccess.item, RS.wrapDefault);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      base.RegisterOutputParams(pManager);
      pManager.AddSurfaceParameter("2D Reference Surface", "RS", "The surface mapped to a 2D plane using the input surface's u and v dimensions.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref srf)) return false;
      if (!da.GetData(nextInputIndex++, ref wrap)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      SurfaceEnvironmentType environment = new SurfaceEnvironmentType(srf, wrap);
      da.SetData(nextOutputIndex++, environment);
      da.SetData(nextOutputIndex++, environment.RefEnvironment);
    }
  }
}