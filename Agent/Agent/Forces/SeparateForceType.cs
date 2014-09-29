using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Agent
{
  class SeparateForceType : ForceType
  {

    public SeparateForceType()
      : base()
    {

    }

    // Constructor with initial values
    public SeparateForceType(double weight, double visionRadiusMultiplier)
      : base(weight, visionRadiusMultiplier)
    {

    }

    // Copy Constructor
    public SeparateForceType(SeparateForceType force)
      : base(force)
    {

    }


    public override Vector3d calcForce(AgentType agent, List<AgentType> agents)
    {
      Vector3d sum = new Vector3d();
      int count = 0;
      Vector3d steer = new Vector3d();

      foreach (AgentType other in agents)
      {
        double d = Vector3d.Subtract(agent.Location, other.Location).Length;
        //if we are not comparing the seeker to iteself and it is at least
        //desired separation away:
        if ((d > 0) && (d < agent.VisionRadius*this.visionRadiusMultiplier))
        {
          Vector3d diff = Vector3d.Subtract(agent.Location, other.Location);
          diff.Unitize();

          //Weight the magnitude by distance to other
          diff = Vector3d.Divide(diff, d);

          sum = Vector3d.Add(sum, diff);
          
          //For an average, we need to keep track of how many boids
          //are in our vision.
          count++;
        }
      }

      if (count > 0)
      {
        sum = Vector3d.Divide(sum, count);

        //Scale average to maxSpeed (this becomes desired)
        sum.Unitize();
        sum = Vector3d.Multiply(sum, agent.MaxSpeed);

        //Renyold's steer formula
        steer = Vector3d.Subtract(sum, agent.Velocity);
        steer = limit(steer, agent.MaxForce);
      }

      //Multiply the resultant vector by weight.
      steer = Vector3d.Multiply(this.weight, steer);

      //Seek the average location of our neighbors.
      return steer;
    }

    public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
    {
      return new SeparateForceType(this);
    }
  }
}
