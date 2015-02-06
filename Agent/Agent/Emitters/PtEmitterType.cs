using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public class PtEmitterType : EmitterType
  {

    private Point3d pt;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public PtEmitterType()
      : base()
    {
      this.pt = Point3d.Origin;
    }

    // Constructor with initial values.
    public PtEmitterType(Point3d pt, bool continuousFlow, int creationRate, int numAgents)
      :base(continuousFlow, creationRate, numAgents)
    {
      this.pt = pt;
    }

    // Constructor with initial values.
    public PtEmitterType(Point3d pt)
      :base()
    {
      this.pt = pt;
    }

    // Copy Constructor
    public PtEmitterType(PtEmitterType ptEmitType)
      : base(ptEmitType.continuousFlow, ptEmitType.creationRate, ptEmitType.numAgents)
    {
      this.pt = ptEmitType.pt;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      PtEmitterType p = obj as PtEmitterType;
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.pt.Equals(p.pt);
    }

    public bool Equals(PtEmitterType p)
    {
      return base.Equals((PtEmitterType)p) && this.pt.Equals(p.pt);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this.pt.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new PtEmitterType(this);
    }

    public override Point3d Emit()
    {
      return this.pt;
    }

    public override bool IsValid
    {
      get
      {
        return (this.pt.IsValid && this.creationRate > 0 && this.numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = "Origin Point: " + pt.ToString() + "\n";
      string continuousFlow = "ContinuousFlow: " + this.continuousFlow.ToString() + "\n";
      string creationRate = "Creation Rate: " + this.creationRate.ToString() + "\n";
      string numAgents = "Number of Agents: " + this.numAgents.ToString() + "\n";
      return origin + continuousFlow + creationRate + numAgents;
    }

    public override string TypeDescription
    {
      get { return "A Point Emitter"; }
    }

    public override string TypeName
    {
      get { return "PointEmitterType"; }
    }


    public override BoundingBox GetBoundingBox()
    {
      return new BoundingBox(this.pt, this.pt);
    }
  }
}
