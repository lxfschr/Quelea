using Rhino.Geometry;

namespace Quelea
{
  public interface IAgent : IParticle
  {
    Vector3d SteerAcceleration { get; set; }
    double MaxSpeed { get; set; }

    double MaxForce { get; set; }

    double VisionRadius { get; set; }

    double VisionAngle { get; set; }
    double Lon { get; set; }
    double Lat { get; set; }
    Vector3d ApplySteeringForce(Vector3d force, double weightMultiplier, bool apply);
  }
}
