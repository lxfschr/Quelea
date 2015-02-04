using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent.Agent2
{
  public abstract class BoidForceComponent : GH_Component
  {
    protected double visionRadiusMultiplier;
    /// <summary>
    /// Initializes a new instance of the CoheseForceComponent class.
    /// </summary>
    public BoidForceComponent(string name, string nickname, string description, 
                              string category, string subCategory)
      : base(name, nickname, description, category, subCategory)
    {
      this.visionRadiusMultiplier = 1.0;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter("Agent", "A", "The Agent to affect.", GH_ParamAccess.item);
      pManager.AddGenericParameter("Neighbors", "AC", "The neighbors to react to.", GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected abstract override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager);

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      AgentType agent = new AgentType();
      SpatialCollectionType neighbors = new SpatialCollectionType();
      double visionAngle = Constants.VisionAngle;
      double visionRadiusMultiplier = this.visionRadiusMultiplier;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref agent)) return;
      if (!DA.GetData(1, ref neighbors)) return;

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      Vector3d force = run(agent, neighbors);

      // Finally assign the output parameter.
      DA.SetData(0, force);
    }

    protected Vector3d run(AgentType agent, SpatialCollectionType neighbors)
    {
      Vector3d force = calcForce(agent, (List<AgentType>)neighbors.Agents.SpatialObjects);
      agent.applyForce(force);
      return force;
    }

    protected abstract Vector3d calcForce(AgentType agent, List<AgentType> neighbors);
  }
}