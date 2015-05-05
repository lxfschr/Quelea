using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SurfaceEmitterComponent : AbstractEmitterComponent
  {
    private Surface srf;
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public SurfaceEmitterComponent()
      : base("Surface Emitter", "SrfEmit",
          "A surface from which a quelea can be emitted.", null, "27384c1e-1a3e-4871-b5b5-d88284378b1d")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddCurveParameter(RS.surfaceName, RS.surfaceNickname, "Surface for emitter.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref srf)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEmitterType emitter = new SurfaceEmitterType(srf, continuousFlow, creationRate, numAgents, velocityMin, velocityMax);
      da.SetData(nextOutputIndex++, emitter);
    }
  }
}
