using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent.Environment
{
  public class BrepEnvironmentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the BrepEnvironmentComponent class.
    /// </summary>
    public BrepEnvironmentComponent()
      : base("Brep Environment", "BrepEnv",
          "Brep Environment",
          "Agent", "Environments")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddBrepParameter("Brep", "B", "A closed Brep with normals facing out.", GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Brep Environment", "En", "Brep Environment", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      //Interval interval = new Interval(-100.0, 100.0);
      //Box box = new Box(Plane.WorldXY, interval, interval, interval);
      Brep brep = new Cone(Plane.WorldXY, 100, 100).ToBrep(true);
      //Brep brep = Brep.CreateFromBox(box);

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref brep)) return;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(brep.IsSolid))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Brep must be closed.");
        return;
      }

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      EnvironmentType environment = new BrepEnvironmentType(brep);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, environment);
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
      get { return new Guid("{27402fe3-133b-4ce2-9b3d-bbc081f4db71}"); }
    }
  }
}