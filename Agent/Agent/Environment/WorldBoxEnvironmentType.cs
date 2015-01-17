using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Collections.Generic;

namespace Agent
{
  public class WorldBoxEnvironmentType : EnvironmentType
  {
    private double minX;
    private double maxX;
    private double minY;
    private double maxY;
    private double minZ;
    private double maxZ;

    private Box environment;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public WorldBoxEnvironmentType()
      : base()
    {
      Interval interval = new Interval(-100.0, 100.0);
      this.environment = new Box(Plane.WorldXY, interval, interval, interval);
      BoundingBox boundingBox = environment.BoundingBox;
      this.minX = boundingBox.Corner(true, false, false).X;
      this.maxX = boundingBox.Corner(false, false, false).X;
      this.minY = boundingBox.Corner(false, true, false).Y;
      this.maxY = boundingBox.Corner(false, false, false).Y;
      this.minZ = boundingBox.Corner(false, false, true).Z;
      this.maxZ = boundingBox.Corner(false, false, false).Z;
    }

    // Constructor with initial values.
    public WorldBoxEnvironmentType(Box box)
    {
      this.environment = box;
      BoundingBox boundingBox = environment.BoundingBox;
      this.minX = boundingBox.Corner(true, false, false).X;
      this.maxX = boundingBox.Corner(false, false, false).X;
      this.minY = boundingBox.Corner(false, true, false).Y;
      this.maxY = boundingBox.Corner(false, false, false).Y;
      this.minZ = boundingBox.Corner(false, false, true).Z;
      this.maxZ = boundingBox.Corner(false, false, false).Z;
    }

    // Copy Constructor
    public WorldBoxEnvironmentType(WorldBoxEnvironmentType environment)
    {
      this.environment = environment.environment;
      this.minX = environment.minX;
      this.maxX = environment.maxX;
      this.minY = environment.minY;
      this.maxY = environment.maxY;
      this.minZ = environment.minZ;
      this.maxZ = environment.maxZ;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      WorldBoxEnvironmentType p = obj as WorldBoxEnvironmentType;
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.environment.Equals(p.environment);
    }

    public bool Equals(WorldBoxEnvironmentType p)
    {
      return base.Equals((EnvironmentType)p) && this.environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return this.environment.GetHashCode();
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

      string environment = "Box: " + this.environment.ToString() + "\n";
      return environment;
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

      if (agent.RefPosition.Z < minZ + distance)
      {
        desired = new Vector3d(velocity.X, velocity.Y, maxSpeed);
      }
      else if (agent.RefPosition.Z > maxZ - distance)
      {
        desired = new Vector3d(velocity.X, velocity.Y, -maxSpeed);
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
        agent.RefPosition = position;
        agent.Velocity = velocity;
        return true;
      }
      else if (position.X <= minX)
      {
        position.X = minX;
        velocity.X *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        return true;
      }
      if (position.Y >= maxY)
      {
        position.Y = maxY;
        velocity.Y *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        return true;
      }
      else if (position.Y <= minY)
      {
        position.Y = minY;
        velocity.Y *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        return true;
      }
      if (position.Z >= maxZ)
      {
        position.Z = maxZ;
        velocity.Z *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        return true;
      }
      else if (position.Z <= minZ)
      {
        position.Z = minZ;
        velocity.Z *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        return true;
      }
      return false;
    }

    public override BoundingBox getBoundingBox()
    {
      return this.environment.BoundingBox;
    }
  }
}
