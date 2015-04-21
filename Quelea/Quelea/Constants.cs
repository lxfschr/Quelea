using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;

namespace Quelea
{
  public class Constants
  {
    public static double Tolerance
    {
      get 
      {
        return RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
      }
    }
  }
}
