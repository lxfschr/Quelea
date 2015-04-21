using Quelea.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class CrvEmitterType : AbstractEmitterType
  {

    private readonly Curve crv;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public CrvEmitterType()
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
    {
      this.crv = crv;
    }

    // Copy Constructor
    public CrvEmitterType(CrvEmitterType emitCrvType)
      : base(emitCrvType.continuousFlow, emitCrvType.creationRate, emitCrvType.numAgents)
    {
      crv = emitCrvType.crv;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
        CrvEmitterType p = obj as CrvEmitterType;
        if (p == null)
        {
            return false;
        }

      return base.Equals(obj) && crv.Equals(p.crv);
    }

    public bool Equals(CrvEmitterType p)
    {
      return base.Equals(p) && crv.Equals(p.crv);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ crv.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new CrvEmitterType(this);
    }

    public override Point3d Emit()
    {

      const double min = 0;
      const double max = 1;
      return crv.PointAtNormalizedLength((Random.RandomDouble(min, max)));

    }

    public override bool IsValid
    {
      get
      {
        return (crv.IsValid && creationRate > 0 && numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = String.ToString(RS.curveName, crv);
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
      return crv.GetBoundingBox(false);
    }
  }
}