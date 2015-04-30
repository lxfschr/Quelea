using System;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Quelea
{
  public interface IQuelea : IGH_Goo, IPosition
  {
    
    Point3d Position { get; set; }

    Vector3d Velocity { get; set; }

    Vector3d Acceleration { get; set; }

    Point3d Position3D { get; set; }

    Vector3d Velocity3D { get; set; }

    Vector3d Acceleration3D { get; set; }


    Vector3d PreviousAcceleration3D { get; set; }

    Vector3d PreviousAcceleration { get; set; }

    int Lifespan { get; set; }

    double Mass { get; set; }

    double BodySize { get; set; }

    int HistoryLength { get; set; }

    CircularArray<Point3d> Position3DHistory { get; }

    AbstractEnvironmentType Environment { get; set; }

    void Run();

    Boolean IsDead();

    bool Equals(Object obj);

    int GetHashCode();
  }
}
