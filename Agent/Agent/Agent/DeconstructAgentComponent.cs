using System;
using System.Drawing;
using RS = Agent.Properties.Resources;
using Grasshopper.Kernel;

namespace Agent
{
  public class DeconstructAgentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the DecomposeAgent class.
    /// </summary>
    public DeconstructAgentComponent()
      : base(RS.deconstructAgentName, RS.deconstructAgentNickName,
          RS.deconstructAgentDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPointParameter(RS.positionName, RS.positionNickName, RS.positionDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter(RS.velocityName, RS.velocityNickName, RS.velocityDescription, GH_ParamAccess.item);
      pManager.AddVectorParameter(RS.accelerationName, RS.accelerationNickName, RS.accelerationDescription, GH_ParamAccess.item);
      pManager.AddIntegerParameter(RS.lifespanName, RS.lifespanNickName, RS.lifespanDescription, GH_ParamAccess.item);
      pManager.AddPointParameter(RS.positionHistoryName, RS.positionHistoryNickName, RS.positionHistoryDescription, GH_ParamAccess.list);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      AgentType agent = new AgentType();

      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(0, ref agent)) return;

      // We should now validate the data and warn the user if invalid data is supplied.

      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:

      // Finally assign the spiral to the output parameter.
      da.SetData(0, agent.Position);
      da.SetData(1, agent.Velocity);
      da.SetData(2, agent.Acceleration);
      da.SetData(3, agent.Lifespan);
      da.SetDataList(4, agent.GetPositionHistoryList());
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
        return RS.icon_deconstructAgent;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(RS.deconstructAgentGUID); }
    }
  }
}