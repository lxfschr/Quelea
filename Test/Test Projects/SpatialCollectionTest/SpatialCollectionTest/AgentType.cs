using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  public class AgentType : IPosition
  {
    private Point3d position;

    private Point3d refPosition;

    public AgentType()
    {
      this.position = Point3d.Origin;
      this.refPosition = Point3d.Origin;
    }

    public AgentType(Point3d position)
    {
      this.position = position;
      this.refPosition = position;
    }

    public AgentType(AgentType agent)
    {
      this.position = agent.position;
      this.refPosition = agent.refPosition;
    }

    public AgentType(AgentType agent, Point3d position)
    {
      this.refPosition = position;
      this.position = position;
    }

    public AgentType(AgentType agent, Point3d position, Point3d refPosition)
    {

      this.position = position;
      this.refPosition = refPosition;
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
      return this.position.Equals(p.position) &&
             this.refPosition.Equals(p.refPosition);
    }

    public bool Equals(AgentType p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return this.position.Equals(p.position) &&
             this.refPosition.Equals(p.refPosition);
    }

    public override int GetHashCode()
    {
      // Return true if the fields match:
      return this.position.GetHashCode() ^ this.refPosition.GetHashCode();
    }

    public override string ToString()
    {
      string position = "Position: " + this.refPosition.ToString() + "\n";
      return position;
    }

    public Point3d getPoint3d()
    {
      return this.refPosition;
    }
  }
}
