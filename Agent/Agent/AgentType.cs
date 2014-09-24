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

    private Vector3d location;
    private Vector3d velocity;
    private Vector3d acceleration;

    public AgentType()
    {
      this.lifespan = 30;
      this.mass = 1.0;
      this.bodySize = 1.0;
      this.maxSpeed = 1.0;
      this.maxForce = 1.0;
      this.visionAngle = 15.0;
      this.visionRadius = 5.0;
      this.historyLength = 0;
      this.location = Vector3d.Zero;
      this.velocity = Util.Random.RandomVector(-0.5, 0.5);
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

      this.location = Vector3d.Zero;
      this.velocity = Util.Random.RandomVector(-0.5, 0.5);
      this.acceleration = Vector3d.Zero;
    }

    public AgentType(int lifespan, double mass, double bodySize,
                     double maxSpeed, double maxForce, double visionAngle,
                     double visionRadius, int historyLength, Vector3d location)
    {
      this.lifespan = lifespan;
      this.mass = mass;
      this.bodySize = bodySize;
      this.maxSpeed = maxSpeed;
      this.maxForce = maxForce;
      this.visionAngle = visionAngle;
      this.visionRadius = visionRadius;
      this.historyLength = historyLength;

      this.location = location;
      this.velocity = Util.Random.RandomVector(-0.5, 0.5);
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

      this.location = agent.location;
      this.velocity = agent.velocity;
      this.acceleration = agent.acceleration;
    }

    public AgentType(AgentType agent, Vector3d location)
    {
      this.lifespan = agent.lifespan;
      this.mass = agent.mass;
      this.bodySize = agent.bodySize;
      this.maxSpeed = agent.maxSpeed;
      this.maxForce = agent.maxForce;
      this.visionAngle = agent.visionAngle;
      this.visionRadius = agent.visionRadius;
      this.historyLength = agent.historyLength;

      this.location = location;
      this.velocity = agent.velocity;
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

    public void update()
    {
      velocity = Vector3d.Add(velocity, acceleration);
      location = Vector3d.Add(location, velocity);
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
