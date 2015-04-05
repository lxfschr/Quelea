using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
  public interface IVehicle : IAgent
  {
    Plane Orientation { get; set; }
    IWheel WheelLeft { get; set; }
    IWheel WheelRight { get; set; }
    double WheelRadius { get; set; }
  }

  public interface ISensor
  {
    Point3d Position { get; set; }
    double MaxReading { get; set; }
    double Reading { get; set; }
  }

  public interface IWheel
  {
    Point3d Position { get; set; }
    double AngularSpeed { get; set; }
    double Angle { get; set; }
    double Radius { get; set; }
    double RadialSpeed { get; set; }
    void SetSpeed(double angularSpeed);
    void SetSpeedChange(double increment);
    void Run();
  }
}
