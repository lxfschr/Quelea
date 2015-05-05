using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractVehicleForceComponent : AbstractForceComponent
  {
    protected IVehicle vehicle;
    protected bool crossed;
    protected double sensorLeftValue, sensorRightValue;
    protected Point3d sensorLeftPos, sensorRightPos;
    private double visionAngleMultiplier, visionRadiusMultiplier;
    
    /// <summary>
    /// Initializes a new instance of the AbstractParticleForceComponent class.
    /// </summary>
    protected AbstractVehicleForceComponent(string name, string nickname, string description,
                                          Bitmap icon, String componentGuid)
      : base(name, nickname, description, RS.vehicleName + " " + RS.rulesName, icon, componentGuid)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter(RS.vehicleName, RS.vehicleNickname, RS.vehicleDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.visionRadiusName + " " + RS.multiplierName, RS.visionRadiusNickname + RS.multiplierNickname, RS.visionRadiusMultiplierDescription, GH_ParamAccess.item, RS.visionRadiusMultiplierDefault/5);
      pManager.AddNumberParameter(RS.visionAngleName + " " + RS.multiplierName, RS.visionAngleNickname + RS.multiplierNickname, RS.visionAngleMultiplierDescription, GH_ParamAccess.item, RS.visionAngleMultiplierDefault/8);
      pManager.AddBooleanParameter("Crossed?", "C", "If true, the sensors will affect the wheels on the opposite side. If false, a higher sensor reading on the left side will cause the left wheel to turn faster causing the vehicle to turn to its right. Generally, if the sensors are not crossed, then the vehicle will steer away from areas with high values.",
        GH_ParamAccess.item, false);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      base.RegisterOutputParams(pManager);
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddNumberParameter("Sensor Values", "V", "The values read by the left and right sensor.", GH_ParamAccess.list);
      pManager.AddPointParameter("Sensor Positions", "P", "The position of the left and right sensor.", GH_ParamAccess.list);
      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      // First, we need to retrieve all data from the input parameters.

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref vehicle)) return false;
      if (!da.GetData(nextInputIndex++, ref visionRadiusMultiplier)) return false;
      if (!da.GetData(nextInputIndex++, ref visionAngleMultiplier)) return false;
      if (!da.GetData(nextInputIndex++, ref crossed)) return false;
      if (!(0.0 <= visionRadiusMultiplier && visionRadiusMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionRadiusMultiplierErrorMessage);
        return false;
      }
      if (!(0.0 <= visionAngleMultiplier && visionAngleMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleMultiplierErrorMessage);
        return false;
      }
      sensorLeftPos = vehicle.GetSensorPosition(visionRadiusMultiplier, visionAngleMultiplier);
      sensorRightPos = vehicle.GetSensorPosition(visionRadiusMultiplier, -visionAngleMultiplier);
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      base.SetOutputs(da);
      da.SetDataList(nextOutputIndex++, new List<double>(2) {sensorLeftValue, sensorRightValue});
      da.SetDataList(nextOutputIndex++, new List<Point3d>(2) { sensorLeftPos, sensorRightPos });
    }

    protected override Vector3d ApplyDesiredVelocity()
    {
      if (apply)
      {
        return vehicle.ApplyDesiredVelocity(desiredVelocity, weightMultiplier);
      }
      return Vector3d.Zero;
    }

    protected override Vector3d CalculateDesiredVelocity()
    {
      GetSensorReadings();
      if (crossed)
      {
        return vehicle.CalculateSensorForce(sensorRightValue, sensorLeftValue);
      }
      return vehicle.CalculateSensorForce(sensorLeftValue, sensorRightValue);
    }

    protected abstract void GetSensorReadings();
  }
}