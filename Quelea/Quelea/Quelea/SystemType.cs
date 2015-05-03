using System;
using System.Collections.Generic;
using System.Linq;
using Quelea.Util;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SystemType : ISystem
  {
    public ISpatialCollection<IQuelea> Quelea { get; private set; }
    protected readonly List<IQuelea> queleaSettings;
    protected readonly List<AbstractEmitterType> emitters;
    protected readonly AbstractEnvironmentType environment;
    protected int timestep;
    protected int nextIndex;
    protected Point3d min;
    protected Point3d max;

    public SystemType()
      : this(null, new List<AbstractEmitterType> {new PtEmitterType(), }, new AxisAlignedBoxEnvironmentType())
    {
    }

    public SystemType(List<IQuelea> queleaSettings, List<AbstractEmitterType> emitters, AbstractEnvironmentType environment)
    {
      timestep = 0;
      nextIndex = 0;
      this.queleaSettings = queleaSettings;
      this.emitters = emitters;
      this.environment = environment;
      UpdateBounds();
      Quelea = MakeDynamicSpatialDataStructure();//new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)));
    }

    private ISpatialCollection<IQuelea> MakeDynamicSpatialDataStructure()
    {
      return new SpatialCollectionAsList<IQuelea>();
      if (queleaSettings[0].GetType() == typeof(AgentType))
      {
        IAgent agent = (AgentType)queleaSettings[0];
        if (environment.GetType() == typeof(WorldEnvironmentType) && emitters[0].GetType() != typeof(PtEmitterType))
        {
          return new SpatialCollectionAsOctTree<IQuelea>(min, max, 1);
        }
        return new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)agent.VisionRadius);
      }
      if (!emitters[0].ContinuousFlow)
      {
        return new SpatialCollectionAsList<IQuelea>(emitters[0].NumAgents);
      }
      return new SpatialCollectionAsList<IQuelea>();
    }

    private ISpatialCollection<IQuelea> UpdateDynamicSpatialDataStructure(IList<IQuelea> spatialObjects)
    {
      //return new SpatialCollectionAsList<IQuelea>(spatialObjects);
      if (queleaSettings[0].GetType() == typeof(AgentType))
      {
        IAgent agent = (AgentType)queleaSettings[0];
        if (environment.GetType() == typeof(WorldEnvironmentType))
        {
          if (min.DistanceTo(max) <= agent.VisionRadius*2 || Quelea.Count <= 10)
          {
            return new SpatialCollectionAsList<IQuelea>(spatialObjects);
          }
          if(min.DistanceTo(max) <= agent.VisionRadius*10) {
            return new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)agent.VisionRadius, spatialObjects);
          }
          return new SpatialCollectionAsOctTree<IQuelea>(min, max, spatialObjects);
        }
        return new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)agent.VisionRadius, spatialObjects);
      }
      return new SpatialCollectionAsList<IQuelea>(spatialObjects);
    }

    public SystemType(List<IQuelea> queleaSettings, List<AbstractEmitterType> emitters, AbstractEnvironmentType environment, SystemType system)
    {
      timestep = system.timestep;
      //nextIndex = system.nextIndex;
      this.queleaSettings = queleaSettings;
      this.emitters = emitters;
      this.environment = environment;
      //this.min = system.min;
      //this.max = system.max;
      this.Quelea = system.Quelea;
      UpdateBounds();
      Quelea = UpdateDynamicSpatialDataStructure((IList<IQuelea>)system.Quelea.SpatialObjects);//new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<IQuelea>)system.Quelea.SpatialObjects);
    }

    public SystemType(SystemType system)
    {
      // private ISpatialCollection<AgentType> particles;
      queleaSettings = system.queleaSettings;
      emitters = system.emitters;
      environment = system.environment;
      UpdateBounds();
      Quelea = new SpatialCollectionAsBinLattice<IQuelea>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<IQuelea>)system.Quelea.SpatialObjects);
    }

    public void Add(AbstractEmitterType emitter)
    {
      Point3d emittionPt = emitter.Emit();
      IQuelea quelea = MakeParticle(queleaSettings[nextIndex], emittionPt);
      Quelea.Add(quelea);
      nextIndex = (nextIndex + 1) % queleaSettings.Count;
    }

    public IQuelea MakeParticle(IQuelea p, Point3d emittionPt)
    {
      if (p.GetType() == typeof(VehicleType))
      {
        return new VehicleType((IVehicle)p, emittionPt, environment);
      }
      if (p.GetType() == typeof(AgentType))
      {
        return new AgentType((IAgent)p, emittionPt, environment);
      }
      return new ParticleType((IParticle)p, emittionPt, environment);
    }

    private void UpdateBounds()
    {
      min.X = min.Y = min.Z = Double.MaxValue;
      max.X = max.Y = max.Z = Double.MinValue;
      foreach (AbstractEmitterType emitter in emitters)
      {
        BoundingBox bounds = emitter.GetBoundingBox();
        min.X = bounds.Min.X < min.X ? bounds.Min.X : min.X;
        min.Y = bounds.Min.Y < min.Y ? bounds.Min.Y : min.Y;
        min.Z = bounds.Min.Z < min.Z ? bounds.Min.Z : min.Z;
        max.X = bounds.Max.X > max.X ? bounds.Max.X : max.X;
        max.Y = bounds.Max.Y > max.Y ? bounds.Max.Y : max.Y;
        max.Z = bounds.Max.Z > max.Z ? bounds.Max.Z : max.Z;
      }
      if (Quelea != null && Quelea.Count != 0)
      {
        foreach (IQuelea quelea in Quelea)
        {
          min.X = quelea.Position.X < min.X ? quelea.Position.X : min.X;
          min.Y = quelea.Position.Y < min.Y ? quelea.Position.Y : min.Y;
          min.Z = quelea.Position.Z < min.Z ? quelea.Position.Z : min.Z;
          max.X = quelea.Position.X > max.X ? quelea.Position.X : max.X;
          max.Y = quelea.Position.Y > max.Y ? quelea.Position.Y : max.Y;
          max.Z = quelea.Position.Z > max.Z ? quelea.Position.Z : max.Z;
        }
      }
      if (min.X.Equals(Double.MinValue))
      {
        min.X = min.Y = min.Z = 0;
        max.X = max.Y = max.Z = 0;
      }
    }

    public void Run()
    {
      UpdateBounds();
      Quelea = UpdateDynamicSpatialDataStructure((IList<IQuelea>)Quelea.SpatialObjects);
      IList<IQuelea> toRemove = new List<IQuelea>();
      foreach (IQuelea quelea in Quelea)
      {
        quelea.Environment = environment;
        quelea.Run();
        
        if (quelea.IsDead())
        {
          toRemove.Add(quelea);
        }
      }

      foreach (AbstractEmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow && (timestep % emitter.CreationRate == 0))
        {
          if ((emitter.NumAgents == 0) || (Quelea.Count < emitter.NumAgents))
          {
            Add(emitter);
          }
        }
      }

      foreach (IQuelea deadParticle in toRemove)
      {
        Quelea.Remove(deadParticle);
      }
      timestep++;
    }

    public void Populate()
    {
      Quelea = MakeDynamicSpatialDataStructure();
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
             (queleaSettings.Equals(s.Quelea.SpatialObjects)) &&
             (environment.Equals(s.environment));
    }

    public override int GetHashCode()
    {
      int agentHash = queleaSettings.Aggregate(1, (current, agent) => current * agent.GetHashCode());
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
