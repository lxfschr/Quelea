using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agent.Util;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;
using RS = Agent.Properties.Resources;
using String = Agent.Util.String;

namespace Agent
{
  class PolysurfaceEnvironmentType : AbstractEnvironmentType, IDisposable
  {
    private readonly Brep environment;
    private readonly Brep[][] borderWallsArray;
    private readonly Vector3d borderDir;

    public void Dispose() {
      environment.Dispose();
    }

    // Default Constructor.
    public PolysurfaceEnvironmentType()
    {
      environment = new Brep();
    }

    public PolysurfaceEnvironmentType(Brep environment, Vector3d borderDir)
    {
      this.environment = environment;
      this.borderDir = borderDir;
      this.borderWallsArray = CreateBorderWalls();
    }

    public PolysurfaceEnvironmentType(PolysurfaceEnvironmentType environment)
      : this(environment.environment, environment.borderDir)
    {
    }

    private Brep[][] CreateBorderWalls()
    {
      Curve[] borderCrvs = GetNakedEdges();

      int n = borderCrvs.Length;

      Point3d[][] edgePtsArray = new Point3d[n][];
      for (int i = 0; i < n; i++)
      {
        edgePtsArray[i] = DivByAngle(borderCrvs[i], 1.0).ToArray();
      }

      Vector3d[][] normalsArray = new Vector3d[n][];
      for (int i = 0; i < n; i++)
      {
        if (borderDir.Equals(Vector3d.Zero))
        {
          normalsArray[i] = GetBrepNormals(edgePtsArray[i]).ToArray();
        }
        else
        {
          int m = edgePtsArray[i].Length;
          normalsArray[i] = new Vector3d[m];
          for (int j = 0; j < m; j++)
          {
            normalsArray[i][j] = borderDir;
          }
        }
        
      }

      double borderSize = environment.GetBoundingBox(Plane.WorldXY).Diagonal.Length / 10;

      Brep[][] borderWallsArray = new Brep[n][];
      for (int i = 0; i < n; i++)
      {
        borderWallsArray[i] = GetBorderWalls(borderCrvs[i], edgePtsArray[i], normalsArray[i], borderSize);
      }
      return borderWallsArray;
    }

    private Curve[] GetNakedEdges()
    {
      List<Curve> nakedEdges = new List<Curve>();
      foreach (BrepEdge edge in environment.Edges)
      {
        // Find only the naked edges
        if (edge.Valence == EdgeAdjacency.Naked)
        {
          Curve crv = edge.DuplicateCurve();
          if (null != crv)
            nakedEdges.Add(crv);
        }
      }
      RhinoDoc doc = RhinoDoc.ActiveDoc;
      double tol = 2.1 * doc.ModelAbsoluteTolerance;

      return Curve.JoinCurves(nakedEdges, tol);
    }

    /**
     * divideCrvsByDeltaTan : Curve[] * double -> LinkedList<Plane>
     * REQUIRES: theta > 0
     * ENSURES: divideCrvsByDeltaTan(crvs, theta) returns a linked list of planes
     *          along the curves s.t. there is a plane at every point along
     *          the curve where the change in the tangent vector between
     *          two points is greater than theta.
    **/
    private IEnumerable<Point3d> DivByAngle(Curve crv, double angle)
    {

      //initialize parameters
      double theta = angle;
      const double stepSize = 0.5;

      //initialize list
      List<Point3d> pts = new List<Point3d>();

      Continuity c = Continuity.C1_continuous;
      //initialize data
      Interval dom = crv.Domain;
      double rover = dom.Min; //steps along the curve by stepSize

      //Add plane at start point of curve to list
      Point3d pt = crv.PointAt(rover);
      pts.Add(pt);

      //Increment
      Vector3d prevTan = crv.TangentAt(rover);
      double oldRover = rover; //stores the previous rover for comparison
      rover += stepSize;

      while (rover < dom.Max)
      {
        Vector3d currTan = crv.TangentAt(rover);
        //If there is a discontinuity between the oldRover and rover
        //then place a point at the discontinuity and update prevTan.
        double discontinuity;
        bool isDisc = crv.GetNextDiscontinuity(c, oldRover, rover,
          out discontinuity);
        if (isDisc)
        {
          pt = crv.PointAt(discontinuity);
          pts.Add(pt);
          prevTan = crv.TangentAt(discontinuity);
        }

        //If the change in tangent vector is greater than theta,
        //then drop a target at the rover and update prevTan.
        double delta = RhinoMath.ToDegrees(Math.Abs(Vector3d.VectorAngle(prevTan, currTan)));
        if (delta > theta)
        {
          pt = crv.PointAt(rover);
          pts.Add(pt);
          prevTan = currTan;
        }
        //Increment
        oldRover = rover;
        rover += stepSize;
      }

      //Add target at end point of curve
      pt = crv.PointAt(dom.Max);
      pts.Add(pt);
      return pts;
    }

