using System;
using System.Collections.Generic;
using System.Drawing;
using RS = Agent.Properties.Resources;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace Agent
{
  public class EngineComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the Engine class.
    /// </summary>
    public EngineComponent()
      : base(RS.engineName, RS.engineNickName,
          RS.engineDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddBooleanParameter(RS.resetName, RS.resetNickName, RS.resetDescription, GH_ParamAccess.item, RS.resetDefault);
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.systemDescription, GH_ParamAccess.item);
      
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      Boolean reset = RS.resetDefault;
      AgentSystemType system = new AgentSystemType();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref reset)) return;
      if (!da.GetData(1, ref system)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      Run(reset, system);
    }

    private void Run(Boolean reset, AgentSystemType system)
    {
      if (reset)
      {
        system.Agents.Clear();
        Populate(system);
      }
      else
      {
        system.Run();
      }
    }

    private void Populate(AgentSystemType system)
    {
      foreach (AbstractEmitterType emitter in system.Emitters)
      {
        if (!emitter.ContinuousFlow)
        {
          for (int i = 0; i < emitter.NumAgents; i++)
          {
            system.AddAgent(emitter);
          }
        }
      }
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
        return RS.icon_engine;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(RS.engineGUID); }
    }
  }
}