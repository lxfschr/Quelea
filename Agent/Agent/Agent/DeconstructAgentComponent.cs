using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class DeconstructAgentComponent : AbstractComponent
  {
    private AgentType agent;
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DeconstructAgentComponent()
      : base(RS.deconstructAgentName, RS.deconstructAgentNickName,
             RS.deconstructAgentDescription, RS.pluginCategoryName, 
             RS.pluginSubCategoryName, RS.icon_deconstructAgent, RS.deconstructAgentGUID)
    {
      agent = new AgentType();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager, int particlesName)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPointParameter(RS.positionName, RS.positionNickName, RS.positionDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter(RS.velocityName, RS.velocityNickName, RS.velocityDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter(RS.accelerationName, RS.accelerationNickName, RS.accelerationDescription, GH_ParamAccess.item);
      pManager.AddIntegerParameter(RS.lifespanName, RS.lifespanNickName, RS.lifespanDescription, GH_ParamAccess.item);
      pManager.AddPointParameter("Reference Position", "RP", "For Agents bound to Surface Environments, the position of the Agent mapped to a 2d plane representing the bounds of the surface ", GH_ParamAccess.item);
      pManager.HideParameter(4);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetDataList(nextOutputIndex++, agent.PositionHistory.ToList());
      da.SetData(nextOutputIndex++, agent.Velocity);
      da.SetData(nextOutputIndex++, agent.Acceleration);
      da.SetData(nextOutputIndex++, agent.Lifespan);
      da.SetData(nextOutputIndex++, agent.RefPosition);
    }
  }
}