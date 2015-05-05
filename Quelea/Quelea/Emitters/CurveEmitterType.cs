using Quelea.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class CurveEmitterType : AbstractEmitterType
  {

    private readonly Curve crv;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public CurveEmitterType()
    {
      crv = new Line().ToNurbsCurve();
    }

    // Constructor with initial values.
    public CurveEmitterType(Curve crv, bool continuousFlow, int creationRate, int numAgents, Vector3d velocityMin, Vector3d velocityMax)
      :base(continuousFlow, creationRate, numAgents, velocityMin, velocityMax)
    {
      this.crv = crv;
    }

    // Constructor with initial values.
    public CurveEmitterType(Curve crv)
    {
      this.crv = crv;
    }

    // Copy Constructor
    public CurveEmitterType(CurveEmitterType emitCrvType)
      : base(emitCrvType.continuousFlow, emitCrvType.creationRate, emitCrvType.numAgents, emitCrvType.velocityMin, emitCrvType.velocityMax)
    {
      crv = emitCrvType.crv;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
        CurveEmitterType p = obj as CurveEmitterType;
        if (p == null)
        {
            return false;
        }

      return base.Equals(obj) && crv.Equals(p.crv);
    }

    public bool Equals(CurveEmitterType p)
    {
      return base.Equals(p) && crv.Equals(p.crv);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ crv.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new CurveEmitterType(this);
    }

    protected override Point3d GetEmittionPoint()
    {
      return crv.PointAtNormalizedLength((Random.RandomDouble(0, 1)));

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