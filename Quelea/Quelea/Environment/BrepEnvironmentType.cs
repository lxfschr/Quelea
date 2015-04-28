using System;
using Quelea.Util;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using RS = Quelea.Properties.Resources;

namespace Quelea
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
      if (!environment.IsPointInside(pt, Constants.AbsoluteTolerance, true))
      {
        return environment.ClosestPoint(pt);
      }
      return pt;
    }

    public override Point3d MapTo2D(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Point3d ClosestPointOnRef(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Point3d MapTo3D(Point3d pt)
    {
      return ClosestPoint(pt);
    }

    public override Vector3d ClosestNormal(Point3d pt)
    {
      Point3d closestPoint;
      ComponentIndex componentIndex;
      double s;
      double t;
      const double maxDist = 100;
      Vector3d normal;
      environment.ClosestPoint(pt, out closestPoint, out componentIndex, out s, out t, maxDist, out normal);
      return normal;
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

    private static Curve[] GetFeelerCrvs(IParticle particle, double visionDistance, 
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
      
      double feelerAngle = RS.HALF_PI;
      //Calculate straight ahead feeler with length visionDistance
      Vector3d feelerVec = particle.Velocity;
      feelerVec.Unitize();
      feelerVec = Vector3d.Multiply(feelerVec, visionDistance);
      feelers[0] = new Line(particle.Position3D, feelerVec).ToNurbsCurve();

      if (!accurate)
      {
        return feelers;
      }

      //Calculate tertiary feelers with length bodySize
      feelerVec = particle.Velocity;
      feelerVec.Unitize();
      Plane rotPln = new Plane(particle.Position3D, particle.Velocity);
      Vector3d rotAxis = rotPln.XAxis;
      feelers[1] = GetFeelerCrv(feelerVec, particle.Position, particle.BodySize, feelerAngle, rotAxis);
      feelers[2] = GetFeelerCrv(feelerVec, particle.Position, particle.BodySize, -feelerAngle, rotAxis);
      rotAxis = rotPln.YAxis;
      feelers[3] = GetFeelerCrv(feelerVec, particle.Position, particle.BodySize, feelerAngle, rotAxis);
      feelers[4] = GetFeelerCrv(feelerVec, particle.Position, particle.BodySize, -feelerAngle, rotAxis);

      return feelers;
    }

    public override Vector3d AvoidEdges(IAgent agent, double distance)
    {
      Vector3d steer = new Vector3d();
      Vector3d avoidVec, parVec;
      
      Vector3d velocity = agent.Velocity;
      Point3d position = agent.Position3D;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = GetFeelerCrvs(agent, distance, true);
      int count = 0;

      foreach (Curve feeler in feelers)
      {
        //Check feeler intersection with each brep face
        foreach (BrepFace face in environment.Faces)
        {
          Intersection.CurveBrepFace(feeler, face, Constants.AbsoluteTolerance, out overlapCrvs, out intersectPts);
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
            avoidVec = avoidVec / position.DistanceTo(intersectPts[0]);
            steer = steer + avoidVec;
            count++;
            break; //Break when we hit a face
          }
        }
      }
      if (count > 0)
      {
        steer = steer / count;
      }

      return steer;
    }

    public override bool BounceContain(IParticle particle)
    {
      Vector3d velocity = particle.Velocity;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = GetFeelerCrvs(particle, particle.BodySize, false);

      foreach (Curve feeler in feelers)
      {
        //Check feeler intersection with each brep face
        foreach (BrepFace face in environment.Faces)
        {
          Intersection.CurveBrepFace(feeler, face, Constants.AbsoluteTolerance, out overlapCrvs, out intersectPts);
          if (intersectPts.Length > 0)
          {
            Point3d testPt = intersectPts[0];
            double u, v;
            face.ClosestPoint(testPt, out u, out v);
            Vector3d normal = face.NormalAt(u, v);
            normal.Reverse();
            velocity = Vector.Reflect(velocity, normal);
            particle.Velocity = velocity;
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

    public override bool Contains(Point3d pt)
    {
      return environment.IsPointInside(pt, Constants.AbsoluteTolerance, true);
    }
  }
}
