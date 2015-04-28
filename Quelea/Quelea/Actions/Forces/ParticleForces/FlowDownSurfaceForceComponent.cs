using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class FlowDownSurfaceForceComponent : AbstractParticleForceComponent
  {
    private AbstractEnvironmentType environment;
    private double stepDistance;
    public FlowDownSurfaceForceComponent()
      : base("Flow Down Surface Force", "SurfaceFlow",
          "Applies a force to simulate water flowing over the surface.",
          RS.icon_flowDownSurfaceForce, "a020520c-2da2-444e-a014-4a1bc0d844a5")
    {
      stepDistance = 
        1;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter("Environment", "En", "The Surface or Polysurface Environment to flow over.", GH_ParamAccess.item);
      pManager.AddNumberParameter("Step distance", "D", "The distance the particle will move each timestep. Smaller distances will lead to more accurate results.",
        GH_ParamAccess.item, 0.1);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref environment)) return false;
      if (!da.GetData(nextInputIndex++, ref stepDistance)) return false;
      return true;
    }

    protected override Vector3d CalcForce()
    {
      Vector3d nrml = environment.ClosestNormal(particle.Position3D);
      Vector3d drainVec = Vector3d.CrossProduct(nrml, Vector3d.ZAxis);
      drainVec.Unitize();
      drainVec.Transform(Transform.Rotation(RS.HALF_PI, nrml, particle.Position3D));
      drainVec = drainVec * stepDistance;
      return drainVec;
    }
  }
}
