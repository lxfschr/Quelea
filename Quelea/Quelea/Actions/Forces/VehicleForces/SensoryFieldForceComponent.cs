using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SensoryFieldForceComponent : AbstractVehicleForceComponent
  {
    private string imagePath;
    private double radius;
    private double sensorLeftValue, sensorRightValue;
    private bool crossed;
    public SensoryFieldForceComponent()
      : base("Sensory Field Force", "SenseField",
          "Sensory Field Force",
          null, "8fd658ec-90df-41ef-8f1a-490a05a58a13")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddTextParameter("Bitmap", "I", "Bitmap image.", GH_ParamAccess.item);
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
      if (!da.GetData(nextInputIndex++, ref imagePath)) return false;
      if (!da.GetData(nextInputIndex++, ref crossed)) return false;
      if (radius < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Radius must be positive.");
        return false;
      }
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
      Bitmap bitmap = new Bitmap(imagePath);
      GH_MemoryBitmap memoryBitmap = new GH_MemoryBitmap(bitmap);
      try
      {
        memoryBitmap.Filter_LumScale();
        Color color = Color.Transparent;
        if (memoryBitmap.Sample(sensorLeftPos.X * 10, sensorLeftPos.Y * 10, ref color))
        {
          sensorLeftValue = color.GetBrightness();
        }
        if (memoryBitmap.Sample(sensorRightPos.X * 10, sensorRightPos.Y * 10, ref color))
        {
          sensorRightValue = color.GetBrightness();
        }
      }
      catch (Exception e)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Sensor position is outside of image bounds.");
      }
      finally
      {
        if (!memoryBitmap.Equals(null))
        {
          memoryBitmap.Release(false);
        }
      }
      
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
