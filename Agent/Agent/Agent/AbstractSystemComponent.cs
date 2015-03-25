using System.Collections.Generic;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public abstract class AbstractSystemComponent<T> : AbstractComponent where T : class, IParticle
  {
    protected AbstractSystemType<T> system;
    protected List<AbstractEmitterType> emitters;
    protected AbstractEnvironmentType environment;
    /// <summary>
    /// Initializes a new instance of the AbstractSystemComponent class.
    /// </summary>
    protected AbstractSystemComponent(string name, string nickname, string description, string subcategory, System.Drawing.Bitmap icon, string guid)
      : base(name, nickname, description, RS.pluginCategoryName, subcategory, icon, guid)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams (GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.emittersName, RS.emitterNickName, RS.emittersDescription,
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, "(Optional) Restricts and Agent's postion to be contained within the environment. This is most useful for Surface Environments.",
                                    GH_ParamAccess.item);
      pManager[1].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams
      (GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.systemName, RS.systemNickName, RS.systemDescription, 
                                   GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      emitters = new List<AbstractEmitterType>();
      environment = null;
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this
      // method.
      if (!da.GetDataList(nextInputIndex++, emitters)) return false;
      da.GetData(nextInputIndex++, ref environment);

      if (emitters.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.emittersCountErrorMessage);
        return false;
      }
      return true;
    }
  }
}