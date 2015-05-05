using Quelea.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class PointEmitterType : AbstractEmitterType
  {

    private readonly Point3d pt;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public PointEmitterType()
    {
      pt = Point3d.Origin;
    }

    // Constructor with initial values.
    public PointEmitterType(Point3d pt, bool continuousFlow, int creationRate, int numAgents, Vector3d velocityMin, Vector3d velocityMax)
      :base(continuousFlow, creationRate, numAgents, velocityMin, velocityMax)
    {
      this.pt = pt;
    }

    // Constructor with initial values.
    public PointEmitterType(Point3d pt)
    {
      this.pt = pt;
    }

    // Copy Constructor
    public PointEmitterType(PointEmitterType p)
      : base(p.continuousFlow, p.creationRate, p.numAgents, p.velocityMin, p.velocityMax)
    {
      pt = p.pt;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      PointEmitterType p = obj as PointEmitterType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj) && pt.Equals(p.pt);
    }

    public bool Equals(PointEmitterType p)
    {
      return base.Equals(p) && pt.Equals(p.pt);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ pt.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new PointEmitterType(this);
    }

    protected override Point3d GetEmittionPoint()
    {
      return pt;
    }

    public override bool IsValid
    {
      get
      {
        return (pt.IsValid && creationRate > 0 && numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = String.ToString(RS.pointName, pt);
      string continuousFlowStr = String.ToString(RS.continuousFlowName, continuousFlow);
      string creationRateStr = String.ToString(RS.creationRateName, creationRate);
      string numAgentsStr = String.ToString(RS.numQueleaName, numAgents);
      return origin + continuousFlowStr + creationRateStr + numAgentsStr;
    }

    public override string TypeDescription
    {
      get { return RS.pointEmitterDescription; }
    }

    public override string TypeName
    {
      get { return RS.pointEmitterName; }
    }


    public override BoundingBox GetBoundingBox()
    {
      return new BoundingBox(pt, pt);
    }
  }
}
