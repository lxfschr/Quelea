using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Agent
{
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

    internal static Point3d RandomPoint(double min, double max)
    {
      double x = Random.RandomDouble(min, max);
      double y = Random.RandomDouble(min, max);
      double z = Random.RandomDouble(min, max);
      return new Point3d(x, y, z);
    }

    internal static Point3d RandomPoint(Point3d min, Point3d max)
    {
      double x = Random.RandomDouble(min.X, max.X);
      double y = Random.RandomDouble(min.Y, max.Y);
      double z = Random.RandomDouble(min.Z, max.Z);
      return new Point3d(x, y, z);
    }
  }

  class Program
  {
    private static int NUM_AGENTS = 4000;
    private static Point3d min = new Point3d(-50, -50, -50);
    private static Point3d max = new Point3d(50, 50, 50);
    private static double visionRadius = 5;
    private static int binSize = 5;
    private static bool CHECKMATCH = false;
    static void Main(string[] args)
    {
      //ISpatialCollection<AgentType> testingAgents = new SpatialCollectionAsBinLattice3<AgentType>(min, max, (int)binSize);
      //testSpatialCollection(testingAgents);
      testCircularArray();
      Console.WriteLine("Done!");
      Console.ReadLine();
      
    }

    public static void testCircularArray()
    {
      CircularArray<String> cArray = new CircularArray<String>(0);
      cArray.Add("A");
      cArray.Add("B");
      cArray.Add("C");
      cArray.Add("D");
      cArray.Add("E");
      cArray.Add("F");
      cArray.Add("G");
      cArray.Add("H");
      cArray.Add("I");
      Console.WriteLine(cArray.ToString());
    }

    public static void testSpatialCollection(ISpatialCollection<AgentType> testingAgents)
    {
      Console.WriteLine("Running test for " + testingAgents.GetType().Name);
      ISpatialCollection<AgentType> baseAgents = new SpatialCollectionAsList<AgentType>();
      List<AgentType> agents = new List<AgentType>();
      Console.WriteLine("Creating Agents.");
      for (int i = 0; i < NUM_AGENTS; i++) agents.Add(new AgentType(Random.RandomPoint(min, max)));
      // DK: For testing/debugging, was using just these 2 points:
      // agents.Add(new AgentType(new Point3d(1, 1, 1)));
      // agents.Add(new AgentType(new Point3d(1, 1, 1.01)));

      Stopwatch stopwatchBase = new Stopwatch();
      Stopwatch stopwatchTesting = new Stopwatch();
      Console.WriteLine("Getting add time data.");

      stopwatchBase.Start();
      foreach (AgentType agent in agents)
      {
          baseAgents.Add(agent);
      }
      //baseAgents.Add(new AgentType(new Point3d(min.X - 100, 0, 0)));
      stopwatchBase.Stop();

      stopwatchTesting.Start();
      foreach (AgentType agent in agents)
      {
        testingAgents.Add(agent);
      }
      //testingAgents.Add(new AgentType(new Point3d(min.X - 100, 0, 0)));
      stopwatchTesting.Stop();

      TimeSpan baseAddTime = stopwatchBase.Elapsed;
      TimeSpan testAddTime = stopwatchTesting.Elapsed;
      Console.WriteLine("Base time elapsed: {0}", baseAddTime);
      Console.WriteLine("Testing time elapsed: {0}", testAddTime);

      Console.WriteLine("Elapsed time ratio: {0}", 1.0 * stopwatchTesting.ElapsedTicks / stopwatchBase.ElapsedTicks);

      if (CHECKMATCH) // DK: added so we can easily turn on and off this expensive check
      {
          Console.WriteLine("Checking neighbors match.");
          foreach (AgentType agent in agents)
          {
              ISpatialCollection<AgentType> testingNeighbors = testingAgents.getNeighborsInSphere(agent, visionRadius);
              ISpatialCollection<AgentType> baseNeighbors = baseAgents.getNeighborsInSphere(agent, visionRadius);
              foreach (AgentType neighbor in testingNeighbors)
              {
                  if (!listContainsByReferenceEquals(neighbor, baseNeighbors))
                  {
                    Console.WriteLine("Mismatch1! testingNeighbors size: " + testingNeighbors.Count + " baseNeighbors size: " + baseNeighbors.Count);
                      Console.ReadLine();
                      //throw new Exception();
                  }
              }
              foreach (AgentType neighbor in baseNeighbors)
              {
                  if (!listContainsByReferenceEquals(neighbor, testingNeighbors))
                  {
                    Console.WriteLine("Mismatch2! testingNeighbors size: " + testingNeighbors.Count + " baseNeighbors size: " + baseNeighbors.Count);
                      Console.ReadLine();
                      //throw new Exception();
                  }
              }
          }
      }
      Console.WriteLine("Getting getNeighbors timing data.");
      stopwatchBase.Restart();
      foreach (AgentType agent in agents)
      {
        ISpatialCollection<AgentType> neighbors = baseAgents.getNeighborsInSphere(agent, visionRadius);
      }
      stopwatchBase.Stop();
      TimeSpan baseNeighborsTime = stopwatchBase.Elapsed;
      Console.WriteLine("Base time elapsed: {0}", baseNeighborsTime);

      
      stopwatchTesting.Restart();
      foreach (AgentType agent in agents)
      {
        ISpatialCollection<AgentType> neighbors = testingAgents.getNeighborsInSphere(agent, visionRadius);
      }
      stopwatchTesting.Stop();
      TimeSpan testNeighborsTime = stopwatchTesting.Elapsed;
      Console.WriteLine("Testing time elapsed: {0}", testNeighborsTime);

      Console.WriteLine("Elapsed time ratio: {0}", 1.0 * stopwatchTesting.ElapsedMilliseconds / stopwatchBase.ElapsedMilliseconds);

      TimeSpan totalBaseTime = baseAddTime.Add(baseNeighborsTime);
      Console.WriteLine("Total base time: {0}", totalBaseTime);
      TimeSpan totalTestTime = testAddTime.Add(testNeighborsTime);
      Console.WriteLine("Total test time: {0}", totalTestTime);
      Console.WriteLine("Total elapsed time ratio: {0}", 1.0 * totalTestTime.TotalMilliseconds / totalBaseTime.TotalMilliseconds);
    }

    private static bool listContainsByReferenceEquals(AgentType agent, ISpatialCollection<AgentType> neighbors)
    {
      foreach(AgentType neighbor in neighbors) {
          if(Object.ReferenceEquals(agent, neighbor)) {
            return true;
          }
      }
      return false;
      
    }
  }
}
