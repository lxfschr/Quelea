using System;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class EnvironmentType : GH_Goo<Object>
  {
    public override bool Equals(object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Point return false.
      EnvironmentType p = obj as EnvironmentType;
      return p != null;

      // Return true if the fields match:
    }

    public bool Equals(EnvironmentType p)
    {
      // If parameter is null return false:
      return p != null;

      // Return true if the fields match:
    }

    abstract public override int GetHashCode();

    abstract public override IGH_Goo Duplicate();

    public override bool IsValid
    {
      get
      {
        return true;
      }
    }

    abstract public override string ToString();

    public override string TypeDescription
    {
      get { return RS.environmentDescription; }
    }

    public override string TypeName
    {
      get { return RS.environmentName; }
    }

    abstract public Point3d ClosestPoint(Point3d pt);

    abstract public Point3d ClosestRefPoint(Point3d pt);

    abstract public Point3d ClosestRefPointOnRef(Point3d pt);

    abstract public Point3d ClosestPointOnRef(Point3d pt);

    abstract public Vector3d AvoidEdges(AgentType agent, double distance);

    abstract public bool BounceContain(AgentType agent);

    abstract public BoundingBox GetBoundingBox();
  }
}
