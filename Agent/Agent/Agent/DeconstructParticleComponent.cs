using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class DeconstructParticleComponent : AbstractComponent
  {
    private IParticle particle;
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DeconstructParticleComponent()
      : base(RS.deconstructQueleaName, RS.deconstructQueleaNickName,
             RS.deconstructQueleaDescription, RS.pluginCategoryName, 
             RS.pluginSubCategoryName, RS.icon_deconstructAgent, RS.deconstructQueleaGuid)
    {
      particle = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPointParameter(RS.positionName, RS.positionNickName, RS.positionDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter(RS.velocityName, RS.velocityNickname, RS.velocityDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter(RS.accelerationName, RS.accelerationNickName, RS.accelerationDescription, GH_ParamAccess.item);
      pManager.AddIntegerParameter(RS.lifespanName, RS.lifespanNickname, RS.lifespanDescription, GH_ParamAccess.item);
      pManager.AddPointParameter("Reference Position", "RP", "For particles bound to Surface Environments, the position of the Agent mapped to a 2d plane representing the bounds of the surface ", GH_ParamAccess.item);
      pManager.HideParameter(4);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref particle)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetDataList(nextOutputIndex++, particle.PositionHistory.ToList());
      da.SetData(nextOutputIndex++, particle.Velocity);
      da.SetData(nextOutputIndex++, particle.Acceleration);
      da.SetData(nextOutputIndex++, particle.Lifespan);
      da.SetData(nextOutputIndex++, particle.RefPosition);
    }
  }
}