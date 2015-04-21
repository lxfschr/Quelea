using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class DeconstructVehicleComponent : AbstractComponent
  {
    private IVehicle vehicle;
    /// <summary>
    /// Initializes a new instance of the DeconstructVehicleComponent class.
    /// </summary>
    public DeconstructVehicleComponent()
      : base("Deconstruct Vehicle", "DeVehicle",
             "Deconstructs a Vehicle to expose its fields such as orientation and wheel fields. Use Deconstruct Particle to expose particle fields such as position.", RS.pluginCategoryName,
             RS.pluginSubCategoryName, RS.icon_deconstructVehicle, "ad29903c-e0e2-4545-8753-1fd96a6dfa2a")
    {
      vehicle = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.vehicleName, RS.vehicleNickname + "/" + RS.queleaNickname, RS.vehicleDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPlaneParameter(RS.orientationName, RS.orientationNickname, RS.orientationDescription, GH_ParamAccess.item);
      pManager.AddPointParameter("Wheel Positions", "WP", "A list of positions of each wheel on the vehicle.", GH_ParamAccess.list);
      pManager.AddVectorParameter("Wheel Tangential Velocity", "WV", "The distance each wheel travels on the vehicle.", GH_ParamAccess.list);
      pManager.AddNumberParameter("Wheel Angles", "WA", "A list of angles of each wheel on the vehicle.", GH_ParamAccess.list);
      pManager.AddNumberParameter("Wheel Radii", "WR", "A list of radii of each wheel on the vehicle.", GH_ParamAccess.list);
      pManager.AddNumberParameter("Wheel Angular Velocity", "WS", "The speeds the wheels are rotating at.", GH_ParamAccess.list);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref vehicle)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      int n = vehicle.Wheels.Length;
      da.SetData(nextOutputIndex++, vehicle.Orientation);
      List<Point3d> wheelPositions = new List<Point3d>(n);
      List<Vector3d> wheelVelocities = new List<Vector3d>(n);
      List<double> wheelAngles = new List<double>(n);
      List<double> wheelRadii = new List<double>(n);
      List<double> wheelSpeeds = new List<double>(n);
      foreach (IWheel wheel in vehicle.Wheels)
      {
        wheelPositions.Add(wheel.Position);
        Vector3d wheelVelocity = vehicle.Velocity;
        wheelVelocity.Unitize();
        wheelVelocity = Vector3d.Multiply(wheelVelocity, wheel.TangentialVelocity);
        wheelVelocities.Add(wheelVelocity);
        wheelAngles.Add(wheel.Angle);
        wheelRadii.Add(wheel.Radius);
        wheelSpeeds.Add(wheel.AngularVelocity);
      }
      da.SetDataList(nextOutputIndex++, wheelPositions);
      da.SetDataList(nextOutputIndex++, wheelVelocities);
      da.SetDataList(nextOutputIndex++, wheelAngles);
      da.SetDataList(nextOutputIndex++, wheelRadii);
      da.SetDataList(nextOutputIndex++, wheelSpeeds);
    }
  }
}