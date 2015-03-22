using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AgentComponent : AbstractComponent
  {
    private int lifespan;
    private double mass;
    private double bodySize;
    private double maxSpeed;
    private double maxForce;
    private int historyLength;
    /// <summary>
    /// Initializes a new instance of the AgentComponent class.
    /// </summary>
    public AgentComponent()
      : base(RS.constructAgentName, RS.constructAgentNickName,
          RS.constructAgentDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName, RS.icon_agent, RS.constructAgentComponentGUID)
    {
      lifespan = RS.lifespanDefault;
      mass = RS.massDefault;
      bodySize = RS.bodySizeDefault;
      maxSpeed = RS.maxSpeedDefault;
      maxForce = RS.maxForceDefault;
      historyLength = RS.historyLenDefault;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddIntegerParameter(RS.lifespanName, RS.lifespanNickName, RS.lifespanDescription, GH_ParamAccess.item, RS.lifespanDefault);
      pManager.AddNumberParameter(RS.massName, RS.massNickName, RS.massDescription, GH_ParamAccess.item, RS.massDefault);
      pManager.AddNumberParameter(RS.bodySizeName, RS.bodySizeNickName, RS.bodySizeDescription, GH_ParamAccess.item, RS.bodySizeDefault);
      pManager.AddNumberParameter(RS.maxSpeedName, RS.maxSpeedNickName, RS.maxSpeedDescription, GH_ParamAccess.item, RS.maxSpeedDefault);
      pManager.AddNumberParameter(RS.maxForceName, RS.maxForceNickName, RS.maxForceDescription, GH_ParamAccess.item, RS.maxForceDefault);
      pManager.AddIntegerParameter(RS.historyLenName, RS.historyLenNickName, RS.historyLenDescription, GH_ParamAccess.item, RS.historyLenDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref lifespan)) return false;
      if (!da.GetData(nextInputIndex++, ref mass)) return false;
      if (!da.GetData(nextInputIndex++, ref bodySize)) return false;
      if (!da.GetData(nextInputIndex++, ref maxSpeed)) return false;
      if (!da.GetData(nextInputIndex++, ref maxForce)) return false;
      if (!da.GetData(nextInputIndex++, ref historyLength)) return false;

      // We should now validate the data and warn the user if invalid data is supplied.
      //if (lifespan <= 0)
      //{
      //  AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.lifespanErrorMessage);
      //  return;
      //}
      if (mass <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.massErrorMessage);
        return false;
      }
      if (bodySize < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.bodySizeErrorMessage);
        return false;
      }
      if (maxSpeed < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.maxSpeedErrorMessage);
        return false;
      }
      if (maxForce < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.maxForceErrorMessage);
        return false;
      }
      if (historyLength < 1)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "History length must be at least 1.");
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AgentType agent = new AgentType(lifespan, mass, bodySize, maxSpeed,
                                      maxForce, historyLength);

      // Finally assign the spiral to the output parameter.
      da.SetData(nextOutputIndex++, agent);
    }
  }
}