using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class VehicleComponent : AbstractConstructTypeComponent
  {
    private IAgent agent;
    private Plane orientation;
    private double wheelRadius;

    /// <summary>
    /// Initializes a new instance of the AgentComponent class.
    /// </summary>
    public VehicleComponent()
      : base("Construct Vehicle", RS.vehicleName,
          "Constructs settings for a Vehicle", RS.icon_constructVehicle, "c785d70e-6196-4068-a7f6-78444450b518")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName + " " + RS.queleaName + " Settings", RS.agentNickname + RS.queleaNickname + "S", RS.agentDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter("Wheel Radius", "W", "The radius of the wheels.", GH_ParamAccess.item, RS.wheelRadiusDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.vehicleName + " " + RS.queleaName + " Settings", RS.vehicleNickname + RS.queleaNickname + "S", RS.vehicleDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
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
      IVehicle vehicle = new VehicleType(agent, wheelRadius);

      // Finally assign the spiral to the output parameter.
      da.SetData(nextOutputIndex++, vehicle);
    }
  }
}