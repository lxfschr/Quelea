using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public interface ISystem<T> : IGH_Goo where T : class, IParticle
  {
    ISpatialCollection<T> Quelea { get; }

    void Add(AbstractEmitterType emitter);

    void Run();

    bool Equals(object obj);

    int GetHashCode();

    void Populate();
  }
}
