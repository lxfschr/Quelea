using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class GetOrientationComponent : AbstractDeconstructTypeComponent
  {
    private IParticle particle;
    /// <summary>
    /// Initializes a new instance of the DeconstructParticleComponent class.
    /// </summary>
    public GetOrientationComponent()
      : base("Get Orientation", "GetOrientation",
             "Gets the position of anything that inherits from Particle.", null, "074d0d1a-9142-496e-9fe6-14bfbcd3e346")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.particleName, RS.particleNickname, RS.particleDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPlaneParameter(RS.orientationName, RS.orientationNickname, RS.orientationDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref particle)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetData(nextOutputIndex++, particle.Orientation);
    }
  }
}