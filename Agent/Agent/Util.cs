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
      public static double dotProduct(Vector3d a, Vector3d b)
      {
        return a.Length * b.Length * Math.Cos(Vector3d.VectorAngle(a, b));
      }
    }
    class Random
    {
      private static readonly System.Random random = new System.Random();
      private static readonly object syncLock = new object();
      public static double RandomDouble(double min, double max)
      {
        lock (syncLock)
        { // synchronize
          return random.NextDouble() * (max - min) + min;
        }
      }

      public static int RandomInt(int min, int max)
      {
        lock (syncLock)
        { // synchronize
          return random.Next() * (max - min) + min;
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
