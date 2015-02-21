using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractEnvironmentalForceComponent : AbstractForceComponent
  {
    protected AbstractEnvironmentType environment;
    protected double visionRadius;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractEnvironmentalForceComponent(string name, string nickname, string description,
                              string category, string subCategory, Bitmap icon, String componentGuid)
      : base(name, nickname, description, category, subCategory, icon, componentGuid)
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

      if (!da.GetData(4, ref environment)) return false;
      if (!da.GetData(5, ref visionRadius)) return false;

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      Vector3d force = Run();

      // Finally assign the output parameter.
      da.SetData(0, force);

      return true;
    }
  }
}