using Quelea.Util;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SensePointForceComponent : AbstractVehicleForceComponent
  {
    private Point3d sourcePt;
    private double radius;
    
    public SensePointForceComponent()
      : base("Sense Point Force", "SensePt",
          "Sense Point Force",
          RS.icon_sensePointForce, "2e0a5081-6a73-440e-91b5-c5ed96d512f0")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddPointParameter(RS.pointName, RS.pointNickname, "The source point for the sensory field.", GH_ParamAccess.item, Point3d.Origin);
      pManager.AddNumberParameter("Radius", "R", "The radius of the range of the sensory field falloff.", GH_ParamAccess.item, 10);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref sourcePt)) return false;
      if (!da.GetData(nextInputIndex++, ref radius)) return false;
      if (radius < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Radius must be positive.");
        return false;
      }
      return true;
    }

    protected override void GetSensorReadings()
    {
      sourcePt = vehicle.Environment.MapTo2D(sourcePt);
      sensorLeftValue = sensorLeftPos.DistanceTo(sourcePt);
      sensorRightValue = sensorRightPos.DistanceTo(sourcePt);
      sensorLeftValue = Number.Map(sensorLeftValue, 0, radius, 0, 1, false);
      sensorRightValue = Number.Map(sensorRightValue, 0, radius, 0, 1, false);
    }
  }
}
