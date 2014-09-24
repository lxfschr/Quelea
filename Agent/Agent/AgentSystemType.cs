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
    private int timestep;
    private int nextIndex;

    public AgentSystemType()
    {
      this.agents = new List<AgentType>();
      this.agentsSettings = new AgentType[] { new AgentType() };
      this.emitters = new EmitterType[] { new EmitterPtType() };
      this.timestep = 0;
      this.nextIndex = 0;
    }

    public AgentSystemType(AgentType[] agentsSettings, EmitterType[] emitters)
    {
      this.agents = new List<AgentType>();
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
    }

    public AgentSystemType(AgentSystemType system)
    {
      this.agents = system.agents;
      this.agentsSettings = system.agentsSettings;
      this.emitters = system.emitters;
    }

    public List<AgentType> Agents
    {
      get
      {
        return this.agents;
      }
    }

    public EmitterType[] Emitters
    {
      get
      {
        return this.emitters;
      }
    }

    public void applyForce(Vector3d f)
    {
      foreach (AgentType a in this.agents)
      {
        a.applyForce(f);
      }
    }

    public void addAgent(EmitterType emitter)
    {
      Vector3d emittionPt = emitter.emit();
      agents.Add(new AgentType(agentsSettings[nextIndex % agentsSettings.Length], emittionPt));
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
        a.run();
        if (a.isDead())
        {
          agents.Remove(a);
        }
      }
      timestep++;
    }

    public override IGH_Goo Duplicate()
    {
      return new AgentSystemType(this);
    }

    public override bool IsValid
    {
      get
      {
        foreach (AgentType agent in this.agents)
        {
          if (!agent.IsValid)
          {
            return false;
          }
        }
        foreach (EmitterType emitter in this.emitters)
        {
          if (!emitter.IsValid)
          {
            return false;
          }
        }
        return true;
      }
    }

    public override string ToString()
    {
      string agents = "Agents: " + this.agents.ToString() + "\n";
      string emitters = "Emitters: " + this.emitters.ToString() + "\n";
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
