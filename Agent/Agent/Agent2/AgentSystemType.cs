using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Collections.ObjectModel;

using OctreeSearch;
using KdTree;

namespace Agent.Agent2
{
  public class AgentSystemType : GH_Goo<Object>
  {
    protected ISpatialCollection<AgentType> agents;
    protected AgentType[] agentsSettings;
    protected EmitterType[] emitters;
    protected int timestep;
    protected int nextIndex;
    protected Point3d min;
    protected Point3d max;

    public AgentSystemType()
    {
      this.agents = new SpatialCollectionAsBinLattice<AgentType>();
      this.agentsSettings = new AgentType[] { new AgentType() };
      this.emitters = new EmitterType[] { new BoxEmitterType() };
      this.timestep = 0;
      this.nextIndex = 0;
    }

    public AgentSystemType(AgentType[] agentsSettings, EmitterType[] emitters)
    {
      
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      updateBounds();
      this.agents = new SpatialCollectionAsBinLattice<AgentType>(this.min, this.max, (int)agentsSettings[0].VisionRadius);
    }

    public AgentSystemType(AgentType[] agentsSettings, EmitterType[] emitters, AgentSystemType system)
    {
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      updateBounds();
      this.agents = new SpatialCollectionAsBinLattice<AgentType>(this.min, this.max, (int)agentsSettings[0].VisionRadius, (IList<AgentType>)system.Agents.SpatialObjects);
    }

    public AgentSystemType(AgentSystemType system)
    {
      // private ISpatialCollection<AgentType> agents;
      this.agentsSettings = system.agentsSettings;
      this.emitters = system.emitters;
      updateBounds();
      this.agents = new SpatialCollectionAsBinLattice<AgentType>(this.min, this.max, (int)agentsSettings[0].VisionRadius, (IList<AgentType>)system.Agents.SpatialObjects);
    }

    public ISpatialCollection<AgentType> Agents
    {
      get
      {
        return this.agents;
      }
    }

    public IEnumerable<AgentType> AgentsSettings
    {
      get
      {
        return this.agentsSettings;
      }
      set
      {
        this.agentsSettings = (AgentType[]) value;
      }
    }

    public IEnumerable<EmitterType> Emitters
    {
      get
      {
        return this.emitters;
      }
      set // ToDo remove this
      {
        this.emitters = (EmitterType[])value;
      }
    }

    public void addAgent(EmitterType emitter)
    {
      Point3d emittionPt = emitter.emit();
      AgentType agent = new AgentType(agentsSettings[nextIndex % agentsSettings.Length], emittionPt);
      agents.Add(agent);
      nextIndex++;
    }

    public void run(List<Vector3d> forces, List<bool> behaviors)
    {
      updateBounds();
      agents.updateDatastructure(min, max, (int)this.agentsSettings[0].VisionRadius, (IList<AgentType>)this.Agents.SpatialObjects);
      int index = 0;
      IList<AgentType> toRemove = new List<AgentType>();
      if (forces.Count > 0 && behaviors.Count > 0)
      {
        foreach (AgentType agent in this.agents)
        {
          if (!behaviors[index]) agent.applyForce(forces[index]);
          index++;
          agent.run();
          if (agent.isDead())
          {
            toRemove.Add(agent);
          }
        }
      }
      else if (forces.Count > 0)
      {
        foreach (AgentType agent in this.agents)
        {
          agent.applyForce(forces[index]);
          index++;
          agent.run();
          if (agent.isDead())
          {
            toRemove.Add(agent);
          }
        }
      }
      else
      {
        foreach (AgentType agent in this.agents)
        {
          agent.run();
          if (agent.isDead())
          {
            toRemove.Add(agent);
          }
        }
      }
      
      foreach (EmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow && (timestep % emitter.CreationRate == 0))
        {
          if ((emitter.NumAgents == 0) || (this.agents.Count < emitter.NumAgents))
          {
            addAgent(emitter);
          }
        }
      }

      foreach (AgentType deadAgent in toRemove)
      {
        this.agents.Remove(deadAgent);
      }
      timestep++;
    }

    private void updateBounds()
    {
      this.min.X = this.min.Y = this.min.Z = Double.MaxValue;
      this.max.X = this.max.Y = this.max.Z = Double.MinValue;
      IList<Point3d> boundingPts = new List<Point3d>();
      BoundingBox bounds;
      foreach (EmitterType emitter in this.emitters)
      {
        bounds = emitter.getBoundingBox();
        this.min.X = bounds.Min.X < this.min.X ? bounds.Min.X : this.min.X;
        this.min.Y = bounds.Min.Y < this.min.Y ? bounds.Min.Y : this.min.Y;
        this.min.Z = bounds.Min.Z < this.min.Z ? bounds.Min.Z : this.min.Z;
        this.max.X = bounds.Max.X > this.max.X ? bounds.Max.X : this.max.X;
        this.max.Y = bounds.Max.Y > this.max.Y ? bounds.Max.Y : this.max.Y;
        this.max.Z = bounds.Max.Z > this.max.Z ? bounds.Max.Z : this.max.Z;
      }
      if (this.agents != null)
      {
        foreach (AgentType agent in this.agents)
        {
          this.min.X = agent.RefPosition.X < this.min.X ? agent.RefPosition.X : this.min.X;
          this.min.Y = agent.RefPosition.Y < this.min.Y ? agent.RefPosition.Y : this.min.Y;
          this.min.Z = agent.RefPosition.Z < this.min.Z ? agent.RefPosition.Z : this.min.Z;
          this.max.X = agent.RefPosition.X > this.max.X ? agent.RefPosition.X : this.max.X;
          this.max.Y = agent.RefPosition.Y > this.max.Y ? agent.RefPosition.Y : this.max.Y;
          this.max.Z = agent.RefPosition.Z > this.max.Z ? agent.RefPosition.Z : this.max.Z;
        }
      }
    }

    public override bool Equals(object obj) //ToDo fix bugs in equals
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      AgentSystemType s = obj as AgentSystemType;
      if ((System.Object)s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (this.emitters.Equals(s.emitters)) && 
             (this.agentsSettings.Equals(s.agentsSettings));
    }

    public bool Equals(AgentSystemType s) //ToDo fix bugs in equals
    {
      // If parameter is null return false:
      if ((object)s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (this.emitters.Equals(s.emitters)) && 
             (this.agentsSettings.Equals(s.agents));
    }

    public override int GetHashCode() //ToDo fix bugs in equals
    {
      int agentHash = 1;
      int emitterHash = 1;
      foreach (AgentType agent in this.agentsSettings)
      {
        agentHash *= agent.GetHashCode();
      }
      foreach (EmitterType emitter in this.emitters)
      {
        emitterHash *= emitter.GetHashCode();
      }
      return agentHash ^ emitterHash;
    }

    public override IGH_Goo Duplicate()
    {
      return new AgentSystemType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (this.timestep >= 0) && (this.nextIndex >= 0);
      }
    }

    public override string ToString() //ToDo
    {
      string agents = "Agents: " + this.agentsSettings.Length.ToString() + "\n";
      string emitters = "Emitters: " + this.emitters.Length.ToString() + "\n";
      return agents + emitters;
    }

    public override string TypeDescription
    {
      get { return "An Agent System"; }
    }

    public override string TypeName
    {
      get { return "Agent System"; }
    }
  }
}
