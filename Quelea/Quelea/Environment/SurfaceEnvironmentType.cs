using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SurfaceEnvironmentType : AbstractEnvironmentType
  {

    private readonly Surface environment;
    public Surface RefEnvironment { get; private set; }

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
      RefEnvironment = NurbsSurface.CreateFromCorners(pt1, pt2, pt3);

      Interval uDom = RefEnvironment.Domain(0);
      Interval vDom = RefEnvironment.Domain(1);

      minX = uDom.Min;
      maxX = uDom.Max;
      minY = vDom.Min;
      maxY = vDom.Max;

      Width = maxX - minX;
      Height = maxY - minY;
    }

    // Constructor with initial values.
    public SurfaceEnvironmentType(Surface srf, bool wrap)
    {
      environment = srf;
      Wrap = wrap;
      double width, height;
      srf.GetSurfaceSize(out width, out height);
      Width = width;
      Height = height;

      Interval widthInterval = new Interval(0, width);
      Interval heightInterval = new Interval(0, height);

      Interval u = srf.Domain(0);
      Interval v = srf.Domain(1);
      
      Surface refEnvironment = new PlaneSurface(Plane.WorldXY, widthInterval, heightInterval);
      refEnvironment.SetDomain(0, u);
      refEnvironment.SetDomain(1, v);
      RefEnvironment = refEnvironment;

      Interval uDom = RefEnvironment.Domain(0);
      Interval vDom = RefEnvironment.Domain(1);

      minX = 0;
      maxX = width;
      minY = 0;
      maxY = height;
    }

    // Copy Constructor
    public SurfaceEnvironmentType(SurfaceEnvironmentType environment)
    {
      this.environment = environment.environment;
      Width = environment.Width;
      Height = environment.Height;
      Wrap = environment.Wrap;
      RefEnvironment = environment.RefEnvironment;

      Interval uDom = RefEnvironment.Domain(0);
      Interval vDom = RefEnvironment.Domain(1);

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
      return environment.GetHashCode() ^ RefEnvironment.GetHashCode();
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

    public override Point3d MapTo2D(Point3d pt)
    {
      double u, v;
      environment.ClosestPoint(pt, out u, out v);
      return RefEnvironment.PointAt(u, v);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      double u, v;
      RefEnvironment.ClosestPoint(pt, out u, out v);
      return RefEnvironment.PointAt(u, v);
    }

    public override Point3d MapTo3D(Point3d pt)
    {
      double u, v;
      RefEnvironment.ClosestPoint(pt, out u, out v);
      return environment.PointAt(u, v);
    }

    public override Vector3d ClosestNormal(Point3d pt)
    {
      double u, v;
      environment.ClosestPoint(pt, out u, out v);
      return environment.NormalAt(u, v);
    }

    public override Vector3d AvoidEdges(IAgent agent, double distance)
    {
      Point3d refPosition = agent.Position;
      double maxSpeed = agent.MaxSpeed;
      Vector3d velocity = agent.Velocity;
      bool avoided = false;

      Vector3d desired = velocity;

      if (refPosition.X < minX + distance)
      {
        //desired = new Vector3d(maxSpeed, velocity.Y, velocity.Z);
        desired.X = maxSpeed;
        avoided = true;
      }
      else if (refPosition.X >= maxX - distance)
      {
        //desired = new Vector3d(-maxSpeed, velocity.Y, velocity.Z);
        desired.X = -maxSpeed;
        avoided = true;
      }

      if (refPosition.Y < minY + distance)
      {
        //desired = new Vector3d(velocity.X, maxSpeed, velocity.Z);
        desired.Y = maxSpeed;
        avoided = true;
      }
      else if (refPosition.Y >= maxY - distance)
      {
        //desired = new Vector3d(velocity.X, -maxSpeed, velocity.Z);
        desired.Y = -maxSpeed;
        avoided = true;
      }
      if (avoided) return desired;
      return Vector3d.Zero;
      //return desired;
    }

    public override bool BounceContain(IParticle agent)
    {
      Point3d position = agent.Position;
      Vector3d velocity = agent.Velocity;
      if (position.X >= maxX)
      {
        position.X = maxX;
        velocity.X *= -1;
        //system.environment.closestPt handles setting refPosition
        agent.Velocity = velocity;
        return true;
      }
      if (position.X < minX)
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
      if (position.Y < minY)
      {
        position.Y = minY;
        velocity.Y *= -1;
        agent.Velocity = velocity;
        return true;
      }
      return false;
    }

    public override Point3d WrapPoint(Point3d position, out bool wrapped)
    {
      wrapped = false;
      Point3d wrappedPoint = position;
      if (wrappedPoint.X >= maxX)
      {
        wrappedPoint.X -= Width;
        wrapped = true;
      }
      if (wrappedPoint.X <= minX)
      {
        wrappedPoint.X += Width;
        wrapped = true;
      }
      if (wrappedPoint.Y >= maxY)
      {
        wrappedPoint.Y -= Height;
        wrapped = true;
      }
      if (wrappedPoint.Y <= minY)
      {
        wrappedPoint.Y += Height;
        wrapped = true;
      }
      return wrappedPoint;
    }

    public override BoundingBox GetBoundingBox()
    {
      return RefEnvironment.GetBoundingBox(false);
    }

    public override bool Contains(Point3d pt)
    {
      return (pt.X < maxX) && (pt.X > minX) &&
             (pt.Y < maxY) && (pt.Y > minY);
    }
  }
}
