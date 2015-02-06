using Agent.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class BoxEmitterType : EmitterType
  {

    private readonly Box box;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public BoxEmitterType()
      : base()
    {
      Plane pln = new Plane();
      Interval size = new Interval(-RS.boxBoundsDefault, RS.boxBoundsDefault);
      box = new Box(pln, size, size, size);
    }

    // Constructor with initial values.
    public BoxEmitterType(Box box, bool continuousFlow, int creationRate, int numAgents)
      :base(continuousFlow, creationRate, numAgents)
    {
      this.box = box;
    }

    // Constructor with initial values.
    public BoxEmitterType(Box box)
      :base()
    {
      this.box = box;
    }

    // Copy Constructor
    public BoxEmitterType(BoxEmitterType boxEmitter)
      :base()
    {
      box = boxEmitter.box;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
        BoxEmitterType p = obj as BoxEmitterType;
        if (p == null)
        {
            return false;
        }

      return base.Equals(obj) && box.Equals(p.box);
    }

    public bool Equals(BoxEmitterType p)
    {
      return base.Equals(p) && box.Equals(p.box);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ box.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new BoxEmitterType(this);
    }

    public override Point3d Emit()
    {
      const double min = 0;
      const double max = 1;
      double x = Random.RandomDouble(min, max);
      double y = Random.RandomDouble(min, max);
      double z = Random.RandomDouble(min, max);
      return box.PointAt(x, y, z);
    }

    public override bool IsValid
    {
      get
      {
        return (box.IsValid && creationRate > 0 && numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = RS.boxName + ": " + box + "\n";
      string continuousFlowStr = RS.continuousFlowName + ": " + continuousFlow + "\n";
      string creationRateStr = RS.creationRateName + ": " + creationRate + "\n";
      string numAgentsStr = RS.numAgentsName + ": " + numAgents + "\n";
      return origin + continuousFlowStr + creationRateStr + numAgentsStr;
    }

    public override string TypeDescription
    {
      get { return RS.boxEmitDescription; }
    }

    public override string TypeName
    {
      get { return RS.boxEmitName; }
    }


    public override BoundingBox GetBoundingBox()
    {
      return box.BoundingBox;
    }
  }
}
