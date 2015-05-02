using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class FollowPathForceComponent : AbstractAgentForceComponent
  {
    private Curve path;
    private double radius;
    private double predictionDistance;
    private double pathTargetDistance;
    private Point3d predictLoc, pathPt;
    public FollowPathForceComponent()
      : base(RS.followPathForceName, RS.followPathForceComponentNickname,
          RS.followPathForceDescription, RS.icon_FollowPathForce, RS.followPathForceGuid)
    {
      path = null;
      radius = 5.0;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddCurveParameter(RS.curveName, RS.curveNickname, RS.curveForFollowPathDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.pathRadiusName, RS.radiusNickname,
        RS.pathRadiusDescription, GH_ParamAccess.item, RS.pathRadiusDefault);
      pManager.AddNumberParameter(RS.predictionDistanceName, RS.predictionDistanceNickName, RS.predictionDistanceDescription,
        GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.pathTargetDistanceName, RS.pathTargetDistanceNickName,
        RS.pathTargetDistanceDescription,
        GH_ParamAccess.item);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      base.RegisterOutputParams(pManager);
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddPointParameter("Prediction Location", "PL",
                                   "The location the agent predicts to be its future location.", GH_ParamAccess.item);
      pManager.AddPointParameter("Path Point", "PP",
                                   "The point on the path that the agent will steer towards.", GH_ParamAccess.item);
      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref path)) return false;
      if (!da.GetData(nextInputIndex++, ref radius)) return false;
      if (!da.GetData(nextInputIndex++, ref predictionDistance)) predictionDistance = agent.VisionRadius/5;
      if (!da.GetData(nextInputIndex++, ref pathTargetDistance)) pathTargetDistance = agent.VisionRadius/5;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      base.SetOutputs(da);
      da.SetData(nextOutputIndex++, predictLoc);
      da.SetData(nextOutputIndex++, pathPt);
    }

    protected override Vector3d CalculateDesiredVelocity()
    {
      Vector3d desired = new Vector3d();
      //Predict the vehicle's future location
      Vector3d predict = agent.Velocity3D;
      predict.Unitize();
      predict = predict * predictionDistance;
      predictLoc = agent.Position3D + predict;

      //Find the normal point along the path
      double t;
      path.ClosestPoint(predictLoc, out t);
      pathPt = path.PointAt(t);

      //Move a little further along the path and set a target


      //If we are off the path, seek that target in order to stay on the path
      double distance = pathPt.DistanceTo(predictLoc);
      if (distance > radius)
      {
        pathPt = path.PointAt(t + pathTargetDistance);
        if(agent.Environment.GetType() == typeof(SurfaceEnvironmentType))
        {
          pathPt = agent.Environment.MapTo2D(pathPt);
        }
        // Seek that point
        desired = Util.Agent.Seek(agent, pathPt);
      }
      return desired;
    }
  }
}
