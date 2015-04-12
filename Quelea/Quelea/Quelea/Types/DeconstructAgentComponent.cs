using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class DeconstructAgentComponent : AbstractComponent
  {
    private IAgent agent;
    /// <summary>
    /// Initializes a new instance of the DeconstructAgentComponent class.
    /// </summary>
    public DeconstructAgentComponent()
      : base("Deconstruct Agent", "DeAgent",
             "Deconstructs a Agent to expose its fields such as max speed, max force, and vision radius. Use Deconstruct Particle to expose particle fields such as position.", RS.pluginCategoryName,
             RS.pluginSubCategoryName, RS.icon_deconstructAgent, "4b324c0d-07d7-432b-bef4-983ef0af8cc5")
    {
      agent = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickname + "/" + RS.queleaNickname, RS.agentDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddIntegerParameter(RS.maxSpeedName, RS.maxSpeedNickname, RS.maxSpeedDescription, GH_ParamAccess.item);
      pManager.AddIntegerParameter(RS.maxForceName, RS.maxForceNickname, RS.maxForceDescription, GH_ParamAccess.item);
      pManager.AddIntegerParameter(RS.visionRadiusName, RS.visionRadiusNickname, RS.visionRadiusDescription, GH_ParamAccess.item);
      pManager.AddIntegerParameter(RS.visionAngleName, RS.visionAngleNickname, RS.visionAngleDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetData(nextOutputIndex++, agent.MaxSpeed);
      da.SetData(nextOutputIndex++, agent.MaxForce);
      da.SetData(nextOutputIndex++, agent.VisionRadius);
      da.SetData(nextOutputIndex++, agent.VisionAngle);
    }
  }
}