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
  }

  class Program
  {
    private static int NUM_AGENTS = 4000;
    private static Point3d min = new Point3d(-50, -50, -50);
    private static Point3d max = new Point3d(50, 50, 50);
    private static double visionRadius = 5;
    static void Main(string[] args)
    {
      //ISpatialCollection<AgentType> testingAgents = new SpatialCollectionAsList<AgentType>();
      ISpatialCollection<AgentType> testingAgents = new SpatialCollectionAsBinLattice<AgentType>(min, max, (int) visionRadius);
      testSpatialCollection(testingAgents);
      Console.WriteLine("Done!");
      Console.ReadLine();
    }

    public static void testSpatialCollection(ISpatialCollection<AgentType> testingAgents)
    {
      Console.WriteLine("Running test for " + testingAgents.GetType().Name);
      ISpatialCollection<AgentType> baseAgents = new SpatialCollectionAsList<AgentType>();
      List<AgentType> agents = new List<AgentType>();
      Console.WriteLine("Creating Agents.");
      for (int i = 0; i < NUM_AGENTS; i++) agents.Add(new AgentType(Random.RandomPoint(-50, 50)));
      // DK: For testing/debugging, was using just these 2 points:
      // agents.Add(new AgentType(new Point3d(1, 1, 1)));
      // agents.Add(new AgentType(new Point3d(1, 1, 1.01)));
      foreach (AgentType agent in agents)
      {
          testingAgents.Add(agent);
          baseAgents.Add(agent);
      }
      testingAgents.Add(new AgentType(new Point3d(min.X - 100, 0, 0)));
      baseAgents.Add(new AgentType(new Point3d(min.X - 100, 0, 0)));
      if (true) // DK: added so we can easily turn on and off this expensive check
      {
          Console.WriteLine("Checking neighbors match.");
          foreach (AgentType agent in agents)
          {
              ISpatialCollection<AgentType> testingNeighbors = testingAgents.getNeighborsInSphere(agent, 5);
              ISpatialCollection<AgentType> baseNeighbors = baseAgents.getNeighborsInSphere(agent, 5);
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
      Console.WriteLine("Getting timing data.");
      Stopwatch stopwatchBase = new Stopwatch();
      stopwatchBase.Start();
      foreach (AgentType agent in agents)
      {
        ISpatialCollection<AgentType> neighbors = baseAgents.getNeighborsInSphere(agent, 5);
      }
      stopwatchBase.Stop();
      Console.WriteLine("Base time elapsed: {0}", stopwatchBase.Elapsed);

      Stopwatch stopwatchTesting = new Stopwatch();
      stopwatchTesting.Start();
      foreach (AgentType agent in agents)
      {
        ISpatialCollection<AgentType> neighbors = testingAgents.getNeighborsInSphere(agent, 5);
      }
      stopwatchTesting.Stop();
      Console.WriteLine("Testing time elapsed: {0}", stopwatchTesting.Elapsed);

      Console.WriteLine("Elapsed time ratio: {0}", 1.0 * stopwatchTesting.ElapsedMilliseconds / stopwatchBase.ElapsedMilliseconds);
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
