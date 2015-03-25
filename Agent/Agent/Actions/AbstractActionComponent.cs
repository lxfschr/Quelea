using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractActionComponent : AbstractComponent
  {
    protected AgentType agent;
    protected bool apply;

    /// <summary>
    /// Initializes a new instance of the AbstractActionComponent class.
    /// </summary>
    protected AbstractActionComponent(string name, string nickname, string description, 
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, RS.pluginCategoryName, subcategory, icon, componentGuid)
    {
      agent = new AgentType();
      apply = true;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager, int particlesName)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
      pManager.AddBooleanParameter(RS.applyName, RS.booleanNickname, RS.applyDescription,
         GH_ParamAccess.item, RS.applyDefault);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      if (!da.GetData(nextInputIndex++, ref apply)) return false;

      return true;
    }
  }
}