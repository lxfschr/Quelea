using Rhino.Geometry;

namespace Agent
{
  public interface IParticle : IQuelea
  {
    Vector3d VelocityMin { get; set; }

    Vector3d VelocityMax { get; set; }

    bool InitialVelocitySet { get; set; }

    void ApplyForce(Vector3d force);

    void Die();
  }
}
