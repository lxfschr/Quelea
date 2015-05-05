using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class CrvEmitterComponent : AbstractEmitterComponent
  {
    private Curve crv;
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public CrvEmitterComponent()
      : base(RS.curveEmitterName, RS.curveEmitterComponentNickname,
          RS.curveEmitterDescription, RS.icon_crvEmitter, RS.curveEmitterGuid)
    {
      crv = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddCurveParameter(RS.curveName, RS.curveNickname, RS.curveForEmitterDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref crv)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEmitterType emitter = new CrvEmitterType(crv, continuousFlow, creationRate, numAgents, velocityMin, velocityMax);
      da.SetData(nextOutputIndex++, emitter);
    }
  }
}
