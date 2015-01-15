using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using OctreeSearch;

namespace Agent
{
  public abstract class ForceType : GH_Goo<Object>
  {
    protected double weight;
    protected double visionRadiusMultiplier;

    // Default Constructor
    public ForceType()
    {
      this.weight = 1.0;
      this.visionRadiusMultiplier = 1.0;
    }

    // Constructor with initial values
    public ForceType(double weight, double visionRadiusMultiplier)
    {
      this.weight = weight;
      this.visionRadiusMultiplier = visionRadiusMultiplier;
    }

    // Copy Constructor
    public ForceType(ForceType force)
    {
      this.weight = force.weight;
      this.visionRadiusMultiplier = force.visionRadiusMultiplier;
    }

    public abstract Vector3d calcForce(AgentType agent, ISpatialCollection<AgentType> neighbors);

    protected Vector3d calcSum(AgentType agent, IList<AgentType> agents, out int count)
    {
      Vector3d sum = new Vector3d();
      count = 0;
      foreach (AgentType other in agents)
      {
        double d = agent.Position.DistanceTo(other.Position);
        //double d = Vector3d.Subtract(agent.Position, other.Position).Length;
        //if we are not comparing the seeker to iteself and it is at least
        //desired separation away:
        if ((d > 0) && (d < agent.VisionRadius * this.visionRadiusMultiplier))
        {
          Vector3d diff = Point3d.Subtract(agent.Position, other.Position);
          diff.Unitize();

          //Weight the magnitude by distance to other
          diff = Vector3d.Divide(diff, d);

          sum = Vector3d.Add(sum, diff);

          //For an average, we need to keep track of how many boids
          //are in our vision.
          count++;
        }
      }
      return sum;
    }

    protected Vector3d seek(AgentType agent, Vector3d target)
    {
      Vector3d desired = Vector3d.Subtract(target, new Vector3d(agent.Position));
      desired.Unitize();
      desired = Vector3d.Multiply(desired, agent.MaxSpeed);

      //Seek the average position of our neighbors.
      Vector3d steer = Vector3d.Subtract(desired, agent.Velocity);

      if (steer.Length > agent.MaxForce)
      {
        steer.Unitize();
        steer = Vector3d.Multiply(steer, agent.MaxForce);
      }

      //Multiply the resultant vector by weight
      steer = Vector3d.Multiply(this.weight, steer);
      return steer;
    }

    public static Vector3d limit(Vector3d vec, double max)
    {
      if (vec.Length > max)
      {
        vec.Unitize();
        vec = Vector3d.Multiply(vec, max);
      }
      return vec;
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      ForceType p = obj as ForceType;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.weight.Equals(p.weight) &&
             this.visionRadiusMultiplier.Equals(p.visionRadiusMultiplier);
    }

    public bool Equals(ForceType p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.weight.Equals(p.weight) &&
             this.visionRadiusMultiplier.Equals(p.visionRadiusMultiplier);
    }

    public override int GetHashCode()
    {
      return this.weight.GetHashCode() ^ this.visionRadiusMultiplier.GetHashCode();
    }

    public abstract override IGH_Goo Duplicate();

    public override bool IsValid
    {
      get
      {
        return (0 <= this.visionRadiusMultiplier && this.visionRadiusMultiplier <= 1) &&
               (-1 <= this.weight && this.weight <= 1);
      }
    }

    public override string ToString()
    {
      string weight = "Weight: " + this.weight.ToString() + "\n";
      string visionRadiusMultiplier = "Vision Radius Multiplier: " + this.visionRadiusMultiplier.ToString() + "\n";
      return weight + visionRadiusMultiplier;
    }

    public override string TypeDescription
    {
      get { return "A Force"; }
    }

    public override string TypeName
    {
      get { return "Force"; }
    }
  }
}
