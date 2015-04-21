using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class PtEmitterComponent : AbstractEmitterComponent
  {
    private Point3d pt;
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public PtEmitterComponent()
      : base(RS.pointEmitterName, RS.pointEmitterComponentNickname,
          RS.pointEmitterDescription, RS.icon_ptEmitter, RS.pointEmitterGuid)
    {
      pt = Point3d.Origin;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddPointParameter(RS.pointName, RS.pointNickname, RS.pointForEmitterDescription, GH_ParamAccess.item, Point3d.Origin);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref pt)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEmitterType emitterPt = new PtEmitterType(pt, continuousFlow, creationRate, numAgents);
      da.SetData(nextOutputIndex++, emitterPt);
    }
  }
}
