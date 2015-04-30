using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SystemComponent : AbstractComponent
  {
    private ISystem system;
    private List<IQuelea> agents;
    private List<AbstractEmitterType> emitters;
    private AbstractEnvironmentType environment;
    /// <summary>
    /// Initializes a new instance of the SystemComponent class.
    /// </summary>
    public SystemComponent()
      : base(RS.systemName, RS.systemComponentNickname,
          RS.systemDescription,
          RS.pluginCategoryName, RS.pluginSubcategoryName, RS.icon_system, RS.agentSystemGuid)
    {
      
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.queleaName, RS.queleaNickname, "The settings for your particle, agent, or vehicle; collectively reffered to as 'quelea'.", 
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.emittersName, RS.emitterNickname, RS.emittersDescription,
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickname, "Restricts and Agent's postion to be contained within the environment. This is most useful for Surface and Polysurface Environments. For Brep Environments, consider using the Contain rules as they will run much faster.",
                                    GH_ParamAccess.item);
      pManager[2].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams
      (GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickname, RS.systemDescription, 
                                   GH_ParamAccess.item);
      pManager.AddGenericParameter(RS.queleaName, RS.queleaNickname, RS.queleaDescription, GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.queleaNetworkName, RS.queleaNetworkNickname, RS.queleaNetworkDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      agents = new List<IQuelea>();
      emitters = new List<AbstractEmitterType>();
      environment = new WorldEnvironmentType();
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
        system = new SystemType(agents, emitters, environment);
      }
      else
      {
        system = new SystemType(agents, emitters, environment, (SystemType) system);
      }

      // Finally assign the system to the output parameter.
      da.SetData(nextOutputIndex++, system);
      da.SetDataList(nextOutputIndex++, (List<IQuelea>)system.Quelea.SpatialObjects);
      da.SetData(nextOutputIndex++, new SpatialCollectionType(system.Quelea));
    }
    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.secondary; }
    }
  }
}