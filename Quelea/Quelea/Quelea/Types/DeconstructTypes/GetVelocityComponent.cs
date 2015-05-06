using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class GetVelocityComponent : AbstractDeconstructTypeComponent
  {
    private IParticle particle;
    /// <summary>
    /// Initializes a new instance of the DeconstructParticleComponent class.
    /// </summary>
    public GetVelocityComponent()
      : base("Get Particle Velocity", "GetVelocity",
             "Gets the velocity of anything that inherits from Particle.", RS.icon_getVelocity, "340f3b90-5038-46a1-bc7f-7903c09037dc")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.particleName + " " + RS.queleaName, RS.particleNickname + RS.queleaNickname, RS.particleDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddVectorParameter(RS.velocityName, RS.velocityNickname, RS.velocityDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter("Surface Velocity", "SV", "For particles bound to Surface Environments, the velocity of the Agent mapped to a 2D plane representing the bounds of the surface.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref particle)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      //da.SetDataList(nextOutputIndex++, particle.velocity3DHistory.ToList());
      da.SetData(nextOutputIndex++, particle.Velocity3D);
      if (particle.Environment.GetType() == typeof (SurfaceEnvironmentType))
      {
        da.SetData(nextOutputIndex++, particle.Velocity);
      }
    }
  }
}