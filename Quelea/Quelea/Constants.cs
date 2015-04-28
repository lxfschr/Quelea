using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;

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
  }
}
