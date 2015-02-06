using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace Agent
{
  namespace Util
  {
    public class Agent
    {
      public static Vector3d CalcSum(AgentType agent, IList<AgentType> agents, out int count)
      {
        Vector3d sum = new Vector3d();
        count = 0;
        foreach (AgentType other in agents)
        {
          double d = agent.Position.DistanceTo(other.Position);
          //double d = Vector3d.Subtract(agent.Position, other.Position).Length;
          //if we are not comparing the seeker to iteself and it is at least
          //desired separation away:
          if (d > 0)
          {
            Vector3d diff = Point3d.Subtract(agent.Position, other.Position);
            diff.Unitize();

            //Weight the magnitude by distance to other
            diff = Vector3d.Divide(diff, d);

            sum = Vector3d.Add(sum, diff);

            //For an average, we need to keep track of how many boids
            //are in our vision.
            count++;
          }
        }
        return sum;
      }

      public static Vector3d Seek(AgentType agent, Vector3d target)
      {
        Vector3d desired = Vector3d.Subtract(target, new Vector3d(agent.Position));
        desired.Unitize();
        // The agent desires to move towards the target at maximum speed.
        // Instead of teleporting to the target, the agent will move incrementally.
        desired = Vector3d.Multiply(desired, agent.MaxSpeed);

        //Seek the average position of our neighbors.
        desired /*steer*/ = Vector3d.Subtract(desired, agent.Velocity);
        // Optimumization so we don't need to create a new Vector3d called steer

        // Steering ability can be controlled by limiting the magnitude of the steering force.
        desired = Vector.Limit(desired, agent.MaxForce);
        return desired;
      }
    }
    class Point
    {
      private Point() { }

      public static double DistanceSquared(Point3d p1, Point3d p2)
      {
        return (Math.Pow(p1.X - p2.X, 2) +
                                 Math.Pow(p1.Y - p2.Y, 2) +
                                 Math.Pow(p1.Z - p2.Z, 2));
      }
    }
    class Vector
    {
      private Vector() { }

      public static double DotProduct(Vector3d a, Vector3d b)
      {
        return a.Length * b.Length * Math.Cos(Vector3d.VectorAngle(a, b));
      }

      public static Vector3d Reflect(Vector3d to, Vector3d about)
      {
        return Vector3d.Subtract(to, Vector3d.Multiply(2 * DotProduct(to, about), about));
      }

      public static void GetProjectionComponents(Vector3d of, Vector3d to, out Vector3d parVec, out Vector3d perpVec)
      {
        double scalar = DotProduct(of, to) / (to.Length * to.Length);
        parVec = Vector3d.Multiply(to, scalar);
        perpVec = Vector3d.Subtract(of, parVec);
      }

      public static double LengthSq(Vector3d vec)
      {
        return Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2) + Math.Pow(vec.Z, 2);
      }

      public static Vector3d Limit(Vector3d vec, double max)
      {
        if (LengthSq(vec) > max * max)
        {
          vec.Unitize();
          vec = Vector3d.Multiply(vec, max);
        }
        return vec;
      }
    }

    public static class String
    {
      public static string ToString(string name, Object obj)
      {
        return name + ": " + obj.ToString() + "\n";
      }
    }
    class Random
    {
      /* Private constructor to precent compiler from generating a default 
       * constructor.
       */
      private Random() { }

      private static readonly System.Random random = new System.Random();
      private static readonly object SyncLock = new object();
      public static double RandomDouble(double min, double max)
      {
        lock (SyncLock)
        { // synchronize
          return random.NextDouble() * (max - min) + min;
        }
      }

      internal static Vector3d RandomVector(double min, double max)
      {
        double x = RandomDouble(min, max);
        double y = RandomDouble(min, max);
        double z = RandomDouble(min, max);
        return new Vector3d(x, y, z);
      }
    }
  }
}
