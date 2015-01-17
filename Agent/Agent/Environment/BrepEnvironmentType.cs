using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{
  class BrepEnvironmentType : EnvironmentType, IDisposable
  {
    private Brep environment;

    public void Dispose() {
      this.environment.Dispose();
    }

    // Default Constructor.
    public BrepEnvironmentType()
      : base()
    {
      Interval interval = new Interval(-100.0, 100.0);
      this.environment = new Brep();
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
      if ((object)p == null)
      {
        return false;
      }

      return base.Equals(obj) && this.environment.Equals(p.environment);
    }

    public bool Equals(BrepEnvironmentType p)
    {
      return base.Equals((EnvironmentType)p) && this.environment.Equals(p.environment);
    }

    public override int GetHashCode()
    {
      return this.environment.GetHashCode();
    }

    public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
    {
      return new BrepEnvironmentType(this);
    }

    public override bool IsValid
    {
      get
      {
        return (this.environment.IsValid && this.environment.IsSolid);
      }

    }

    public override string ToString()
    {
      string environment = "Brep: " + this.environment.ToString() + "\n";
      return environment;
    }

    public override Point3d closestPoint(Point3d pt)
    {
      double tol = 0.01;
      if (!environment.IsPointInside(pt, tol, true))
      {
        return this.environment.ClosestPoint(pt);
      }
      return pt;
    }

    public override Point3d closestRefPoint(Point3d pt)
    {
      return closestPoint(pt);
    }

    public override Point3d closestRefPointOnRef(Point3d pt)
    {
      return closestPoint(pt);
    }

    public override Point3d closestPointOnRef(Point3d pt)
    {
      return closestPoint(pt);
    }

    //visionAngle in radians
    private static Curve getFeelerCrv(Vector3d feelerVec, Point3d position, 
                                      double bodySize, double visionAngle, 
                                      Vector3d rotAxis)
    {
      feelerVec.Rotate(visionAngle, rotAxis);
      Vector3d.Multiply(feelerVec, bodySize);
      return new Line(position, feelerVec).ToNurbsCurve();
    }

    private static Curve[] getFeelerCrvs(AgentType agent, double visionDistance, 
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
      feelers[1] = getFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, feelerAngle, rotAxis);
      feelers[2] = getFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, -feelerAngle, rotAxis);
      rotAxis = rotPln.YAxis;
      feelers[3] = getFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, feelerAngle, rotAxis);
      feelers[4] = getFeelerCrv(feelerVec, agent.RefPosition, agent.BodySize, -feelerAngle, rotAxis);

      return feelers;
    }

    public override Vector3d avoidEdges(AgentType agent, double distance)
    {
      Vector3d steer = new Vector3d();
      Vector3d avoidVec, parVec;
      
      Vector3d velocity = agent.Velocity;
      Point3d position = agent.Position;
      
      double tol = 0.01;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = getFeelerCrvs(agent, distance, true);
      int count = 0;

      foreach (Curve feeler in feelers)
      {
        //Check feeler intersection with each brep face
        foreach (BrepFace face in environment.Faces)
        {
          Rhino.Geometry.Intersect.Intersection.CurveBrepFace(feeler, face, tol, out overlapCrvs, out intersectPts);
          if (intersectPts.Length > 0)
          {
            Point3d testPt = feeler.PointAtEnd;
            double u, v;
            face.ClosestPoint(testPt, out u, out v);
            Vector3d normal = face.NormalAt(u, v);
            normal.Reverse();
            Util.Vector.getProjectionComponents(normal, velocity, out parVec, out avoidVec);
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

    public override bool bounceContain(AgentType agent)
    {
      Vector3d velocity = agent.Velocity;
      
      double tol = 0.01;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = getFeelerCrvs(agent, agent.BodySize, false);

      foreach (Curve feeler in feelers)
      {
        //Check feeler intersection with each brep face
        foreach (BrepFace face in environment.Faces)
        {
          Rhino.Geometry.Intersect.Intersection.CurveBrepFace(feeler, face, tol, out overlapCrvs, out intersectPts);
          if (intersectPts.Length > 0)
          {
            Point3d testPt = intersectPts[0];
            double u, v;
            face.ClosestPoint(testPt, out u, out v);
            Vector3d normal = face.NormalAt(u, v);
            normal.Reverse();
            velocity = Util.Vector.reflect(velocity, normal);
            agent.Velocity = velocity;
            return true;
          }
        }
      }
      return false;
    }

    public override BoundingBox getBoundingBox()
    {
      return this.environment.GetBoundingBox(false);
    }
  }
}
