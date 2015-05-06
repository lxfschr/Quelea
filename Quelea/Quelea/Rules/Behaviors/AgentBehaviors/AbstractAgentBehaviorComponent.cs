using System.Drawing;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractAgentBehaviorComponent : AbstractBehaviorComponent
  {
    protected IAgent agent;
    /// <summary>
    /// Initializes a new instance of the AbstractAgentBehaviorComponent class.
    /// </summary>
    protected AbstractAgentBehaviorComponent(string name, string nickname, string description, 
                                             Bitmap icon, string componentGuid)
      : base(name, nickname, description, RS.agentName + " " + RS.rulesName, icon, componentGuid)
    {
    }
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter(RS.agentName + " " + RS.queleaName, RS.agentNickname + RS.queleaNickname, RS.agentDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      if (!base.GetInputs(da)) return false;
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;

      return true;
    }
  }
}