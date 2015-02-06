using RS = Agent.Properties.Resources;
using Agent.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public class CrvEmitterType : EmitterType
  {

    private readonly Curve crv;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public CrvEmitterType()
      :base()
    {
      crv = new Line().ToNurbsCurve();
    }

    // Constructor with initial values.
    public CrvEmitterType(Curve crv, bool continuousFlow, int creationRate, int numAgents)
      :base(continuousFlow, creationRate, numAgents)
    {
      this.crv = crv;
    }

    // Constructor with initial values.
    public CrvEmitterType(Curve crv)
      :base()
    {
      this.crv = crv;
    }

    // Copy Constructor
    public CrvEmitterType(CrvEmitterType emitCrvType)
      : base(emitCrvType.continuousFlow, emitCrvType.creationRate, emitCrvType.numAgents)
    {
      this.crv = emitCrvType.crv;
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

    public override Point3d Emit()
    {

      const double min = 0;
      const double max = 1;
      return this.crv.PointAtNormalizedLength((Random.RandomDouble(min, max)));

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

      string origin = Util.String.ToString(RS.curveName, crv);
      string continuousFlowStr = Util.String.ToString(RS.continuousFlowName, continuousFlow);
      string creationRateStr = Util.String.ToString(RS.creationRateName, creationRate);
      string numAgentsStr = Util.String.ToString(RS.numAgentsName, numAgents);
      return origin + continuousFlowStr + creationRateStr + numAgentsStr;
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
      return this.crv.GetBoundingBox(false);
    }
  }
}