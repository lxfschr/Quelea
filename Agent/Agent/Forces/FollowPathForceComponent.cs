using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class FollowPathForceComponent : GH_Component
  {
    protected readonly Bitmap icon;
    protected readonly Guid componentGuid;
    public FollowPathForceComponent()
      : base(RS.followPathForceName, RS.followPathForceComponentNickName,
          RS.followPathForceDescription,
          RS.pluginCategoryName, RS.forcesSubCategoryName)
    {
      this.icon = RS.icon_coheseForce;
      this.componentGuid = new Guid(RS.followPathForceGuid);
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentToAffect, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.weightMultiplierName, RS.weightMultiplierNickName, RS.weightMultiplierDescription,
        GH_ParamAccess.item, RS.weightMultiplierDefault);
      pManager.AddCurveParameter(RS.curveName, RS.curveNickName, RS.curveForFollowPathDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.pathRadiusName, RS.radiusNickName,
        RS.pathRadiusDescription, GH_ParamAccess.item, RS.pathRadiusDefault);
      pManager.AddNumberParameter(RS.predictionDistanceName, RS.predictionDistanceNickName, RS.predictionDistanceDescription,
        GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddNumberParameter(RS.pathTargetDistanceName, RS.pathTargetDistanceNickName,
        RS.pathTargetDistanceDescription,
        GH_ParamAccess.item, RS.visionRadiusDefault);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddVectorParameter(RS.followPathForceName, RS.forceNickName, RS.followPathForceDescription, GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess da)
    {
      AgentType agent = new AgentType();
      double weightMultiplier = RS.weightMultiplierDefault;
      Curve path = null;
      double radius = 5.0;
      double predictionDistance = RS.predictionDistanceDefault;
      double pathTargetDistance = RS.visionRadiusDefault;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;
      if (!da.GetData(1, ref weightMultiplier)) return;
      if (!da.GetData(2, ref path)) return;
      if (!da.GetData(3, ref radius)) return;
      if (!da.GetData(4, ref predictionDistance)) return;
      if (!da.GetData(5, ref pathTargetDistance)) return;


      Vector3d force = Run(agent, weightMultiplier, path, radius, predictionDistance, pathTargetDistance);

      // Finally assign the output parameter.
      da.SetData(0, force);
    }

    protected Vector3d Run(AgentType agent, double weightMultiplier, Curve path, double radius, double predictionDistance, double pathTargetDistance)
    {
      Vector3d force = CalcForce(agent, path, radius, predictionDistance, pathTargetDistance);
      Vector3d.Multiply(force, weightMultiplier);
      agent.ApplyForce(force);
      return force;
    }

    private Vector3d CalcForce(AgentType agent, Curve path, double radius, double predictionDistance, double pathTargetDistance)
    {
      Vector3d steer = new Vector3d();
      //Predict the vehicle's future location
      Vector3d predict = agent.Velocity;
      predict.Unitize();
      predict = Vector3d.Multiply(predict, predictionDistance);
      Point3d predictLoc = Point3d.Add(agent.Position, predict);

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

    public override Guid ComponentGuid
    {
      get { return this.componentGuid; }
    }
  }
}
