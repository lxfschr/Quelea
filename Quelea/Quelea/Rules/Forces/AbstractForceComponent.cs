using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractForceComponent : AbstractRuleComponent
  {
    protected double weightMultiplier;
    protected Vector3d desiredVelocity;
    /// <summary>
    /// Initializes a new instance of the AbstractAgentRuleComponent class.
    /// </summary>
    protected AbstractForceComponent(string name, string nickname, string description, 
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, subcategory, icon, componentGuid)
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
      pManager.AddNumberParameter(RS.weightMultiplierName, RS.weightMultiplierNickname, RS.weightMultiplierDescription,
        GH_ParamAccess.item, RS.weightMultiplierDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Force", RS.forceNickname,
                                   "The resulting force vector for debugging purposes.", GH_ParamAccess.item);
      pManager.AddGenericParameter("Desired Velocity", "D",
                                   "The calcuted desired velocity of this rule before it is applied to the quelea. Supplied for debugging and visualization purposes.", GH_ParamAccess.item);
      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      if (!base.GetInputs(da)) return false;
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref weightMultiplier)) return false;

      if (!(-1.0 <= weightMultiplier && weightMultiplier <= 1.0))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Weight multiplier must be between 0.0 and 1.0.");
        return false;
      }

      return true;
    }
    protected override void SetOutputs(IGH_DataAccess da)
    {
      desiredVelocity = CalculateDesiredVelocity();
      Vector3d appliedForce = ApplyDesiredVelocity();
      da.SetData(nextOutputIndex++, appliedForce);
      da.SetData(nextOutputIndex++, desiredVelocity);
    }

    protected abstract Vector3d ApplyDesiredVelocity();

    protected abstract Vector3d CalculateDesiredVelocity();
  }
}