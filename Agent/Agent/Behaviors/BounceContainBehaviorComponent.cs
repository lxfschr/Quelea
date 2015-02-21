using System;
using System.Collections.Generic;
using System.Drawing;
using RS = Agent.Properties.Resources;
using Grasshopper.Kernel;

namespace Agent
{
  public class BounceContainBehaviorComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the EatBehaviorComponent class.
    /// </summary>
    public BounceContainBehaviorComponent()
      : base(RS.bounceContainBehName, RS.bounceContainBehNickName,
          RS.bounceContainBehDescription,
          RS.pluginCategoryName, RS.behaviorsSubCategoryName)
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
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, RS.bounceContainBehEnvDescription, GH_ParamAccess.item);

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
      pManager.AddBooleanParameter(RS.behaviorAppliedName, RS.behaviorNickName, RS.behaviorAppliedDescription, GH_ParamAccess.item);

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
      IModifiableAgent agent = new AgentType();
      AbstractEnvironmentType environment = new AxisAlignedBoxEnvironmentType();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;
      if (!da.GetData(1, ref environment)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:


      bool behaviorApplied = Run(agent, environment);

      // Finally assign the spiral to the output parameter.
      da.SetData(0, behaviorApplied);
    }

    private static bool Run(IModifiableAgent agent, AbstractEnvironmentType environment)
    {
      return environment.BounceContain(agent);
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
        return RS.icon_bounceContainBehavior;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(RS.bounceContainBehGUID); }
    }
  }
}