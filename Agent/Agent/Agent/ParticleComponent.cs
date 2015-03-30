using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ParticleComponent : AbstractComponent
  {

    private Vector3d velocityMin, velocityMax;

    private Vector3d acceleration;

    private int lifespan;

    private double mass;

    private double bodySize;

    private int historyLength;

    /// <summary>
    /// Initializes a new instance of the ParticleComponent class.
    /// </summary>
    public ParticleComponent()
      : base(RS.particleSettingsName, RS.particleSettingNickname,
          RS.particleSettingsDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName, RS.icon_constructParticle, "dd2877f8-e247-4a67-9802-3c68c968779d")
    {
      velocityMin = new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault);
      velocityMax = new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault);
      acceleration = Vector3d.Zero;
      lifespan = RS.lifespanDefault;
      mass = RS.massDefault;
      bodySize = RS.bodySizeDefault;
      historyLength = RS.historyLenDefault;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddVectorParameter("Minimum Initial Velocity", "mV",
        "The minimum initial velocity from which a random value will be taken.", GH_ParamAccess.item,
        new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault));
      pManager.AddVectorParameter("Maximum Initial Velocity", "MV",
        "The maximum initial velocity from which a random value will be taken.", GH_ParamAccess.item,
        new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault));
      pManager.AddVectorParameter(RS.accelerationName, RS.accelerationNickName, 
                                  RS.accelerationDescription, GH_ParamAccess.item, Vector3d.Zero);
      pManager.AddIntegerParameter(RS.lifespanName, RS.lifespanNickname, RS.lifespanDescription, GH_ParamAccess.item, RS.lifespanDefault);
      pManager.AddNumberParameter(RS.massName, RS.massNickname, RS.massDescription, GH_ParamAccess.item, RS.massDefault);
      pManager.AddNumberParameter(RS.bodySizeName, RS.bodySizeNickName, RS.bodySizeDescription, GH_ParamAccess.item, RS.bodySizeDefault);
      pManager.AddIntegerParameter(RS.historyLengthName, RS.historyLengthNickName, RS.historyLengthDescription, GH_ParamAccess.item, RS.historyLenDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.particleName, RS.particleNickname + RS.queleaNickname, RS.particleDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref velocityMin)) return false;
      if (!da.GetData(nextInputIndex++, ref velocityMax)) return false;
      if (!da.GetData(nextInputIndex++, ref acceleration)) return false;
      if (!da.GetData(nextInputIndex++, ref lifespan)) return false;
      if (!da.GetData(nextInputIndex++, ref mass)) return false;
      if (!da.GetData(nextInputIndex++, ref bodySize)) return false;
      if (!da.GetData(nextInputIndex++, ref historyLength)) return false;

      // We should now validate the data and warn the user if invalid data is supplied.
      //if (lifespan <= 0)
      //{
      //  AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.lifespanErrorMessage);
      //  return;
      //}
      if (mass <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.massErrorMessage);
        return false;
      }
      if (bodySize < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.bodySizeErrorMessage);
        return false;
      }
      if (historyLength < 1)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "History length must be at least 1.");
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      IParticle particle = new ParticleType(velocityMin, velocityMax, acceleration, lifespan, mass, bodySize, historyLength);

      // Finally assign the spiral to the output parameter.
      da.SetData(nextOutputIndex++, particle);
    }
  }
}