using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AgentSystemComponent : AbstractComponent
  {
    private AgentSystemType system;
    private List<AgentType> agents;
    private List<AbstractEmitterType> emitters;
    private AbstractEnvironmentType environment;
    /// <summary>
    /// Initializes a new instance of the AbstractSystemComponent class.
    /// </summary>
    public AgentSystemComponent()
      : base(RS.systemName, RS.systemComponentNickName,
          RS.systemDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName, RS.icon_system, RS.agentSystemGuid)
    {
      
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams (GH_InputParamManager pManager, int particlesName)
    {
      pManager.AddGenericParameter(RS.agentsName, RS.agentNickName, RS.agentDescription, 
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.emittersName, RS.emitterNickName, RS.emittersDescription,
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, "Restricts and Agent's postion to be contained within the environment. This is most useful for Surface Environments.",
                                    GH_ParamAccess.item);
      pManager[2].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams
      (GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.systemDescription, 
                                   GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      agents = new List<AgentType>();
      emitters = new List<AbstractEmitterType>();
      environment = null;
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this
      // method.
      if (!da.GetDataList(nextInputIndex++, agents)) return false;
      if (!da.GetDataList(nextInputIndex++, emitters)) return false;
      da.GetData(nextInputIndex++, ref environment);

      // We should now validate the data and warn the user if invalid data is 
      // supplied.
      if (agents.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.agentsCountErrorMessage);
        return false;
      }
      if (emitters.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.emittersCountErrorMessage);
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      if (system == null)
      {
        system = new AgentSystemType(agents.ToArray(), emitters.ToArray(), environment);
      }
      else
      {
        system = new AgentSystemType(agents.ToArray(), emitters.ToArray(), environment, system);
      }

      // Finally assign the system to the output parameter.
      da.SetData(nextOutputIndex++, system);
    }
  }
}