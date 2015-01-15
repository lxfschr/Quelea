using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
      this.cols = this.rows = this.layers = 100 / binSize;

      //Initialize lattice as 3D array of empty LinkedLists
      this.lattice = new LinkedList<T>[cols][][];
      for(int i = 0; i < cols; i ++) {
        this.lattice[i] = new LinkedList<T>[rows][];
        for(int j = 0; j < rows; j++) {
          this.lattice[i][j] = new LinkedList<T>[layers];
          for(int k = 0; k < layers; k++) {
            lattice[i][j][k] = new LinkedList<T>();
          }
        }
      }
    }

    public SpatialCollectionAsBinLattice(Point3d min, Point3d max, int binSize)
    {
      this.spatialObjects = new List<T>();
      this.binSize = binSize;
      this.min = min;
      this.max = max;
      this.cols = (int)(max.X - min.X ) / binSize + 1;
      this.rows = (int)(max.Y - min.Y) / binSize + 1;
      this.layers = (int)(max.Z - min.Z) / binSize + 1;

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
    }

    public SpatialCollectionAsBinLattice(int worldXSize, int worldYSize, int worldZSize, int binSize)
    {
      this.spatialObjects = new List<T>();
      this.binSize = binSize;
      this.cols = worldXSize / binSize;
      this.rows = worldYSize / binSize;
      this.layers = worldZSize / binSize;

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
    }

    public SpatialCollectionAsBinLattice(SpatialCollectionAsBinLattice<T> collection)
    {
      this.spatialObjects = collection.spatialObjects;
      this.lattice = collection.lattice;
    }

    public SpatialCollectionAsBinLattice(ISpatialCollection<T> spatialCollection)
    {
      // TODO: Complete member initialization
      this.spatialObjects = ((SpatialCollectionAsBinLattice<T>)spatialCollection).spatialObjects;
      this.lattice = ((SpatialCollectionAsBinLattice<T>)spatialCollection).lattice;
    }

    public ISpatialCollection<T> getNeighborsInSphere(T item, double r)
    {
      // ISpatialCollection<T> neighbors = new SpatialCollectionAsBinLattice<T>();
      IPosition position = (IPosition)item;
      Point3d p3d = position.getPoint3d();
      Vector3 positionVec = new Vector3((float)p3d.X, (float)p3d.Y, (float)p3d.Z);
      int col = (int)(p3d.X - min.X) / this.binSize;
      int row = (int)(p3d.Y - min.Y) / this.binSize;
      int layer = (int)(p3d.Z - min.Z) / this.binSize;
      LinkedList<T> possibleNeighbors = this.lattice[col][row][layer];
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
          if (p1.DistanceSquared(p2) < r * r)
          {
            neighbors.Add(other);
          }
        }
      }
      return neighbors;
    }

    public void Add(T item)
    {
      this.spatialObjects.Add(item);
      Point3d p = ((IPosition)item).getPoint3d();
      int col = (int) (p.X -min.X) / this.binSize;
      int row = (int) (p.Y - min.Y) / this.binSize;
      int layer = (int) (p.Z - min.Z) / this.binSize;
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
      return this.spatialObjects.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this.spatialObjects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.spatialObjects.GetEnumerator();
    }
  }
}
