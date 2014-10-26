using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public class EmitterPtType : EmitterType
  {

    private Point3d pt;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public EmitterPtType()
      : base()
    {
      this.pt = Point3d.Origin;
    }

    // Constructor with initial values.
    public EmitterPtType(Point3d pt, bool continuousFlow, int creationRate, int numAgents)
    {
      this.pt = pt;
      this.continuousFlow = continuousFlow;
      this.creationRate = creationRate;
      this.numAgents = numAgents;
    }

    // Constructor with initial values.
    public EmitterPtType(Point3d pt)
    {
      this.pt = pt;
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Copy Constructor
    public EmitterPtType(EmitterPtType ptEmitType)
    {
      this.pt = ptEmitType.pt;
      this.continuousFlow = ptEmitType.continuousFlow;
      this.creationRate = ptEmitType.creationRate;
      this.numAgents = ptEmitType.numAgents;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      EmitterPtType p = obj as EmitterPtType;
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.pt.Equals(p.pt);
    }

    public bool Equals(EmitterPtType p)
    {
      return base.Equals((EmitterPtType)p) && this.pt.Equals(p.pt);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this.pt.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new EmitterPtType(this);
    }

    public override Point3d emit()
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

  }
}
