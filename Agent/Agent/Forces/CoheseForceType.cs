using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
using System.Collections;

namespace Agent
{
  class CoheseForceType : ForceType
  {
    
    public CoheseForceType()
      : base() 
    { 

    }

    // Constructor with initial values
    public CoheseForceType(double weight, double visionRadiusMultiplier)
      : base(weight, visionRadiusMultiplier)
    {

    }

    // Copy Constructor
    public CoheseForceType(CoheseForceType force)
      : base(force)
    {

    }

    public override Vector3d CalcForce(AgentType agent, ISpatialCollection<AgentType> neighbors)
    {
      Vector3d sum = new Vector3d();
      int count = 0;
      Vector3d steer = new Vector3d();

      if (this.visionRadiusMultiplier != 1.0)
      {
        neighbors = neighbors.GetNeighborsInSphere(agent, agent.VisionRadius * this.visionRadiusMultiplier);
      }

      foreach (AgentType other in neighbors)
      {
        //Adding up all the others' location
        sum = Vector3d.Add(sum, new Vector3d(other.RefPosition));
        //For an average, we need to keep track of how many boids
        //are in our vision.
        count++;
      }

      if (count > 0)
      {
        //We desire to go in that direction at maximum speed.
        sum = Vector3d.Divide(sum, count);
        steer = this.Seek(agent, sum);
        //Multiply the resultant vector by weight.
        steer = Vector3d.Multiply(this.weight, steer);
      }
      //Seek the average location of our neighbors.
      return steer;
    }

    public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
    {
      return new CoheseForceType(this);
    }
  }
}
