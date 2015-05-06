using System;
using Grasshopper.Kernel;
using Quelea.Util;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class AvoidUnalignedCollisionForceComponent : AbstractBoidForceComponent
  {
    private double minTimeToCollision, potentialCollisionDistance;
    private Point3d ourPositionAtNearestApproach, hisPositionAtNearestApproach;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    public AvoidUnalignedCollisionForceComponent()
      : base("Avoid Unaligned Collision Force", "AvoidCollision",
          "Applied a force to steer the Agent away from a predicted potential collision.", RS.icon_avoidUnalignedCollision, "7b046f6a-0e29-44c2-a500-8b484346effb")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter("Minumum Time To Collision", "T",
        "The number of seconds of travel at the Agent's current velocity that the Agent will predict its position to avoid collisions.", GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddNumberParameter("Potential Collision Distance", "D",
        "The distance which will be used to indicate a potential collision is iminent. This number is added to the Agent's and the neighbor" +
        "s Body Sizes.", GH_ParamAccess.item, 0);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref minTimeToCollision)) return false;
      if (!da.GetData(nextInputIndex++, ref potentialCollisionDistance)) return false;
      return true;
    }

    // References:
    // http://rocketmandevelopment.com/blog/steering-behaviors-unaligned-collision-avoidance/
    // http://steeringfluid.googlecode.com/svn-history/r17/trunk/opensteer/include/OpenSteer/SteerLibrary.h
    // http://www.red3d.com/cwr/steer/Unaligned.html
    protected override Vector3d CalculateDesiredVelocity()
    {
      double steer = 0;

      IParticle threat = null;

      // Time (in seconds) until the most immediate collision threat found
      // so far.  Initial value is a threshold: don't look more than this
      // many frames into the future.
      double minTime = minTimeToCollision;

      // xxx solely for annotation
      Point3d xxxThreatPositionAtNearestApproach = Point3d.Unset;
      Point3d xxxOurPositionAtNearestApproach = Point3d.Unset;

      // for each of the other vehicles, determine which (if any)
      // pose the most immediate threat of collision.
      foreach (IParticle neighbor in neighbors)
      {
        if (!neighbor.Position.Equals(agent.Position))
        {
          // avoid when future positions are this close (or less)
          double collisionDangerThreshold = agent.BodySize / 2 + neighbor.BodySize / 2 + potentialCollisionDistance;

          // predicted time until nearest approach of "this" and "other"
          double time = PredictNearestApproachTime(neighbor);

          // If the time is in the future, sooner than any other
          // threatened collision...
          if ((time >= 0) && (time < minTime))
          {

            // if the two will be close enough to collide,
            // make a note of it
            if (ComputeNearestApproachPositions(neighbor, time) < collisionDangerThreshold)
            {
              //D = neighbor.Position;
              minTime = time;
              threat = neighbor;
              xxxThreatPositionAtNearestApproach = hisPositionAtNearestApproach;
              xxxOurPositionAtNearestApproach = ourPositionAtNearestApproach;
            }
          }
        }
      }
      // if a potential collision was found, compute steering to avoid
      if (threat != null)
      {
        // parallel: +1, perpendicular: 0, anti-parallel: -1
        double parallelness = Util.Vector.DotProduct(agent.Forward, threat.Forward);
        double angle = 0.707;
        if (parallelness < -angle)
        {
          // anti-parallel "head on" paths:
          // steer away from future threat position
          Vector3d offset = xxxThreatPositionAtNearestApproach - agent.Position;
          double sideDot = Util.Vector.DotProduct(offset, agent.Side);
          steer = (sideDot > 0) ? -1.0 : 1.0;
        }
        else
        {
          if (parallelness > angle)
          {
            // parallel paths: steer away from threat
            Vector3d offset = threat.Position - agent.Position;
            double sideDot = Util.Vector.DotProduct(offset, agent.Side);
            steer = (sideDot > 0) ? -1.0 : 1.0;
          }
          else
          {
            // perpendicular paths: steer behind threat
            // (only the slower of the two does this)
            if (threat.SquareSpeed <= agent.SquareSpeed)
            {
              double sideDot = Util.Vector.DotProduct(agent.Side, threat.Velocity);
              steer = (sideDot > 0) ? -1.0 : 1.0;
            }
          }
        }
      }

      return agent.Side * steer * agent.MaxSpeed;
    }

    // Given two vehicles, based on their current positions and velocities,
    // determine the time until nearest approach
    //
    // XXX should this return zero if they are already in contact?
    private double PredictNearestApproachTime(IParticle neighbor)
    {
      // imagine we are at the origin with no velocity,
      // compute the relative velocity of the other vehicle
      Vector3d agentVelocity = agent.Velocity;
      Vector3d neighborVelocity = neighbor.Velocity;
      Vector3d relativeVelocity = neighborVelocity - agentVelocity;
      double relativeSpeed = relativeVelocity.Length;

      // for parallel paths, the vehicles will always be at the same distance,
      // so return 0 (aka "now") since "there is no time like the present"
      if (relativeSpeed == 0) return 0;

      // Now consider the path of the other vehicle in this relative
      // space, a line defined by the relative position and velocity.
      // The distance from the origin (our vehicle) to that line is
      // the nearest approach.

      // Take the unit tangent along the other vehicle's path
      Vector3d relativeTangent = relativeVelocity / relativeSpeed;

      // find distance from its path to origin (compute offset from
      // other to us, find length of projection onto path)
      Vector3d relativePosition = agent.Position - neighbor.Position;
      double projection = Util.Vector.DotProduct(relativeTangent, relativePosition);

      return projection / relativeSpeed;
    }

    private double ComputeNearestApproachPositions(IParticle neighbor, double time)
    {

      Vector3d agentTravel = agent.Forward * agent.Speed * time;
      Vector3d neighborTravel = neighbor.Forward * neighbor.Speed * time;

      Point3d agentFinal = agent.Position + agentTravel;
      Point3d neighborFinal = neighbor.Position + neighborTravel;

      // xxx for annotation
      ourPositionAtNearestApproach = agentFinal;
      hisPositionAtNearestApproach = neighborFinal;

      return Util.Vector.Vector2Point(agentFinal, neighborFinal).Length;
    }
  }
}