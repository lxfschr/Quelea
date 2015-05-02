using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractAvoidObstacleForceComponent : AbstractAgentForceComponent
  {
    protected Brep obstacle;
    protected double visionRadiusMultiplier;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractAvoidObstacleForceComponent(string name, string nickname, string description,
                                                  Bitmap icon, String componentGuid)
      : base(name, nickname, description, icon, componentGuid)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddBrepParameter("Obstacle", "O", "A brep representing the obstacle to avoid.", GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.visionRadiusMultiplierName, RS.visionRadiusMultiplierNickname, RS.visionRadiusMultiplierDescription,
        GH_ParamAccess.item, RS.visionRadiusMultiplierDefault / 5);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref obstacle)) return false;
      if (!da.GetData(nextInputIndex++, ref visionRadiusMultiplier)) return false;

      return true;
    }
  }
}