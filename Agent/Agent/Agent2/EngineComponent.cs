using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace Agent.Agent2
{
  public class EngineComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the Engine class.
    /// </summary>
    public EngineComponent()
      : base("Engine", "Engine",
          "Engine that runs the simulation.",
          "Agent", "Agent2")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddBooleanParameter("Reset", "R", "Reset the scene?", GH_ParamAccess.item, true);
      pManager.AddGenericParameter("System", "S", "System in scene.", GH_ParamAccess.item);
      pManager.AddVectorParameter("Forces", "F", "Forces in scene.", GH_ParamAccess.list);
      pManager.AddBooleanParameter("Behaviors", "B", "Behaviors in scene.", GH_ParamAccess.list);
      pManager[2].Optional = true;
      pManager[3].Optional = true;
      
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      Boolean reset = true;
      AgentSystemType system = new AgentSystemType();
      List<Vector3d> forces = new List<Vector3d>();
      List<bool> behaviors = new List<bool>();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref reset)) return;
      if (!DA.GetData(1, ref system)) return;
      DA.GetDataList(2, forces);
      DA.GetDataList(3, behaviors);

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      run(reset, system, forces, behaviors);
    }

    private void run(Boolean reset, AgentSystemType system,
                                    List<Vector3d> forces,
                                    List<bool> behaviors)
    {
      if (reset)
      {
        system.Agents.Clear();
        forces.Clear();
        behaviors.Clear();
        populate(system);
      }
      else
      {
        system.run(forces, behaviors);
      }

      return;
    }

    private void populate(AgentSystemType system)
    {
      foreach (EmitterType emitter in system.Emitters)
      {
        if (!emitter.ContinuousFlow)
        {
          for (int i = 0; i < emitter.NumAgents; i++)
          {
            system.addAgent(emitter);
          }
        }
      }
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
        return Properties.Resources.icon_engine;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{b0f076cb-8558-46a1-bf94-20a97a1b4d08}"); }
    }
  }
}