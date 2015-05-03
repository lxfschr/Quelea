using Rhino.Geometry;

namespace Quelea
{
  public interface IParticle : IQuelea
  {
    Plane Orientation { get; set; }
    Vector3d Up { get; set; }
    Vector3d Forward { get; }
    Vector3d Side { get; }
    double Speed { get; }
    double SquareSpeed { get; }
    Vector3d VelocityMin { get; set; }

    Vector3d VelocityMax { get; set; }

    bool InitialVelocitySet { get; set; }

    Vector3d ApplyForce(Vector3d force, double weightMultiplier);
  }
}
