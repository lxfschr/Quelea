using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractBoidBehaviorComponent : AbstractAgentBehaviorComponent
  {
    protected ISpatialCollection<IParticle> neighbors;
    /// <summary>
    /// Initializes a new instance of the EatBehaviorComponent class.
    /// </summary>
    protected AbstractBoidBehaviorComponent(string name, string nickname, string description,
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
      neighbors = new SpatialCollectionAsList<IParticle>();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddGenericParameter(RS.neighborsName, RS.queleaNetworkNickname, RS.neighborsToReactTo, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      SpatialCollectionType neighborsCollection = new SpatialCollectionType();
      if (!da.GetData(nextInputIndex++, ref neighborsCollection)) return false;
      neighbors = neighborsCollection.Particles;
      return true;
    }
  }
}