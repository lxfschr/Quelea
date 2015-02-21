using System;
using System.Collections.Generic;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public interface IAgent
  {
    Point3d Position { get; }

    Vector3d Velocity { get; }

    Vector3d Acceleration { get; }

    Point3d RefPosition { get; }

    int Lifespan { get; }

    double Mass { get; }

    double BodySize { get; }

    double MaxSpeed { get; }

    double MaxForce { get; }

    int HistoryLength { get; }

    CircularArray<Point3d> PositionHistory { get; }

    List<Point3d> GetPositionHistoryList();

    void Update();

    void ApplyForce(Vector3d force);

    Boolean IsDead();

    void Run();

    bool Equals(Object obj);

    int GetHashCode();

    string ToString();
  }
}