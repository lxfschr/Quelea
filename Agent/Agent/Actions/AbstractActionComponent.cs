using System;
using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractActionComponent : GH_Component
  {
    private readonly Bitmap icon;
    private readonly Guid componentGuid;

    protected AgentType agent;
    protected bool apply;

    protected int nextInputIndex, nextOutputIndex;

    /// <summary>
    /// Initializes a new instance of the AbstractActionComponent class.
    /// </summary>
    protected AbstractActionComponent(string name, string nickname, string description, 
                                     string subcategory, Bitmap icon, string componentGuid)
      : base(name, nickname, description, RS.pluginCategoryName, subcategory)
    {
      this.icon = icon;
      this.componentGuid = new Guid(componentGuid);

      agent = new AgentType();
      apply = true;
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
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
      pManager.AddBooleanParameter(RS.applyName, RS.booleanNickname, RS.applyDescription,
         GH_ParamAccess.item, RS.applyDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected abstract override void RegisterOutputParams(GH_OutputParamManager pManager);

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      nextInputIndex = nextOutputIndex = 0;
      if (!GetInputs(da)) return;
    }

    protected virtual bool GetInputs(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref agent)) return false;
      if (!da.GetData(nextInputIndex++, ref apply)) return false;

      return true;
    }

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