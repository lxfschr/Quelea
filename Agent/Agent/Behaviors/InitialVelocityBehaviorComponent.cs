using System;
using System.Drawing;
using Agent.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class InitialVelocityBehaviorComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the InitialVelocityBehaviorComponent class.
    /// </summary>
    public InitialVelocityBehaviorComponent()
      : base("Initial Velocity", "InitialVel",
          "Applys a 1 time force to the Agent when it is first emitted.",
          Resources.pluginCategoryName, Resources.behaviorsSubCategoryName)
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
      pManager.AddGenericParameter(Resources.agentName, Resources.agentNickName, Resources.agentDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter("Initial Direction", "V", "The direction to travel in initially.", GH_ParamAccess.item);

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
      pManager.AddBooleanParameter(Resources.behaviorAppliedName, Resources.behaviorNickName, Resources.behaviorAppliedDescription, GH_ParamAccess.list);

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
      // We'll start by declaring variables and assigning them starting values
      AgentType agent = new AgentType();
      Vector3d initialVelocity = new Vector3d();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;
      if (!da.GetData(1, ref initialVelocity)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:


      bool behaviorApplied = Run(agent, initialVelocity);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, behaviorApplied);
    }

    private static bool Run(AgentType agent, Vector3d initialVelocity)
    {
      if (!agent.InitialVelocitySet)
      {
        agent.Velocity = initialVelocity;
        agent.InitialVelocitySet = true;
        return true;
      }
      return false;
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
        return Resources.icon_InitialVelocity;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{e8da8a7b-9d58-4583-ab88-b4c9c8bd7fca}"); }
    }
  }
}