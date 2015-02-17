using Grasshopper.Kernel;
using Rhino.Geometry;
using RS = Agent.Properties.Resources;

namespace Agent
{
  public class AttractForceComponent : AbstractAttractionForceComponent
  {
    public AttractForceComponent()
      : base(RS.attractForceName, RS.attractForceComponentNickName,
          RS.attractForceDescription,
          RS.pluginCategoryName, RS.forcesSubCategoryName, RS.icon_AttractForce, RS.attractForceGuid)
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter(RS.massName, RS.massNickName, "More massive attractors will exert a stronger attraction force than smaller ones.",
        GH_ParamAccess.item, RS.massDefault);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddVectorParameter(RS.attractForceName, RS.forceNickName, RS.attractForceName, GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess da)
    {
      base.SolveInstance(da);
      double mass = RS.weightMultiplierDefault;

      if (!da.GetData(4, ref mass)) return;

      if (mass <= 0)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, RS.massErrorMessage);
        return;
      }

      Vector3d force = Run(mass);

      // Finally assign the output parameter.
      da.SetData(0, force);
    }

    protected Vector3d Run(double mass)
    {
      Vector3d force = CalcForce(mass);
      agent.ApplyForce(force);
      return force;
    }

    protected Vector3d CalcForce(double mass)
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
      distance = Util.Number.Clamp(distance, 5, 100);
      force.Unitize();
      // Divide by distance squared so the farther away the Attractor is,
      // the weaker the force.
      double strength = (weightMultiplier * mass * agent.Mass) / (distance * distance);
      force = Vector3d.Multiply(force, strength);
      force = Util.Vector.Limit(force, agent.MaxForce);
      return force;
    }
  }
}
