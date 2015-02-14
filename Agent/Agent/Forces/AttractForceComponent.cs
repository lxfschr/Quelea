using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AttractForceComponent : AbstractAttractionForceComponent
  {
    public AttractForceComponent()
      : base(RS.attractForceName, RS.attractForceComponentNickName,
          RS.attractForceDescription,
          RS.pluginCategoryName, RS.forcesSubCategoryName, RS.icon_coheseForce, RS.attractForceGuid)
    {
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddVectorParameter(RS.attractForceName, RS.forceNickName, RS.attractForceName, GH_ParamAccess.item);
    }

    protected override Vector3d CalcForce(AgentType agent, Point3d pt, 
                                          double weightMultiplier, 
                                          double mass, double radius)
    {
      Vector3d force = Vector3d.Subtract(new Vector3d(pt), new Vector3d(agent.RefPosition));
      double distance = force.Length;
      // If the distance is greater than the radius of the attractor,
      // and the radius is positive, do not apply the force.
      // Negative radius causes all Agents to be affected, regardless of distance.
      if (distance > radius && radius >= 0)
      {
        return new Vector3d();
      }
      // Clamp the distance so the force lies within a reasonable value.
      distance = Util.Number.Clamp(distance, 5, 100);
      force.Unitize();
      // Divide by distance squared so the farther away the Attractor is,
      // the weaker the force.
      double strength = (weightMultiplier * mass * agent.Mass) / (distance * distance);
      force = Vector3d.Multiply(force, strength);
      force = Util.Vector.Limit(force, agent.MaxForce);
      return force;
    }
  }
}
