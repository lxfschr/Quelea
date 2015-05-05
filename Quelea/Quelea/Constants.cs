using RS = Quelea.Properties.Resources;
using Rhino;
using Rhino.Geometry;

namespace Quelea
{
  public class Constants
  {
    public static double AbsoluteTolerance
    {
      get 
      {
        return RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
      }
    }
    public static double RelativeTolerance
    {
      get
      {
        return RhinoDoc.ActiveDoc.ModelRelativeTolerance;
      }
    }
    public static double AngleToleranceDegrees
    {
      get
      {
        return RhinoDoc.ActiveDoc.ModelAngleToleranceDegrees;
      }
    }
    public static double AngleToleranceRadians
    {
      get
      {
        return RhinoDoc.ActiveDoc.ModelAngleToleranceRadians;
      }
    }

    public static Vector3d VelocityMin
    {
      get
      {
        return new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault);
      }
    }

    public static Vector3d VelocityMax
    {
      get
      {
        return new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault);
      }
    }
  }
}
