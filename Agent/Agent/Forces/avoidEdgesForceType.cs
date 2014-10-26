using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Agent
{
  class AvoidEdgesForceType : ForceType
  {

    private EnvironmentType environment;
    public AvoidEdgesForceType()
      : base()
    {
      this.environment = null;
    }

    // Constructor with initial values
    public AvoidEdgesForceType(double weight, double visionRadiusMultiplier, 
                               EnvironmentType environment)
      : base(weight, visionRadiusMultiplier)
    {
      this.environment = environment;
    }

    // Copy Constructor
    public AvoidEdgesForceType(AvoidEdgesForceType force)
      : base(force)
    {
      this.environment = force.environment;
    }

    public override Rhino.Geometry.Vector3d calcForce(AgentType agent, List<AgentType> agents)
    {
      Vector3d steer = new Vector3d();
      if (environment != null)
      {
        steer = environment.avoidEdges(agent, agent.VisionRadius * this.visionRadiusMultiplier);
        //Multiply the resultant vector by weight.
        steer = Vector3d.Multiply(this.weight, steer);
      }
      return steer;
    }

    public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
    {
      return new AvoidEdgesForceType(this);
    }
  }
}
