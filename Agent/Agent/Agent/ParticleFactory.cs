using Rhino.Geometry;

namespace Agent
{
  class ParticleFactory : IParticleFactory
  { 
    public IParticle MakeParticle(IParticle p, Point3d emittionPt, Point3d refEmittionPt)
    {
      if (p.GetType() == typeof (ParticleType))
      {
        return new ParticleType(p, emittionPt, refEmittionPt);
      }
      else if (p.GetType() == typeof (AgentType))
      {
        return new AgentType((IAgent) p, emittionPt, refEmittionPt);
      }
      return new AgentType((IAgent)p, emittionPt, refEmittionPt);
    }
  }
}
