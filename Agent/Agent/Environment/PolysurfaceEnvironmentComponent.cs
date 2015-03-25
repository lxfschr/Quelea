using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class PolysurfaceEnvironmentComponent : AbstractEnvironmentComponent
  {
    private Brep brep;
    private Vector3d borderDir;
    /// <summary>
    /// Initializes a new instance of the WorldBoxEnvironmentComponent class.
    /// </summary>
    public PolysurfaceEnvironmentComponent()
      : base("Polysurface Environment", "PolysrfEnv",
          "A 3D Polysurface Environment.", RS.icon_polysurfaceEnvironment, "646e7585-d3b0-4ebd-8ce3-8efc5cd6b7d9")
    {
      brep = new Brep();
      borderDir = Vector3d.Zero;
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
      base.RegisterOutputParams(pManager);
      pManager.AddGenericParameter("Border Walls", "BW", "The walls that represent the boundary of the polysurface.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref brep)) return false;
      if (!da.GetData(nextInputIndex++, ref borderDir)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEnvironmentType environment = new PolysurfaceEnvironmentType(brep, borderDir);
      da.SetData(nextOutputIndex++, environment);
      da.SetDataTree(nextOutputIndex++, BrepArray2DToDatatree(((PolysurfaceEnvironmentType)environment).BorderWallsArray));
    }

    private static DataTree<Brep> BrepArray2DToDatatree(Brep[][] array)
    {
      DataTree<Brep> tree = new DataTree<Brep>();
      GH_Path trunk = new GH_Path();

      for (int i = 0; i < array.Length; i++)
      {
        GH_Path branch = trunk.AppendElement(i);

        for (int j = 0; j < array[i].Length; j++)
        {
          tree.Add(array[i][j], branch);
        }
      }
      return tree;
    }
  }
}