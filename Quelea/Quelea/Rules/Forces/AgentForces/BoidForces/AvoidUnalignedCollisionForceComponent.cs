using System;
using Grasshopper.Kernel;
using Quelea.Util;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class AvoidUnalignedCollisionForceComponent : AbstractBoidForceComponent
  {
    private double checkDistance, potentialCollisionDistanceThreshold, rotationAngleMultiplier;
    /// <summary>
    /// Initializes a new instance of the ViewForceComponent class.
    /// </summary>
    public AvoidUnalignedCollisionForceComponent()
      : base("Avoid Unaligned Collision Force", "AvoidCollision",
          "Applied a force to steer the Agent away from a predicted potential collision.", null, "7b046f6a-0e29-44c2-a500-8b484346effb")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      base.RegisterInputParams(pManager);
      pManager.AddNumberParameter("Check Distance", "D",
        "The distance to project the agent and neighbors' future positions to check for potential collision.", GH_ParamAccess.item, RS.visionRadiusDefault);
      pManager.AddNumberParameter("Potential Collision Distance Threshold", "T", "The distance from the neighbor's position plus their body size to say is a potential collision.",
        GH_ParamAccess.item, RS.bodySizeDefault/2);
      pManager.AddNumberParameter("Rotation Angle Mutliplier", "R",
        "The number by which the angle of vector to the potential collision point will be rotated by.",
        GH_ParamAccess.item, 0.5);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!base.GetInputs(da)) return false;
      if (!da.GetData(nextInputIndex++, ref checkDistance)) return false;
      if (!da.GetData(nextInputIndex++, ref potentialCollisionDistanceThreshold)) return false;
      if (!da.GetData(nextInputIndex++, ref rotationAngleMultiplier)) return false;
      return true;
    }

    // References:
    // http://rocketmandevelopment.com/blog/steering-behaviors-unaligned-collision-avoidance/
    // http://steeringfluid.googlecode.com/svn-history/r17/trunk/opensteer/include/OpenSteer/SteerLibrary.h
    // http://www.red3d.com/cwr/steer/Unaligned.html
    protected override Vector3d CalculateDesiredVelocity()
    {
      Vector3d desired = Vector3d.Zero;
      Vector3d forward = agent.Velocity;
      forward.Unitize();
      double minDistanceSoFar = Double.MaxValue;

      // Determine which neighbor presents the most 
      // immediate threat for collision.
      foreach (IQuelea neighbor in neighbors)
      {
        // Predict neighbor's future position.
        Point3d neighborFuturePosition = neighbor.Position;
        Vector3d neighborForward = neighbor.Velocity;
        neighborForward.Unitize();
        neighborFuturePosition += neighborForward * checkDistance;

        // Get the vector to the neighbors future position.
        Vector3d vectorToNeighborFuturePosition = Vector.Vector2Point(agent.Position, neighborFuturePosition);

        // Check to see if they might collide in the future.
        // (i.e. the future position is in front of the agent.
        double dotProduct = Vector.DotProduct(vectorToNeighborFuturePosition, forward);
        if (dotProduct > 0) // They might collide in the future.
        {
          // Cast a ray in the direction of current velocity with a length of checkDistance.
          Vector3d ray = forward;
          ray *= checkDistance;
          Vector3d projection = forward;
          projection *= dotProduct;

          Vector3d projectionToNeighborFuturePosition = Vector.Vector2Point(vectorToNeighborFuturePosition, projection);

          double distance = projectionToNeighborFuturePosition.Length;

          if (distance < agent.BodySize / 2 + neighbor.BodySize/2 + potentialCollisionDistanceThreshold && projection.Length < ray.Length && distance < minDistanceSoFar)
          {
            minDistanceSoFar = distance;
            desired = agent.Velocity;
            desired.Unitize();
            desired *= agent.MaxSpeed;
            desired.Rotate(rotationAngleMultiplier * Math.PI, Vector3d.ZAxis); // Rotate it laterally from the collision site.
            desired *= 1 - projection.Length / ray.Length; // scale it based on the distance between the position and collision site.
          }
        }

      }
      //Seek the average position of our neighbors.
      return desired;
    }
  }
}