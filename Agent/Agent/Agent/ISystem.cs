using Grasshopper.Kernel.Types;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public interface ISystem : IGH_Goo
  {
    ISpatialCollection<IParticle> Particles { get; }
    void Run();

    void Populate();

    bool Equals(object obj);

    int GetHashCode();
  }
}
