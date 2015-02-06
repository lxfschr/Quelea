using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class AlignForceComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the AlignForceComponent class.
    /// </summary>
    public AlignForceComponent()
      : base("Align Force", "Align",
          "Cohesion",
          "Agent", "Forces")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddNumberParameter("Weight", "W", "Weight multiplier.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Vision Multiplier", "V", "Vision multiplier.", GH_ParamAccess.item, 2.0/3.0);

      // If you want to change properties of certain parameters, 
      // you can use the pManager instance to access them by index:
      //pManager[0].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Cohesion Force", "F", "Cohesion Force", GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      double weight = 1.0;
      double visionRadiusMultiplier = 2.0 / 3.0;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref weight)) return;
      if (!da.GetData(1, ref visionRadiusMultiplier)) return;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(0.0 <= visionRadiusMultiplier && visionRadiusMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Vision Multiplier must be between 0.0 and 1.0");
        return;
      }
      if (!(-1.0 <= weight && weight <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Weight must be between -1.0 and 1.0");
        return;
      }


      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AlignForceType force = new AlignForceType(weight, visionRadiusMultiplier);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, force);
      //DA.SetData(1, pt);
    }

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override System.Drawing.Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return Resources.IconForThisComponent;
        return Properties.Resources.icon_alignForce;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{2734fde9-1c90-4ecd-bcf8-c58dafc5e161}"); }
    }
  }
}