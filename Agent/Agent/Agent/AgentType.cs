using System;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Random = Agent.Util.Random;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AgentType : IModifiableAgent
  {
    private int lifespan;
    private double mass;
    private double bodySize;
    private double maxSpeed;
    private double maxForce;
    private int historyLength;
    private readonly CircularArray<Point3d> positionHistory;

    private Point3d position;
    private Vector3d velocity;
    private Vector3d acceleration;

    private Point3d refPosition;

    public AgentType()
    {
      lifespan = RS.lifespanDefault;
      mass = RS.massDefault;
      bodySize = RS.bodySizeDefault;
      maxSpeed = RS.maxSpeedDefault;
      maxForce = RS.maxForceDefault;
      historyLength = RS.historyLenDefault;
      positionHistory = new CircularArray<Point3d>(historyLength);
      position = Point3d.Origin;
      refPosition = Point3d.Origin;
      positionHistory.Add(position);
      velocity = Random.RandomVector(-RS.velocityDefault, RS.velocityDefault);
      acceleration = Vector3d.Zero;
      InitialVelocitySet = false;
    }

    public AgentType(int lifespan, double mass, double bodySize,
                     double maxSpeed, double maxForce, int historyLength)
    {
      this.lifespan = lifespan;
      this.mass = mass;
      this.bodySize = bodySize;
      this.maxSpeed = maxSpeed;
      this.maxForce = maxForce;
      this.historyLength = historyLength;
      positionHistory = new CircularArray<Point3d>(historyLength);
      position = Point3d.Origin;
      refPosition = Point3d.Origin;
      positionHistory.Add(position);
      velocity = Random.RandomVector(-RS.velocityDefault, RS.velocityDefault);
      acceleration = Vector3d.Zero;
      InitialVelocitySet = false;
    }

    public AgentType(int lifespan, double mass, double bodySize,
                     double maxSpeed, double maxForce, int historyLength, Point3d position)
    {
      this.lifespan = lifespan;
      this.mass = mass;
      this.bodySize = bodySize;
      this.maxSpeed = maxSpeed;
      this.maxForce = maxForce;
      this.historyLength = historyLength;
      positionHistory = new CircularArray<Point3d>(historyLength);
      this.position = position;
      refPosition = position;
      positionHistory.Add(position);
      velocity = Random.RandomVector(-RS.velocityDefault, RS.velocityDefault);
      acceleration = Vector3d.Zero;
      InitialVelocitySet = false;
    }

    // Copy constructor
    public AgentType(AgentType agent)
    {
      lifespan = agent.lifespan;
      mass = agent.mass;
      bodySize = agent.bodySize;
      maxSpeed = agent.maxSpeed;
      maxForce = agent.maxForce;
      historyLength = agent.historyLength;
      positionHistory = new CircularArray<Point3d>(historyLength);
      position = agent.position;
      refPosition = agent.refPosition;
      positionHistory.Add(position);
      velocity = agent.velocity;
      acceleration = agent.acceleration;
      InitialVelocitySet = false;
    }

    public AgentType(IAgent agent, Point3d position)
    {
      lifespan = agent.Lifespan;
      mass = agent.Mass;
      bodySize = agent.BodySize;
      maxSpeed = agent.MaxSpeed;
      maxForce = agent.MaxForce;
      historyLength = agent.HistoryLength;
      positionHistory = new CircularArray<Point3d>(historyLength);
      refPosition = position;
      this.position = position;
      positionHistory.Add(position);
      velocity = velocity = Random.RandomVector(-RS.velocityDefault, RS.velocityDefault);
      acceleration = agent.Acceleration;
      InitialVelocitySet = false;
    }

    public AgentType(IAgent agent, Point3d position, Point3d refPosition)
    {
      lifespan = agent.Lifespan;
      mass = agent.Mass;
      bodySize = agent.BodySize;
      maxSpeed = agent.MaxSpeed;
      maxForce = agent.MaxForce;
      historyLength = agent.HistoryLength;
      positionHistory = new CircularArray<Point3d>(historyLength);
      this.position = position;
      this.refPosition = refPosition;
      positionHistory.Add(position);
      velocity = velocity = Random.RandomVector(-RS.velocityDefault, RS.velocityDefault);
      acceleration = agent.Acceleration;
      InitialVelocitySet = false;
    }

    public int Lifespan
    {
      get
      {
        return lifespan;
      }
      set { lifespan = value; }
    }

    public double Mass
    {
      get
      {
        return mass;
      }
      set { mass = value; }
    }

    public double BodySize
    {
      get
      {
        return bodySize;
      }
      set { bodySize = value; }
    }

    public double MaxSpeed
    {
      get
      {
        return maxSpeed;
      }
      set { maxSpeed = value; }
    }

    public double MaxForce
    {
      get
      {
        return maxForce;
      }
      set { maxForce = value; }
    }

    public int HistoryLength
    {
      get
      {
        return historyLength;
      }
      set { historyLength = value; }
    }

    public CircularArray<Point3d> PositionHistory
    {
      get
      {
        return positionHistory;
      }
    }

    public List<Point3d> GetPositionHistoryList()
    {
      return positionHistory.ToList();
    }

    public Point3d Position
    {
      get
      {
        return position;
      }
      set
      {
        position = value;
      }
    }

    public Vector3d Velocity
    {
      get
      {
        return velocity;
      }
      set
      {
        velocity = value;
      }
    }

    public Vector3d Acceleration
    {
      get
      {
        return acceleration;
      }
      set { acceleration = value; }
    }

    public Point3d RefPosition
    {
      get
      {
        return refPosition;
      }
      set
      {
        refPosition = value;
      }
    }

    public void Update()
    {
      velocity = Vector3d.Add(velocity, acceleration);
      refPosition.Transform(Transform.Translation(velocity));
      position.Transform(Transform.Translation(velocity)); //So disconnecting the environment allows the agent to continue from its current position.
      positionHistory.Add(position);
      acceleration = Vector3d.Multiply(acceleration, 0);
      lifespan -= 1;
    }

    public void ApplyForce(Vector3d force)
    {
      force = Vector3d.Divide(force, mass);
      acceleration = Vector3d.Add(acceleration, force);
    }

    public Boolean IsDead()
    {
      return (lifespan == 0);
    }

    public void Run()
    {
      Update();
    }

    public override bool Equals(Object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Point return false.
      AgentType p = obj as AgentType;
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return acceleration.Equals(p.acceleration) &&
             bodySize.Equals(p.bodySize) &&
             historyLength.Equals(p.historyLength) &&
             lifespan.Equals(p.lifespan) &&
             position.Equals(p.position) &&
             refPosition.Equals(p.refPosition) &&
             mass.Equals(p.mass) &&
             maxForce.Equals(p.maxForce) &&
             maxSpeed.Equals(p.maxSpeed) &&
             velocity.Equals(p.velocity);
    }

    public bool Equals(AgentType p)
    {
      // If parameter is null return false:
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return acceleration.Equals(p.acceleration) &&
             bodySize.Equals(p.bodySize) &&
             historyLength.Equals(p.historyLength) &&
             lifespan.Equals(p.lifespan) &&
             position.Equals(p.position) &&
             refPosition.Equals(p.refPosition) &&
             mass.Equals(p.mass) &&
             maxForce.Equals(p.maxForce) &&
             maxSpeed.Equals(p.maxSpeed) &&
             velocity.Equals(p.velocity);
    }

    public override int GetHashCode()
    {
      // Return true if the fields match:
      return bodySize.GetHashCode() ^
             historyLength.GetHashCode() ^
             mass.GetHashCode() ^
             maxForce.GetHashCode() ^
             maxSpeed.GetHashCode();
    }

    public IGH_Goo Duplicate()
    {
      return new AgentType(this);
    }

    public object ScriptVariable()
    {
      throw new NotImplementedException();
    }

    public bool IsValid
    {
      get
      {
        return (lifespan > 0 && mass > 0 && bodySize >= 0 && maxForce >= 0 &&
                maxSpeed >= 0 && historyLength >= 1);
      }
    }

    public string IsValidWhyNot
    {
      get { throw new NotImplementedException(); }
    }

    public override string ToString()
    {
      string lifespanStr = RS.lifespanName + ": " + lifespan + "\n";
      string massStr = RS.massName + ": " + mass + "\n";
      string bodySizeStr = RS.bodySizeName + ": " + bodySize + "\n";
      string maxForceStr = RS.maxForceName + ": " + maxForce + "\n";
      string maxSpeedStr = RS.maxSpeedName + ": " + maxSpeed + "\n";
      string historyLengthStr = RS.historyLenName + ": " + historyLength + "\n";
      return lifespanStr + massStr + bodySizeStr + maxForceStr + maxSpeedStr
             + historyLengthStr;
    }

    public IGH_GooProxy EmitProxy()
    {
      throw new NotImplementedException();
    }

    public bool CastFrom(object source)
    {
      throw new NotImplementedException();
    }

    public bool CastTo<T>(out T target)
    {
      throw new NotImplementedException();
    }

    public string TypeDescription
    {
      get { return RS.agentDescription; }
    }

    public string TypeName
    {
      get { return RS.agentName; }
    }

    public Point3d GetPoint3D()
    {
      return refPosition;
    }

    public bool InitialVelocitySet { get; set; }
    public bool Write(GH_IWriter writer)
    {
      throw new NotImplementedException();
    }

    public bool Read(GH_IReader reader)
    {
      throw new NotImplementedException();
    }
  }
}