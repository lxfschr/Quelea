using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Agent
{
  public class AgentType : GH_Goo<Object>
  {
    private int lifespan;
    private double mass;
    private double bodySize;
    private double maxSpeed;
    private double maxForce;
    private double visionAngle;
    private double visionRadius;
    private int historyLength;

    private Point3d position;
    private Vector3d velocity;
    private Vector3d acceleration;

    private Point3d refPosition;

    public AgentType()
    {
      this.lifespan = 30;
      this.mass = 1.0;
      this.bodySize = 1.0;
      this.maxSpeed = 0.5;
      this.maxForce = 0.1;
      this.visionAngle = 15.0;
      this.visionRadius = 5.0;
      this.historyLength = 0;
      this.position = Point3d.Origin;
      this.refPosition = Point3d.Origin;
      this.velocity = Util.Random.RandomVector(-0.1, 0.1);
      this.acceleration = Vector3d.Zero;
    }

    public AgentType(int lifespan, double mass, double bodySize,
                     double maxSpeed, double maxForce, double visionAngle,
                     double visionRadius, int historyLength)
    {
      this.lifespan = lifespan;
      this.mass = mass;
      this.bodySize = bodySize;
      this.maxSpeed = maxSpeed;
      this.maxForce = maxForce;
      this.visionAngle = visionAngle;
      this.visionRadius = visionRadius;
      this.historyLength = historyLength;

      this.position = Point3d.Origin;
      this.refPosition = Point3d.Origin;
      this.velocity = Util.Random.RandomVector(-0.1, 0.1);
      this.acceleration = Vector3d.Zero;
    }

    public AgentType(int lifespan, double mass, double bodySize,
                     double maxSpeed, double maxForce, double visionAngle,
                     double visionRadius, int historyLength, Point3d position)
    {
      this.lifespan = lifespan;
      this.mass = mass;
      this.bodySize = bodySize;
      this.maxSpeed = maxSpeed;
      this.maxForce = maxForce;
      this.visionAngle = visionAngle;
      this.visionRadius = visionRadius;
      this.historyLength = historyLength;

      this.position = position;
      this.refPosition = position;
      this.velocity = Util.Random.RandomVector(-0.1, 0.1);
      this.acceleration = Vector3d.Zero;
    }

    public AgentType(AgentType agent)
    {
      this.lifespan = agent.lifespan;
      this.mass = agent.mass;
      this.bodySize = agent.bodySize;
      this.maxSpeed = agent.maxSpeed;
      this.maxForce = agent.maxForce;
      this.visionAngle = agent.visionAngle;
      this.visionRadius = agent.visionRadius;
      this.historyLength = agent.historyLength;

      this.position = agent.position;
      this.refPosition = agent.refPosition;
      this.velocity = agent.velocity;
      this.acceleration = agent.acceleration;
    }

    public AgentType(AgentType agent, Point3d position)
    {
      this.lifespan = agent.lifespan;
      this.mass = agent.mass;
      this.bodySize = agent.bodySize;
      this.maxSpeed = agent.maxSpeed;
      this.maxForce = agent.maxForce;
      this.visionAngle = agent.visionAngle;
      this.visionRadius = agent.visionRadius;
      this.historyLength = agent.historyLength;

      this.refPosition = position;
      this.position = position;
      this.velocity = this.velocity = Util.Random.RandomVector(-0.1, 0.1);
      this.acceleration = agent.acceleration;
    }

    public AgentType(AgentType agent, Point3d position, Point3d refPosition)
    {
      this.lifespan = agent.lifespan;
      this.mass = agent.mass;
      this.bodySize = agent.bodySize;
      this.maxSpeed = agent.maxSpeed;
      this.maxForce = agent.maxForce;
      this.visionAngle = agent.visionAngle;
      this.visionRadius = agent.visionRadius;
      this.historyLength = agent.historyLength;

      this.position = position;
      this.refPosition = refPosition;
      this.velocity = this.velocity = Util.Random.RandomVector(-0.1, 0.1);
      this.acceleration = agent.acceleration;
    }

    public int Lifespan
    {
      get
      {
        return this.lifespan;
      }
    }

    public double Mass
    {
      get
      {
        return this.mass;
      }
    }

    public double BodySize
    {
      get
      {
        return this.bodySize;
      }
    }

    public double MaxSpeed
    {
      get
      {
        return this.maxSpeed;
      }
    }

    public double MaxForce
    {
      get
      {
        return this.maxForce;
      }
    }

    public double VisionAngle
    {
      get
      {
        return this.visionAngle;
      }
    }

    public double VisionRadius
    {
      get
      {
        return this.visionRadius;
      }
    }

    public int HistoryLength
    {
      get
      {
        return this.historyLength;
      }
    }

    public Point3d Position 
    {
      get
      {
        return this.position;
      }
      set
      {
        this.position = value;
      }
    }

    public Vector3d Velocity
    {
      get
      {
        return this.velocity;
      }
      set
      {
        this.velocity = value;
      }
    }

    public Vector3d Acceleration
    {
      get
      {
        return this.acceleration;
      }
    }

    public Point3d RefPosition
    {
      get
      {
        return this.refPosition;
      }
      set
      {
        this.refPosition = value;
      }
    }

    public void update()
    {
      velocity = Vector3d.Add(velocity, acceleration);
      refPosition.Transform(Transform.Translation(velocity));
      position.Transform(Transform.Translation(velocity)); //So disconnecting the environment allows the agent to continue from its current position.
      acceleration = Vector3d.Multiply(acceleration, 0);
      lifespan -= 1;

    }

    public void applyForce(Vector3d force)
    {
      Vector3d f = force;
      f = Vector3d.Divide(f, mass);
      acceleration = Vector3d.Add(acceleration, f);
    }

    public Boolean isDead()
    {
      return (lifespan <= 0.0);
    }

    public void run()
    {
      update();
    }

    public override bool Equals(System.Object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      AgentType p = obj as AgentType;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.acceleration.Equals(p.acceleration) &&
             this.bodySize.Equals(p.bodySize) &&
             this.historyLength.Equals(p.historyLength) &&
             this.lifespan.Equals(p.lifespan) &&
             this.position.Equals(p.position) &&
             this.refPosition.Equals(p.refPosition) &&
             this.mass.Equals(p.mass) &&
             this.maxForce.Equals(p.maxForce) &&
             this.maxSpeed.Equals(p.maxSpeed) &&
             this.velocity.Equals(p.velocity) &&
             this.visionAngle.Equals(p.visionAngle) &&
             this.visionRadius.Equals(p.visionRadius);
    }

    public bool Equals(AgentType p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.acceleration.Equals(p.acceleration) &&
             this.bodySize.Equals(p.bodySize) &&
             this.historyLength.Equals(p.historyLength) &&
             this.lifespan.Equals(p.lifespan) &&
             this.position.Equals(p.position) &&
             this.refPosition.Equals(p.refPosition) &&
             this.mass.Equals(p.mass) &&
             this.maxForce.Equals(p.maxForce) &&
             this.maxSpeed.Equals(p.maxSpeed) &&
             this.velocity.Equals(p.velocity) &&
             this.visionAngle.Equals(p.visionAngle) &&
             this.visionRadius.Equals(p.visionRadius);
    }

    public override int GetHashCode()
    {
      // Return true if the fields match:
      return this.acceleration.GetHashCode() ^
             this.bodySize.GetHashCode() ^
             this.historyLength.GetHashCode() ^
             this.lifespan.GetHashCode() ^
             this.position.GetHashCode() ^
             this.mass.GetHashCode() ^
             this.maxForce.GetHashCode() ^
             this.maxSpeed.GetHashCode() ^
             this.velocity.GetHashCode() ^
             this.visionAngle.GetHashCode() ^
             this.visionRadius.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new AgentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (lifespan > 0 && mass > 0 && bodySize >= 0 && maxForce >= 0 &&
                maxSpeed >= 0 && visionAngle >= 0 && visionRadius >= 0 &&
                historyLength >= 0);
      }
    }

    public override string ToString()
    {
      string lifespan = "Lifespan: " + this.lifespan.ToString() + "\n";
      string mass = "Mass: " + this.mass.ToString() + "\n";
      string bodySize = "Body Size: " + this.bodySize.ToString() + "\n";
      string maxForce = "Maximum Force: " + this.maxForce.ToString() + "\n";
      string maxSpeed = "Maximum Speed: " + this.maxSpeed.ToString() + "\n";
      string visionAngle = "Vision Angle: " + this.visionAngle.ToString() +
                           "\n";
      string visionRadius = "Vision Radius: " + this.visionRadius.ToString() +
                            "\n";
      string historyLength = "History Length: " + this.historyLength.ToString()
                             + "\n";
      return lifespan + mass + bodySize + maxForce + maxSpeed + visionAngle +
             visionRadius + historyLength;
    }

    public override string TypeDescription
    {
      get { return "An Agent"; }
    }

    public override string TypeName
    {
      get { return "Agent"; }
    }

    
  }
}
