using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class DeconstructParticleSystemComponent : AbstractComponent
  {
    private ParticleSystemType system;
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DeconstructParticleSystemComponent()
      : base("Decontruct Particle System", "DeconPSystem",
             RS.deconstructSystemDescription, RS.pluginCategoryName,
             RS.pluginSubCategoryName, RS.icon_deconstructSystem, "aec8d615-b646-480e-833e-3416a6095c3d")
    {
      system = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.systemDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentsName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.agentCollectionName, RS.agentCollectionNickName, RS.agentCollectionDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref system)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetDataList(nextOutputIndex++, (List<IParticle>)system.Particles.SpatialObjects);
      //da.SetData(nextOutputIndex++, new SpatialCollectionType(system.Particles));
    }
  }
}