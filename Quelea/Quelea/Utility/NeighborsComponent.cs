using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class NeighborsComponent : AbstractComponent
  {
    private IAgent agent;
    private SpatialCollectionType agentCollection;
    private double visionRadiusMultiplier;
    private double visionAngleMultiplier;
    /// <summary>
    /// Initializes a new instance of the NeighborsComponent class.
    /// </summary>
    public NeighborsComponent()
      : base(RS.getNeighborsInRadiusName, RS.getNeighborsInRadiusComponentNickname,
          RS.getNeighborsInRadiusDescription,
          RS.pluginCategoryName, RS.utilitySubcategoryName, RS.icon_neighborsInRadius, RS.neighborsGuid)
    {
      agent = null;
      agentCollection = null;
      visionRadiusMultiplier = RS.visionRadiusMultiplierDefault;
      visionAngleMultiplier = RS.visionAngleMultiplierDefault;
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
      pManager.AddGenericParameter(RS.agentName + " " + RS.queleaName, RS.agentNickname + RS.queleaNickname, RS.agentToGetNeighborsFor, GH_ParamAccess.item);
      pManager.AddGenericParameter(RS.queleaNetworkName, RS.queleaNetworkNickname, RS.queleaNetworkToSearch, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.visionRadiusName + " " + RS.multiplierName, RS.visionRadiusNickname + RS.multiplierNickname, RS.visionRadiusMultiplierDescription, GH_ParamAccess.item, RS.visionRadiusMultiplierDefault);
      pManager.AddNumberParameter(RS.visionAngleName + " " + RS.multiplierName, RS.visionAngleNickname + RS.multiplierNickname, "The factor by which the Agent's Vision Angle will be multiplied. The result will be used to determine the angle from the velocity that the agent will be able to see neighbors.", GH_ParamAccess.item, RS.visionAngleMultiplierDefault);
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
      pManager.AddGenericParameter(RS.queleaNetworkName, RS.queleaNetworkNickname, RS.neighborsDescription, GH_ParamAccess.item);

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
      da.GetData(nextInputIndex++, ref visionRadiusMultiplier);
      da.GetData(nextInputIndex++, ref visionAngleMultiplier);
      
      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(0.0 <= visionRadiusMultiplier && visionRadiusMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionRadiusMultiplierErrorMessage);
        return false;
      }
      if (!(0.0 <= visionAngleMultiplier && visionAngleMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleMultiplierErrorMessage);
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
      ISpatialCollection<IQuelea> neighborsInSphere = agentCollection.Quelea.GetNeighborsInSphere(agent, agent.VisionRadius*visionRadiusMultiplier);

      if (Util.Number.ApproximatelyEqual(visionAngleMultiplier, 1.0, Constants.AbsoluteTolerance))
      {
        return new SpatialCollectionType(neighborsInSphere);
      }

      ISpatialCollection<IQuelea> neighbors = new SpatialCollectionAsList<IQuelea>();

      Point3d position = agent.Position;
      Vector3d velocity = agent.Velocity;
      Plane pl1 = new Plane(position, velocity);
      pl1.Rotate(-RS.HALF_PI, pl1.YAxis);
      Plane pl2 = pl1;
      pl2.Rotate(-RS.HALF_PI, pl1.XAxis);
      double halfVisionAngle = agent.VisionAngle*visionAngleMultiplier/2;
      foreach (IQuelea neighbor in neighborsInSphere)
      {
        Vector3d diff = Util.Vector.Vector2Point(position, neighbor.Position);
        double angle1 = Util.Vector.CalcAngle(velocity, diff, pl1);
        double angle2 = Util.Vector.CalcAngle(velocity, diff, pl2);
        if (Util.Number.DefinitelyLessThan(angle1, halfVisionAngle, Constants.AbsoluteTolerance) &&
            Util.Number.DefinitelyLessThan(angle2, halfVisionAngle, Constants.AbsoluteTolerance))
        {
          neighbors.Add(neighbor);
        }
      }

      return new SpatialCollectionType(neighbors);
    }
  }
}