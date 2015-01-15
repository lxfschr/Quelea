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

namespace Agent
{
  public class AgentSystemTypeList : AgentSystemType
  {

    public AgentSystemTypeList()
    {
      this.agents = new SpatialCollectionAsList<AgentType>();
      this.agentsSettings = new AgentType[] { new AgentType() };
      this.emitters = new EmitterType[] { new PtEmitterType() };
      this.environment = null;
      this.forces = new ForceType[] { };
      this.behaviors = new BehaviorType[] { };
      this.timestep = 0;
      this.nextIndex = 0;
    }

    public AgentSystemTypeList(AgentType[] agentsSettings, EmitterType[] emitters, 
                           EnvironmentType environment, ForceType[] forces,
                           BehaviorType[] behaviors)
    {
      this.agents = new SpatialCollectionAsList<AgentType>(); 
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      this.environment = environment;
      this.forces = forces;
      this.behaviors = behaviors;
    }

    public AgentSystemTypeList(AgentType[] agentsSettings, EmitterType[] emitters,
                           EnvironmentType environment, ForceType[] forces,
                           BehaviorType[] behaviors, AgentSystemTypeList system)
    {
      this.agents = new SpatialCollectionAsList<AgentType>(system.agents);
      this.agentsSettings = agentsSettings;
      this.emitters = emitters;
      this.environment = environment;
      this.forces = forces;
      this.behaviors = behaviors;
    }

    public AgentSystemTypeList(AgentSystemTypeList system)
    {
      // private ISpatialCollection<AgentType> agents;
      this.agents = new SpatialCollectionAsList<AgentType>(system.agents);
      this.agentsSettings = system.agentsSettings;
      this.emitters = system.emitters;
      this.environment = system.environment;
      this.forces = system.forces;
      this.behaviors = system.behaviors;
    }

    
  }
}