    private IEnumerable<Vector3d> GetBrepNormals(IEnumerable<Point3d> pts)
    {
      //Initialize variables
      List<Surface> brepSrfs = new List<Surface>();
      List<Vector3d> normals = new List<Vector3d>();
      const double epsilon = 0.0001; //The minimum distance to determine which surface a pt is touching.

      //Explode brep
      BrepFaceList brepFaces = environment.Faces;
      foreach (BrepFace face in brepFaces)
      {
        Surface srf = face.ToBrep().Surfaces[0];
        if (face.OrientationIsReversed)
        {
          brepSrfs.Add(srf.Reverse(0));
        }
        else
        {
          brepSrfs.Add(srf);
        }
      }

      /*
      for each point, get the closest point to each surface
      */
      foreach (Point3d pt in pts)
      {
        List<Plane> framesTemp = new List<Plane>();
        foreach (Surface srf in brepSrfs)
        {
          double u;
          double v;
          srf.ClosestPoint(pt, out u, out v);
          Plane frame;
          srf.FrameAt(u, v, out frame);
          if (pt.DistanceTo(frame.Origin) <= epsilon)
          {
            framesTemp.Add(frame);
          }
        }
        /*
        if there is 1 close surface:
          add the two normal vectors together to get the bisector
        */
        Vector3d brepNormal = framesTemp[0].Normal;
        /*
        if there are 2 equally close surfaces:
          add the two normal vectors together to get the bisector
        */
        if (framesTemp.Count > 1)
        {
          for (int j = 1; j < framesTemp.Count; j++)
          {
            Plane pln = framesTemp[j];
            brepNormal = Vector3d.Add(brepNormal, pln.Normal);
          }
        }

        brepNormal.Unitize();

        normals.Add(brepNormal);
      }
      return normals;
    }

    private Brep[] GetBorderWalls(Curve borderCrv, IEnumerable<Point3d> pts, IEnumerable<Vector3d> nrmls, double dist)
    {
      List<Point3d> ptsOut = new List<Point3d>();
      List<Point3d> ptsIn = new List<Point3d>();
      IEnumerator ptEnum = pts.GetEnumerator();
      IEnumerator nrmlEnum = nrmls.GetEnumerator();
      while ((ptEnum.MoveNext()) && (nrmlEnum.MoveNext()))
      {
        Point3d pt = (Point3d)ptEnum.Current;
        Vector3d nrml = (Vector3d)nrmlEnum.Current;
        Transform mvOut = Transform.Translation(nrml * dist);
        Transform mvIn = Transform.Translation(nrml * -dist);
        Point3d ptOut = new Point3d(pt);
        Point3d ptIn = new Point3d(pt);
        ptOut.Transform(mvOut);
        ptIn.Transform(mvIn);
        ptsOut.Add(ptOut);
        ptsIn.Add(ptIn);
      }

      Curve crvOut;
      Curve crvIn;

      if (borderCrv.IsPolyline())
      {
        crvOut = new PolylineCurve(ptsOut);
        crvIn = new PolylineCurve(ptsIn);
      }
      else
      {
        crvOut = Curve.CreateInterpolatedCurve(ptsOut, 3);
        crvIn = Curve.CreateInterpolatedCurve(ptsIn, 3);
      }
      List<Curve> crvs = new List<Curve>();
      crvs.Add(crvOut);
      crvs.Add(crvIn);
      Brep[] lofts = Brep.CreateFromLoft(crvs, Point3d.Unset, Point3d.Unset, LoftType.Normal, false);

      double minSoFar = Double.MaxValue;
      Interval interval = new Interval(0, 1);
      Point3d loftPt = lofts[0].Faces[0].PointAt(0, 0);
      BrepFace testFace = environment.Faces[0];
      Point3d facePt;
      foreach (BrepFace face in environment.Faces)
      {
        double u, v;
        face.ClosestPoint(loftPt, out u, out v);
        facePt = face.PointAt(u, v);
        dist = loftPt.DistanceTo(facePt);
        if (dist < minSoFar)
        {
          minSoFar = dist;
          testFace = face;
        }
      }

      Vector3d loftNrml = lofts[0].Faces[0].NormalAt(0, 0);
      testFace.SetDomain(0, interval);
      testFace.SetDomain(1, interval);
      facePt = testFace.PointAt(0.5, 0.5);
      Vector3d testVec = Vector3d.Subtract(new Vector3d(facePt), new Vector3d(loftPt));
      double dotProd = Vector3d.Multiply(loftNrml, testVec) / (loftNrml.Length * testVec.Length);
      if (dotProd >= 0)
      {
        foreach (Brep brep in lofts)
        {
          brep.Flip();
        }
      }
      return lofts;
    }


