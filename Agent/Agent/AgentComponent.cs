using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class AgentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the AgentComponent class.
    /// </summary>
    public AgentComponent()
      : base("Agent", "Agent",
          "Defines parameters for an Agent",
          "Agent", "Agent")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddIntegerParameter("Lifespan", "L", "Timesteps the agent will be alive for.", GH_ParamAccess.item, 30);
      pManager.AddNumberParameter("Mass", "M", "The mass of the agent.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Body Size", "B", "The diameter of the extent of the agent's size.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Maximum Speed", "S", "The maximum speed of the agent.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Maximum Force", "F", "The maximum force of the agent.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Vision Angle", "A", "The maximum angle, taken from the velocity vector,  that the agent can see around it.", GH_ParamAccess.item, 15.0);
      pManager.AddNumberParameter("Vision Radius", "R", "The maximum radius around the agent that it can see.", GH_ParamAccess.item, 5.0);
      pManager.AddIntegerParameter("Length of location history", "N", "The length of location history", GH_ParamAccess.item, 1);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Agent", "A", "Agent", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      int lifespan = 0;
      double mass = 1.0;
      double bodySize = 1.0;
      double maxSpeed = 1.0;
      double maxForce = 1.0;
      double visionAngle = 15.0;
      double visionRadius = 5.0;
      int historyLength = 0;


      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref lifespan)) return;
      if (!DA.GetData(1, ref mass)) return;
      if (!DA.GetData(2, ref bodySize)) return;
      if (!DA.GetData(3, ref maxSpeed)) return;
      if (!DA.GetData(4, ref maxForce)) return;
      if (!DA.GetData(5, ref visionAngle)) return;
      if (!DA.GetData(6, ref visionRadius)) return;
      if (!DA.GetData(7, ref historyLength)) return;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (lifespan <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Lifespan must be greater than 0.");
        return;
      }
      if (mass <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mass must be greater than 0.");
        return;
      }
      if (bodySize < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Body Size must be greater than or equal to 0.");
        return;
      }
      if (maxSpeed < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Maximum Speed must be greater than or equal to 0.");
        return;
      }
      if (maxForce < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Maximum Force must be greater than or equal to 0.");
        return;
      }
      if (visionAngle < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Vision Angle must be greater than or equal to 0.");
        return;
      }
      if (visionRadius < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Vision Radius must be greater than or equal to 0.");
        return;
      }


      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AgentType agent = new AgentType(lifespan, mass, bodySize, maxSpeed,
                                      maxForce, visionAngle, visionRadius
                                      , historyLength);

      // Finally assign the spiral to the output parameter.
      DA.SetData(0, agent);
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
        return Properties.Resources.icon_agent;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{beb5cf41-ab3c-4bbf-b8d0-0cbe63c67b14}"); }
    }
  }
}