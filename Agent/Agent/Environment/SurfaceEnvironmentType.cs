using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public class SurfaceEnvironmentType : EnvironmentType
  {

    private Surface environment;
    private Surface refEnvironment;
    private bool wrap;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public SurfaceEnvironmentType()
      : base()
    {
      Point3d pt1 = new Point3d(0, 0, 0);
      Point3d pt2 = new Point3d(100, 0, 0);
      Point3d pt3 = new Point3d(0, 100, 0);
      this.environment = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);
      Interval u = environment.Domain(0);
      Interval v = environment.Domain(1);
      pt1 = new Point3d(u.Min, v.Min, 0);
      pt2 = new Point3d(u.Max, v.Min, 0);
      pt3 = new Point3d(u.Min, v.Max, 0);
      this.refEnvironment = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);
      this.wrap = false;
    }

    // Constructor with initial values.
    public SurfaceEnvironmentType(Surface srf, bool wrap)
    {
      this.environment = srf;
      this.wrap = wrap;
      Interval u = srf.Domain(0);
      Interval v = srf.Domain(1);
      this.refEnvironment = new PlaneSurface(Plane.WorldXY, u, v);
    }

    // Copy Constructor
    public SurfaceEnvironmentType(SurfaceEnvironmentType environment)
    {
      this.environment = environment.environment;
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

      return base.Equals(obj) && this.environment.Equals(p.environment) && this.wrap.Equals(p.wrap);
    }

    public bool Equals(SurfaceEnvironmentType p)
    {
      return base.Equals((SurfaceEnvironmentType)p) && this.environment.Equals(p.environment) && this.wrap.Equals(p.wrap);
    }

    public override int GetHashCode()
    {
      return this.environment.GetHashCode() ^ this.wrap.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new SurfaceEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (this.environment.IsValid);
      }

    }

    public override string ToString()
    {

      string box = "Surface: " + this.environment.ToString() + "\n";
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
      environment.ClosestPoint(pt, out u, out v);
      return environment.PointAt(u, v);
    }

    public override Point3d closestRefPoint(Point3d pt)
    {
      double u, v;
      environment.ClosestPoint(pt, out u, out v);
      return refEnvironment.PointAt(u, v);
    }

    public override Point3d closestRefPointOnRef(Point3d pt)
    {
      double u, v;
      refEnvironment.ClosestPoint(pt, out u, out v);
      return refEnvironment.PointAt(u, v);
    }

    public override Point3d closestPointOnRef(Point3d pt)
    {
      double u, v;
      refEnvironment.ClosestPoint(pt, out u, out v);
      return environment.PointAt(u, v);
    }

    public override Vector3d avoidEdges(AgentType agent, double distance)
    {
      Vector3d steer = new Vector3d();

      Interval uDom = refEnvironment.Domain(0);
      Interval vDom = refEnvironment.Domain(1);

      double minX = uDom.Min;
      double maxX = uDom.Max;
      double minY = vDom.Min;
      double maxY = vDom.Max;

      Point3d refPosition = agent.RefPosition;
      double maxSpeed = agent.MaxSpeed;
      Vector3d velocity = agent.Velocity;

      Vector3d desired = new Vector3d(0, 0, 0);

      if (refPosition.X < minX + distance)
      {
        desired = new Vector3d(maxSpeed, velocity.Y, velocity.Z);
      }
      else if (refPosition.X > maxX - distance)
      {
        desired = new Vector3d(-maxSpeed, velocity.Y, velocity.Z);
      }

      if (refPosition.Y < minY + distance)
      {
        desired = new Vector3d(velocity.X, maxSpeed, velocity.Z);
      }
      else if (refPosition.Y > maxY - distance)
      {
        desired = new Vector3d(velocity.X, -maxSpeed, velocity.Z);
      }

      if (!desired.IsZero)
      {
        desired.Unitize();
        desired = Vector3d.Multiply(desired, maxSpeed);
        steer = Vector3d.Subtract(desired, velocity);
        steer = ForceType.limit(steer, agent.MaxForce);
      }
      return steer;
    }
  }
}
