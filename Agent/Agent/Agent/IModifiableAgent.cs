using Rhino.Geometry;

namespace Agent
{
  public interface IModifiableAgent : IAgent
  {
    new Point3d RefPosition { get; set; }
    new Vector3d Velocity { get; set; }

    new int Lifespan { get; set; }

    bool InitialVelocitySet { get; set; }
  }
}
