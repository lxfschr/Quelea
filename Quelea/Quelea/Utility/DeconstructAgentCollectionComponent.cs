using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class DeconstructAgentCollectionComponent : AbstractComponent
  {
    private SpatialCollectionType agentCollection;
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DeconstructAgentCollectionComponent()
      : base(RS.deconstructQNName, RS.deconstructQNNickname,
          RS.deconstructQNDescription,
          RS.pluginCategoryName, RS.utilitySubcategoryName, RS.icon_deconstructAC, RS.desconstructQNGuid)
    {
      agentCollection = new SpatialCollectionType();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.queleaNetworkName, RS.queleaNetworkNickname, RS.queleaNetworkDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.queleaName, RS.queleaNickname, RS.queleaDescription, GH_ParamAccess.list);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref agentCollection)) return false;

      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      da.SetDataList(nextOutputIndex++, (List<IQuelea>)agentCollection.Quelea.SpatialObjects);
    }
  }
}