using System;
using System.Drawing;
using Agent.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class BoxEmitterComponent : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public BoxEmitterComponent()
      : base(Resources.boxEmitName, Resources.boxEmitNickName,
          Resources.boxEmitDescription,
          Resources.pluginCategoryName, Resources.emittersSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddBoxParameter(Resources.boxName, Resources.boxNickName, Resources.boxForEmitterDescription, GH_ParamAccess.item);
      pManager.AddBooleanParameter(Resources.continuousFlowName, Resources.continuousFlowNickName, Resources.continuousFlowDescription, GH_ParamAccess.item, true);
      pManager.AddIntegerParameter(Resources.creationRateName, Resources.creationRateNickName, Resources.creationRateDescription, GH_ParamAccess.item, 1);
      pManager.AddIntegerParameter(Resources.numAgentsName, Resources.numAgentsNickName, Resources.numAgentsDescription, GH_ParamAccess.item, 0);

      pManager[2].Optional = true;
      pManager[3].Optional = true;
      // If you want to change properties of certain parameters, 
      // you can use the pManager instance to access them by index:
      //pManager[0].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(Resources.boxEmitName, Resources.emitterNickName, Resources.boxEmitDescription, GH_ParamAccess.item);
      //pManager.AddPointParameter("Point Emitter Location", "P", "Point Emitter Location", GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      Box box = new Box();
      bool continuousFlow = true;
      int creationRate = Resources.creationRateDefault;
      int numAgents = Resources.numAgentsDefault;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref box)) return;
      if (!da.GetData(1, ref continuousFlow)) return;
      if (!da.GetData(2, ref creationRate)) return;
      if (!da.GetData(3, ref numAgents)) return;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (creationRate <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, Resources.creationRateErrorMessage);
        return;
      }
      if (numAgents < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, Resources.numAgentsErrorMessage);
        return;
      }

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      BoxEmitterType emitter = new BoxEmitterType(box, continuousFlow, creationRate, numAgents);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, emitter);
    }

    /// <summary>
    /// The Exposure property controls where in the panel a component icon 
    /// will appear. There are seven possible locations (primary to septenary), 
    /// each of which can be combined with the GH_Exposure.obscure flag, which 
    /// ensures the component will only be visible on panel dropdowns.
    /// </summary>
    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.primary; }
    }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// </summary>
    protected override Bitmap Icon
    {
      get
      {
        // You can add image files to your project resources and access them like this:
        //return Resources.IconForThisComponent;
        //return null;
        return Resources.icon_crvEmitter;
      }
    }

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(Resources.boxEmitGUID); }
    }
  }
}
