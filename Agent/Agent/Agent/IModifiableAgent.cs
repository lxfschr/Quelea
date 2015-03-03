using Rhino.Geometry;

namespace Agent
{
  public interface IModifiableAgent : IAgent
  {
    new Point3d Position { get; set; }
    new Vector3d Velocity { get; set; }
    new Vector3d Acceleration { get; set; }
    new Point3d RefPosition { get; set; }
    new int Lifespan { get; set; }

    new double Mass { get; set; }

    new double BodySize { get; set; }

    new double MaxSpeed { get; set; }

    new double MaxForce { get; set; }

    new int HistoryLength { get; set; }

    bool InitialVelocitySet { get; set; }

    void Kill();
  }
}
