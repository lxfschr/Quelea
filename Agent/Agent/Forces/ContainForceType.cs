using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Agent
{
  public class ContainForceType : ForceType
  {

    private EnvironmentType environment;
    public ContainForceType()
      : base()
    {
      this.environment = null;
    }

    // Constructor with initial values
    public ContainForceType(double weight, double visionRadiusMultiplier, 
                               EnvironmentType environment)
      : base(weight, visionRadiusMultiplier)
    {
      this.environment = environment;
    }

    // Copy Constructor
    public ContainForceType(ContainForceType force)
      : base(force)
    {
      this.environment = force.environment;
    }

    public override Vector3d CalcForce(AgentType agent, ISpatialCollection<AgentType> neighbors)
    {
      Vector3d steer = new Vector3d();
      if (environment != null)
      {
        steer = environment.AvoidEdges(agent, agent.VisionRadius * this.visionRadiusMultiplier);
        if (!steer.IsZero)
        {
          steer.Unitize();
          steer = Vector3d.Multiply(steer, agent.MaxSpeed);
          steer = Vector3d.Subtract(steer, agent.Velocity);
          steer = Limit(steer, agent.MaxForce);
          //Multiply the resultant vector by weight.
          steer = Vector3d.Multiply(this.weight, steer);
        }
      }
      return steer;
    }

    public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
    {
      return new ContainForceType(this);
    }
  }
}
