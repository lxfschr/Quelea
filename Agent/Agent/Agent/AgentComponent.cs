using System;
using System.Drawing;
using Grasshopper.Kernel;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AgentComponent : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the AgentComponent class.
    /// </summary>
    public AgentComponent()
      : base(RS.constructAgentName, RS.constructAgentNickName,
          RS.constructAgentDescription,
          RS.pluginCategoryName, RS.pluginSubCategoryName)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddIntegerParameter(RS.lifespanName, RS.lifespanNickName, RS.lifespanDescription, GH_ParamAccess.item, RS.lifespanDefault);
      pManager.AddNumberParameter(RS.massName, RS.massNickName, RS.massDescription, GH_ParamAccess.item, RS.massDefault);
      pManager.AddNumberParameter(RS.bodySizeName, RS.bodySizeNickName, RS.bodySizeDescription, GH_ParamAccess.item, RS.bodySizeDefault);
      pManager.AddNumberParameter(RS.maxSpeedName, RS.maxSpeedNickName, RS.maxSpeedDescription, GH_ParamAccess.item, RS.maxSpeedDefault);
      pManager.AddNumberParameter(RS.maxForceName, RS.maxForceNickName, RS.maxForceDescription, GH_ParamAccess.item, RS.maxForceDefault);
      pManager.AddNumberParameter(RS.visionAngleName, RS.visionAngleNickName, RS.visionAngleDescription, GH_ParamAccess.item, RS.visionAngleDefault);
      pManager.AddNumberParameter(RS.visionRadiusName, RS.visionRadiusNickName, RS.visionRadiusDescription, GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddIntegerParameter(RS.historyLenName, RS.historyLenNickName, RS.historyLenDescription, GH_ParamAccess.item, RS.historyLenDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName, RS.agentNickName, RS.agentDescription, GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    // ReSharper disable once InconsistentNaming
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      int lifespan = RS.lifespanDefault;
      double mass =RS.massDefault;
      double bodySize = RS.bodySizeDefault;
      double maxSpeed = RS.maxSpeedDefault;
      double maxForce = RS.maxForceDefault;
      double visionAngle = RS.visionAngleDefault;
      double visionRadius = RS.visionRadiusDefault;
      int historyLength = RS.historyLenDefault;


      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!DA.GetData(0, ref lifespan)) return;
      if (!DA.GetData(1, ref mass)) return;
      if (!DA.GetData(2, ref bodySize)) return;
      if (!DA.GetData(3, ref maxSpeed)) return;
      if (!DA.GetData(4, ref maxForce)) return;
      if (!DA.GetData(5, ref visionAngle)) return;
      if (!DA.GetData(6, ref visionRadius)) return;
      if (!DA.GetData(7, ref historyLength)) return;

      // We should now validate the data and warn the user if invalid data is supplied.
      if (lifespan <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.lifespanErrorMessage);
        return;
      }
      if (mass <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.massErrorMessage);
        return;
      }
      if (bodySize < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.bodySizeErrorMessage);
        return;
      }
      if (maxSpeed < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.maxSpeedErrorMessage);
        return;
      }
      if (maxForce < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.maxForceErrorMessage);
        return;
      }
      if (visionAngle < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleErrorMessage);
        return;
      }
      if (visionRadius < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionRadiusErrorMessage);
        return;
      }


      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      AgentType agent = new AgentType(lifespan, mass, bodySize, maxSpeed,
                                      maxForce, visionAngle, visionRadius
                                      , historyLength);

      // Finally assign the spiral to the output parameter.
      DA.SetData(0, agent);
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
        return RS.icon_agent;
      }
    }

    /// <summary>
    /// Gets the unique ID for this component. Do not change this ID after release.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid(RS.constructAgentComponentGUID); }
    }
  }
}