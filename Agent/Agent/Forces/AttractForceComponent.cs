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

    protected override Vector3d CalcForce(AgentType agent, Point3d pt, double weightMultiplier, double radius)
    {
      Vector3d dir = Vector3d.Subtract(new Vector3d(agent.RefPosition), new Vector3d(pt));
      double d = dir.Length;
      if (d > radius && radius >= 0)
      {
        return new Vector3d();
      }
      d = Util.Number.Clamp(d, 5, 100);
      dir.Unitize();
      double force = weightMultiplier/(d*d);
      dir = Vector3d.Multiply(dir, force);
      dir = Util.Vector.Limit(dir, agent.MaxForce);
      return dir;
    }
  }
}
