using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel.Types;

namespace Agent.Agent2
{
  public class SpatialCollectionType : GH_Goo<Object>
  {
    private ISpatialCollection<AgentType> agents;

    public SpatialCollectionType()
    {
      this.agents = new SpatialCollectionAsBinLattice<AgentType>();
    }

    public SpatialCollectionType(ISpatialCollection<AgentType> agents)
    {
      this.agents = new SpatialCollectionAsList<AgentType>(agents);
    }

    public SpatialCollectionType(SpatialCollectionType spatialCollection)
    {
      this.agents = new SpatialCollectionAsBinLattice<AgentType>(spatialCollection.agents);
    }

    public ISpatialCollection<AgentType> Agents
    {
      get
      {
        return this.agents;
      }
    }

    public override IGH_Goo Duplicate()
    {
      return new SpatialCollectionType(this);
    }

    public override bool IsValid
    {
      get { return true; }
    }

    public override string ToString()
    {
      return this.agents.ToString();
    }

    public override string TypeDescription
    {
      get { return "A collection of Agents."; }
    }

    public override string TypeName
    {
      get { return "Agent System"; }
    }
  }
}
