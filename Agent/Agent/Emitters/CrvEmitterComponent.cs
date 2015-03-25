using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
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
      : base(RS.crvEmitName, RS.crvEmitComponentNickName,
          RS.crvEmitDescription, RS.icon_crvEmitter, RS.crvEmitGUID)
    {
      crv = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager, int particlesName)
    {
      base.RegisterInputParams(pManager, pManager.AddGenericParameter("Particles","P", RS.agentDescription, 
        GH_ParamAccess.list));
      pManager.AddCurveParameter(RS.curveName, RS.curveNickName, RS.crvForEmitDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref crv)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEmitterType emitter = new CrvEmitterType(crv, continuousFlow, creationRate, numAgents);
      da.SetData(nextOutputIndex++, emitter);
    }
  }
}
