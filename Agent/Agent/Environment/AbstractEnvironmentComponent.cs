using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractEnvironmentComponent : AbstractComponent
  {
    /// <summary>
    /// Initializes a new instance of the AxisAlignedBoxEnvironmentComponent class.
    /// </summary>
    protected AbstractEnvironmentComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
      : base(name, nickname, description, RS.pluginCategoryName,
             RS.environmentsSubcategoryName, icon, componentGuid)
    {
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickname, RS.environmentDescription, GH_ParamAccess.item);
    }
  }
}