using System;
using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractEnvironmentalForceComponent : AbstractAgentForceComponent
  {
    protected AbstractEnvironmentType environment;
    protected double visionRadius;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractEnvironmentalForceComponent(string name, string nickname, string description,
                                                  string subcategory, Bitmap icon, String componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
      environment = new AxisAlignedBoxEnvironmentType();
      visionRadius = RS.bodySizeDefault;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, RS.environmentDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.visionRadiusName, RS.visionRadiusNickName, RS.visionAngleDescription,
        GH_ParamAccess.item, RS.bodySizeDefault);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref environment)) return false;
      if (!da.GetData(nextInputIndex++, ref visionRadius)) return false;

      return true;
    }
  }
}