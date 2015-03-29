using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SurfaceEnvironmentType : AbstractEnvironmentType
  {

    private readonly Surface environment;
    private readonly Surface refEnvironment;

    private readonly double minX;
    private readonly double maxX;
    private readonly double minY;
    private readonly double maxY;

    // Default Constructor.
    public SurfaceEnvironmentType()
    {
      Point3d pt1 = new Point3d(0, 0, 0);
      Point3d pt2 = new Point3d(RS.boxBoundsDefault, 0, 0);
      Point3d pt3 = new Point3d(0, RS.boxBoundsDefault, 0);
      environment = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);
      Interval u = environment.Domain(0);
      Interval v = environment.Domain(1);
      pt1 = new Point3d(u.Min, v.Min, 0);
      pt2 = new Point3d(u.Max, v.Min, 0);
      pt3 = new Point3d(u.Min, v.Max, 0);
      refEnvironment = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);

      Interval uDom = refEnvironment.Domain(0);
      Interval vDom = refEnvironment.Domain(1);

      minX = uDom.Min;
      maxX = uDom.Max;
      minY = vDom.Min;
      maxY = vDom.Max;
    }

    // Constructor with initial values.
    public SurfaceEnvironmentType(Surface srf)
    {
      environment = srf;
      Interval u = srf.Domain(0);
      Interval v = srf.Domain(1);
      refEnvironment = new PlaneSurface(Plane.WorldXY, u, v);

      Interval uDom = refEnvironment.Domain(0);
      Interval vDom = refEnvironment.Domain(1);

      minX = uDom.Min;
      maxX = uDom.Max;
      minY = vDom.Min;
      maxY = vDom.Max;
    }

    // Copy Constructor
    public SurfaceEnvironmentType(SurfaceEnvironmentType environment)
    {
      this.environment = environment.environment;

      Interval uDom = environment.refEnvironment.Domain(0);
      Interval vDom = environment.refEnvironment.Domain(1);

      minX = uDom.Min;
      maxX = uDom.Max;
      minY = vDom.Min;
      maxY = vDom.Max;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      SurfaceEnvironmentType p = obj as SurfaceEnvironmentType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj) && environment.Equals(p.environment);
    }

    public bool Equals(SurfaceEnvironmentType p)
    {
      return base.Equals(p) && environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return environment.GetHashCode() ^ refEnvironment.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new SurfaceEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (environment.IsValid);
      }

    }

    public override string ToString()
    {

      string environmentStr = Util.String.ToString(RS.surfaceName, environment);
      return environmentStr;
    }

    public override string TypeDescription
    {
      get { return RS.surfaceEnvironmentDescription; }
    }

    public override string TypeName
    {
      get { return RS.surfaceEnvironmentName; }
    }


    public override Point3d ClosestPoint(Point3d pt)
    {
      double u, v;
      environment.ClosestPoint(pt, out u, out v);
      return environment.PointAt(u, v);
    }

    public override Point3d ClosestRefPoint(Point3d pt)
    {
      double u, v;
      environment.ClosestPoint(pt, out u, out v);
      return refEnvironment.PointAt(u, v);
    }

    public override Point3d ClosestRefPointOnRef(Point3d pt)
    {
      double u, v;
      refEnvironment.ClosestPoint(pt, out u, out v);
      return refEnvironment.PointAt(u, v);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      double u, v;
      refEnvironment.ClosestPoint(pt, out u, out v);
      return environment.PointAt(u, v);
    }

    public override Vector3d AvoidEdges(IAgent agent, double distance)
    {
      Point3d refPosition = agent.RefPosition;
      double maxSpeed = agent.MaxSpeed;
      Vector3d velocity = agent.Velocity;

      Vector3d desired = new Vector3d();

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

      return desired;
    }

    public override bool BounceContain(IAgent agent)
    {
      Point3d position = agent.RefPosition;
      Vector3d velocity = agent.Velocity;
      if (position.X >= maxX)
      {
        position.X = maxX;
        velocity.X *= -1;
        //system.environment.closestPt handles setting refPosition
        agent.Velocity = velocity;
        return true;
      }
      if (position.X <= minX)
      {
        position.X = minX;
        velocity.X *= -1;
        agent.Velocity = velocity;
        return true;
      }
      if (position.Y >= maxY)
      {
        position.Y = maxY;
        velocity.Y *= -1;
        agent.Velocity = velocity;
        return true;
      }
      if (position.Y <= minY)
      {
        position.Y = minY;
        velocity.Y *= -1;
        agent.Velocity = velocity;
        return true;
      }
      return false;
    }

    public override BoundingBox GetBoundingBox()
    {
      return environment.GetBoundingBox(false);
    }
  }
}
