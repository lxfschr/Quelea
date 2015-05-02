using Rhino.Geometry;

namespace Quelea
{
  public interface IParticle : IQuelea
  {
    Vector3d VelocityMin { get; set; }

    Vector3d VelocityMax { get; set; }

    bool InitialVelocitySet { get; set; }

    Vector3d ApplyDesiredVelocity(Vector3d desiredVelocity, double weightMultiplier);
  }
}
