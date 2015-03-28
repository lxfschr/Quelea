using System;
using Grasshopper.Kernel.Types;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SpatialCollectionType : GH_Goo<Object>
  {
    private ISpatialCollection<IParticle> particles;

    public SpatialCollectionType()
    {
      particles = new SpatialCollectionAsBinLattice<IParticle>();
    }

    public SpatialCollectionType(ISpatialCollection<IParticle> particles)
    {
      this.particles = particles;
    }

    public SpatialCollectionType(SpatialCollectionType spatialCollection)
    {
      particles = new SpatialCollectionAsBinLattice<IParticle>(spatialCollection.particles);
    }

    public ISpatialCollection<IParticle> Particles
    {
      get
      {
        return particles;
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
      return particles.ToString();
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
