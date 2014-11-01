using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent.Forces
{
  public class ContainForceComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the ContainForceComponent class.
    /// </summary>
    public ContainForceComponent()
      : base("ContainForce", "Contain",
          "Contain Force",
          "Agent", "Forces")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddNumberParameter("Weight", "W", "Weight multiplier.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Vision Multiplier", "V", "Vision multiplier.", GH_ParamAccess.item, 1.0);
      pManager.AddGenericParameter("Environment", "En", "Environment", GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Contain Force", "F", "Contain Force", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      double weight = 1.0;
      double visionRadiusMultiplier = 1.0;
      EnvironmentType environment = new WorldBoxEnvironmentType();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref weight)) return;
      if (!DA.GetData(1, ref visionRadiusMultiplier)) return;
      if (!DA.GetData(2, ref environment)) return;

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
      ContainForceType force = new ContainForceType(weight, 
                                                        visionRadiusMultiplier, 
                                                        environment);

      // Finally assign the spiral to the output parameter.
      DA.SetData(0, force);
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
        return null;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{56b405d7-46b8-46a7-aaeb-c3fc2e8992c9}"); }
    }
  }
}