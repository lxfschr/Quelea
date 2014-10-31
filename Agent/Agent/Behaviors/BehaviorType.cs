using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Agent
{
  public abstract class BehaviorType : GH_Goo<Object>
  {
    public abstract bool applyBehavior(AgentType agent, AgentSystemType system);
    public abstract override IGH_Goo Duplicate();

    public override bool IsValid
    {
      get
      {
        return true;
      }
    }

    public abstract override string ToString();

    public override string TypeDescription
    {
      get { return "A Behavior"; }
    }

    public override string TypeName
    {
      get { return "Behavior"; }
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      BehaviorType p = obj as BehaviorType;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return true;
    }

    public bool Equals(BehaviorType p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return true;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
