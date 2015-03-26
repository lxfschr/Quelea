using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class ParticleSystemComponent : AbstractSystemComponent<ParticleType>
  {
    //private ParticleSystemType system;
    private List<IParticle> particles;
    /// <summary>
    /// Initializes a new instance of the ParticleSystemComponent class.
    /// </summary>
    public ParticleSystemComponent()
      : base(RS.systemName, RS.systemComponentNickName, RS.systemDescription,
             RS.pluginSubCategoryName, RS.icon_system, "bec6bc5b-4dc1-40bb-ad6e-1f00844bbcf3")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams (GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter(RS.particlesName, RS.particleNickname, 
                                   RS.particleDescription, GH_ParamAccess.list);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams
      (GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.systemDescription, 
                                   GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      particles = new List<IParticle>();
      if (!da.GetDataList(nextInputIndex++, particles)) return false;

      // We should now validate the data and warn the user if invalid data is 
      // supplied.
      if (particles.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.agentsCountErrorMessage);
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      if (system == null)
      {
        system = new ParticleSystemType(particles.ToArray(), emitters.ToArray(), environment);
      }
      else
      {
        system = new ParticleSystemType(particles.ToArray(), emitters.ToArray(), environment, (AbstractSystemType<IParticle>) system);
      }

      // Finally assign the system to the output parameter.
      da.SetData(nextOutputIndex++, system);
    }
  }
}