using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AttractForceComponent : AbstractAttractionForceComponent
  {
    private double mass;
    private double lowerLimit, upperLimit;

    public AttractForceComponent()
      : base(RS.attractForceName, RS.attractForceComponentNickName,
          RS.attractForceDescription, RS.forcesSubCategoryName, 
          RS.icon_AttractForce, RS.attractForceGuid)
    {
      mass = RS.weightMultiplierDefault;
      lowerLimit = RS.attractLowerLimitDefault;
      upperLimit = RS.attractUpperLimitDefault;
    }

    protected override void RegisterInputParams4(GH_InputParamManager pManager)
    {
      pManager.AddNumberParameter(RS.massName, RS.massNickName, "More massive attractors will exert a stronger attraction force than smaller ones.",
        GH_ParamAccess.item, RS.massDefault);
      pManager.AddNumberParameter("Distance Lower Limit", "L",
        "The lower limit of the distance by which the strength is divided by.", GH_ParamAccess.item, RS.attractLowerLimitDefault);
      pManager.AddNumberParameter("Distance Upper Limit", "U",
        "The upper limit of the distance by which the strength is divided by.", GH_ParamAccess.item, RS.attractUpperLimitDefault);
    }

    protected override void RegisterOutputParams2(GH_OutputParamManager pManager)
    {
    }

    protected override bool GetInputs4(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref mass)) return false;
      if (!da.GetData(nextInputIndex++, ref lowerLimit)) return false;
      if (!da.GetData(nextInputIndex++, ref upperLimit)) return false;

      if (mass <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.massErrorMessage);
        return false;
      }

      if (lowerLimit <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Lower limit must be greater than 0.");
        return false;
      }
      if (upperLimit <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Upper limit must be greater than 0.");
        return false;
      }
      if (upperLimit < lowerLimit)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Upper limit must be greater than lower limit.");
        return false;
      }

      return true;
    }

    protected override Vector3d CalcForce()
    {
      Vector3d force = Vector3d.Subtract(new Vector3d(targetPt), new Vector3d(agent.RefPosition));
      double distance = force.Length;
      // If the distance is greater than the radius of the attractor,
      // and the radius is positive, do not apply the force.
      // Negative radius causes all Agents to be affected, regardless of distance.
      if (distance > radius && radius >= 0)
      {
        return new Vector3d();
      }
      // Clamp the distance so the force lies within a reasonable value.
      distance = Util.Number.Clamp(distance, lowerLimit, upperLimit);
      force.Unitize();
      // Divide by distance squared so the farther away the Attractor is,
      // the weaker the force.
      double strength = (mass * agent.Mass) / (distance * distance);
      force = Vector3d.Multiply(force, strength);
      return force;
    }
  }
}
