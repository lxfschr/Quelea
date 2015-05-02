using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractAgentForceComponent : AbstractForceComponent
  {
    protected IAgent agent;
    
    /// <summary>
    /// Initializes a new instance of the AbstractParticleForceComponent class.
    /// </summary>
    protected AbstractAgentForceComponent(string name, string nickname, string description,
                                          Bitmap icon, String componentGuid)
      : base(name, nickname, description, RS.agentName + " " + RS.rulesName, icon, componentGuid)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter(RS.agentName, RS.agentNickname, RS.agentDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if(!base.GetInputs(da)) return false;
      // First, we need to retrieve all data from the input parameters.

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;

      return true;
    }

    protected override Vector3d ApplyDesiredVelocity()
    {
      if (apply)
      {
        return agent.ApplyDesiredVelocity(desiredVelocity, weightMultiplier);
      }
      return Vector3d.Zero;
    }
  }
}