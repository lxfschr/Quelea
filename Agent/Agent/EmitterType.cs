using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public abstract class EmitterType : GH_Goo<Object>
  {
    protected bool continuousFlow;
    protected int creationRate;
    protected int numAgents;

    public EmitterType()
    {
      this.continuousFlow = true;
      this.creationRate = 1;
      this.numAgents = 0;
    }

    abstract public Vector3d emit();

    public int CreationRate
    {
      get
      {
        return this.creationRate;
      }
    }

    public bool ContinuousFlow
    {
      get
      {
        return this.continuousFlow;
      }
    }

    public int NumAgents
    {
      get
      {
        return this.numAgents;
      }
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      EmitterType p = obj as EmitterType;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.continuousFlow.Equals(p.continuousFlow) && 
             this.creationRate.Equals(p.creationRate) && 
             this.numAgents.Equals(p.numAgents);
    }

    public bool Equals(EmitterType p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.continuousFlow.Equals(p.continuousFlow) &&
             this.creationRate.Equals(p.creationRate) &&
             this.numAgents.Equals(p.numAgents);
    }

    public override int GetHashCode()
    {
      return this.creationRate ^ this.numAgents;
    }

    abstract public override IGH_Goo Duplicate();

    public override bool IsValid
    {
      get
      {
        return (this.creationRate > 0 && this.numAgents >= 0);
      }
    }

    public override string ToString()
    {
      string continuousFlow = "ContinuousFlow: " + this.continuousFlow.ToString() + "\n";
      string creationRate = "Creation Rate: " + this.creationRate.ToString() + "\n";
      string numAgents = "Number of Agents: " + this.numAgents.ToString() + "\n";
      return continuousFlow + creationRate + numAgents;
    }

    public override string TypeDescription
    {
      get { return "An Emitter"; }
    }

    public override string TypeName
    {
      get { return "EmitterType"; }
    }
  }
}
