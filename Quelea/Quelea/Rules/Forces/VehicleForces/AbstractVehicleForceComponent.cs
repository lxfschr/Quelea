using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractVehicleForceComponent : AbstractVehicleRuleComponent
  {
    private double weightMultiplier;
    protected bool crossed;
    protected double sensorLeftValue, sensorRightValue;
    protected Point3d sensorLeftPos, sensorRightPos;
    private double forwardOffset;
    
    /// <summary>
    /// Initializes a new instance of the AbstractParticleForceComponent class.
    /// </summary>
    protected AbstractVehicleForceComponent(string name, string nickname, string description,
                                          Bitmap icon, String componentGuid)
      : base(name, nickname, description, RS.vehicleName + " " + RS.rulesName, icon, componentGuid)
    {
      weightMultiplier = RS.weightMultiplierDefault;
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
      pManager.AddNumberParameter(RS.weightMultiplierName, RS.weightMultiplierNickname, RS.weightMultiplierDescription,
        GH_ParamAccess.item, RS.weightMultiplierDefault);
      pManager.AddBooleanParameter("Crossed?", "C", "If true, the sensors will affect the wheels on the opposite side. If false, a higher sensor reading on the left side will cause the left wheel to turn faster causing the vehicle to turn to its right. Generally, if the sensors are not crossed, then the vehicle will steer away from areas with high values.",
        GH_ParamAccess.item, false);
      pManager.AddNumberParameter("Sensor foward offset distance", "D", "The distance in front of the wheels that the sensors will read values from.",
        GH_ParamAccess.item, 0);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Force", RS.forceNickname,
                                   "The resulting force vector for debugging purposes.", GH_ParamAccess.item);
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
      if (!da.GetData(nextInputIndex++, ref weightMultiplier)) return false;
      if (!da.GetData(nextInputIndex++, ref crossed)) return false;
      if (!da.GetData(nextInputIndex++, ref forwardOffset)) return false;
      if (!(0.0 <= weightMultiplier && weightMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Weight multiplier must be between 0.0 and 1.0.");
        return false;
      }
      sensorLeftPos = vehicle.GetSensorPosition(vehicle.BodySize, forwardOffset, RS.HALF_PI);
      sensorRightPos = vehicle.GetSensorPosition(vehicle.BodySize, forwardOffset, -RS.HALF_PI);
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      Vector3d force = Run();
      da.SetData(nextOutputIndex++, force);
      da.SetDataList(nextOutputIndex++, new List<double>(2) {sensorLeftValue, sensorRightValue});
      da.SetDataList(nextOutputIndex++, new List<Point3d>(2) { sensorLeftPos, sensorRightPos });
    }

    protected Vector3d Run()
    {
      GetSensorReadings();
      if (crossed)
      {
        return vehicle.ApplySensorForce(sensorRightValue, sensorLeftValue, weightMultiplier, apply);
      }
      return vehicle.ApplySensorForce(sensorLeftValue, sensorRightValue, weightMultiplier, apply);
    }

    protected abstract void GetSensorReadings();
  }
}