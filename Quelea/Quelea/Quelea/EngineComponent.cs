using System;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;


namespace Quelea
{
  public class EngineComponent : AbstractComponent
  {
    private Boolean reset;
    private ISystem system;
    /// <summary>
    /// Initializes a new instance of the Engine class.
    /// </summary>
    public EngineComponent()
      : base(RS.engineName, RS.engineNickname,
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
      pManager.AddBooleanParameter(RS.resetName, RS.resetNickname, RS.resetDescription, GH_ParamAccess.item, RS.resetDefault);
      pManager.AddGenericParameter(RS.systemName, RS.systemNickname, RS.systemDescription, GH_ParamAccess.item);
      
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
        system.Populate();
      }
      else
      {
        system.Run();
      }
    }
  }
}