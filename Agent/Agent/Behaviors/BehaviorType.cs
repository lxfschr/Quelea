using System;
using Agent.Properties;
using Grasshopper.Kernel.Types;

namespace Agent
{
  public abstract class BehaviorType : GH_Goo<Object>
  {
    public abstract bool ApplyBehavior(AgentType agent, AgentSystemType system);
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
      get { return Resources.behaviorDescription; }
    }

    public override string TypeName
    {
      get { return Resources.behaviorName; }
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Point return false.
      BehaviorType p = obj as BehaviorType;
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return true;
    }

    public bool Equals(BehaviorType p)
    {
      // If parameter is null return false:
      return p != null;

      // Return true if the fields match:
    }

    public abstract override int GetHashCode();
  }
}
