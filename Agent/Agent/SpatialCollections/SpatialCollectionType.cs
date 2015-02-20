using System;
using Grasshopper.Kernel.Types;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SpatialCollectionType : GH_Goo<Object>
  {
    private ISpatialCollection<AgentType> agents;

    public SpatialCollectionType()
    {
      agents = new SpatialCollectionAsBinLattice<AgentType>();
    }

    public SpatialCollectionType(ISpatialCollection<AgentType> agents)
    {
      this.agents = agents;
    }

    public SpatialCollectionType(SpatialCollectionType spatialCollection)
    {
      agents = new SpatialCollectionAsBinLattice<AgentType>(spatialCollection.agents);
    }

    public ISpatialCollection<AgentType> Agents
    {
      get
      {
        return agents;
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
      return agents.ToString();
    }

    public override string TypeDescription
    {
      get { return RS.agentCollectionDescription; }
    }

    public override string TypeName
    {
      get { return RS.agentCollectionName; }
    }
  }
}
