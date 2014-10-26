using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public class WorldBoxEnvironmentType : EnvironmentType
  {

    private Box environment;
    private bool wrap;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public WorldBoxEnvironmentType()
      : base()
    {
      Interval interval = new Interval(-100.0, 100.0);
      this.environment = new Box(Plane.WorldXY, interval, interval, interval);
      this.wrap = false;
    }

    // Constructor with initial values.
    public WorldBoxEnvironmentType(Box box, bool wrap)
    {
      this.environment = box;
      this.wrap = wrap;
    }

    // Copy Constructor
    public WorldBoxEnvironmentType(WorldBoxEnvironmentType environment)
    {
      this.environment = environment.environment;
      this.wrap = environment.wrap;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      WorldBoxEnvironmentType p = obj as WorldBoxEnvironmentType;
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.environment.Equals(p.environment) && this.wrap.Equals(p.wrap);
    }

    public bool Equals(WorldBoxEnvironmentType p)
    {
      return base.Equals((WorldBoxEnvironmentType)p) && this.environment.Equals(p.environment) && this.wrap.Equals(p.wrap);
    }

    public override int GetHashCode()
    {
      return this.environment.GetHashCode() ^ this.wrap.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new WorldBoxEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (this.environment.IsValid) && 
          environment.Plane.XAxis.Equals(Plane.WorldXY.XAxis) &&
          environment.Plane.YAxis.Equals(Plane.WorldXY.YAxis);
      }

    }

    public override string ToString()
    {

      string box = "Box: " + this.environment.ToString() + "\n";
      string wrap = "Wrap: " + this.wrap.ToString() + "\n";
      return box + wrap;
    }

    public override string TypeDescription
    {
      get { return "A World Box Environment"; }
    }

    public override string TypeName
    {
      get { return "World Box Environment"; }
    }


    public override Point3d closestPoint(Point3d pt)
    {
      return this.environment.ClosestPoint(pt);
    }

    public override Point3d closestRefPoint(Point3d pt) {
      return this.environment.ClosestPoint(pt);
    }

    public override Point3d closestRefPointOnRef(Point3d pt)
    {
      return this.environment.ClosestPoint(pt);
    }

    public override Point3d closestPointOnRef(Point3d pt)
    {
      return this.environment.ClosestPoint(pt);
    }

    public override Vector3d avoidEdges(AgentType agent, double distance)
    {
      Vector3d steer = new Vector3d();
      double minX = environment.BoundingBox.Corner(true, false, false).X;
      double maxX = environment.BoundingBox.Corner(false, false, false).X;
      double minY = environment.BoundingBox.Corner(false, true, false).Y;
      double maxY = environment.BoundingBox.Corner(false, false, false).Y;
      double minZ = environment.BoundingBox.Corner(false, false, true).Z;
      double maxZ = environment.BoundingBox.Corner(false, false, false).Z;

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

      if (agent.RefPosition.Z < minZ + distance)
      {
        desired = new Vector3d(velocity.X, velocity.Y, maxSpeed);
      }
      else if (agent.RefPosition.Z > maxZ - distance)
      {
        desired = new Vector3d(velocity.X, velocity.Y, -maxSpeed);
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
