using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  public class OctTree
  {
    private OctTreeNode root;
    public static int makeChildNodesDepth = 0;

    public OctTree()
    {
      this.root = new OctTreeNode();
      makeChildNodesDepth = 0;
    }
    public void AddPoint(double x, double y, double z, Object obj)
    {
      this.root.AddPoint(new Point(x, y, z, obj));
    }

    public List<Object> GetNeighborsInRadius(double x, double y, double z, double r) {
        return this.root.GetNeighborsInRadius(x, y, z, r);
    }

    class OctTreeNode
    {
      private List<Point> points;
      private Boolean useChildNodes;
      private OctTreeNode[] childNodes;
      private Point maxPoint, minPoint;
      private double xAxis, yAxis, zAxis;

      public OctTreeNode()
      {
        this.points = new List<Point>();
        this.childNodes = new OctTreeNode[8];
        this.maxPoint = this.minPoint = null;
        this.useChildNodes = false;
      }

      public void FindAxes()
      {
        double xSum = 0;
        double ySum = 0;
        double zSum = 0;
        foreach (Point point in this.points)
        {
          xSum += point.X;
          ySum += point.Y;
          zSum += point.Z;
        }
        int numPoints = this.points.Count();
        double xAvg = xSum / numPoints;
        double yAvg = ySum / numPoints;
        double zAvg = zSum / numPoints;
        this.xAxis = xAvg;
        this.yAxis = yAvg;
        this.zAxis = zAvg;
      }

      public void MakeChildNodes() {
        Assert(makeChildNodesDepth == 0);
        makeChildNodesDepth++;
        this.FindAxes();
        for (int i = 0; i < 8; i++)
        {
          this.childNodes[i] = new OctTreeNode();
        }
        List<Point> points = this.points;
        this.points = null; // not using points anymore. Using subtrees.
        this.useChildNodes = true;
        foreach (Point point in points)
        {
          this.AddPointToChildren(point);
        }
        makeChildNodesDepth--;
        Assert(makeChildNodesDepth == 0);
      }

      public int GetChildIndex(Point point)
      {
        int xDir = (point.X < this.xAxis) ? 1 : 0;
        int yDir = (point.Y < this.yAxis) ? 1 : 0;
        int zDir = (point.Z < this.zAxis) ? 1 : 0;
        return 4 * zDir + 2 * yDir + xDir;
      }

      public void AddPoint(Point point)
      {
        int maxpoints = 1;
        if (this.maxPoint == null)
        {
          this.maxPoint = new Point(point);
          this.minPoint = new Point(point);
        }
        else
        {
          Point maxPoint = this.maxPoint;
          Point minPoint = this.minPoint;
          if (point.X > maxPoint.X) maxPoint.X = point.X;
          if (point.Y > maxPoint.Y) maxPoint.Y = point.Y;
          if (point.Z > maxPoint.Z) maxPoint.Z = point.Z;
          if (point.X < minPoint.X) minPoint.X = point.X;
          if (point.Y < minPoint.Y) minPoint.Y = point.Y;
          if (point.Z < minPoint.Z) minPoint.Z = point.Z;
        }
        if (this.useChildNodes == false)
        {
          this.points.Add(point);
          if (this.points.Count() > maxpoints)
          {
            this.MakeChildNodes();
          }
        }
        else
        {
          this.AddPointToChildren(point);
        }
      }

      public static void Assert(Boolean b)
      {
        if (b == false)
        {
          throw new Exception("Failed assertion");
        }
      }

      public void AddPointToChildren(Point point)
      {
          Assert(this.useChildNodes == true);
          int childIndex = this.GetChildIndex(point);
          this.childNodes[childIndex].AddPoint(point);
      }

      public List<Object> GetNeighborsInRadius(double x, double y, double z, double r)
      {
        List<Object> neighbors = new List<Object>();
        if (this.maxPoint == null)
        {
          return neighbors;
        }
        if (!this.DoesCubeIntersectSphere(this.minPoint, this.maxPoint, x, y, z, r))
        {
          return neighbors;
        }
        if (this.points != null)
        {
          double rSquared = r * r;
          foreach (Point point in this.points)
          {
            double dSquared = (Math.Pow(point.X - x, 2) + Math.Pow(point.Y - y, 2) + Math.Pow(point.Z - z, 2));
            if (dSquared < rSquared)
            {
              neighbors.Add(point.Obj);
            }
          }
        }
        else
        {
          foreach (OctTreeNode childNode in this.childNodes)
          {
            neighbors.AddRange(childNode.GetNeighborsInRadius(x, y, z, r));
          }
        }
        return neighbors;
      }

      private Boolean DoesCubeIntersectSphere(Point minPoint, Point maxPoint, double x, double y, double z, double r)
      {
        double rSquared = r * r;
        if (x < minPoint.X) rSquared -= Math.Pow(x - minPoint.Z, 2);
        else if (x > maxPoint.X) rSquared -= Math.Pow(x - maxPoint.Z,2);
        if (y < minPoint.Y) rSquared -= Math.Pow(y - minPoint.Y,2);
        else if (y > maxPoint.Y) rSquared -= Math.Pow(y - maxPoint.Y,2);
        if (z < minPoint.Z) rSquared -= Math.Pow(z - minPoint.Z,2);
        else if (z > maxPoint.Z) rSquared -= Math.Pow(z - maxPoint.Z,2);
        return rSquared > 0;
      }
    }

    
  }
  class Point
  {
    private double x, y, z;
    private Object obj;
    public Point(Point point)
    {
      this.x = point.x;
      this.y = point.y;
      this.z = point.z;
      this.obj = point.obj;
    }

    public Point(double x, double y, double z, Object obj)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.obj = obj;
    }

    public double X
    {
      get { return this.x; }
      set { this.x = value; }
    }

    public double Y
    {
      get { return this.y; }
      set { this.y = value; }
    }
    public double Z
    {
      get { return this.z; }
      set { this.z = value; }
    }

    public Object Obj
    {
      get { return this.obj; }
      set { this.obj = value; }
    }

    public static void Main()
    {
      OctTree octTree = new OctTree();
      Console.WriteLine("Hello World!");
    }
  }
  
}

