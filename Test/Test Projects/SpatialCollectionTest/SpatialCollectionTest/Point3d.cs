using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent
{
  public class Point3d
  {
    private double x, y, z;

    public Point3d()
    {
      this.x = this.y = this.z = 0;
    }

    public Point3d(double x, double y, double z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public Point3d(Point3d pt)
    {
      this.x = pt.x;
      this.y = pt.y;
      this.z = pt.z;
    }

    public double X
    {
      get
      {
        return this.x;
      }
    }

    public double Y
    {
      get
      {
        return this.y;
      }
    }

    public double Z
    {
      get
      {
        return this.z;
      }
    }

    public double DistanceTo(Point3d pt)
    {
      return Math.Sqrt(Math.Pow(this.x + pt.x, 2) + Math.Pow(this.y + pt.y, 2) + Math.Pow(this.z + pt.z, 2));
    }

    public double DistanceSquared(Point3d pt)
    {
      return (Math.Pow(this.X - pt.X, 2) +
                               Math.Pow(this.Y - pt.Y, 2) +
                               Math.Pow(this.Z - pt.Z, 2));
    }

    public static Point3d Origin
    {
      get
      {
        return new Point3d();
      }
    }
  }
}
