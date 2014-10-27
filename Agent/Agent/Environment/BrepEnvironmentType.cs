using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{
  class BrepEnvironmentType : EnvironmentType
  {
    private Brep environment;

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
      return base.Equals((BrepEnvironmentType)p) && this.environment.Equals(p.environment);
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

    public override Vector3d avoidEdges(AgentType agent, double distance)
    {
      Vector3d steer = new Vector3d();
      Vector3d velocity = agent.Velocity;
      Vector3d probeVec = velocity;
      probeVec.Unitize();
      probeVec = Vector3d.Multiply(probeVec, distance);
      Curve crv = new Line(agent.Position, probeVec).ToNurbsCurve();
      double tol = 0.01;

      Curve[] overlapCrvs;
      Point3d[] intersectPts;
      foreach (BrepFace face in environment.Faces)
      {
        Rhino.Geometry.Intersect.Intersection.CurveBrepFace(crv, face, tol, out overlapCrvs, out intersectPts);
        if (intersectPts.Length > 0)
        {
          Point3d testPt = crv.PointAtEnd;
          double u, v;
          face.ClosestPoint(testPt, out u, out v);
          Vector3d normal = face.NormalAt(u, v);
          normal.Reverse();
          double scalar = Util.Vector.dotProduct(normal, velocity) / (velocity.Length * velocity.Length);
          Vector3d parallelVec = Vector3d.Multiply(velocity, scalar);
          steer = Vector3d.Subtract(normal, parallelVec);
          break;
        }
      }

      return steer;
    }

  }
}
