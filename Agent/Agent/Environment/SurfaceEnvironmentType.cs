using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public class SurfaceEnvironmentType : EnvironmentType
  {

    private Surface srf;
    private Surface refSrf;
    private bool wrap;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public SurfaceEnvironmentType()
      : base()
    {
      Point3d pt1 = new Point3d(0, 0, 0);
      Point3d pt2 = new Point3d(100, 0, 0);
      Point3d pt3 = new Point3d(0, 100, 0);
      this.srf = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);
      Interval u = srf.Domain(0);
      Interval v = srf.Domain(1);
      pt1 = new Point3d(u.Min, v.Min, 0);
      pt2 = new Point3d(u.Max, v.Min, 0);
      pt3 = new Point3d(u.Min, v.Max, 0);
      this.refSrf = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);
      this.wrap = false;
    }

    // Constructor with initial values.
    public SurfaceEnvironmentType(Surface srf, bool wrap)
    {
      this.srf = srf;
      this.wrap = wrap;
      Interval u = srf.Domain(0);
      Interval v = srf.Domain(1);
      this.refSrf = new PlaneSurface(Plane.WorldXY, u, v);
    }

    // Copy Constructor
    public SurfaceEnvironmentType(SurfaceEnvironmentType environment)
    {
      this.srf = environment.srf;
      this.wrap = environment.wrap;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      SurfaceEnvironmentType p = obj as SurfaceEnvironmentType;
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.srf.Equals(p.srf) && this.wrap.Equals(p.wrap);
    }

    public bool Equals(SurfaceEnvironmentType p)
    {
      return base.Equals((SurfaceEnvironmentType)p) && this.srf.Equals(p.srf) && this.wrap.Equals(p.wrap);
    }

    public override int GetHashCode()
    {
      return this.srf.GetHashCode() ^ this.wrap.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new SurfaceEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (this.srf.IsValid);
      }

    }

    public override string ToString()
    {

      string box = "Surface: " + this.srf.ToString() + "\n";
      string wrap = "Wrap: " + this.wrap.ToString() + "\n";
      return box + wrap;
    }

    public override string TypeDescription
    {
      get { return "A Surface Environment"; }
    }

    public override string TypeName
    {
      get { return "Surface Environment"; }
    }


    public override Point3d closestPoint(Point3d pt)
    {
      double u, v;
      srf.ClosestPoint(pt, out u, out v);
      return srf.PointAt(u, v);
    }

    public override Point3d closestRefPoint(Point3d pt)
    {
      double u, v;
      srf.ClosestPoint(pt, out u, out v);
      return refSrf.PointAt(u, v);
    }

    public override Point3d closestRefPointOnRef(Point3d pt)
    {
      double u, v;
      refSrf.ClosestPoint(pt, out u, out v);
      return refSrf.PointAt(u, v);
    }

    public override Point3d closestPointOnRef(Point3d pt)
    {
      double u, v;
      refSrf.ClosestPoint(pt, out u, out v);
      return srf.PointAt(u, v);
    }
  }
}
