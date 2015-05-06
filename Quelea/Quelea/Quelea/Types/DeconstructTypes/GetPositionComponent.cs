using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class GetPositionComponent : AbstractDeconstructTypeComponent
  {
    private IParticle particle;
    /// <summary>
    /// Initializes a new instance of the DeconstructParticleComponent class.
    /// </summary>
    public GetPositionComponent()
      : base("Get Particle Position", "GetPosition",
             "Gets the position of anything that inherits from Particle.", RS.icon_getPosition, "2b044f4c-908b-42aa-9df0-d89656cdfba7")
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
      pManager.AddPointParameter(RS.positionName, RS.positionNickname, RS.positionDescription, GH_ParamAccess.item);
      pManager.AddPointParameter("Surface Position", "SP", "For particles bound to Surface Environments, the position of the Agent mapped to a 2D plane representing the bounds of the surface.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref particle)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      //da.SetDataList(nextOutputIndex++, particle.Position3DHistory.ToList());
      da.SetData(nextOutputIndex++, particle.Position3D);
      if (particle.Environment.GetType() == typeof (SurfaceEnvironmentType))
      {
        da.SetData(nextOutputIndex++, particle.Position);
      }
    }
  }
}