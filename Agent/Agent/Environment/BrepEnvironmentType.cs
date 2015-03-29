using System;
using Agent.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using RS = Agent.Properties.Resources;

namespace Agent
{
  class BrepEnvironmentType : AbstractEnvironmentType, IDisposable
  {
    private readonly Brep environment;

    public void Dispose() {
      environment.Dispose();
    }

    // Default Constructor.
    public BrepEnvironmentType()
    {
      environment = new Brep();
    }

    public BrepEnvironmentType(Brep environment)
    {
      this.environment = environment;
    }

    public BrepEnvironmentType(BrepEnvironmentType environment)
    {
      this.environment = environment.environment;
    }

    public override bool Equals(object obj)
    {
      BrepEnvironmentType p = obj as BrepEnvironmentType;
      if (p == null)
      {
        return false;
      }

      return base.Equals(obj) && environment.Equals(p.environment);
    }

    public bool Equals(BrepEnvironmentType p)
    {
      return base.Equals(p) && environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return environment.GetHashCode();
    }

    public override IGH_Goo Duplicate()
    {
      return new BrepEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (environment.IsValid && environment.IsSolid);
      }

    }

    public override string ToString()
    {
      string environmentStr = Util.String.ToString(RS.brepEnvironmentName, environment);
      return environmentStr;
    }

    public override Point3d ClosestPoint(Point3d pt)
    {
      double tol = 0.01;
      if (!environment.IsPointInside(pt, tol, true))
      {
        return environment.ClosestPoint(pt);
      }
      return pt;
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

    private static Curve[] GetFeelerCrvs(IAgent agent, double visionDistance, 
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

    public override Vector3d AvoidEdges(IAgent agent, double distance)
    {
      Vector3d steer = new Vector3d();
      Vector3d avoidVec, parVec;
      
      Vector3d velocity = agent.Velocity;
      Point3d position = agent.Position;
      
      double tol = 0.01;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = GetFeelerCrvs(agent, distance, true);
      int count = 0;

      foreach (Curve feeler in feelers)
      {
        //Check feeler intersection with each brep face
        foreach (BrepFace face in environment.Faces)
        {
          Intersection.CurveBrepFace(feeler, face, tol, out overlapCrvs, out intersectPts);
          if (intersectPts.Length > 0)
          {
            Point3d testPt = feeler.PointAtEnd;
            double u, v;
            face.ClosestPoint(testPt, out u, out v);
            Vector3d normal = face.NormalAt(u, v);
            normal.Reverse();
            Vector.GetProjectionComponents(normal, velocity, out parVec, out avoidVec);
            avoidVec.Unitize();
            //weight by distance
            avoidVec = Vector3d.Divide(avoidVec, position.DistanceTo(intersectPts[0]));
            steer = Vector3d.Add(steer, avoidVec);
            count++;
            break; //Break when we hit a face
          }
        }
      }
      if (count > 0)
      {
        steer = Vector3d.Divide(steer, count);
      }

      return steer;
    }

    public override bool BounceContain(IAgent agent)
    {
      Vector3d velocity = agent.Velocity;
      
      double tol = 0.01;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = GetFeelerCrvs(agent, agent.BodySize, false);

      foreach (Curve feeler in feelers)
      {
        //Check feeler intersection with each brep face
        foreach (BrepFace face in environment.Faces)
        {
          Intersection.CurveBrepFace(feeler, face, tol, out overlapCrvs, out intersectPts);
          if (intersectPts.Length > 0)
          {
            Point3d testPt = intersectPts[0];
            double u, v;
            face.ClosestPoint(testPt, out u, out v);
            Vector3d normal = face.NormalAt(u, v);
            normal.Reverse();
            velocity = Vector.Reflect(velocity, normal);
            agent.Velocity = velocity;
            return true;
          }
        }
      }
      return false;
    }

    public override BoundingBox GetBoundingBox()
    {
      return environment.GetBoundingBox(false);
    }
  }
}
