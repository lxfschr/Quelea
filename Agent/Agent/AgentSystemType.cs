using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  class AgentSystemType : GH_Goo<Object>
  {
    private List<AgentType> agents;
    private EmitterType[] emitters;

    public AgentSystemType()
    {
      this.agents = new List<AgentType>() { new AgentType() };
      this.emitters = new EmitterType[] { new EmitterPtType() };
    }

    public AgentSystemType(List<AgentType> agents, EmitterType[] emitters)
    {
      this.agents = agents;
      this.emitters = emitters;
    }

    public AgentSystemType(AgentSystemType system)
    {
      this.agents = system.agents;
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
