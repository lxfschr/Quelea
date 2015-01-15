using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace Agent
{
  public class EngineComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the Engine class.
    /// </summary>
    public EngineComponent()
      : base("Engine", "Engine",
          "Engine that runs the simulation.",
          "Agent", "Agent")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddBooleanParameter("Reset", "R", "Reset the scene?", GH_ParamAccess.item, true);
      pManager.AddGenericParameter("Systems", "S", "Systems in scene.", GH_ParamAccess.list);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Agents", "A", "Agents", GH_ParamAccess.tree);
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
      List<AgentSystemType> systems = new List<AgentSystemType>();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref reset)) return;
      if (!DA.GetDataList(1, systems)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      DataTree<AgentType> agents = run(reset, systems);
      //List<Point3d> agents = new List<Point3d>();

      // Finally assign the spiral to the output parameter.
      DA.SetDataTree(0, agents);
    }

    // Declare systems outide loop so they do not reset each time.
    //List<AgentSystemType> agentSystems = new List<AgentSystemType>();
    private DataTree<AgentType> run(Boolean reset, List<AgentSystemType> systems)
    {
      int index = 0;
      if (reset)
      {
        foreach (AgentSystemType system in systems)
        {
          system.Agents.Clear();
        }
        foreach (AgentSystemType system in systems)
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
          index++;
        }


      }
      else
      {
        foreach (AgentSystemType system in systems)
        {
          system.run();
        }
      }

      DataTree<AgentType> tree = new DataTree<AgentType>();
      int counter = 0;
      foreach (AgentSystemType system in systems)
      {
        foreach (AgentType agent in system.Agents)
        {
          tree.Add(agent, new GH_Path(counter));
        }
        counter++;
      }

      return tree;
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
      get { return new Guid("{95e8bf8a-8551-4fea-8561-0d2b0225029e}"); }
    }
  }
}