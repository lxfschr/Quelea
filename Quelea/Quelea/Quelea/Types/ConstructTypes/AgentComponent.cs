using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class AgentComponent : AbstractConstructTypeComponent
  {
    private IParticle particle;
    private double maxSpeed;
    private double maxForce;
    private double visionRadius;
    private double visionAngle;
    /// <summary>
    /// Initializes a new instance of the AgentComponent class.
    /// </summary>
    public AgentComponent()
      : base(RS.constructAgentName, RS.constructAgentNickname,
          RS.constructAgentDescription, RS.icon_constructAgent, RS.constructAgentComponentGuid)
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.particleName + " " + RS.queleaName + " Settings", RS.particleNickname + RS.queleaNickname + "S", RS.particleDescription, GH_ParamAccess.item);
      pManager.AddNumberParameter(RS.maxSpeedName, RS.maxSpeedNickname, RS.maxSpeedDescription, GH_ParamAccess.item, RS.maxSpeedDefault);
      pManager.AddNumberParameter(RS.maxForceName, RS.maxForceNickname, RS.maxForceDescription, GH_ParamAccess.item, RS.maxForceDefault);
      pManager.AddNumberParameter(RS.visionRadiusName, RS.visionRadiusNickname, RS.visionRadiusDescription, GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddNumberParameter(RS.visionAngleName, RS.visionAngleNickname, RS.visionAngleDescription, GH_ParamAccess.item, RS.visionAngleDefault);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.agentName + " " + RS.queleaName + " Settings", RS.agentNickname + RS.queleaNickname + "S", RS.agentDescription, GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      // Then we need to access the input parameters individually. 
      // When data cannot be extracted from a parameter, we should abort this method.
      if (!da.GetData(nextInputIndex++, ref particle)) return false;
      if (!da.GetData(nextInputIndex++, ref maxSpeed)) return false;
      if (!da.GetData(nextInputIndex++, ref maxForce)) return false;
      if (!da.GetData(nextInputIndex++, ref visionRadius)) return false;
      if (!da.GetData(nextInputIndex++, ref visionAngle)) return false;

      // We should now validate the data and warn the user if invalid data is supplied.
      //if (lifespan <= 0)
      //{
      //  AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.lifespanErrorMessage);
      //  return;
      //}
      if (maxSpeed < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.maxSpeedErrorMessage);
        return false;
      }
      if (maxForce < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.maxForceErrorMessage);
        return false;
      }
      if (visionRadius < 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionRadiusErrorMessage);
        return false;
      }
      if (!(0 <= visionAngle && visionAngle <= 360))
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.visionAngleErrorMessage);
        return false;
      }
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      // We're set to create the output now. To keep the size of the SolveInstance() method small, 
      // The actual functionality will be in a different method:
      IAgent agent = new AgentType(particle, maxSpeed, maxForce, visionRadius, visionAngle);

      // Finally assign the spiral to the output parameter.
      da.SetData(nextOutputIndex++, agent);
    }
  }
}