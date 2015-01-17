using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public abstract class EnvironmentType : GH_Goo<Object>
  {

    public EnvironmentType()
    {
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      EnvironmentType p = obj as EnvironmentType;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return true;
    }

    public bool Equals(EnvironmentType p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return true;
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
      get { return "An Environment"; }
    }

    public override string TypeName
    {
      get { return "Environment"; }
    }

    abstract public Point3d closestPoint(Point3d pt);

    abstract public Point3d closestRefPoint(Point3d pt);

    abstract public Point3d closestRefPointOnRef(Point3d pt);

    abstract public Point3d closestPointOnRef(Point3d pt);

    abstract public Vector3d avoidEdges(AgentType agent, double distance);

    abstract public bool bounceContain(AgentType agent);

    abstract public BoundingBox getBoundingBox();
  }
}
