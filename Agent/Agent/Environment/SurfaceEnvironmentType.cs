using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Collections.Generic;

namespace Agent
{
  public class SurfaceEnvironmentType : EnvironmentType
  {

    private Surface environment;
    private Surface refEnvironment;

    private double minX;
    private double maxX;
    private double minY;
    private double maxY;

    // Default Constructor.
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

      Interval uDom = refEnvironment.Domain(0);
      Interval vDom = refEnvironment.Domain(1);

      this.minX = uDom.Min;
      this.maxX = uDom.Max;
      this.minY = vDom.Min;
      this.maxY = vDom.Max;
    }

    // Constructor with initial values.
    public SurfaceEnvironmentType(Surface srf)
    {
      this.environment = srf;
      Interval u = srf.Domain(0);
      Interval v = srf.Domain(1);
      this.refEnvironment = new PlaneSurface(Plane.WorldXY, u, v);

      Interval uDom = refEnvironment.Domain(0);
      Interval vDom = refEnvironment.Domain(1);

      this.minX = uDom.Min;
      this.maxX = uDom.Max;
      this.minY = vDom.Min;
      this.maxY = vDom.Max;
    }

    // Copy Constructor
    public SurfaceEnvironmentType(SurfaceEnvironmentType environment)
    {
      this.environment = environment.environment;

      Interval uDom = environment.refEnvironment.Domain(0);
      Interval vDom = environment.refEnvironment.Domain(1);

      this.minX = uDom.Min;
      this.maxX = uDom.Max;
      this.minY = vDom.Min;
      this.maxY = vDom.Max;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      SurfaceEnvironmentType p = obj as SurfaceEnvironmentType;
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.environment.Equals(p.environment);
    }

    public bool Equals(SurfaceEnvironmentType p)
    {
      return base.Equals((EnvironmentType)p) && this.environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return this.environment.GetHashCode() ^ this.refEnvironment.GetHashCode();
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

      string environment = "Surface: " + this.environment.ToString() + "\n";
      return environment;
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

    public override bool bounceContain(AgentType agent)
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
      else if (position.X <= minX)
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
      else if (position.Y <= minY)
      {
        position.Y = minY;
        velocity.Y *= -1;
        agent.Velocity = velocity;
        return true;
      }
      return false;
    }
  }
}
