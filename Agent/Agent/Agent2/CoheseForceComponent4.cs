using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent.Agent2
{
  public class CoheseForceComponent4 : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the CoheseForceComponent class.
    /// </summary>
    public CoheseForceComponent4()
      : base("Cohese Force", "Cohese4",
          "Cohesion",
          "Agent", "Agent2")
    {
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
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Cohesion Force", "F", "Cohesion Force", GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

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
      double visionRadiusMultiplier = Constants.VisionRadiusMultiplier;

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

    private Vector3d run(AgentType agent, SpatialCollectionType neighbors)
    {
      Vector3d force = calcForce(agent, (List<AgentType>) neighbors.Agents.SpatialObjects);
      agent.applyForce(force);
      return force;
    }

    private Vector3d calcForce(AgentType agent, List<AgentType> neighbors)
    {
      Vector3d sum = new Vector3d();
      int count = 0;

      foreach (AgentType neighbor in neighbors)
      {
        //Adding up all the others' location
        sum = Vector3d.Add(sum, new Vector3d(neighbor.RefPosition));
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        //We desire to go in that direction at maximum speed.
        sum = Vector3d.Divide(sum, count);
        sum = Util.Agent.seek(agent, sum);
      }
      //Seek the average location of our neighbors.
      return sum;
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
        return Properties.Resources.icon_coheseForce;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{c8fd8532-661e-47f1-99c1-b8552bb83b73}"); }
    }
  }
}