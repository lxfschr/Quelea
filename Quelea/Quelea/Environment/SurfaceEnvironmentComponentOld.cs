using System.Runtime.CompilerServices;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SurfaceEnvironmentComponentOld : AbstractEnvironmentComponent
  {
    private Surface srf;
    /// <summary>
    /// Initializes a new instance of the WorldBoxEnvironmentComponent class.
    /// </summary>
    public SurfaceEnvironmentComponentOld()
      : base(RS.surfaceEnvironmentName, RS.surfaceEnvironmentComponentNickname,
          RS.surfaceEnvironmentDescription, RS.icon_srfEnvironment, RS.surfaceEnvironmentOldGuid)
    {
      Point3d pt1 = Point3d.Origin;
      Point3d pt2 = new Point3d(RS.boxBoundsDefault, 0, 0);
      Point3d pt3 = new Point3d(0, RS.boxBoundsDefault, 0);
      srf = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddSurfaceParameter(RS.surfaceName, RS.surfaceNickname, RS.surfaceForEnvironmentDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref srf)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEnvironmentType environment = new SurfaceEnvironmentType(srf, false);
      da.SetData(nextOutputIndex++, environment);
    }

    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.hidden; }
    }
  }
}