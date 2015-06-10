using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
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
      Width = maxX - minX;
      Height = maxY - minY;
      Depth = maxZ - minZ;
    }

    // Constructor with initial values.
    public AxisAlignedBoxEnvironmentType(Box box, bool wrap)
    {
      environment = box;
      Wrap = wrap;
      BoundingBox boundingBox = environment.BoundingBox;
      minX = boundingBox.Corner(true, false, false).X;
      maxX = boundingBox.Corner(false, false, false).X;
      minY = boundingBox.Corner(false, true, false).Y;
      maxY = boundingBox.Corner(false, false, false).Y;
      minZ = boundingBox.Corner(false, false, true).Z;
      maxZ = boundingBox.Corner(false, false, false).Z;
      Width = maxX - minX;
      Height = maxY - minY;
      Depth = maxZ - minZ;
    }

    // Copy Constructor
    public AxisAlignedBoxEnvironmentType(AxisAlignedBoxEnvironmentType environment)
    {
      this.environment = environment.environment;
      this.Wrap = environment.Wrap;
      minX = environment.minX;
      maxX = environment.maxX;
      minY = environment.minY;
      maxY = environment.maxY;
      minZ = environment.minZ;
      maxZ = environment.maxZ;
      Width = maxX - minX;
      Height = maxY - minY;
      Depth = maxZ - minZ;
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

    public override Point3d MapTo2D(Point3d pt) {
      return environment.ClosestPoint(pt);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      return environment.ClosestPoint(pt);
    }

    public override Point3d MapTo3D(Point3d pt)
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

      if (refPosition.Z < minZ + distance)
      {
        //desired = new Vector3d(velocity.X, maxSpeed, velocity.Z);
        desired.Z = maxSpeed;
        avoided = true;
      }
      else if (refPosition.Z >= maxZ - distance)
      {
        //desired = new Vector3d(velocity.X, -maxSpeed, velocity.Z);
        desired.Z = -maxSpeed;
        avoided = true;
      }

      if (avoided) return desired;
      return Vector3d.Zero;
    }

    public override bool BounceContain(IParticle agent)
    {
      double d = agent.BodySize/2;
      bool bounced = false;
      Point3d position = agent.Position;
      Vector3d velocity = agent.Velocity;
      if (position.X+d >= maxX)
      {
        position.X = maxX-d;
        velocity.X *= -1;
        agent.Position = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      else if (position.X-d <= minX)
      {
        position.X = minX+d;
        velocity.X *= -1;
        agent.Position = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      if (position.Y+d >= maxY)
      {
        position.Y = maxY-d;
        velocity.Y *= -1;
        agent.Position = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      else if (position.Y-d <= minY)
      {
        position.Y = minY+d;
        velocity.Y *= -1;
        agent.Position = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      if (position.Z+d >= maxZ)
      {
        position.Z = maxZ-d;
        velocity.Z *= -1;
        agent.Position = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      else if (position.Z-d <= minZ)
      {
        position.Z = minZ+d;
        velocity.Z *= -1;
        agent.Position = position;
        agent.Velocity = velocity;
        bounced = true;
      }
      return bounced;
    }

    public override Point3d WrapPoint(Point3d position, out bool wrapped)
    {
      //Point3d position = quelea.Position;
      wrapped = false;
      if (position.X >= maxX)
      {
        position.X -= Width;
        wrapped = true;
      }
      if (position.X < minX)
      {
        position.X += Width;
        wrapped = true;
      }
      if (position.Y >= maxY)
      {
        position.Y -= Height;
        wrapped = true;
      }
      if (position.Y < minY)
      {
        position.Y += Height;
        wrapped = true;
      }
      if (position.Z >= maxZ)
      {
        position.Z -= Depth;
        wrapped = true;
      }
      if (position.Z < minZ)
      {
        position.Z += Depth;
        wrapped = true;
      }
      //return wrapped;
      return position;
    }

    public override BoundingBox GetBoundingBox()
    {
      return environment.BoundingBox;
    }

    public override bool Contains(Point3d pt)
    {
      return (pt.X < maxX) && (pt.X > minX) &&
             (pt.Y < maxY) && (pt.Y > minY) &&
             (pt.Z < maxZ) && (pt.Z > minZ);
    }
  }
}
