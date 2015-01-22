using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{

  public class SpatialCollectionAsBinLattice3<T> : ISpatialCollection<T> where T : class
  {
    private IList<T> spatialObjects; //List of all the spatial objects
    private LinkedList<T>[][][] lattice; // Lattice of DoublyLinkedLists for intersection test
    private int cols, rows, layers;
    private int binSize;
    private Point3d min, max;

    public SpatialCollectionAsBinLattice3()
    {
      this.spatialObjects = new List<T>();
      this.binSize = 5;
      this.min = new Point3d(-50, -50, -50);
      this.max = new Point3d(50, 50, 50);
      populateLattice();
    }

    public SpatialCollectionAsBinLattice3(Point3d min, Point3d max, int binSize)
    {
      this.spatialObjects = new List<T>();
      this.binSize = binSize;
      if (min.Equals(max))
      {
        this.min = new Point3d(min.X - 50, min.Y - 50, min.Z - 50);
        this.max = new Point3d(max.X + 50, max.Y + 50, max.Z + 50);
      }
      else
      {
        this.min = min;
        this.max = max;
      }
      populateLattice();
    }

    public SpatialCollectionAsBinLattice3(Point3d min, Point3d max, int binSize, IList<T> items)
    {
      this.spatialObjects = items;
      this.binSize = binSize;
      if (min.Equals(max))
      {
        this.min = new Point3d(min.X - 50, min.Y - 50, min.Z - 50);
        this.max = new Point3d(max.X + 50, max.Y + 50, max.Z + 50);
      }
      else
      {
        this.min = min;
        this.max = max;
      }
      populateLattice();
    }

    private void populateLattice()
    {
      this.cols = (int)(this.max.X - this.min.X) / this.binSize + 1;
      this.rows = (int)(this.max.Y - this.min.Y) / this.binSize + 1;
      this.layers = (int)(this.max.Z - this.min.Z) / this.binSize + 1;

      //Initialize lattice as 3D array of empty LinkedLists
      this.lattice = new LinkedList<T>[cols][][];
      for (int i = 0; i < cols; i++)
      {
        this.lattice[i] = new LinkedList<T>[rows][];
        for (int j = 0; j < rows; j++)
        {
          this.lattice[i][j] = new LinkedList<T>[layers];
          for (int k = 0; k < layers; k++)
          {
            lattice[i][j][k] = new LinkedList<T>();
          }
        }
      }

      foreach (T item in this.spatialObjects)
      {
        this.addToLattice(item);
      }
    }

    public SpatialCollectionAsBinLattice3(SpatialCollectionAsBinLattice3<T> collection)
    {
      this.spatialObjects = collection.spatialObjects;
      this.min = collection.min;
      this.max = collection.max;
      populateLattice();
    }

    public SpatialCollectionAsBinLattice3(ISpatialCollection<T> spatialCollection)
    {
      // TODO: Complete member initialization
      SpatialCollectionAsBinLattice3<T> sC = ((SpatialCollectionAsBinLattice3<T>)spatialCollection);
      this.spatialObjects = sC.spatialObjects;
      this.binSize = sC.binSize;
      this.min = sC.min;
      this.max = sC.max;
      populateLattice();
    }

    public ISpatialCollection<T> getNeighborsInSphere(T item, double r)
    {
      double rSquared = r * r;
      // ISpatialCollection<T> neighbors = new SpatialCollectionAsBinLattice3<T>();
      IPosition position = (IPosition)item;
      //LinkedList<T> possibleNeighbors = getBin(item);
      List<T> possibleNeighbors = getBins(item, r);
      ISpatialCollection<T> neighbors = new SpatialCollectionAsList<T>();

      foreach (T other in possibleNeighbors)
      {
        if (!Object.ReferenceEquals(item, other))
        {
          Point3d p1 = position.getPoint3d();
          Point3d p2 = ((IPosition)other).getPoint3d();
          if (Point.DistanceSquared(p1, p2) < rSquared)
          {
            neighbors.Add(other);
          }
        }
      }
      
      return neighbors;
    }

    private LinkedList<T> getBin(T item)
    {
      Point3d p = ((IPosition)item).getPoint3d();
      int col = (int)(p.X - min.X) / this.binSize;
      int row = (int)(p.Y - min.Y) / this.binSize;
      int layer = (int)(p.Z - min.Z) / this.binSize;
      return this.lattice[col][row][layer];
    }

    private double check(double pn, double bmin, double bmax)
    {
      double result = 0;
      double v = pn;
      if (v < bmin)
      {
        double val = (bmin - v);
        result += val * val;
      }
      if (v > bmax)
      {
        double val = (v - bmax);
        result += val * val;
      }
      return result;
    }

    double SquaredDistPointAABB(Point3d p, Point3d lb, Point3d rt)
    {
      // Squared distance
      double sq = 0.0;
      sq += check(p.X - this.min.X, lb.X, rt.X);
      sq += check(p.Y - this.min.Y, lb.Y, rt.Y);
      sq += check(p.Z - this.min.Z, lb.Z, rt.Z);
      return sq;
    }


    public bool IntersectSphere(Point3d center, double radius, Point3d min, Point3d max)
    {
      // True if the Sphere and AABB intersects
      double squaredDistance = SquaredDistPointAABB(center, min, max);
      return squaredDistance <= (radius * radius);
    }

    private List<T> getBins(T item, double radius)
    {
      List<T> possibleNeighbors = new List<T>();
      Point3d p = ((IPosition)item).getPoint3d();

      double offsetX = p.X - min.X;
      double offsetY = p.Y - min.Y;
      double offsetZ = p.Z - min.Z;
      int firstCol = (int)(offsetX - radius) / this.binSize;
      int firstRow = (int)(offsetY - radius) / this.binSize;
      int firstLayer = (int)(offsetZ - radius) / this.binSize;
      int lastCol = (int)(offsetX + radius) / this.binSize;
      int lastRow = (int)(offsetY + radius) / this.binSize;
      int lastLayer = (int)(offsetZ + radius) / this.binSize;

      if (firstCol < 0) firstCol = 0;
      if (firstRow < 0) firstRow = 0;
      if (firstLayer < 0) firstLayer = 0;
      if (lastCol >= this.cols) lastCol = this.cols-1;
      if (lastRow >= this.rows) lastRow = this.rows-1;
      if (lastLayer >= this.layers) lastLayer = this.layers-1;


      for (int c = firstCol; c <= lastCol; c++)
      {
        for (int r = firstRow; r <= lastRow; r++)
        {
          for (int l = firstLayer; l <= lastLayer; l++)
          {
            Point3d lb = new Point3d(c * this.binSize, r*this.binSize, l*this.binSize);
            Point3d rt = new Point3d((c+1) * this.binSize, (r+1)*this.binSize, (l+1)*this.binSize);
            if (this.IntersectSphere(p, radius, lb, rt))
            {
              possibleNeighbors.AddRange(this.lattice[c][r][l]);
            }
          }
        }
      }

      return possibleNeighbors;
    }

    private bool checkBounds(Point3d p)
    {
      bool beyondMax = false;
      double sizeX, sizeY, sizeZ;
      while (p.X >= max.X)
      {
        sizeX = max.X - min.X;
        max.X += sizeX;
        beyondMax = true;
      }
      while (p.X <= min.X)
      {
        sizeX = max.X - min.X;
        min.X -= sizeX;
        beyondMax = true;
      }
      while (p.Y >= max.Y)
      {
        sizeY = max.Y - min.Y;
        max.Y += sizeY;
        beyondMax = true;
      }
      while (p.Y <= min.Y)
      {
        sizeY = max.Y - min.Y;
        max.Y -= sizeY;
        beyondMax = true;
      }
      while (p.Z >= max.Z)
      {
        sizeZ = max.Z - min.Z;
        max.Z += sizeZ;
        beyondMax = true;
      }
      while (p.Z <= min.Z)
      {
        sizeZ = max.Z - min.Z;
        max.Z -= sizeZ;
        beyondMax = true;
      }

      if (beyondMax)
      {
        this.populateLattice();
      }

      return beyondMax;
    }

    public void Add(T item)
    {
      this.spatialObjects.Add(item);
      addToLattice(item);
    }

    private void addToLattice(T item)
    {
      Point3d p = ((IPosition)item).getPoint3d();

      checkBounds(p);

      int col = (int)(p.X - min.X) / this.binSize;
      int row = (int)(p.Y - min.Y) / this.binSize;
      int layer = (int)(p.Z - min.Z) / this.binSize;
      // It goes in 27 cells, i.e. every Thing is tested against other Things in its cell
      // as well as its 26 neighbors 
      //for (int dCol = -1; dCol <= 1; dCol++)
      //{
      //  for (int dRow = -1; dRow <= 1; dRow++)
      //  {
      //    for (int dLayer = -1; dLayer <= 1; dLayer++)
      //    {
      //      if (col + dCol >= 0 && col + dCol < this.cols &&
      //          row + dRow >= 0 && row + dRow < this.rows &&
      //          layer + dLayer >= 0 && layer + dLayer < this.layers)
      //      {
      //        lattice[col + dCol][row + dRow][layer + dLayer].AddLast(item);
      //      }
      //    }
      //  }
      //}
      lattice[col][row][layer].AddLast(item);
    }

    public void Clear()
    {
      this.spatialObjects.Clear();
      for (int i = 0; i < cols; i++)
      {
        for (int j = 0; j < rows; j++)
        {
          for (int k = 0; k < layers; k++)
          {
            lattice[i][j][k].Clear();
          }
        }
      }
    }

    public bool Contains(T item)
    {
      return this.spatialObjects.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      this.spatialObjects.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return this.spatialObjects.Count; }
    }

    public bool IsReadOnly
    {
      get { return this.spatialObjects.IsReadOnly; }
    }

    public bool Remove(T item)
    {
      LinkedList<T> bin = getBin(item);
      return this.spatialObjects.Remove(item) && bin.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this.spatialObjects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.spatialObjects.GetEnumerator();
    }

    public IEnumerable<T> SpatialObjects
    {
      get { return this.spatialObjects; }
    }

    public void updateDatastructure(Point3d min, Point3d max, int minNodeSize, IList<T> spatialObjects)
    {
      this.spatialObjects = spatialObjects;
      this.binSize = minNodeSize;
      if (min.Equals(max))
      {
        this.min = new Point3d(min.X - 50, min.Y - 50, min.Z - 50);
        this.max = new Point3d(max.X + 50, max.Y + 50, max.Z + 50);
      }
      //else
      //{
      //  this.min = new Point3d(min.X - Math.Abs(max.X - min.X)/2, min.Y - Math.Abs(max.Y - min.Y)/2, min.Z - Math.Abs(max.Z - min.Z)/2);
      //  this.max = new Point3d(max.X + Math.Abs(max.X - min.X)/2, max.Y + Math.Abs(max.Y - min.Y)/2, max.Z + Math.Abs(max.Z - min.Z)/2);
      //}
      this.min = min;
      this.max = max;
      populateLattice();
    }
  }
}
