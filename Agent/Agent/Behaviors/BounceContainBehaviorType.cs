using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  class BounceContainBehaviorType : BehaviorType
  {
    private EnvironmentType environment;

    public BounceContainBehaviorType()
    {
      this.environment = null;
    }

    public BounceContainBehaviorType(EnvironmentType environment) {
      this.environment = environment;
    }

    public BounceContainBehaviorType(BounceContainBehaviorType behavior)
    {
      this.environment = behavior.environment;
    }
    public override IGH_Goo Duplicate()
    {
      return new BounceContainBehaviorType(this);
    }

    public override bool IsValid
    {
      get { return true;  }
    }

    public override string ToString() //ToDo improve ToString()'s
    {
      return environment.ToString();
    }

    public override string TypeDescription
    {
      get { return "Causes agents to bounce off of environment boundaries.";  }
    }

    public override string TypeName
    {
      get { return "Bounce Contain Behavior"; }
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      BounceContainBehaviorType p = obj as BounceContainBehaviorType;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return true;
    }

    public bool Equals(BounceContainBehaviorType p)
    {
      return base.Equals((BehaviorType)p);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool applyBehavior(AgentType agent, AgentSystemType system)
    {
      return environment.bounceContain(agent);
    }
  }
}
