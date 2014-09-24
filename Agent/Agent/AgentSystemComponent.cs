using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class AgentSystemComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the AgentSystemComponent class.
    /// </summary>
    public AgentSystemComponent()
      : base("AgentSystemComponent", "System",
          "Represents a self-contained Agent System of Agents and Emitters.",
          "Agent", "Agent")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams
      (GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("Agents", "A", "Agents", 
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter("Emitters", "E", "Emitters",
                                    GH_ParamAccess.list);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams
      (GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Agent System", "S", "Agent System", 
                                   GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and 
    /// store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      List<AgentType> agents = new List<AgentType>();
      List<EmitterType> emitters = new List<EmitterType>();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this
      // method.
      if (!DA.GetDataList(0, agents)) return;
      if (!DA.GetDataList(1, emitters)) return;

      // We should now validate the data and warn the user if invalid data is 
      // supplied.
      if (agents.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "There must be at " + 
          "least 1 Agent.");
        return;
      }
      if (emitters.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "There must be at " +
          "least 1 Emitter.");
        return;
      }

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AgentSystemType system = new AgentSystemType(agents, emitters.ToArray());

      // Finally assign the spiral to the output parameter.
      DA.SetData(0, system);
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
      get { return new Guid("{3865e673-4756-465e-ae00-04d8af6d3811}"); }
    }
  }
}