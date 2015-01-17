using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
namespace Agent
{
  public class BoxEmitterType : EmitterType
  {

    private Box box;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public BoxEmitterType()
    {
      Plane pln = new Plane();
      Interval size  = new Interval(0, 10);
      this.box = new Box(pln, size, size, size);
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Constructor with initial values.
    public BoxEmitterType(Box box, bool continuousFlow, int creationRate, int numAgents)
    {
      this.box = box;
      this.continuousFlow = continuousFlow;
      this.creationRate = creationRate;
      this.numAgents = numAgents;
    }

    // Constructor with initial values.
    public BoxEmitterType(Box box)
    {
      this.box = box;
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Copy Constructor
    public BoxEmitterType(BoxEmitterType boxEmitter)
    {
      this.box = boxEmitter.box;
      this.continuousFlow = boxEmitter.continuousFlow;
      this.creationRate = boxEmitter.creationRate;
      this.numAgents = boxEmitter.numAgents;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
        BoxEmitterType p = obj as BoxEmitterType;
        if ((object)p == null)
        {
            return false;
        }

      return base.Equals(obj) && this.box.Equals(p.box);
    }

    public bool Equals(BoxEmitterType p)
    {
      return base.Equals((BoxEmitterType)p) && this.box.Equals(p.box);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this.box.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new BoxEmitterType(this);
    }

    public override Point3d emit()
    {

      double min = 0;
      double max = 1;
      double x = Util.Random.RandomDouble(min, max);
      double y = Util.Random.RandomDouble(min, max);
      double z = Util.Random.RandomDouble(min, max);
      return box.PointAt(x, y, z); ;

    }

    public override bool IsValid
    {
      get
      {
        return (this.box.IsValid && this.creationRate > 0 && this.numAgents >= 0);
      }

    }

    public override string ToString()
    {

      string origin = "Box: " + box.ToString() + "\n";
      string continuousFlow = "ContinuousFlow: " + this.continuousFlow.ToString() + "\n";
      string creationRate = "Creation Rate: " + this.creationRate.ToString() + "\n";
      string numAgents = "Number of Agents: " + this.numAgents.ToString() + "\n";
      return origin + continuousFlow + creationRate + numAgents;
    }

    public override string TypeDescription
    {
      get { return "A Box Emitter"; }
    }

    public override string TypeName
    {
      get { return "BoxEmitter"; }
    }


    public override BoundingBox getBoundingBox()
    {
      return this.box.BoundingBox;
    }
  }
}
