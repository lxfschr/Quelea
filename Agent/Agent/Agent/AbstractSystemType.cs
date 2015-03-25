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
  public abstract class AbstractSystemType<T> : ISystem<T> where T : class, IParticle
  {
    public ISpatialCollection<T> Quelea { get; private set; }
    protected T[] queleaSettings;
    protected AbstractEmitterType[] emitters;
    protected AbstractEnvironmentType environment;
    protected int timestep;
    protected int nextIndex;
    protected Point3d min;
    protected Point3d max;

    protected AbstractSystemType()
      : this(null, new AbstractEmitterType[] {new PtEmitterType(), }, new AxisAlignedBoxEnvironmentType())
    {
    }

    protected AbstractSystemType(T[] queleaSettings, AbstractEmitterType[] emitters, AbstractEnvironmentType environment)
    {
      timestep = 0;
      nextIndex = 0;
      this.queleaSettings = queleaSettings;
      this.emitters = emitters;
      this.environment = environment;
      UpdateBounds();
      Quelea = new SpatialCollectionAsBinLattice<T>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)));
    }

    protected AbstractSystemType(T[] queleaSettings, AbstractEmitterType[] emitters, AbstractEnvironmentType environment, AbstractSystemType<T> system)
    {
      timestep = system.timestep;
      nextIndex = system.nextIndex;
      this.queleaSettings = queleaSettings;
      this.emitters = emitters;
      this.environment = environment;
      UpdateBounds();
      Quelea = new SpatialCollectionAsBinLattice<T>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<T>)system.Quelea.SpatialObjects);
    }

    protected AbstractSystemType(AbstractSystemType<T> system)
    {
      // private ISpatialCollection<AgentType> agents;
      queleaSettings = system.queleaSettings;
      emitters = system.emitters;
      environment = system.environment;
      UpdateBounds();
      Quelea = new SpatialCollectionAsBinLattice<T>(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<T>)system.Quelea.SpatialObjects);
    }
    
    public abstract void Add(AbstractEmitterType emitter);

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
      if (Quelea == null || Quelea.Count == 0)
      {
        min.X = min.Y = min.Z = 0;
        max.X = max.Y = max.Z = 0;
      }
      else
      {
        foreach (T quelea in Quelea)
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
      Quelea.UpdateDatastructure(min, max, (int)(Number.Clamp((min.DistanceTo(max) / 5), 5, 25)), (IList<T>)Quelea.SpatialObjects);
      IList<T> toRemove = new List<T>();
      foreach (T quelea in Quelea)
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
          if ((emitter.NumAgents == 0) || (Quelea.Count < emitter.NumAgents))
          {
            Add(emitter);
          }
        }
      }

      foreach (T deadQuelea in toRemove)
      {
        Quelea.Remove(deadQuelea);
      }
      timestep++;
    }

    public void Populate()
    {
      foreach (AbstractEmitterType emitter in emitters)
      {
        if (emitter.ContinuousFlow) continue;
        for (int i = 0; i < emitter.NumAgents; i++)
        {
          Add(emitter);
        }
      }
    }

    public abstract IGH_Goo Duplicate();

    public override bool Equals(object obj)
    {
      return Equals(obj as AbstractSystemType<T>);
    }

    public bool Equals(AbstractSystemType<T> s)
    {
      // If parameter is null return false:
      if (s == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (emitters.Equals(s.emitters)) &&
             (queleaSettings.Equals(s.Quelea)) &&
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
