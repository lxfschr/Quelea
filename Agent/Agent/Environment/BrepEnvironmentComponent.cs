using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class BrepEnvironmentComponent : AbstractEnvironmentComponent
  {
    private Brep brep;
    /// <summary>
    /// Initializes a new instance of the BrepEnvironmentComponent class.
    /// </summary>
    public BrepEnvironmentComponent()
      : base(RS.brepEnvName, RS.brepEnvComponentNickName,
          RS.brepEnvDescription, RS.icon_BrepEnvironment, RS.brepEnvGUID)
    {
      brep = new Cone(Plane.WorldXY, RS.boxBoundsDefault, RS.boxBoundsDefault).ToBrep(true);
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddBrepParameter(RS.brepName, RS.brepNickName, RS.brepForEnvDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref brep)) return false;
      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(brep.IsSolid))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.brepErrorMessage);
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEnvironmentType environment = new BrepEnvironmentType(brep);
      da.SetData(nextOutputIndex++, environment);
    }
  }
}