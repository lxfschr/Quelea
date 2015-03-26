using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ParticleSystemType : AbstractSystemType<IParticle>
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
      : this(particleSystem.queleaSettings, particleSystem.emitters, particleSystem.environment)
    {
      
    }

    public ParticleSystemType(IParticle[] particleSettings, AbstractEmitterType[] emitters,
      AbstractEnvironmentType environment, AbstractSystemType<IParticle> system)
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
        agent = new ParticleType(queleaSettings[nextIndex % queleaSettings.Length], emittionPt, refEmittionPt);
      }
      else
      {
        agent = new ParticleType(queleaSettings[nextIndex % queleaSettings.Length], emittionPt, emittionPt);
      }
      Quelea.Add(agent);
      nextIndex++;
    }

    public override IGH_Goo Duplicate()
    {
      return new ParticleSystemType(this);
    }
  }
}
