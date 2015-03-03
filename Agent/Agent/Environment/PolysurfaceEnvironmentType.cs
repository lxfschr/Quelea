using System;
using Agent.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;
using RS = Agent.Properties.Resources;

namespace Agent
{
  class PolysurfaceEnvironmentType : AbstractEnvironmentType, IDisposable
  {
     private readonly Brep environment;

    public void Dispose() {
      environment.Dispose();
    }

    // Default Constructor.
    public PolysurfaceEnvironmentType()
    {
      environment = new Brep();
    }

    public PolysurfaceEnvironmentType(Brep environment)
    {
      this.environment = environment;
    }

    public PolysurfaceEnvironmentType(PolysurfaceEnvironmentType environment)
    {
      this.environment = environment.environment;
    }

    public override Point3d ClosestPoint(Point3d pt)
    {
      Point3d closestPoint;
      ComponentIndex componentIndex;
      double s;
      double t;
      double maxDist = 10000;
      Vector3d normal;
      environment.ClosestPoint(pt, out closestPoint, out componentIndex, out s, out t, maxDist, out normal);
      return closestPoint;
    }

    public override Point3d ClosestRefPoint(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Point3d ClosestRefPointOnRef(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    //visionAngle in radians
    private static Curve GetFeelerCrv(Vector3d feelerVec, Point3d position, 
                                      double bodySize, double visionAngle, 
                                      Vector3d rotAxis)
    {
      feelerVec.Rotate(visionAngle, rotAxis);
      feelerVec = Vector3d.Multiply(feelerVec, bodySize);
      return new Line(position, feelerVec).ToNurbsCurve();
    }

    private static Curve[] GetFeelerCrvs(AgentType agent, double visionDistance, 
                                  bool accurate)
    {
      Curve[] feelers;
      if (accurate)
      {
        feelers = new Curve[5];
      }
      else
      {
        feelers = new Curve[1];
      }
      
      double feelerAngle = Math.PI/2;
      //Calculate straight ahead feeler with length visionDistance
      Vector3d feelerVec = agent.Velocity;
      feelerVec.Unitize();
      feelerVec = Vector3d.Multiply(feelerVec, visionDistance);
      feelers[0] = new Line(agent.Position, feelerVec).ToNurbsCurve();

      if (!accurate)
      {
        return feelers;
      }

      //Calculate tertiary feelers with length bodySize
      feelerVec = agent.Velocity;
      feelerVec.Unitize();
      Plane rotPln = new Plane(agent.Position, agent.Velocity);
      Vector3d rotAxis = rotPln.XAxis;
      feelers[1] = GetFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, feelerAngle, rotAxis);
      feelers[2] = GetFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, -feelerAngle, rotAxis);
      rotAxis = rotPln.YAxis;
      feelers[3] = GetFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, feelerAngle, rotAxis);
      feelers[4] = GetFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, -feelerAngle, rotAxis);

      return feelers;
    }

    public override Vector3d AvoidEdges(AgentType agent, double distance)
    {
      throw new NotImplementedException();
    }

    public override bool BounceContain(AgentType agent)
    {
      throw new NotImplementedException();
    }

    public override bool Equals(object obj)
    {
      PolysurfaceEnvironmentType p = obj as PolysurfaceEnvironmentType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj) && environment.Equals(p.environment);
    }

    public bool Equals(PolysurfaceEnvironmentType p)
    {
      return base.Equals(p) && environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return environment.GetHashCode();
    }

   

    public override bool IsValid
    {
      get
      {
        return (environment.IsValid);
      }

    }
    public override IGH_Goo Duplicate()
    {
      return new PolysurfaceEnvironmentType(this);
    }

    public override string ToString()
    {
      string environmentStr = Util.String.ToString(RS.brepEnvName, environment);
      return environmentStr;
    }

    public override BoundingBox GetBoundingBox()
    {
      return environment.GetBoundingBox(false);
    }
  }
}
