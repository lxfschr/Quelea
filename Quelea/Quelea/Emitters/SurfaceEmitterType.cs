using Quelea.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SurfaceEmitterType : AbstractEmitterType
  {

    private readonly Surface srf;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public SurfaceEmitterType()
    {
      srf = new PlaneSurface(Plane.WorldXY, new Interval(0, 1), new Interval(0, 1));
    }

    // Constructor with initial values.
    public SurfaceEmitterType(Surface srf, bool continuousFlow, int creationRate, int numAgents, Vector3d velocityMin, Vector3d velocityMax)
      :base(continuousFlow, creationRate, numAgents, velocityMin, velocityMax)
    {
      Interval interval = new Interval(0, 1);
      srf.SetDomain(0, interval);
      srf.SetDomain(1, interval);
      this.srf = srf;
    }

    // Constructor with initial values.
    public SurfaceEmitterType(Surface srf)
    {
      Interval interval = new Interval(0,1);
      srf.SetDomain(0, interval);
      srf.SetDomain(1, interval);
      this.srf = srf;
    }

    // Copy Constructor
    public SurfaceEmitterType(SurfaceEmitterType e)
      : base(e.continuousFlow, e.creationRate, e.numAgents, e.velocityMin, e.velocityMax)
    {
      srf = e.srf;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      SurfaceEmitterType p = obj as SurfaceEmitterType;
        if (p == null)
        {
            return false;
        }

      return base.Equals(obj) && srf.Equals(p.srf);
    }

    public bool Equals(SurfaceEmitterType p)
    {
      return base.Equals(p) && srf.Equals(p.srf);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ srf.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new SurfaceEmitterType(this);
    }

    protected override Point3d GetEmittionPoint()
    {
      return srf.PointAt(Util.Random.RandomDouble(0, 1), Util.Random.RandomDouble(0, 1));

    }

    public override bool IsValid
    {
      get
      {
        return (srf.IsValid && creationRate > 0 && numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = String.ToString(RS.surfaceName, srf);
      string continuousFlowStr = String.ToString(RS.continuousFlowName, continuousFlow);
      string creationRateStr = String.ToString(RS.creationRateName, creationRate);
      string numAgentsStr = String.ToString(RS.numQueleaName, numAgents);
      return origin + continuousFlowStr + creationRateStr + numAgentsStr;
    }

    public override string TypeDescription
    {
      get { return RS.curveEmitterDescription; }
    }

    public override string TypeName
    {
      get { return RS.curveEmitterName; }
    }


    public override BoundingBox GetBoundingBox()
    {
      return srf.GetBoundingBox(false);
    }
  }
}