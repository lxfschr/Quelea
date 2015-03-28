using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractParticleActionComponent : AbstractActionComponent
  {
    protected IParticle particle;

    /// <summary>
    /// Initializes a new instance of the AbstractParticleActionComponent class.
    /// </summary>
    protected AbstractParticleActionComponent(string name, string nickname, string description, 
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
      particle = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      if (!base.GetInputs(da)) return false;
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref particle)) return false;

      return true;
    }
  }
}