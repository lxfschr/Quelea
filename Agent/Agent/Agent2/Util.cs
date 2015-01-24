using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
namespace Agent.Agent2
{
  namespace Util
  {
    public class Agent
    {
      public Agent() { }

      public static Vector3d calcSum(AgentType agent, IList<AgentType> agents, out int count)
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

      public static Vector3d seek(AgentType agent, Vector3d target)
      {
        Vector3d desired = Vector3d.Subtract(target, new Vector3d(agent.Position));
        desired.Unitize();
        desired = Vector3d.Multiply(desired, agent.MaxSpeed);

        //Seek the average position of our neighbors.
        Vector3d steer = Vector3d.Subtract(desired, agent.Velocity);

        if (steer.Length > agent.MaxForce)
        {
          steer.Unitize();
          steer = Vector3d.Multiply(steer, agent.MaxForce);
        }
        return steer;
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

      public static double dotProduct(Vector3d a, Vector3d b)
      {
        return a.Length * b.Length * Math.Cos(Vector3d.VectorAngle(a, b));
      }

      public static Vector3d reflect(Vector3d to, Vector3d about)
      {
        return Vector3d.Subtract(to, Vector3d.Multiply(2 * dotProduct(to, about), about));
      }

      public static void getProjectionComponents(Vector3d of, Vector3d to, out Vector3d parVec, out Vector3d perpVec)
      {
        double scalar = Util.Vector.dotProduct(of, to) / (to.Length * to.Length);
        parVec = Vector3d.Multiply(to, scalar);
        perpVec = Vector3d.Subtract(of, parVec);
      }

      public static Vector3d limit(Vector3d vec, double max)
      {
        if (vec.Length > max)
        {
          vec.Unitize();
          vec = Vector3d.Multiply(vec, max);
        }
        return vec;
      }
    }
    class Random
    {
      /* Private constructor to precent compiler from generating a default 
       * constructor.
       */
      private Random() { }

      private static readonly System.Random random = new System.Random();
      private static readonly object syncLock = new object();
      public static double RandomDouble(double min, double max)
      {
        lock (syncLock)
        { // synchronize
          return random.NextDouble() * (max - min) + min;
        }
      }

      internal static Vector3d RandomVector(double min, double max)
      {
        double x = Util.Random.RandomDouble(min, max);
        double y = Util.Random.RandomDouble(min, max);
        double z = Util.Random.RandomDouble(min, max);
        return new Vector3d(x, y, z);
      }
    }
  }
}
