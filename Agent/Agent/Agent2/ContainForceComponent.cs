using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent.Agent2
{
  public class ContainForceComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the ContainForceComponent class.
    /// </summary>
    public ContainForceComponent()
      : base("ContainForce", "Contain",
          "Contain Force",
          "Agent", "Agent2")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("System", "S", "The System to affect.", GH_ParamAccess.item);
      pManager.AddGenericParameter("Environment", "En", "The Environment to react to.", GH_ParamAccess.item);
      pManager.AddNumberParameter("Vision Angle", "A", "The angle around which the Agent will see other Agents.", GH_ParamAccess.item, Constants.VisionAngle);
      pManager.AddNumberParameter("Vision Radius Mutliplier", "R", "The radius around which the Agent will see other Agents.", GH_ParamAccess.item, Constants.VisionRadiusMultiplier);

      pManager[2].Optional = true;
      pManager[3].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Contain Force", "F", "Contain Force", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      AgentSystemType system1 = new AgentSystemType();
      EnvironmentType environment = new AxisAlignedBoxEnvironmentType();
      double visionAngle = Constants.VisionAngle;
      double visionRadiusMultiplier = Constants.VisionRadiusMultiplier;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref system1)) return;
      if (!DA.GetData(1, ref environment)) return;
      DA.GetData(2, ref visionAngle);
      DA.GetData(3, ref visionRadiusMultiplier);

      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(0.0 <= visionAngle && visionAngle <= 360.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Vision Angle must be between 0 and 360.");
        return;
      }
      if (!(0.0 <= visionRadiusMultiplier && visionRadiusMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Vision Radius must be between 0 and 1.");
        return;
      }


      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      List<Vector3d> forces = run(system1, environment, visionAngle, visionRadiusMultiplier);

      // Finally assign the output parameter.
      DA.SetDataList(0, forces);
    }

    private List<Vector3d> run(AgentSystemType system1, EnvironmentType environment,
                               double visionAngle, double visionRadiusMultiplier)
    {
      List<Vector3d> forces = new List<Vector3d>();
      foreach (AgentType agent in system1.Agents)
      {
        forces.Add(calcForce(agent, environment, visionRadiusMultiplier));
      }

      return forces;
    }

    private Vector3d calcForce(AgentType agent, EnvironmentType environment, 
                               double visionRadiusMultiplier)
    {
      Vector3d steer = new Vector3d();
      if (environment != null)
      {
        steer = environment.avoidEdges(agent, agent.VisionRadius * visionRadiusMultiplier);
        if (!steer.IsZero)
        {
          steer.Unitize();
          steer = Vector3d.Multiply(steer, agent.MaxSpeed);
          steer = Vector3d.Subtract(steer, agent.Velocity);
          steer = Util.Vector.limit(steer, agent.MaxForce);
        }
      }
      return steer;
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
      get { return new Guid("{5a6a85be-31d7-47de-bc3b-99a0caf5bcff}"); }
    }
  }
}