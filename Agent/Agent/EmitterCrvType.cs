using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
namespace Agent
{
  public class EmitterCrvType : EmitterType
  {

    private Curve crv;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public EmitterCrvType()
    {
      this.crv = new Line().ToNurbsCurve();
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Constructor with initial values.
    public EmitterCrvType(Curve crv, bool continuousFlow, int creationRate, int numAgents)
    {
      this.crv = crv;
      this.continuousFlow = continuousFlow;
      this.creationRate = creationRate;
      this.numAgents = numAgents;
    }

    // Constructor with initial values.
    public EmitterCrvType(Curve crv)
    {
      this.crv = crv;
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    // Copy Constructor
    public EmitterCrvType(EmitterCrvType emitCrvType)
    {
      this.crv = emitCrvType.crv;
      this.continuousFlow = emitCrvType.continuousFlow;
      this.creationRate = emitCrvType.creationRate;
      this.numAgents = emitCrvType.numAgents;
    }

    public override IGH_Goo Duplicate()
    {
      return new EmitterCrvType(this);
    }

    public override Vector3d emit()
    {

      double min = 0;
      double max = 1;
      return new Vector3d(this.crv.PointAtNormalizedLength((Util.Random.RandomDouble(min, max))));

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
