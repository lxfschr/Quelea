using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractEnvironmentalBehaviorComponent : AbstractBehaviorComponent
  {
    protected AbstractEnvironmentType environment;
    /// <summary>
    /// Initializes a new instance of the EatBehaviorComponent class.
    /// </summary>
    protected AbstractEnvironmentalBehaviorComponent(string name, string nickname, string description,
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
      environment = new AxisAlignedBoxEnvironmentType();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams2(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, RS.bounceContainBehEnvDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams2(GH_OutputParamManager pManager)
    {
    }

    protected override bool GetInputs2(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref environment)) return false;
      return true;
    }
  }
}