using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractEmitterComponent : AbstractComponent
  {
    protected bool continuousFlow;
    protected int creationRate;
    protected int numAgents;
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    protected AbstractEmitterComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
      : base(name, nickname, description, RS.pluginCategoryName, 
             RS.emittersSubCategoryName, icon, componentGuid)
    {
      continuousFlow = RS.continuousFlowDefault;
      creationRate = RS.creationRateDefault;
      numAgents = RS.numAgentsDefault;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddBooleanParameter(RS.continuousFlowName, RS.continuousFlowNickName, RS.continuousFlowDescription, GH_ParamAccess.item, RS.continuousFlowDefault);
      pManager.AddIntegerParameter(RS.creationRateName, RS.creationRateNickName, RS.creationRateDescription, GH_ParamAccess.item, RS.creationRateDefault);
      pManager.AddIntegerParameter(RS.numAgentsName, RS.numAgentsNickName, RS.numAgentsDescription, GH_ParamAccess.item, RS.numAgentsDefault);

      pManager[1].Optional = true;
      pManager[2].Optional = true;
      // If you want to change properties of certain parameters, 
      // you can use the pManager instance to access them by index:
      //pManager[0].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(RS.emitterName, RS.emitterNickName, RS.emitterDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    /// <summary>
    /// The Exposure property controls where in the panel a component icon 
    /// will appear. There are seven possible locations (primary to septenary), 
    /// each of which can be combined with the GH_Exposure.obscure flag, which 
    /// ensures the component will only be visible on panel dropdowns.
    /// </summary>
    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.primary; }
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref continuousFlow)) return false;
      if (!da.GetData(nextInputIndex++, ref creationRate)) return false;
      if (!da.GetData(nextInputIndex++, ref numAgents)) return false;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (creationRate <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.creationRateErrorMessage);
        return false;
      }
      if (numAgents < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.numAgentsErrorMessage);
        return false;
      }
      return true;
    }
  }
}
