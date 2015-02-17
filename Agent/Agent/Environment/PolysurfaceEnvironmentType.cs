using System;
using Agent.Util;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;

namespace Agent
{
  class PolysurfaceEnvironmentType : BrepEnvironmentType
  {
     private Brep environment;

    // Default Constructor.

    public override bool IsValid
    {
      get
      {
        return (environment.IsValid);
      }

    }

    public override Point3d ClosestPoint(Point3d pt)
    {
      BrepFaceList faces = environment.Faces;
      double u, v;
      double minDist = double.MaxValue;
      int index = 0, currIndex = 0;
      foreach (BrepFace face in faces)
      {
        face.ClosestPoint(pt, out u, out v);
        double dist = pt.DistanceTo(face.PointAt(u, v));
        if (dist < minDist)
        {
          minDist = dist;
          index = currIndex;
        }
        currIndex++;
      }
      faces[index].ClosestPoint(pt, out u, out v);
      return faces[index].PointAt(u,v);
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
      Vector3d steer = new Vector3d();
      Vector3d avoidVec, parVec;
      
      Vector3d velocity = agent.Velocity;
      Point3d position = agent.Position;
      
      double tol = 0.01;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = GetFeelerCrvs(agent, distance, true);
      int count = 0;

      Curve[] nakedEdges = environment.DuplicateEdgeCurves(true);
      Curve[] nakedEdgesJoined = Curve.JoinCurves(nakedEdges);

      return steer;
    }

    public override bool BounceContain(AgentType agent)
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
  }
}
