using System;
using System.Collections.Generic;
using System.Linq;
using Agent.Util;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class SystemType : ISystem
  {
    public ISpatialCollection<IQuelea> Particles { get; private set; }
    protected readonly IQuelea[] particlesSettings;
    protected readonly AbstractEmitterType[] emitters;
    protected readonly AbstractEnvironmentType environment;
    protected int timestep;
    protected int nextIndex;
    protected Point3d min;
    protected Point3d max;

    public SystemType()
      : this(null, new AbstractEmitterType[] {new PtEmitterType(), }, new AxisAlignedBoxEnvironmentType())
    {
    }

    public SystemType(IQuelea[] particlesSettings, AbstractEmitterType[] emitters, AbstractEnvironmentType environment)
    {
      timestep = 0;
      nextIndex = 0;
      this.particlesSettings = particlesSettings;
      this.emitters = emitters;
      this.environment = environment;
      UpdateBounds();
      Particles = new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)));
    }

    public SystemType(IQuelea[] particlesSettings, AbstractEmitterType[] emitters, AbstractEnvironmentType environment, SystemType system)
    {
      timestep = system.timestep;
      nextIndex = system.nextIndex;
      this.particlesSettings = particlesSettings;
      this.emitters = emitters;
      this.environment = environment;
      UpdateBounds();
      Particles = new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<IQuelea>)system.Particles.SpatialObjects);
    }

    public SystemType(SystemType system)
    {
      // private ISpatialCollection<AgentType> particles;
      particlesSettings = system.particlesSettings;
      emitters = system.emitters;
      environment = system.environment;
      UpdateBounds();
      Particles = new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<IQuelea>)system.Particles.SpatialObjects);
    }

    public void Add(AbstractEmitterType emitter)
    {
      Point3d emittionPt = emitter.Emit();
      IQuelea particle;
      if (environment != null)
      {
        Point3d refEmittionPt = environment.ClosestRefPoint(emittionPt);
        //agent = new ParticleType(particlesSettings[nextIndex % particlesSettings.Length], emittionPt, refEmittionPt);
        particle = MakeParticle(particlesSettings[nextIndex], emittionPt,
          refEmittionPt);
      }
      else
      {
        particle = MakeParticle(particlesSettings[nextIndex], emittionPt,
          emittionPt);
      }
      Particles.Add(particle);
      nextIndex = (nextIndex + 1) % particlesSettings.Length;
    }

    public IQuelea MakeParticle(IQuelea p, Point3d emittionPt, Point3d refEmittionPt)
    {
      if (p.GetType() == typeof(VehicleType))
      {
        return new VehicleType((IVehicle)p, emittionPt, refEmittionPt);
      }
      if (p.GetType() == typeof(AgentType))
      {
        return new AgentType((IAgent)p, emittionPt, refEmittionPt);
      }
      return new ParticleType((IParticle)p, emittionPt, refEmittionPt);
    }

    private void UpdateBounds()
    {
      min.X = min.Y = min.Z = Double.MaxValue;
      max.X = max.Y = max.Z = Double.MinValue;
      //IList<Point3d> boundingPts = new List<Point3d>();
      //BoundingBox bounds;
      //foreach (AbstractEmitterType emitter in this.emitters)
      //{
      //  bounds = emitter.getBoundingBox();
      //  this.min.X = bounds.Min.X < this.min.X ? bounds.Min.X : this.min.X;
      //  this.min.Y = bounds.Min.Y < this.min.Y ? bounds.Min.Y : this.min.Y;
      //  this.min.Z = bounds.Min.Z < this.min.Z ? bounds.Min.Z : this.min.Z;
      //  this.max.X = bounds.Max.X > this.max.X ? bounds.Max.X : this.max.X;
      //  this.max.Y = bounds.Max.Y > this.max.Y ? bounds.Max.Y : this.max.Y;
      //  this.max.Z = bounds.Max.Z > this.max.Z ? bounds.Max.Z : this.max.Z;
      //}
      if (Particles == null || Particles.Count == 0)
      {
        min.X = min.Y = min.Z = 0;
        max.X = max.Y = max.Z = 0;
      }
      else
      {
        foreach (IQuelea quelea in Particles)
        {
          min.X = quelea.RefPosition.X < min.X ? quelea.RefPosition.X : min.X;
          min.Y = quelea.RefPosition.Y < min.Y ? quelea.RefPosition.Y : min.Y;
          min.Z = quelea.RefPosition.Z < min.Z ? quelea.RefPosition.Z : min.Z;
          max.X = quelea.RefPosition.X > max.X ? quelea.RefPosition.X : max.X;
          max.Y = quelea.RefPosition.Y > max.Y ? quelea.RefPosition.Y : max.Y;
          max.Z = quelea.RefPosition.Z > max.Z ? quelea.RefPosition.Z : max.Z;
        }
      }
    }

    public void Run()
    {
      UpdateBounds();
      Particles.UpdateDatastructure(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<IQuelea>)Particles.SpatialObjects);
      IList<IQuelea> toRemove = new List<IQuelea>();
      foreach (IQuelea quelea in Particles)
      {
        quelea.Run();
        if (environment != null)
        {
          quelea.RefPosition = environment.ClosestRefPointOnRef(quelea.RefPosition);
          quelea.Position = environment.ClosestPointOnRef(quelea.RefPosition);
        }
        else
        {
          quelea.RefPosition = quelea.Position;
          quelea.Position = quelea.RefPosition;
        }
        quelea.PositionHistory.Add(quelea.Position);
        if (quelea.IsDead())
        {
          toRemove.Add(quelea);
        }

      }

      foreach (AbstractEmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow && (timestep % emitter.CreationRate == 0))
        {
          if ((emitter.NumAgents == 0) || (Particles.Count < emitter.NumAgents))
          {
            Add(emitter);
          }
        }
      }

      foreach (IQuelea deadParticle in toRemove)
      {
        Particles.Remove(deadParticle);
      }
      timestep++;
    }

    public void Populate()
    {
      Particles.Clear();
      foreach (AbstractEmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow) continue;
        for (int i = 0; i < emitter.NumAgents; i++)
        {
          Add(emitter);
        }
      }
    }

    public IGH_Goo Duplicate()
    {
      return new SystemType(this);
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as SystemType);
    }

    public bool Equals(SystemType s)
    {
      // If parameter is null return false:
      if (s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (emitters.Equals(s.emitters)) &&
             (particlesSettings.Equals(s.Particles)) &&
             (environment.Equals(s.environment));
    }

    public override int GetHashCode()
    {
      int agentHash = particlesSettings.Aggregate(1, (current, agent) => current * agent.GetHashCode());
      int emitterHash = emitters.Aggregate(1, (current, emitter) => current * emitter.GetHashCode());
      int environmentHash = environment.GetHashCode();
      return agentHash ^ emitterHash * 7 * environmentHash;
    }

    public bool Write(GH_IWriter writer)
    {
      throw new NotImplementedException();
    }

    public bool Read(GH_IReader reader)
    {
      throw new NotImplementedException();
    }

    public IGH_GooProxy EmitProxy()
    {
      throw new NotImplementedException();
    }

    public bool CastFrom(object source)
    {
      throw new NotImplementedException();
    }

    public bool CastTo<T1>(out T1 target)
    {
      throw new NotImplementedException();
    }

    public object ScriptVariable()
    {
      throw new NotImplementedException();
    }

    public bool IsValid
    {
      get
      {
        return (timestep >= 0) && (nextIndex >= 0);
      }
    }
    public string IsValidWhyNot { get { throw new NotImplementedException(); } }
    public string TypeName { get { return RS.systemDescription; } }
    public string TypeDescription { get { return RS.systemName; } }
    
    
  }
}
