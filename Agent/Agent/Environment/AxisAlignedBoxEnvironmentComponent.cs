using System;
using System.Drawing;
using Agent.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class AxisAlignedBoxEnvironmentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the AxisAlignedBoxEnvironmentComponent class.
    /// </summary>
    public AxisAlignedBoxEnvironmentComponent()
      : base(Resources.AABoxEnvName, Resources.AABoxEnvNickName,
          Resources.AABoxEnvDescription,
          Resources.pluginCategoryName, Resources.environmentsSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddBoxParameter(Resources.boxName, Resources.boxNickName, Resources.AABoxDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(Resources.AABoxEnvName, Resources.environmentNickName, Resources.AABoxEnvDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      Interval interval = new Interval(-Resources.boxBoundsDefault, Resources.boxBoundsDefault);
      Box box = new Box(Plane.WorldXY, interval, interval, interval);

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref box)) return;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(box.Plane.XAxis.Equals(Plane.WorldXY.XAxis) && box.Plane.YAxis.Equals(Plane.WorldXY.YAxis)))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, Resources.AABoxError);
        return;
      }

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AbstractEnvironmentType environment = new AxisAlignedBoxEnvironmentType(box);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, environment);
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
        return Resources.icon_AABoxEnvironment;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(Resources.AABoxEnvGUID); }
    }
  }
}