using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SensorForceComponent : AbstractVehicleForceComponent
  {
    private double sensorLeftValue, sensorRightValue;
    private bool crossed;
    public SensorForceComponent()
      : base("Sensor Force", "SensorForce",
          "Sensor Force",
          null, "18b76cb8-408f-47b1-87f2-a0c0589c42dd")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter("SL", "SL", "SL", GH_ParamAccess.item);
      pManager.AddNumberParameter("SR", "SR", "SR", GH_ParamAccess.item);
      pManager.AddBooleanParameter("Crossed?", "C", "If true, the sensors will affect the wheels on the opposite side.",
        GH_ParamAccess.item, false);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      base.RegisterOutputParams(pManager);
      pManager.AddNumberParameter("SL", "SL", "SL", GH_ParamAccess.item);
      pManager.AddNumberParameter("SR", "SR", "SR", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref sensorLeftValue)) return false;
      if (!da.GetData(nextInputIndex++, ref sensorRightValue)) return false;
      if (!da.GetData(nextInputIndex++, ref crossed)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      base.SetOutputs(da);
      da.SetData(nextOutputIndex++, sensorLeftValue);
      da.SetData(nextOutputIndex++, sensorRightValue);
    }

    protected override Vector3d CalcForce()
    {
      double wheelDiff;
      if (crossed)
      {
        vehicle.SetSpeedChanges(sensorRightValue, sensorLeftValue);
        wheelDiff = sensorRightValue * vehicle.WheelRadius - sensorLeftValue * vehicle.WheelRadius;
      }
      else
      {
        vehicle.SetSpeedChanges(sensorLeftValue, sensorRightValue);
        wheelDiff = sensorLeftValue * vehicle.WheelRadius - sensorRightValue * vehicle.WheelRadius;
      }
      double angle = wheelDiff / vehicle.BodySize;
      Vector3d desired = vehicle.Velocity;
      desired.Rotate(angle, vehicle.Orientation.ZAxis);

      return desired;
    }
  }
}
