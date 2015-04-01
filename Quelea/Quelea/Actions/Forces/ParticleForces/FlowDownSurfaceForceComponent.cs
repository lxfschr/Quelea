using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class FlowDownSurfaceForceComponent : AbstractParticleForceComponent
  {
    private Surface srf;
    private Plane frame;
    private double stepDistance;
    public FlowDownSurfaceForceComponent()
      : base("Flow Down Surface Force", "SurfaceFlow",
          "Applies a force to simulate water flowing over the surface.",
          null, "a020520c-2da2-444e-a014-4a1bc0d844a5")
    {
      srf = null;
      stepDistance = RS.predictionDistanceDefault;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter("Surface", "S", "The surface to flow over.", GH_ParamAccess.item);
      pManager.AddNumberParameter("Step distance", "D", "The distance the particle will move each timestep. Smaller distances will lead to more accurate results.",
        GH_ParamAccess.item, RS.predictionDistanceDefault);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      base.RegisterOutputParams(pManager);
      pManager.AddPlaneParameter("P", "P", "P", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref srf)) return false;
      if (!da.GetData(nextInputIndex++, ref stepDistance)) return false;
      return true;
    }

    protected override Vector3d CalcForce()
    {
      double u, v;
      srf.ClosestPoint(particle.Position, out u, out v);
      Vector3d nrml = srf.NormalAt(u, v);
      srf.FrameAt(u, v, out frame);
      Vector3d drainVec = Vector3d.CrossProduct(nrml, Vector3d.ZAxis);
      drainVec.Unitize();
      drainVec.Transform(Transform.Rotation(Math.PI / 2, nrml, particle.Position));
      drainVec = Vector3d.Multiply(drainVec, stepDistance);
      //return drainVec;
      particle.Velocity = drainVec;
      return Vector3d.Zero;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      base.SetOutputs(da);
      da.SetData(nextOutputIndex++, frame);
    }
  }
}
