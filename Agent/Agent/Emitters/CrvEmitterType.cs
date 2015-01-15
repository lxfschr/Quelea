using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
namespace Agent
{
  public class CrvEmitterType : EmitterType
  {

    private Curve crv;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public CrvEmitterType()
    {
      this.crv = new Line().ToNurbsCurve();
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Constructor with initial values.
    public CrvEmitterType(Curve crv, bool continuousFlow, int creationRate, int numAgents)
    {
      this.crv = crv;
      this.continuousFlow = continuousFlow;
      this.creationRate = creationRate;
      this.numAgents = numAgents;
    }

    // Constructor with initial values.
    public CrvEmitterType(Curve crv)
    {
      this.crv = crv;
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Copy Constructor
    public CrvEmitterType(CrvEmitterType emitCrvType)
    {
      this.crv = emitCrvType.crv;
      this.continuousFlow = emitCrvType.continuousFlow;
      this.creationRate = emitCrvType.creationRate;
      this.numAgents = emitCrvType.numAgents;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
        CrvEmitterType p = obj as CrvEmitterType;
        if ((object)p == null)
        {
            return false;
        }

      return base.Equals(obj) && this.crv.Equals(p.crv);
    }

    public bool Equals(CrvEmitterType p)
    {
      return base.Equals((CrvEmitterType)p) && this.crv.Equals(p.crv);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this.crv.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new CrvEmitterType(this);
    }

    public override Point3d emit()
    {

      double min = 0;
      double max = 1;
      return this.crv.PointAtNormalizedLength((Util.Random.RandomDouble(min, max)));

    }

    public override bool IsValid
    {
      get
      {
        return (this.crv.IsValid && this.creationRate > 0 && this.numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = "Origin Curve: " + crv.ToString() + "\n";
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
