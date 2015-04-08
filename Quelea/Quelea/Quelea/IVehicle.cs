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
    Point3d GetPartPosition(double gapSize, double rotation);
    double HalfPi { get; }
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
    double AngularVelocity { get; set; }
    double Angle { get; set; }
    double Radius { get; set; }
    double TangentialVelocity { get; set; }
    void SetSpeed(double angularVelocity);
    void SetSpeedChange(double increment);
    void Run();
  }
}
