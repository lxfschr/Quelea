using System;
using Agent.Util;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class NeighborsComponent : AbstractComponent
  {
    private IAgent agent;
    private SpatialCollectionType agentCollection;
    private double visionRadius;
    private double visionAngle;
    /// <summary>
    /// Initializes a new instance of the NeighborsComponent class.
    /// </summary>
    public NeighborsComponent()
      : base(RS.getNeighborsInRadiusName, RS.getNeighborsInRadiusComponentNickName,
          RS.getNeighborsInRadiusDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName, RS.icon_neighborsInRadius, RS.neighborsGUID)
    {
      agent = null;
      agentCollection = null;
      visionRadius = RS.visionRadiusDefault;
      visionAngle = RS.visionAngleDefault;
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
      pManager.AddGenericParameter(RS.getNeighborsInRadiusComponentNickName, RS.agentCollectionNickName, RS.neighborsDescription, GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      if (!da.GetData(nextInputIndex++, ref agentCollection)) return false;
      da.GetData(nextInputIndex++, ref visionRadius);
      da.GetData(nextInputIndex++, ref visionAngle);
      
      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(0.0 <= visionRadius))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionRadiusErrorMessage);
        return false;
      }
      if (!(0.0 <= visionAngle && visionAngle <= 360.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleErrorMessage);
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      SpatialCollectionType neighbors = Run();
      da.SetData(nextOutputIndex++, neighbors);
    }

    private SpatialCollectionType Run()
    {
      ISpatialCollection<IParticle> neighborsInSphere = agentCollection.Particles.GetNeighborsInSphere(agent, visionRadius);

      if (Number.ApproximatelyEqual(visionAngle, 360, RS.toleranceDefault))
      {
        return new SpatialCollectionType(neighborsInSphere);
      }

      ISpatialCollection<IParticle> neighbors = new SpatialCollectionAsList<IParticle>();

      Point3d position = agent.RefPosition;
      Vector3d velocity = agent.Velocity;
      Plane pl1 = new Plane(position, velocity);
      pl1.Rotate(-Math.PI / 2, pl1.YAxis);
      Plane pl2 = pl1;
      pl2.Rotate(-Math.PI / 2, pl1.XAxis);
      foreach (IParticle neighbor in neighborsInSphere)
      {
        Vector3d diff = Vector3d.Subtract(new Vector3d(neighbor.RefPosition), new Vector3d(position));
        double angle1 = Vector.CalcAngle(velocity, diff, pl1);
        double angle2 = Vector.CalcAngle(velocity, diff, pl2);
        if (Number.ApproximatelyEqual(angle1, visionAngle / 2, RS.toleranceDefault) &&
            Number.ApproximatelyEqual(angle2, visionAngle / 2, RS.toleranceDefault))
        {
          neighbors.Add(neighbor);
        }
      }

      return new SpatialCollectionType(neighbors);
    }
  }
}