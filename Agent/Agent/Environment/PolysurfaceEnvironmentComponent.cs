using System;
using System.Drawing;
using Grasshopper;
using RS = Agent.Properties.Resources;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;

namespace Agent
{
  public class PolysurfaceEnvironmentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the WorldBoxEnvironmentComponent class.
    /// </summary>
    public PolysurfaceEnvironmentComponent()
      : base("Polysurface Environment", "PolysrfEnv",
          "A 3D Polysurface Environment.",
          RS.pluginCategoryName, RS.environmentsSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddBrepParameter(RS.brepName, RS.brepNickName, RS.brepForEnvDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter("Border Extrution Direction", "D", "A vector indicating which direction to extrude the borders of the polysurface to create border walls for containment. If the zero vector is supplied, the default is to extrude each border point normal to the surface.", GH_ParamAccess.item, Vector3d.Zero);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Environment", RS.environmentNickName, RS.environmentDescription, GH_ParamAccess.item);
      pManager.AddGenericParameter("Border Walls", "BW", "The walls that represent the boundary of the polysurface.", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      Brep brep = new Brep();
      Vector3d borderDir = Vector3d.Zero;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref brep)) return;
      if (!da.GetData(1, ref borderDir)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AbstractEnvironmentType environment = new PolysurfaceEnvironmentType(brep, borderDir);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, environment);
      da.SetDataTree(1, BrepArray2DToDatatree(((PolysurfaceEnvironmentType)environment).BorderWallsArray));
    }

    private DataTree<Brep> BrepArray2DToDatatree(Brep[][] array)
    {
      DataTree<Brep> tree = new DataTree<Brep>();
      GH_Path trunk = new GH_Path();
      GH_Path branch = new GH_Path();

      for (int i = 0; i < array.Length; i++)
      {
        branch = trunk.AppendElement(i);

        // ...with 4 items each
        for (int j = 0; j < array[i].Length; j++)
        {
          tree.Add(array[i][j], branch);
        }
      }
      return tree;
    }

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return Resources.IconForThisComponent;
        return RS.icon_srfEnvironment;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{646e7585-d3b0-4ebd-8ce3-8efc5cd6b7d9}"); }
    }
  }
}