using System;
using System.Collections.Generic;
using Agent.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ContainForceComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the ContainForceComponent class.
    /// </summary>
    public ContainForceComponent()
      : base(RS.containForceName, RS.containForceNickName,
          RS.containForceDescription,
          RS.pluginCategoryName, RS.environmentForcesSubCategory)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.agentToAffect, GH_ParamAccess.item);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, Resources.environmentToReactTo, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.visionAngleName, RS.visionAngleNickName, RS.visionAngleDescription, GH_ParamAccess.item, RS.visionAngleDefault);
      pManager.AddNumberParameter(Resources.visionRadiusMultiplierName, Resources.visionRadiusMultiplierNickName, Resources.visionRadiusMultiplierDescription, GH_ParamAccess.item, Resources.visionRadiusMultiplierDefault);

      pManager[2].Optional = true;
      pManager[3].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(Resources.containForceName, RS.forceNickName, Resources.containForceDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      AgentSystemType system1 = new AgentSystemType();
      EnvironmentType environment = new AxisAlignedBoxEnvironmentType();
      double visionAngle = RS.visionAngleDefault;
      double visionRadiusMultiplier = RS.visionRadiusMultiplierDefault;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref system1)) return;
      if (!da.GetData(1, ref environment)) return;
      da.GetData(2, ref visionAngle);
      da.GetData(3, ref visionRadiusMultiplier);

      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(0.0 <= visionAngle && visionAngle <= 360.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleErrorMessage);
        return;
      }
      if (!(0.0 <= visionRadiusMultiplier && visionRadiusMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, Resources.visionRadiusMultiplierErrorMessage);
        return;
      }


      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      List<Vector3d> forces = Run(system1, environment, visionAngle, visionRadiusMultiplier);

      // Finally assign the output parameter.
      da.SetDataList(0, forces);
    }

    private List<Vector3d> Run(AgentSystemType system1, EnvironmentType environment,
                               double visionAngle, double visionRadiusMultiplier)
    {
      List<Vector3d> forces = new List<Vector3d>();
      foreach (AgentType agent in system1.Agents)
      {
        forces.Add(CalcForce(agent, environment, visionRadiusMultiplier));
      }

      return forces;
    }

    private Vector3d CalcForce(AgentType agent, EnvironmentType environment, 
                               double visionRadiusMultiplier)
    {
      Vector3d steer = new Vector3d();
      if (environment != null)
      {
        steer = environment.AvoidEdges(agent, agent.VisionRadius * visionRadiusMultiplier);
        if (!steer.IsZero)
        {
          steer.Unitize();
          steer = Vector3d.Multiply(steer, agent.MaxSpeed);
          steer = Vector3d.Subtract(steer, agent.Velocity);
          steer = Util.Vector.Limit(steer, agent.MaxForce);
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
      get { return new Guid(Resources.containForceGUID); }
    }
  }
}