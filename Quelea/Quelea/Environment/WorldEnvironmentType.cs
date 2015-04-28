using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class WorldEnvironmentType : AbstractEnvironmentType
  {

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public WorldEnvironmentType()
    {
    }

    // Copy Constructor
    public WorldEnvironmentType(WorldEnvironmentType environment)
    {
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      WorldEnvironmentType p = obj as WorldEnvironmentType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj);
    }

    public bool Equals(WorldEnvironmentType p)
    {
      return base.Equals(p);
    }

    public override int GetHashCode()
    {
      return 21;
    }

    public override IGH_Goo Duplicate()
    {
      return new WorldEnvironmentType(this);
    }

    public override bool IsValid
    {
      get { return true; }

    }

    public override string ToString()
    {

      string environmentStr =  "World Environment\n";
      return environmentStr;
    }

    public override string TypeDescription
    {
      get { return "An unbounded environment consisting of all of extents of the 3D space."; }
    }

    public override string TypeName
    {
      get { return "World Environment"; }
    }


    public override Point3d ClosestPoint(Point3d pt)
    {
      return pt;
    }

    public override Point3d ClosestRefPoint(Point3d pt) {
      return ClosestPoint(pt);
    }

    public override Point3d ClosestRefPointOnRef(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Vector3d ClosestNormal(Point3d pt)
    {
      return Vector3d.Zero;
    }

    public override Vector3d AvoidEdges(IAgent agent, double distance)
    {
      return Vector3d.Zero;
    }

    public override bool BounceContain(IParticle agent)
    {
      return false;
    }

    public override BoundingBox GetBoundingBox()
    {
      return BoundingBox.Unset;
    }

    public override bool Contains(Point3d pt)
    {
      return true;
    }
  }
}
