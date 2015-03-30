using System;
using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractBoidForceComponent : AbstractAgentForceComponent
  {
    protected ISpatialCollection<IQuelea> neighbors;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractBoidForceComponent(string name, string nickname, string description, 
                                         Bitmap icon, String componentGuid)
      : base(name, nickname, description, icon, componentGuid)
    {
      neighbors = new SpatialCollectionAsList<IQuelea>();
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
      neighbors = neighborsCollection.Quelea;
      return true;
    }
  }
}