using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{

  public class SpatialCollectionAsOctTree<T> : ISpatialCollection<T> where T : class
  {
    private IList<T> spatialObjects;
    private PointOctree<T> octTree;

    public SpatialCollectionAsOctTree()
    {
      this.spatialObjects = new List<T>();
      this.octTree = new PointOctree<T>(100, new Vector3(0, 0, 0), 1); // DK: these values matter!
    }

    public SpatialCollectionAsOctTree(float initialWorldSize, Vector3 initialWorldPos, float minNodeSize)
    {
      this.spatialObjects = new List<T>();
      this.octTree = new PointOctree<T>(initialWorldSize, initialWorldPos, minNodeSize); // DK: these values matter!
    }

    public SpatialCollectionAsOctTree(SpatialCollectionAsOctTree<T> collection)
    {
      this.spatialObjects = collection.spatialObjects;
      this.octTree = collection.octTree;
    }

    public SpatialCollectionAsOctTree(ISpatialCollection<T> spatialCollection)
    {
      // TODO: Complete member initialization
      this.spatialObjects = ((SpatialCollectionAsOctTree<T>)spatialCollection).spatialObjects;
      this.octTree = ((SpatialCollectionAsOctTree<T>)spatialCollection).octTree;
    }

    public ISpatialCollection<T> getNeighborsInSphere(T item, double r)
    {
      // ISpatialCollection<T> neighbors = new SpatialCollectionAsOctTree<T>();
      IPosition position = (IPosition)item;
      Point3d p3d = position.getPoint3d();
      Vector3 positionVec = new Vector3((float)p3d.X, (float)p3d.Y, (float)p3d.Z);
      T[] neighborsArray = this.octTree.getNeighborsInSphere(item, positionVec, (float)r); // DK: added "item" parameter
      return new SpatialCollectionAsList<T>(neighborsArray);
    }

    public void Add(T item)
    {
      this.spatialObjects.Add(item);
      Point3d p = ((IPosition)item).getPoint3d();
      this.octTree.Add(item, new Vector3((float)p.X, (float)p.Y, (float)p.Z)); // DK: added
    }

    public void Clear()
    {
      foreach (T item in this.spatialObjects) {
        this.octTree.Remove(item);
      }
      this.spatialObjects.Clear();
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


    public IEnumerable<T> SpatialObjects
    {
      get { return this.spatialObjects; }
    }
  }
}
