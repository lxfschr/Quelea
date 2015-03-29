using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class DeconstructSystemComponent : AbstractComponent
  {
    private ISystem system;
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DeconstructSystemComponent()
      : base(RS.deconstructSystemName, RS.deconstructSystemNickname,
             RS.deconstructSystemDescription, RS.pluginCategoryName, 
             RS.pluginSubCategoryName, RS.icon_deconstructSystem, RS.deconstructSystemGuid)
    {
      system = null;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickname, RS.systemDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentsName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.queleaNetworkName, RS.queleaNetworkNickname, RS.agentCollectionDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref system)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetDataList(nextOutputIndex++, (List<IParticle>)system.Particles.SpatialObjects);
      da.SetData(nextOutputIndex++, new SpatialCollectionType(system.Particles));
    }
  }
}