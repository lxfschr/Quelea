using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{

  public class SpatialCollectionAsBinLattice<T> : ISpatialCollection<T> where T : class
  {
    private IList<T> spatialObjects; //List of all the spatial objects
    private LinkedList<T>[][][] lattice; // Lattice of DoublyLinkedLists for intersection test
    private int cols, rows, layers;
    private int binSize;
    private Point3d min, max;

    public SpatialCollectionAsBinLattice()
    {
      this.spatialObjects = new List<T>();
      this.binSize = 5;
      this.min = new Point3d(-50,-50,-50);
      this.max = new Point3d(50, 50, 50); ;
      populateLattice();
    }

    public SpatialCollectionAsBinLattice(Point3d min, Point3d max, int binSize)
    {
      this.spatialObjects = new List<T>();
      this.binSize = binSize;
      this.min = min;
      this.max = max;
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

    public SpatialCollectionAsBinLattice(SpatialCollectionAsBinLattice<T> collection)
    {
      this.spatialObjects = collection.spatialObjects;
      this.min = collection.min;
      this.max = collection.max;
      populateLattice();
    }

    public SpatialCollectionAsBinLattice(ISpatialCollection<T> spatialCollection)
    {
      // TODO: Complete member initialization
      SpatialCollectionAsBinLattice<T> sC = ((SpatialCollectionAsBinLattice<T>)spatialCollection);
      this.spatialObjects = sC.spatialObjects;
      this.binSize = sC.binSize;
      this.min = sC.min;
      this.max = sC.max;
      populateLattice();
    }

    public ISpatialCollection<T> getNeighborsInSphere(T item, double r)
    {
      // ISpatialCollection<T> neighbors = new SpatialCollectionAsBinLattice<T>();
      IPosition position = (IPosition)item;
      LinkedList<T> possibleNeighbors = getBin(item);
      ISpatialCollection<T> neighbors = new SpatialCollectionAsList<T>();
      foreach (T other in possibleNeighbors)
      {
        // DK: changed this:
        // IPosition otherPosition = (IPosition)other;
        // double d = position.getPoint3d().DistanceTo(otherPosition.getPoint3d());
        // if (d < r && !Object.ReferenceEquals(item, other))
        // {
        //   neighbors.Add(other);
        // }
        // to this:
        if (!Object.ReferenceEquals(item, other))
        {
          Point3d p1 = position.getPoint3d();
          Point3d p2 = ((IPosition)other).getPoint3d();
          if (Util.Point.DistanceSquared(p1,p2) < r * r)
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
        min.Y -= sizeY;
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
        min.Z -= sizeZ;
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
      for (int dCol = -1; dCol <= 1; dCol++)
      {
        for (int dRow = -1; dRow <= 1; dRow++)
        {
          for (int dLayer = -1; dLayer <= 1; dLayer++)
          {
            if (col + dCol >= 0 && col + dCol < this.cols &&
                row + dRow >= 0 && row + dRow < this.rows &&
                layer + dLayer >= 0 && layer + dLayer < this.layers)
            {
              lattice[col + dCol][row + dRow][layer + dLayer].AddLast(item);
            }
          }
        }
      }
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
  }
}
