using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SurfaceFlowForceComponent : AbstractParticleForceComponent
  {
    private AbstractEnvironmentType environment;
    private double stepDistance, angle;
    public SurfaceFlowForceComponent()
      : base("Surface Flow Force", "SurfaceFlow",
          "Applies a force to simulate water flowing over the surface.",
          RS.icon_flowDownSurfaceForce, "a020520c-2da2-444e-a014-4a1bc0d844a5")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter("Environment", "En", "The Surface or Polysurface Environment to flow over.", GH_ParamAccess.item);
      pManager.AddNumberParameter("Step distance", "D", "The distance the particle will move each timestep. Smaller distances will lead to more accurate results.",
        GH_ParamAccess.item, 0.1);
      pManager.AddNumberParameter("Rotation Angle", "A", "The angle that the surface tangent vector will be multiplied by PI and rotated by. A 0 angle will cause the particle to flow across the surface whereas a 0.5 angle will cause the particle to flow down the surface.",
        GH_ParamAccess.item, 0.5);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref environment)) return false;
      if (!da.GetData(nextInputIndex++, ref stepDistance)) return false;
      if (!da.GetData(nextInputIndex++, ref angle)) return false;
      return true;
    }

    protected override Vector3d CalcForce()
    {
      Vector3d nrml = environment.ClosestNormal(particle.Position3D);
      Vector3d drainVec = Vector3d.CrossProduct(nrml, Vector3d.ZAxis);
      drainVec.Unitize();
      drainVec.Transform(Transform.Rotation(Math.PI * angle, nrml, particle.Position3D));
      drainVec = drainVec * stepDistance;
      if (environment.GetType() == typeof (SurfaceEnvironmentType))
      {
        Point3d pt2D = particle.Position3D;
        pt2D.Transform(Transform.Translation(drainVec));
        pt2D = environment.MapTo2D(pt2D);
        drainVec = Util.Vector.Vector2Point(particle.Position, pt2D);
      }
      return drainVec;
    }
  }
}
