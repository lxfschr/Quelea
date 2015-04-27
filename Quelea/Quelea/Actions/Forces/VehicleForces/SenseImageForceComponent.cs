using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SensoryFieldForceComponent : AbstractVehicleForceComponent
  {
    private double sensorLeftValue, sensorRightValue;
    private bool crossed;
    private Bitmap bitmap;
    public SensoryFieldForceComponent()
      : base("Sensory Field Force", "SenseField",
          "Sensory Field Force",
          null, "8fd658ec-90df-41ef-8f1a-490a05a58a13")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter("Bitmap Image", "I", "An image from which the brightness values will be used to steer the Vehicle. Ideally, this will be a grayscale somewhat blurry image.", GH_ParamAccess.item);
      pManager.AddBooleanParameter("Crossed?", "C", "If true, the sensors will affect the wheels on the opposite side. If false, a higher sensor reading on the left side will cause the left wheel to turn faster causing the vehicle to turn to its right. Generally, if the sensors are not crossed, then the vehicle will steer away from areas with high values.",
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
      if (!da.GetData(nextInputIndex++, ref bitmap)) return false;
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
      Point3d sensorLeftPos = vehicle.GetPartPosition(vehicle.BodySize, vehicle.HalfPi);
      Point3d sensorRightPos = vehicle.GetPartPosition(vehicle.BodySize, -vehicle.HalfPi);
      int x = (int)sensorLeftPos.X * 10;
      int y = bitmap.Height - (int)sensorLeftPos.Y * 10;
       Color color = crossed ? Color.White : Color.Black;
      
      if ((0 <= x && x < bitmap.Width) && (0 <= y && y < bitmap.Height))
      {
        color = bitmap.GetPixel(x, y);
        Color c = bitmap.GetPixel(0, 0);
      }
      sensorLeftValue = color.GetBrightness();
      x = (int)sensorRightPos.X * 10;
      y = bitmap.Height - (int)sensorRightPos.Y * 10;
      color = crossed ? Color.White : Color.Black;
      if ((0 <= x && x < bitmap.Width) && (0 <= y && y < bitmap.Height))
      {
        color = bitmap.GetPixel(x, y);
      }
      sensorRightValue = color.GetBrightness();
      if (crossed)
      {
        vehicle.SetSpeedChanges(sensorRightValue, sensorLeftValue);
      }
      else
      {
        vehicle.SetSpeedChanges(sensorLeftValue, sensorRightValue);
      }
      
      return Vector3d.Zero;
    }
  }
}
