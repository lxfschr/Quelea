using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public interface IParticle : IGH_Goo, IPosition
  {
    Point3d Position { get; set; }
    Vector3d Velocity { get; set; }
    Vector3d Acceleration { get; set; }
    Point3d RefPosition { get; set; }
    int Lifespan { get; set; }

    double Mass { get; set; }

    double BodySize { get; set; }

    int HistoryLength { get; set; }

    bool InitialVelocitySet { get; set; }

    void Kill();

    CircularArray<Point3d> PositionHistory { get; }

    List<Point3d> GetPositionHistoryList();

    void Update();

    void ApplyForce(Vector3d force);

    Boolean IsDead();

    void Run();

    bool Equals(Object obj);

    int GetHashCode();
  }
}
