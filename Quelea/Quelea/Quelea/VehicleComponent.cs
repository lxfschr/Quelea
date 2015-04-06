using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class VehicleComponent : AbstractComponent
  {
    private IAgent agent;
    private Plane orientation;
    private double wheelRadius;

    /// <summary>
    /// Initializes a new instance of the AgentComponent class.
    /// </summary>
    public VehicleComponent()
      : base("Construct Vehicle", RS.vehicleName,
          "Constructs settings for a Vehicle",
          RS.pluginCategoryName, RS.pluginSubCategoryName, null, "c785d70e-6196-4068-a7f6-78444450b518")
    {
      agent = null;
      orientation = Plane.WorldXY;
      wheelRadius = RS.wheelRadiusDefault;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickname, RS.agentDescription, GH_ParamAccess.item);
      pManager.AddPlaneParameter("Orientation", "O", "A plane representing the orientation of the vehicle in the world.", GH_ParamAccess.item, Plane.WorldXY);
      pManager.AddNumberParameter("Wheel Radius", "W", "The radius of the wheels.", GH_ParamAccess.item, RS.wheelRadiusDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.vehicleName, RS.vehicleNickname + RS.queleaNickname, RS.vehicleDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      if (!da.GetData(nextInputIndex++, ref orientation)) return false;
      if (!da.GetData(nextInputIndex++, ref wheelRadius)) return false;

      // We should now validate the data and warn the user if invalid data is supplied.
      //if (lifespan <= 0)
      //{
      //  AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.lifespanErrorMessage);
      //  return;
      //}
      if (wheelRadius < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Wheel Radius must be positive.");
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      IVehicle vehicle = new VehicleType(agent, orientation, wheelRadius);

      // Finally assign the spiral to the output parameter.
      da.SetData(nextOutputIndex++, vehicle);
    }
  }
}