    public override Point3d ClosestPoint(Point3d pt)
    {
      Point3d closestPoint;
      ComponentIndex componentIndex;
      double s;
      double t;
      const double maxDist = 100;
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
      Vector3d desired = new Vector3d();
      Vector3d avoidVec, parVec;
      double tol = 0.01;
      Curve[] overlapCrvs;
      Point3d[] intersectPts;

      Curve[] feelers = GetFeelerCrvs(agent, distance, true);

      int count = 0;
      foreach (Brep[] borderWalls in borderWallsArray)
      {
        foreach (Brep brep in borderWalls)
        {
          foreach (Curve feeler in feelers)
          {
            //Check feeler intersection with each brep face
            foreach (BrepFace face in brep.Faces)
            {
              Intersection.CurveBrepFace(feeler, face, tol, out overlapCrvs, out intersectPts);
              if (intersectPts.Length > 0)
              {
                Point3d testPt = feeler.PointAtEnd;
                double u, v;
                face.ClosestPoint(testPt, out u, out v);
                Vector3d normal = face.NormalAt(u, v);
                normal.Reverse();
                Vector.GetProjectionComponents(normal, agent.Velocity, out parVec, out avoidVec);
                avoidVec.Unitize();
                //weight by distance
                if (!agent.Position.DistanceTo(intersectPts[0]).Equals(0))
                {
                  avoidVec = Vector3d.Divide(avoidVec, agent.Position.DistanceTo(intersectPts[0]));
                }
                desired = Vector3d.Add(desired, avoidVec);
                count++;
                break; //Break when we hit a face
              }
            }
          }
        }
      }
      if (count > 0)
      {
        desired = Vector3d.Divide(desired, count);
      }

      return desired;
    }

    public override bool BounceContain(IAgent agent)
    {
      Vector3d velocity = agent.Velocity;

      double tol = 0.01;

      Curve[] feelers = GetFeelerCrvs(agent, agent.BodySize, false);

      foreach (Brep[] borderWalls in borderWallsArray)
      {
        foreach (Brep brep in borderWalls)
        {
          foreach (Curve feeler in feelers)
          {
            //Check feeler intersection with each brep face
            foreach (BrepFace face in brep.Faces)
            {
              Curve[] overlapCrvs;
              Point3d[] intersectPts;
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
        }
      }
      return false;
    }

    public Brep[][] BorderWallsArray 
    {
      get { return borderWallsArray; }
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
      string environmentStr = String.ToString(RS.brepEnvName, environment);
      return environmentStr;
    }

    public override BoundingBox GetBoundingBox()
    {
      return environment.GetBoundingBox(false);
    }
  }
}
