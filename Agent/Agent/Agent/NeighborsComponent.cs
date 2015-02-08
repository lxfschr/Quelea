using System;
using System.Drawing;
using RS = Agent.Properties.Resources;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
  public class NeighborsComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the NeighborsComponent class.
    /// </summary>
    public NeighborsComponent()
      : base(RS.getNeighborsInRadiusName, RS.getNeighborsInRadiusNickName,
          RS.getNeighborsInRadiusDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName)
    {
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
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentToGetNeighborsFor, GH_ParamAccess.item);
      pManager.AddGenericParameter(RS.agentCollectionName, RS.agentCollectionNickName, RS.agentCollectionToSearch, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.visionRadiusName, RS.visionRadiusNickName, RS.visionRadiusDescription, GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddNumberParameter(RS.visionAngleName, RS.visionAngleNickName, RS.visionAngleDescription, GH_ParamAccess.item, RS.visionAngleDefault);
      // If you want to change properties of certain parameters, 
      // you can use the pManager instance to access them by index:
      pManager[2].Optional = true;
      pManager[3].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter(RS.neighborsName, RS.agentCollectionNickName, RS.neighborsDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
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
      SpatialCollectionType agentCollection = new SpatialCollectionType();
      double visionRadius = RS.visionRadiusDefault;
      double visionAngle = RS.visionAngleDefault;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;
      if (!da.GetData(1, ref agentCollection)) return;
      da.GetData(2, ref visionRadius);
      da.GetData(3, ref visionAngle);


      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(0.0 <= visionRadius))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionRadiusErrorMessage);
        return;
      }
      if (!(0.0 <= visionAngle && visionAngle <= 360.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleErrorMessage);
        return;
      }

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      SpatialCollectionType neighbors = Run(agent, agentCollection, visionRadius, visionAngle);

      // Finally assign the output parameter.
      da.SetData(0, neighbors);
    }

    private SpatialCollectionType Run(AgentType agent, SpatialCollectionType agentCollection,
                               double visionRadius, double visionAngle)
    {
      ISpatialCollection<AgentType> neighborsInSphere = agentCollection.Agents.GetNeighborsInSphere(agent, visionRadius);

      if (Util.Number.ApproximatelyEqual(visionAngle, 360, RS.toleranceDefault))
      {
        return new SpatialCollectionType(neighborsInSphere);
      }

      ISpatialCollection<AgentType> neighbors = new SpatialCollectionAsList<AgentType>();

      Point3d position = agent.RefPosition;
      Vector3d velocity = agent.Velocity;
      Plane pl1 = new Plane(position, velocity);
      pl1.Rotate(-Math.PI / 2, pl1.YAxis);
      Plane pl2 = pl1;
      pl2.Rotate(-Math.PI / 2, pl1.XAxis);
      foreach (AgentType neighbor in neighborsInSphere)
      {
        Vector3d diff = Vector3d.Subtract(new Vector3d(neighbor.RefPosition), new Vector3d(position));
        double angle1 = Util.Vector.CalcAngle(velocity, diff, pl1);
        double angle2 = Util.Vector.CalcAngle(velocity, diff, pl2);
        if (Util.Number.ApproximatelyEqual(angle1, visionAngle / 2, RS.toleranceDefault) && 
            Util.Number.ApproximatelyEqual(angle2, visionAngle / 2, RS.toleranceDefault))
        {
          neighbors.Add(neighbor);
        }
      }

      return new SpatialCollectionType(neighbors);
    }

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return Resources.IconForThisComponent;
        return RS.icon_coheseForce;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(RS.neighborsGUID); }
    }
  }
}