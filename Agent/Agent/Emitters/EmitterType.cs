using System;
using RS = Agent.Properties.Resources;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public abstract class EmitterType : GH_Goo<Object>
  {
    protected bool continuousFlow;
    protected readonly int creationRate;
    protected readonly int numAgents;

    protected EmitterType()
    {
      continuousFlow = RS.continuousFlowDefault;
      creationRate = RS.creationRateDefault;
      numAgents = RS.numAgentsDefault;
    }

    protected EmitterType(bool continuousFlow, int creationRate, int numAgents)
    {
      this.continuousFlow = continuousFlow;
      this.creationRate = creationRate;
      this.numAgents = numAgents;
    }

    abstract public Point3d Emit();

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
      EmitterType p = obj as EmitterType;
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return continuousFlow.Equals(p.continuousFlow) && 
             creationRate.Equals(p.creationRate) && 
             numAgents.Equals(p.numAgents);
    }

    public bool Equals(EmitterType p)
    {
      // If parameter is null return false:
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return continuousFlow.Equals(p.continuousFlow) &&
             creationRate.Equals(p.creationRate) &&
             numAgents.Equals(p.numAgents);
    }

    public override int GetHashCode()
    {
      return creationRate.GetHashCode() ^ numAgents.GetHashCode();
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
      string continuousFlowStr = RS.continuousFlowName + ": " + continuousFlow + "\n";
      string creationRateStr = RS.creationRateName + ": " + creationRate + "\n";
      string numAgentsStr = RS.numAgentsName + ": " + numAgents + "\n";
      return continuousFlowStr + creationRateStr + numAgentsStr;
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
