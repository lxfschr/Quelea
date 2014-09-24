using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  namespace Util
  {
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
    }
  }
}
