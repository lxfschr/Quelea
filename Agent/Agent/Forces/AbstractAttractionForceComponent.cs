using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractAttractionForceComponent : AbstractForceComponent
  {
    protected Point3d targetPt;
    protected double radius;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractAttractionForceComponent(string name, string nickname, string description,
                                               string subcategory, Bitmap icon, String componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
      targetPt = new Point3d();
      radius = RS.attractionRadiusDefault;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams3(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("Target Point", "P", "Point to be attracted to.", GH_ParamAccess.item);
      pManager.AddNumberParameter("Attraction Radius", "R", "The radius within which Agents will be affected by the attractor. If negative, the radius will be assumed to be infinite.",
        GH_ParamAccess.item, RS.attractionRadiusDefault);
      RegisterInputParams4(pManager);
    }

    protected abstract void RegisterInputParams4(GH_InputParamManager pManager);

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override bool GetInputs3(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref targetPt)) return false;
      if (!da.GetData(nextInputIndex++, ref radius)) return false;

      return GetInputs4(da);
    }

    protected abstract bool GetInputs4(IGH_DataAccess da);
  }
}