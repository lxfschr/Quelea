using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ParticleSystemType : SystemType
  {
    public ParticleSystemType()
    {
    }

    public ParticleSystemType(IParticle[] particleSettings, AbstractEmitterType[] emitters,
      AbstractEnvironmentType environment)
      : base(particleSettings, emitters, environment)
    {
    }

    public ParticleSystemType(ParticleSystemType particleSystem)
      : this(particleSystem.particlesSettings, particleSystem.emitters, particleSystem.environment)
    {
      
    }

    public ParticleSystemType(IParticle[] particleSettings, AbstractEmitterType[] emitters,
      AbstractEnvironmentType environment, SystemType system)
      : base(particleSettings, emitters, environment, system)
    {
    }
    public override void Add(AbstractEmitterType emitter)
    {
      Point3d emittionPt = emitter.Emit();
      ParticleType agent;
      if (environment != null)
      {
        Point3d refEmittionPt = environment.ClosestRefPoint(emittionPt);
        agent = new ParticleType(particlesSettings[nextIndex % particlesSettings.Length], emittionPt, refEmittionPt);
      }
      else
      {
        agent = new ParticleType(particlesSettings[nextIndex % particlesSettings.Length], emittionPt, emittionPt);
      }
      Particles.Add(agent);
      nextIndex++;
    }

    public override IGH_Goo Duplicate()
    {
      return new ParticleSystemType(this);
    }
  }
}
