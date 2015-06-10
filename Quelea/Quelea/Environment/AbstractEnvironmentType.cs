using System;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractEnvironmentType : GH_Goo<Object>
  {

    protected AbstractEnvironmentType()
    {
      Wrap = false;
    }
    public override bool Equals(object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Point return false.
      AbstractEnvironmentType p = obj as AbstractEnvironmentType;
      return p != null;

      // Return true if the fields match:
    }

    public bool Equals(AbstractEnvironmentType p)
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
    public bool Wrap { get; protected set; }
    public abstract Point3d WrapPoint(Point3d position, out bool wrapped);

    public double Width { get; protected set; }
    public double Height { get; protected set; }
    public double Depth { get; protected set; }
    abstract public Point3d ClosestPoint(Point3d pt);

    abstract public Point3d MapTo2D(Point3d pt);

    abstract public Point3d ClosestPointOnRef(Point3d pt);

    abstract public Point3d MapTo3D(Point3d pt);

    public abstract Vector3d ClosestNormal(Point3d pt);

    abstract public Vector3d AvoidEdges(IAgent agent, double distance);

    abstract public bool BounceContain(IParticle particle);

    abstract public BoundingBox GetBoundingBox();
    public abstract bool Contains(Point3d pt);
  }
}
