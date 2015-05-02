using Quelea.Util;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class AvoidObstacleForceComponent : AbstractAvoidObstacleForceComponent
  {
    /// <summary>
    /// Initializes a new instance of the ContainForceComponent class.
    /// </summary>
    public AvoidObstacleForceComponent()
      : base("Avoid Obstacle Force", "AvoidObstacle", "Applied a force to steer the Agent away if it is about to intersect with an obstacle.",
             null, "b1eab486-e541-4529-a895-474576bf5c0c")
    {
    }

    protected override Vector3d CalculateDesiredVelocity()
    {
      Vector3d desired = AvoidEdges(agent.VisionRadius*visionRadiusMultiplier);
      desired.Unitize();
      desired = desired * agent.MaxSpeed;
      return desired;
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

    public Vector3d AvoidEdges(double distance)
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
        foreach (BrepFace face in obstacle.Faces)
        {
          Intersection.CurveBrepFace(feeler, face, Constants.AbsoluteTolerance, out overlapCrvs, out intersectPts);
          if (intersectPts.Length > 0)
          {
            Point3d testPt = feeler.PointAtEnd;
            double u, v;
            face.ClosestPoint(testPt, out u, out v);
            Vector3d normal = face.NormalAt(u, v);
            //normal.Reverse();
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
  }
}