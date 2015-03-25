using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;
using String = Agent.Util.String;

namespace Agent
{
  public class AgentSystemType : GH_Goo<Object>
  {
    protected ISpatialCollection<AgentType> agents;
    protected readonly AgentType[] agentsSettings;
    protected readonly AbstractEmitterType[] emitters;
    protected AbstractEnvironmentType environment;
    protected int timestep;
    protected int nextIndex;
    protected Point3d min;
    protected Point3d max;

    public AgentSystemType()
    {
      agents = new SpatialCollectionAsBinLattice<AgentType>(min, max, RS.binSizeDefault);
      agentsSettings = new[] { new AgentType() };
      emitters = new AbstractEmitterType[] { new BoxEmitterType() };
      environment = null;
      timestep = 0;
      nextIndex = 0;
      
    }
    public AgentSystemType(AgentType[] agentsSettings, AbstractEmitterType[] emitters, AbstractEnvironmentType environment)
    {
      
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      this.environment = environment;
      this.environment = environment as PolysurfaceEnvironmentType;
      UpdateBounds();
      agents = new SpatialCollectionAsBinLattice<AgentType>(min, max, (int)(Util.Number.Clamp((min.DistanceTo(max) / 5), 5, 25)));
    }

    public AgentSystemType(AgentType[] agentsSettings, AbstractEmitterType[] emitters, AbstractEnvironmentType environment, AgentSystemType system)
    {
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      this.environment = environment;
      timestep = system.timestep;
      UpdateBounds();
      agents = new SpatialCollectionAsBinLattice<AgentType>(min, max, (int)(Util.Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<AgentType>)system.agents.SpatialObjects);
    }

    public AgentSystemType(AgentSystemType system)
    {
      // private ISpatialCollection<AgentType> agents;
      agentsSettings = system.agentsSettings;
      emitters = system.emitters;
      environment = system.environment;
      UpdateBounds();
      agents = new SpatialCollectionAsBinLattice<AgentType>(min, max, (int)(Util.Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<AgentType>)system.agents.SpatialObjects);
    }

    public ISpatialCollection<AgentType> Agents
    {
      get
      {
        return agents;
      }
    }

    public void AddAgent(AbstractEmitterType emitter)
    {
      Point3d emittionPt = emitter.Emit();
      AgentType agent;
      if (environment != null)
      {
        Point3d refEmittionPt = environment.ClosestRefPoint(emittionPt);
        agent = new AgentType(agentsSettings[nextIndex % agentsSettings.Length], emittionPt, refEmittionPt);
      }
      else
      {
        agent = new AgentType(agentsSettings[nextIndex % agentsSettings.Length], emittionPt);
      }
      agents.Add(agent);
      nextIndex++;
    }

    public void Run()
    {
      UpdateBounds();
      agents.UpdateDatastructure(min, max, (int)(Util.Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<AgentType>)agents.SpatialObjects);
      IList<AgentType> toRemove = new List<AgentType>();
      foreach (AgentType agent in agents)
      {
        agent.Run();
        if (environment != null)
        {
          agent.RefPosition = environment.ClosestRefPointOnRef(agent.RefPosition);
          agent.Position = environment.ClosestPointOnRef(agent.RefPosition);
        }
        else
        {
          agent.RefPosition = agent.Position;
          agent.Position = agent.RefPosition;
        }
        agent.PositionHistory.Add(agent.Position);
        if (agent.IsDead())
        {
          toRemove.Add(agent);
        }
        
      }
      
      foreach (AbstractEmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow && (timestep % emitter.CreationRate == 0))
        {
          if ((emitter.NumAgents == 0) || (agents.Count < emitter.NumAgents))
          {
            AddAgent(emitter);
          }
        }
      }

      foreach (AgentType deadAgent in toRemove)
      {
        agents.Remove(deadAgent);
      }
      timestep++;
    }

    private void UpdateBounds()
    {
      min.X = min.Y = min.Z = Double.MaxValue;
      max.X = max.Y = max.Z = Double.MinValue;
      //IList<Point3d> boundingPts = new List<Point3d>();
      //BoundingBox bounds;
      //foreach (AbstractEmitterType emitter in this.emitters)
      //{
      //  bounds = emitter.getBoundingBox();
      //  this.min.X = bounds.Min.X < this.min.X ? bounds.Min.X : this.min.X;
      //  this.min.Y = bounds.Min.Y < this.min.Y ? bounds.Min.Y : this.min.Y;
      //  this.min.Z = bounds.Min.Z < this.min.Z ? bounds.Min.Z : this.min.Z;
      //  this.max.X = bounds.Max.X > this.max.X ? bounds.Max.X : this.max.X;
      //  this.max.Y = bounds.Max.Y > this.max.Y ? bounds.Max.Y : this.max.Y;
      //  this.max.Z = bounds.Max.Z > this.max.Z ? bounds.Max.Z : this.max.Z;
      //}
      if (agents == null || agents.Count == 0)
      {
        min.X = min.Y = min.Z = 0;
        max.X = max.Y = max.Z = 0;
      }
      else
      {
        foreach (AgentType agent in agents)
        {
          min.X = agent.RefPosition.X < min.X ? agent.RefPosition.X : min.X;
          min.Y = agent.RefPosition.Y < min.Y ? agent.RefPosition.Y : min.Y;
          min.Z = agent.RefPosition.Z < min.Z ? agent.RefPosition.Z : min.Z;
          max.X = agent.RefPosition.X > max.X ? agent.RefPosition.X : max.X;
          max.Y = agent.RefPosition.Y > max.Y ? agent.RefPosition.Y : max.Y;
          max.Z = agent.RefPosition.Z > max.Z ? agent.RefPosition.Z : max.Z;
        }
      }
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.

      // If parameter cannot be cast to Point return false.
      AgentSystemType s = obj as AgentSystemType;
      if (s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (emitters.Equals(s.emitters)) && 
             (agentsSettings.Equals(s.agentsSettings));
    }

    public bool Equals(AgentSystemType s)
    {
      // If parameter is null return false:
      if (s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (emitters.Equals(s.emitters)) && 
             (agentsSettings.Equals(s.agents)) &&
             (environment.Equals(s.environment));
    }

    public override int GetHashCode()
    {
      int agentHash = agentsSettings.Aggregate(1, (current, agent) => current*agent.GetHashCode());
      int emitterHash = emitters.Aggregate(1, (current, emitter) => current*emitter.GetHashCode());
      int environmentHash = environment.GetHashCode();
      return agentHash ^ emitterHash * 7 * environmentHash;
    }

    public override IGH_Goo Duplicate()
    {
      return new AgentSystemType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (timestep >= 0) && (nextIndex >= 0);
      }
    }

    public override string ToString() //ToDo
    {
      string agentsStr = RS.agentsName + ": " + agentsSettings.Length + "\n";
      string emittersStr = RS.emittersName + ": " + emitters.Length + "\n";
      string environmentStr = "Environment: None\n";
      if (this.environment != null)
      {
        environmentStr = "Environment: " + this.environment.ToString() + "\n";
      }
      return agentsStr + emittersStr + environmentStr;
    }

    public override string TypeDescription
    {
      get { return RS.systemDescription; }
    }

    public override string TypeName
    {
      get { return RS.systemName; }
    }

    public void Populate()
    {
      foreach (AbstractEmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow) continue;
        for (int i = 0; i < emitter.NumAgents; i++)
        {
          AddAgent(emitter);
        }
      }
    }

  }
}
