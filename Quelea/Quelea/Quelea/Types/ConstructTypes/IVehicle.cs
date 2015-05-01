using Rhino.Geometry;

namespace Quelea
{
  public interface IVehicle : IAgent
  {
    Plane Orientation { get; set; }
    IWheel[] Wheels { get; set; }
    double WheelRadius { get; set; }
    Point3d GetWheelPosition(double gapSize, double rotation);
    void SetSpeedChanges(double leftValue, double rightValue);

    Vector3d ApplySensorForce(double leftWheelValue, double rightWheelValue, double weightMultiplier, bool apply);
    Point3d GetSensorPosition(double bodySize, double forwardOffset, double halfPi);
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
