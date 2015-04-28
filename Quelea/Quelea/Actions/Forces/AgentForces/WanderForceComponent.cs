using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class WanderForceComponent : AbstractAgentForceComponent
  {
    private double strength;
    private double rate;
    private Sphere sphere;
    private Point3d targetPt;
    public WanderForceComponent()
      : base("Wander Force", "Wander",
          "Applies a force that causes the agent to randomly steer in a direction that is based off of its previous direction. This produces a seemingly realistic wandering behavior, rather than steering in a completely random direction.",
          RS.icon_wanderForce, "455c0640-0702-4819-9779-c28deea4d551")
    {
      strength = 1.0;
      rate = 0.5;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter("Strength", "S", "Strength affects the size of the search space that the Agent be able to steer towards. Higher strengths will lead to wider range of movement.", GH_ParamAccess.item, 1.0);
      pManager.AddNumberParameter("Rate", "R", "Rate affects the range of distance from its previous wander seek point along the search space that the Agent will choose its next point to seek. Higher rates will lead to greater change in direction from one timestep to the next.", GH_ParamAccess.item, 0.5);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      base.RegisterOutputParams(pManager);
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGeometryParameter("Sphere", "S", "The resulting wander sphere for debugging purposes.", GH_ParamAccess.item);
      pManager.AddPointParameter("Target Point", "P",
        "The target point that the agent will seek for debugging purposes.", GH_ParamAccess.item);
      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      pManager.HideParameter(pManager.ParamCount - 2);
      pManager.HideParameter(pManager.ParamCount - 1);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref strength)) return false;
      if (!da.GetData(nextInputIndex++, ref rate)) return false;
      if (strength < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Strength must be greater than 0.");
        return false;
      }
      if (!(0.0 <= rate && rate <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Rate must be between 0.0 and 1.0.");
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      base.SetOutputs(da);
      da.SetData(nextOutputIndex++, sphere);
      da.SetData(nextOutputIndex++, targetPt);
    }

    protected override Vector3d CalcForce()
    {
      Vector3d offsetVec = agent.Velocity;
      offsetVec.Unitize();
      offsetVec = Vector3d.Multiply(offsetVec, strength + RS.SQUARE_ROOT_OF_TWO * agent.MaxSpeed);
      Transform xform = Transform.Translation(offsetVec);
      Point3d pt = agent.Position3D;
      pt.Transform(xform);
      sphere = new Sphere(pt, strength);
      agent.Lon += Util.Random.RandomDouble(-rate * RS.TWO_PI, rate * RS.TWO_PI);
      agent.Lat += Util.Random.RandomDouble(-rate * Math.PI, rate * Math.PI);
      //if (agent.Lon < 0) agent.Lon = Math.PI - agent.Lon;
      //if (agent.Lon > 2*Math.PI) agent.Lon = agent.Lon - Math.PI;
      //if (agent.Lat < -Math.PI/2) agent.Lon = Math.PI/2 - agent.Lat;
      //if (agent.Lat > Math.PI / 2) agent.Lon = agent.Lat - Math.PI/2;
      //agent.Lon = Util.Number.Clamp(agent.Lon, 0, 2 * Math.PI);
      //agent.Lat = Util.Number.Clamp(agent.Lat, -Math.PI / 2, Math.PI / 2);
      targetPt = sphere.PointAt(agent.Lon, agent.Lat);
      targetPt = agent.Environment.MapTo2D(targetPt);
      Vector3d desired = Util.Agent.Seek(agent, targetPt);
      return desired;
    }
  }
}
