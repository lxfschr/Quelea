using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  class AgentSystemType : GH_Goo<Object>
  {
    private List<AgentType> agents;
    private AgentType[] agentsSettings;
    private EmitterType[] emitters;
    private ForceType[] forces;
    private EnvironmentType environment;
    private int timestep;
    private int nextIndex;

    public AgentSystemType()
    {
      this.agents = new List<AgentType>();
      this.agentsSettings = new AgentType[] { new AgentType() };
      this.emitters = new EmitterType[] { new EmitterPtType() };
      this.environment = null;
      this.forces = new ForceType[] { };
      this.timestep = 0;
      this.nextIndex = 0;
    }

    public AgentSystemType(AgentType[] agentsSettings, EmitterType[] emitters, EnvironmentType environment, ForceType[] forces)
    {
      this.agents = new List<AgentType>();
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      this.environment = environment;
      this.forces = forces;
    }

    public AgentSystemType(AgentSystemType system)
    {
      this.agents = new List<AgentType>(system.agents);
      this.agentsSettings = system.agentsSettings;
      this.emitters = system.emitters;
      this.environment = system.environment;
      this.forces = system.forces;
    }

    public List<AgentType> Agents
    {
      get
      {
        return this.agents;
      }
    }

    public AgentType[] AgentsSettings
    {
      get
      {
        return this.agentsSettings;
      }
      set
      {
        this.agentsSettings = value;
      }
    }

    public EmitterType[] Emitters
    {
      get
      {
        return this.emitters;
      }
      set // ToDo remove this
      {
        this.emitters = value;
      }
    }

    public ForceType[] Forces
    {
      get
      {
        return this.forces;
      }
      set // ToDo remove this
      {
        this.forces = value;
      }
    }

    public EnvironmentType Environment
    {
      get
      {
        return this.environment;
      }
      set // ToDo remove this
      {
        this.environment = value;
      }
    }

    public void applyForces(AgentType a)
    {
      foreach (ForceType force in this.forces)
      {
        Vector3d forceVec = force.calcForce(a, this.agents);
        a.applyForce(forceVec);
      }
    }

    public void addAgent(EmitterType emitter)
    {
      Point3d emittionPt = emitter.emit();
      AgentType agent = new AgentType(agentsSettings[nextIndex % agentsSettings.Length], emittionPt);
      if (environment != null)
      {
        emittionPt = environment.closestRefPoint(emittionPt);
        agent = new AgentType(agentsSettings[nextIndex % agentsSettings.Length], emittionPt);
      }
      agents.Add(agent);
      nextIndex++;
    }

    public void run()
    {
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

      for (int i = agents.Count - 1; i >= 0; i--)
      {
        AgentType a = agents[i];
        applyForces(a);
        a.run();
        if (environment != null)
        {
          a.Position = environment.closestRefPointOnRef(a.Position);
          a.Position3d = environment.closestPoint(a.Position);
        }
        else
        {
          a.Position3d = a.Position;
        }
        
        if (a.isDead())
        {
          agents.Remove(a);
        }
      }
      timestep++;
    }

    public override bool Equals(object obj)
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
             (this.agentsSettings.Equals(s.agentsSettings)) &&
             (this.environment.Equals(s.environment));
    }

    public bool Equals(AgentSystemType s)
    {
      // If parameter is null return false:
      if ((object)s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (this.emitters.Equals(s.emitters)) && 
             (this.agentsSettings.Equals(s.agents)) &&
             (this.environment.Equals(s.environment));
    }

    public override int GetHashCode()
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
      return agentHash ^ emitterHash ^ this.environment.GetHashCode();
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

    public override string ToString()
    {
      string agents = "Agents: " + this.agentsSettings.Length.ToString() + "\n";
      string emitters = "Emitters: " + this.emitters.Length.ToString() + "\n";
      string environment = "Environment: " + this.environment.ToString() + "\n";
      return agents + emitters + environment;
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
