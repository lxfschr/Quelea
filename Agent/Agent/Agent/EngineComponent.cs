using System;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;


namespace Agent
{
  public class EngineComponent : AbstractComponent
  {
    private Boolean reset;
    private AgentSystemType system;
    /// <summary>
    /// Initializes a new instance of the Engine class.
    /// </summary>
    public EngineComponent()
      : base(RS.engineName, RS.engineNickName,
          RS.engineDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName, RS.icon_engine, RS.engineGUID)
    {
      reset = RS.resetDefault;
      system = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddBooleanParameter(RS.resetName, RS.resetNickName, RS.resetDescription, GH_ParamAccess.item, RS.resetDefault);
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.systemDescription, GH_ParamAccess.item);
      
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref reset)) return false;
      if (!da.GetData(nextInputIndex++, ref system)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      Run();
    }

    private void Run()
    {
      if (reset)
      {
        system.Agents.Clear();
        system.Populate();
      }
      else
      {
        system.Run();
      }
    }
  }
}