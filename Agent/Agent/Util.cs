using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
namespace Agent
{
  namespace Util
  {
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
