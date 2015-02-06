using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class DebugDeconstructAgentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DebugDeconstructAgentComponent()
      : base("DebugDeconstructAgent", "DebugDeconstructAgent",
          "DebugDeconstructAgent",
          "Agent", "Agent")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("Agent", "A", "Agent", GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddPointParameter("Position", "P", "Position", GH_ParamAccess.item);
      pManager.AddVectorParameter("Velocity", "V", "Velocity", GH_ParamAccess.item);
      pManager.AddVectorParameter("Acceleration", "A", "Acceleration", GH_ParamAccess.item);
      pManager.AddIntegerParameter("Lifespan", "L", "Lifespan", GH_ParamAccess.item);
      pManager.AddPointParameter("Ref Position", "RP", "Ref Position", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      AgentType agent = new AgentType();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      // Finally assign the spiral to the output parameter.
      da.SetData(0, agent.Position);
      da.SetData(1, agent.Velocity);
      da.SetData(2, agent.Acceleration);
      da.SetData(3, agent.Lifespan);
      da.SetData(4, agent.RefPosition);
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
        return Properties.Resources.icon_deconstructAgent;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{74820305-a37d-4a7f-976a-aef3d8841fde}"); }
    }
  }
}