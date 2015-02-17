using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractEnvironmentalForceComponent : GH_Component
  {
    protected readonly Bitmap icon;
    protected readonly Guid componentGuid;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractEnvironmentalForceComponent(string name, string nickname, string description,
                              string category, string subCategory, Bitmap icon, String componentGuid)
      : base(name, nickname, description, category, subCategory)
    {
      this.icon = icon;
      this.componentGuid = new Guid(componentGuid);
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
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentToAffect, GH_ParamAccess.item);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, RS.environmentDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.weightMultiplierName, RS.weightMultiplierNickName, RS.weightMultiplierDescription,
        GH_ParamAccess.item, RS.weightMultiplierDefault);
      pManager.AddNumberParameter(RS.visionRadiusName, RS.visionRadiusNickName, RS.visionAngleDescription,
        GH_ParamAccess.item, RS.bodySizeDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected abstract override void RegisterOutputParams(GH_OutputParamManager pManager);

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      AgentType agent = new AgentType();
      AbstractEnvironmentType environment = new AxisAlignedBoxEnvironmentType();
      double weightMultiplier = RS.weightMultiplierDefault;
      double visionRadius = RS.bodySizeDefault;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;
      if (!da.GetData(1, ref environment)) return;
      if (!da.GetData(2, ref weightMultiplier)) return;
      if (!da.GetData(3, ref visionRadius)) return;

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      Vector3d force = Run(agent, environment, weightMultiplier, visionRadius);

      // Finally assign the output parameter.
      da.SetData(0, force);
    }

    protected Vector3d Run(AgentType agent, AbstractEnvironmentType environment, double weightMultiplier, double visionRadius)
    {
      Vector3d force = CalcForce(agent, environment, visionRadius);
      force = Vector3d.Multiply(force, weightMultiplier);
      agent.ApplyForce(force);
      return force;
    }

    protected abstract Vector3d CalcForce(AgentType agent, AbstractEnvironmentType environment, double visionRadius);

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return Resources.IconForThisComponent;
        return icon;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return componentGuid; }
    }
  }
}