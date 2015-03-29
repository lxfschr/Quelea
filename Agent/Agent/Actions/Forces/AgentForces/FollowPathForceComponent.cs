using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class FollowPathForceComponent : AbstractAgentForceComponent
  {
    private Curve path;
    private double radius;
    private double predictionDistance;
    private double pathTargetDistance;
    public FollowPathForceComponent()
      : base(RS.followPathForceName, RS.followPathForceComponentNickname,
          RS.followPathForceDescription, RS.forcesSubcategoryName, 
          RS.icon_FollowPathForce, RS.followPathForceGuid)
    {
      path = null;
      radius = 5.0;
      predictionDistance = RS.predictionDistanceDefault;
      pathTargetDistance = RS.visionRadiusDefault;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddCurveParameter(RS.curveName, RS.curveNickname, RS.curveForFollowPathDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.pathRadiusName, RS.radiusNickname,
        RS.pathRadiusDescription, GH_ParamAccess.item, RS.pathRadiusDefault);
      pManager.AddNumberParameter(RS.predictionDistanceName, RS.predictionDistanceNickName, RS.predictionDistanceDescription,
        GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddNumberParameter(RS.pathTargetDistanceName, RS.pathTargetDistanceNickName,
        RS.pathTargetDistanceDescription,
        GH_ParamAccess.item, RS.visionRadiusDefault);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref path)) return false;
      if (!da.GetData(nextInputIndex++, ref radius)) return false;
      if (!da.GetData(nextInputIndex++, ref predictionDistance)) return false;
      if (!da.GetData(nextInputIndex++, ref pathTargetDistance)) return false;
      return true;
    }

    protected override Vector3d CalcForce()
    {
      Vector3d steer = new Vector3d();
      //Predict the vehicle's future location
      Vector3d predict = agent.Velocity;
      predict.Unitize();
      predict = Vector3d.Multiply(predict, predictionDistance);
      Point3d predictLoc = Point3d.Add(agent.RefPosition, predict);

      //Find the normal point along the path
      double t;
      path.ClosestPoint(new Point3d(predictLoc), out t);
      Point3d normal = path.PointAt(t);

      //Move a little further along the path and set a target


      //If we are off the path, seek that target in order to stay on the path
      double distance = normal.DistanceTo(new Point3d(predictLoc));
      if (distance > radius)
      {
        Vector3d offset = new Vector3d(path.PointAt(t + pathTargetDistance));
        steer = Util.Agent.Seek(agent, offset);
      }
      return steer;
    }
  }
}
