using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractForceComponent : GH_Component
  {
    private readonly Bitmap icon;
    private readonly Guid componentGuid;

    protected AgentType agent;
    private double weightMultiplier;
    private bool applyForce;

    protected int nextInputIndex, nextOutputIndex;

    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    protected AbstractForceComponent(string name, string nickname, string description,
                              string category, string subCategory, Bitmap icon, String componentGuid)
      : base(name, nickname, description, category, subCategory)
    {
      this.icon = icon;
      this.componentGuid = new Guid(componentGuid);
      agent = new AgentType();
      weightMultiplier = RS.weightMultiplierDefault;
      applyForce = false;
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentToAffect, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.weightMultiplierName, RS.weightMultiplierNickName, RS.weightMultiplierDescription,
        GH_ParamAccess.item, RS.weightMultiplierDefault);
      pManager.AddBooleanParameter("Apply Force?", "B", "If false, the Force will not be applied to the Agent. This is useful for having Behaviors override Forces. Can also be used for only applying the force if the Agent is within a certain area.",
        GH_ParamAccess.item, true);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Force", RS.forceNickName,
                                   "The resulting force vector for debugging purposes.", GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(1);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      nextInputIndex = nextOutputIndex = 0;
      if (!GetInputs(da)) return;
      if (!applyForce)
      {
        da.SetData(nextOutputIndex++, Vector3d.Zero);
        return;
      }

      Vector3d force = Run();
      da.SetData(nextOutputIndex++, force);
    }

    protected virtual bool GetInputs(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      if (!da.GetData(nextInputIndex++, ref weightMultiplier)) return false;
      if (!da.GetData(nextInputIndex++, ref applyForce)) return false;

      return true;
    }

    protected Vector3d Run()
    {
      Vector3d force = CalcForce();
      return ApplyForce(force);
    }

    protected Vector3d ApplyForce(Vector3d force)
    {
      Vector3d weightedForce = Vector3d.Multiply(force, weightMultiplier);
      agent.ApplyForce(weightedForce);
      return weightedForce;
    }

    protected abstract Vector3d CalcForce();

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return Resources.IconForThisComponent;
        return icon;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return componentGuid; }
    }
  }
}