using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractBoidBehaviorComponent : AbstractBehaviorComponent
  {
    protected ISpatialCollection<AgentType> neighbors;
    /// <summary>
    /// Initializes a new instance of the EatBehaviorComponent class.
    /// </summary>
    protected AbstractBoidBehaviorComponent(string name, string nickname, string description,
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
    {
      neighbors = new SpatialCollectionAsList<AgentType>();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams2(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.neighborsName, RS.agentCollectionNickName, RS.neighborsToReactTo, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams2(GH_OutputParamManager pManager)
    {
    }

    protected override bool GetInputs2(IGH_DataAccess da)
    {
      SpatialCollectionType neighborsCollection = new SpatialCollectionType();
      if (!da.GetData(nextInputIndex++, ref neighborsCollection)) return false;
      neighbors = neighborsCollection.Agents;
      return true;
    }
  }
}