using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AgentSystemComponent : GH_Component
  {
    private AgentSystemType system;
    /// <summary>
    /// Initializes a new instance of the AgentSystemComponent class.
    /// </summary>
    public AgentSystemComponent()
      : base(RS.systemName, RS.systemComponentNickName,
          RS.systemDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams
      (GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentsName, RS.agentNickName, RS.agentDescription, 
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.emittersName, RS.emitterNickName, RS.emittersDescription,
                                    GH_ParamAccess.list);
      pManager.AddGenericParameter(RS.environmentName, RS.environmentNickName, "Restricts and Agent's postion to be contained within the environment. This is most useful for Surface Environments.",
                                    GH_ParamAccess.item);
      pManager[2].Optional = true;
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

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and 
    /// store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      List<IAgent> agents = new List<IAgent>();
      List<AbstractEmitterType> emitters = new List<AbstractEmitterType>();
      AbstractEnvironmentType environment = null;

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this
      // method.
      if (!da.GetDataList(0, agents)) return;
      if (!da.GetDataList(1, emitters)) return;
      da.GetData(2, ref environment);
      
      //if (!DA.GetDataList(2, forces)) return;

      // We should now validate the data and warn the user if invalid data is 
      // supplied.
      if (agents.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.agentsCountErrorMessage);
        return;
      }
      if (emitters.Count <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.emittersCountErrorMessage);
        return;
      }

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      if (system == null)
      {
        system = new AgentSystemType(agents.ToArray(), emitters.ToArray(), environment);
      }
      else
      {
        system = new AgentSystemType(agents.ToArray(), emitters.ToArray(), environment, system);
      }

      // Finally assign the system to the output parameter.
      da.SetData(0, system);
    }

    /// <summary>
    /// Provides an Icon for the component.
    /// </summary>
    protected override Bitmap Icon
    {
      get
      {
        //You can add image files to your project resources and access them like this:
        // return RS.IconForThisComponent;
        return RS.icon_system;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(RS.systemComponentGUID); }
    }
  }
}