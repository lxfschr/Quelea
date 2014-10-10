using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Agent
{
  class AlignForceType : ForceType
  {

    public AlignForceType()
      : base()
    {

    }

    // Constructor with initial values
    public AlignForceType(double weight, double visionRadiusMultiplier)
      : base(weight, visionRadiusMultiplier)
    {

    }

    // Copy Constructor
    public AlignForceType(AlignForceType force)
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
        //Add up all the velocities and divide by the total to calculate
        //the average velocity.
        Vector3d vec = Vector3d.Subtract(agent.Position, other.Position);
        double d = vec.Length;
        if ((d > 0) && (d < agent.VisionRadius * this.visionRadiusMultiplier))
        {
          //Adding up all the others' position
          sum = Vector3d.Add(sum, other.Position);
          //For an average, we need to keep track of how many boids
          //are in our vision.
          count++;
        }
      }

      if (count > 0)
      {
        steer = this.seek(agent, sum);
      }

      //Multiply the resultant vector by weight.
      steer = Vector3d.Multiply(this.weight, steer);

      //Seek the average position of our neighbors.
      return steer;
    }

    public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
    {
      return new AlignForceType(this);
    }
  }
}
