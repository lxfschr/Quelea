using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel.Types;
using Rhino.Geometry;

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

    public abstract Vector3d calcForce(AgentType agent, List<AgentType> agents);

    protected Vector3d seek(AgentType agent, Vector3d target)
    {
      Vector3d desired = Vector3d.Subtract(target, agent.Location);
      desired.Unitize();
      desired = Vector3d.Multiply(desired, agent.MaxSpeed);
      Vector3d steer = Vector3d.Subtract(desired, agent.Velocity);

      if (steer.Length > agent.MaxForce)
      {
        steer.Unitize();
        steer = Vector3d.Multiply(steer, agent.MaxForce);
      }
      return steer;
    }

    protected Vector3d limit(Vector3d vec, double max)
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
