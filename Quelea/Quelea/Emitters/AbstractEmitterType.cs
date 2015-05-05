using System;
using RS = Quelea.Properties.Resources;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Quelea
{
  public abstract class AbstractEmitterType : GH_Goo<Object>
  {
    protected bool continuousFlow;
    protected readonly int creationRate;
    protected readonly int numAgents;
    protected readonly Vector3d velocityMin;
    protected readonly Vector3d velocityMax;

    protected AbstractEmitterType()
      :this(RS.continuousFlowDefault, RS.creationRateDefault, RS.numAgentsDefault, Constants.VelocityMin, Constants.VelocityMax)
    {
    }

    protected AbstractEmitterType(bool continuousFlow, int creationRate, int numAgents, Vector3d velocityMin, Vector3d velocityMax)
    {
      this.continuousFlow = continuousFlow;
      this.creationRate = creationRate;
      this.numAgents = numAgents;
      this.velocityMin = velocityMin;
      this.velocityMax = velocityMax;
    }

    public Point3d Emit(out Vector3d initialVelocity)
    {
      initialVelocity = Util.Random.RandomVector(velocityMin, velocityMax);
      return GetEmittionPoint();
    }

    protected abstract Point3d GetEmittionPoint();

    public int CreationRate
    {
      get
      {
        return creationRate;
      }
    }

    public bool ContinuousFlow
    {
      get
      {
        return continuousFlow;
      }
    }

    public int NumAgents
    {
      get
      {
        return numAgents;
      }
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Point return false.
      return Equals(obj as AbstractEmitterType);
    }

    public bool Equals(AbstractEmitterType p)
    {
      // If parameter is null return false:
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return continuousFlow.Equals(p.continuousFlow) &&
             creationRate.Equals(p.creationRate) &&
             numAgents.Equals(p.numAgents) &&
             velocityMin.Equals(p.velocityMin) &&
             velocityMax.Equals(p.velocityMax);
    }

    public override int GetHashCode()
    {
      return creationRate.GetHashCode() ^ numAgents.GetHashCode() * velocityMin.GetHashCode() * velocityMax.GetHashCode();
    }

    abstract public override IGH_Goo Duplicate();

    public override bool IsValid
    {
      get
      {
        return (creationRate > 0 && numAgents >= 0);
      }
    }

    public override string ToString()
    {
      string continuousFlowStr = Util.String.ToString(RS.continuousFlowName,continuousFlow);
      string creationRateStr = Util.String.ToString(RS.creationRateName, creationRate);
      string numAgentsStr = Util.String.ToString(RS.numQueleaName, numAgents);
      string velocityMinStr = Util.String.ToString("Minimum Velocity", velocityMin);
      string velocityMaxStr = Util.String.ToString("Maximum Velocity", velocityMax);
      return continuousFlowStr + creationRateStr + numAgentsStr + velocityMinStr + velocityMaxStr;
    }

    public override string TypeDescription
    {
      get { return RS.emitterDescription; }
    }

    public override string TypeName
    {
      get { return RS.emitterName; }
    }

    public abstract BoundingBox GetBoundingBox();
  }
}
