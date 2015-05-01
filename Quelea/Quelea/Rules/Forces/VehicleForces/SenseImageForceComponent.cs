using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SenseImageForceComponent : AbstractVehicleForceComponent
  {
    private Bitmap bitmap;
    public SenseImageForceComponent()
      : base("Sensory Field Force", "SenseField",
          "Sensory Field Force",
          null, "8fd658ec-90df-41ef-8f1a-490a05a58a13")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter("Bitmap Image", "I", "An image from which the brightness values will be used to steer the Vehicle. Ideally, this will be a grayscale somewhat blurry image.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref bitmap)) return false;
      return true;
    }

    protected override void GetSensorReadings()
    {
      BoundingBox bbox = vehicle.Environment.GetBoundingBox();
      double minX = bbox.Min.X;
      double maxX = bbox.Max.X;
      double minY = bbox.Min.Y;
      double maxY = bbox.Max.Y;
      int x = (int) Util.Number.Map(sensorLeftPos.X, minX, maxX, 0, bitmap.Width - 1, false);
      int y = (int)(bitmap.Height - Util.Number.Map(sensorLeftPos.Y, minY, maxY, 0, bitmap.Height - 1, false));

      Color color = crossed ? Color.White : Color.Black;
      
      if ((0 <= x && x < bitmap.Width) && (0 <= y && y < bitmap.Height))
      {
        color = bitmap.GetPixel(x, y);
      }
      sensorLeftValue = color.GetBrightness();
      x = (int)Util.Number.Map(sensorRightPos.X, minX, maxX, 0, bitmap.Width - 1, false);
      y = (int)(bitmap.Height - Util.Number.Map(sensorRightPos.Y, minY, maxY, 0, bitmap.Height - 1, false));

      color = crossed ? Color.White : Color.Black;
      if ((0 <= x && x < bitmap.Width) && (0 <= y && y < bitmap.Height))
      {
        color = bitmap.GetPixel(x, y);
      }
      sensorRightValue = color.GetBrightness();
    }
  }
}
