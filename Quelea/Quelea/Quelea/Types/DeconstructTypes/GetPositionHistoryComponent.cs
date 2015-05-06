using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class GetPositionHistoryComponent : AbstractDeconstructTypeComponent
  {
    private IParticle particle;
    /// <summary>
    /// Initializes a new instance of the GetPositionHistoryComponent class.
    /// </summary>
    public GetPositionHistoryComponent()
      : base("Get Particle Position History", "GetPosHist",
             "Gets the position history of anything that inherits from Particle.", RS.icon_getPositionHistory, "76687f32-a550-493f-8262-89dcaf80825c")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.particleName + " " + RS.queleaName, RS.particleNickname + RS.queleaNickname, RS.particleDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPointParameter(RS.positionHistoryName, RS.positionHistoryName, RS.positionHistoryDescription, GH_ParamAccess.list);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref particle)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetDataList(nextOutputIndex++, particle.Position3DHistory.ToList());
    }
  }
}