using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AxisAlignedBoxEnvironmentComponent : AbstractEnvironmentComponent
  {
    private Box box;
    /// <summary>
    /// Initializes a new instance of the AbstractEnvironmentComponent class.
    /// </summary>
    public AxisAlignedBoxEnvironmentComponent()
      : base(RS.AABoxEnvName, RS.AABoxEnvNickName,
          RS.AABoxEnvDescription, RS.icon_AABoxEnvironment, RS.AABoxEnvGUID)
    {
      Interval interval = new Interval(-RS.boxBoundsDefault, RS.boxBoundsDefault);
      box = new Box(Plane.WorldXY, interval, interval, interval);
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddBoxParameter(RS.boxName, RS.boxNickName, RS.AABoxDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref box)) return false;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (!(box.Plane.XAxis.Equals(Plane.WorldXY.XAxis) && box.Plane.YAxis.Equals(Plane.WorldXY.YAxis)))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.AABoxError);
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      AbstractEnvironmentType environment = new AxisAlignedBoxEnvironmentType(box);
      da.SetData(nextOutputIndex++, environment);
    }
  }
}