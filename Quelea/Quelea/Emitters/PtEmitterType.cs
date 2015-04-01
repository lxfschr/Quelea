using Agent.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class PtEmitterType : AbstractEmitterType
  {

    private readonly Point3d pt;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public PtEmitterType()
    {
      pt = Point3d.Origin;
    }

    // Constructor with initial values.
    public PtEmitterType(Point3d pt, bool continuousFlow, int creationRate, int numAgents)
      :base(continuousFlow, creationRate, numAgents)
    {
      this.pt = pt;
    }

    // Constructor with initial values.
    public PtEmitterType(Point3d pt)
    {
      this.pt = pt;
    }

    // Copy Constructor
    public PtEmitterType(PtEmitterType ptEmitType)
      : base(ptEmitType.continuousFlow, ptEmitType.creationRate, ptEmitType.numAgents)
    {
      pt = ptEmitType.pt;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      PtEmitterType p = obj as PtEmitterType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj) && pt.Equals(p.pt);
    }

    public bool Equals(PtEmitterType p)
    {
      return base.Equals(p) && pt.Equals(p.pt);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ pt.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new PtEmitterType(this);
    }

    public override Point3d Emit()
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
