using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AxisAlignedBoxEnvironmentType : AbstractEnvironmentType
  {
    private readonly double minX;
    private readonly double maxX;
    private readonly double minY;
    private readonly double maxY;
    private readonly double minZ;
    private readonly double maxZ;

    private Box environment;

    // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
    public AxisAlignedBoxEnvironmentType()
    {
      Interval interval = new Interval(-RS.boxBoundsDefault, RS.boxBoundsDefault);
      environment = new Box(Plane.WorldXY, interval, interval, interval);
      BoundingBox boundingBox = environment.BoundingBox;
      minX = boundingBox.Corner(true, false, false).X;
      maxX = boundingBox.Corner(false, false, false).X;
      minY = boundingBox.Corner(false, true, false).Y;
      maxY = boundingBox.Corner(false, false, false).Y;
      minZ = boundingBox.Corner(false, false, true).Z;
      maxZ = boundingBox.Corner(false, false, false).Z;
    }

    // Constructor with initial values.
    public AxisAlignedBoxEnvironmentType(Box box)
    {
      environment = box;
      BoundingBox boundingBox = environment.BoundingBox;
      minX = boundingBox.Corner(true, false, false).X;
      maxX = boundingBox.Corner(false, false, false).X;
      minY = boundingBox.Corner(false, true, false).Y;
      maxY = boundingBox.Corner(false, false, false).Y;
      minZ = boundingBox.Corner(false, false, true).Z;
      maxZ = boundingBox.Corner(false, false, false).Z;
    }

    // Copy Constructor
    public AxisAlignedBoxEnvironmentType(AxisAlignedBoxEnvironmentType environment)
    {
      this.environment = environment.environment;
      minX = environment.minX;
      maxX = environment.maxX;
      minY = environment.minY;
      maxY = environment.maxY;
      minZ = environment.minZ;
      maxZ = environment.maxZ;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to ThreeDPoint return false:
      AxisAlignedBoxEnvironmentType p = obj as AxisAlignedBoxEnvironmentType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj) && environment.Equals(p.environment);
    }

    public bool Equals(AxisAlignedBoxEnvironmentType p)
    {
      return base.Equals(p) && environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return (int) (minX * minY * minZ * maxX * maxY * maxZ);
    }

    public override IGH_Goo Duplicate()
    {
      return new AxisAlignedBoxEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (environment.IsValid) && 
          environment.Plane.XAxis.Equals(Plane.WorldXY.XAxis) &&
          environment.Plane.YAxis.Equals(Plane.WorldXY.YAxis);
      }

    }

    public override string ToString()
    {

      string environmentStr = RS.AABoxEnvName + ": " + environment + "\n";
      return environmentStr;
    }

    public override string TypeDescription
    {
      get { return RS.AABoxEnvDescription; }
    }

    public override string TypeName
    {
      get { return RS.AABoxEnvName; }
    }


    public override Point3d ClosestPoint(Point3d pt)
    {
      return environment.ClosestPoint(pt);
    }

    public override Point3d ClosestRefPoint(Point3d pt) {
      return environment.ClosestPoint(pt);
    }

    public override Point3d ClosestRefPointOnRef(Point3d pt)
    {
      return environment.ClosestPoint(pt);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      return environment.ClosestPoint(pt);
    }

    public override Vector3d ClosestNormal(Point3d pt)
    {
      Brep brepEnv = environment.ToBrep();
      Point3d closestPoint;
      ComponentIndex componentIndex;
      double s;
      double t;
      const double maxDist = 100;
      Vector3d normal;
      brepEnv.ClosestPoint(pt, out closestPoint, out componentIndex, out s, out t, maxDist, out normal);
      return normal;
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

    public override bool BounceContain(IParticle agent)
    {
      bool bounced = false;
      Point3d position = agent.RefPosition;
      Vector3d velocity = agent.Velocity;
      if (position.X >= maxX)
      {
        position.X = maxX;
        velocity.X *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      else if (position.X <= minX)
      {
        position.X = minX;
        velocity.X *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      if (position.Y >= maxY)
      {
        position.Y = maxY;
        velocity.Y *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      else if (position.Y <= minY)
      {
        position.Y = minY;
        velocity.Y *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      if (position.Z >= maxZ)
      {
        position.Z = maxZ;
        velocity.Z *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      else if (position.Z <= minZ)
      {
        position.Z = minZ;
        velocity.Z *= -1;
        agent.RefPosition = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      return bounced;
    }

    public override BoundingBox GetBoundingBox()
    {
      return environment.BoundingBox;
    }
  }
}
