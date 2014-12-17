using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{

  public class SpatialCollectionAsList<T> : ISpatialCollection<T>
  {
    private IList<T> spatialObjects;

    public SpatialCollectionAsList() {
      this.spatialObjects = new List<T>();
    }

    public ISpatialCollection<T> getNeighborsInSphere(T item, double r)
    {
      ISpatialCollection<T> neighbors = new SpatialCollectionAsList<T>();
      IPosition position = (IPosition)item;
      foreach (T other in this.spatialObjects) {
        IPosition otherPosition = (IPosition)other;
        double d = position.getPoint3d().DistanceTo(otherPosition.getPoint3d());
        if (d < r && !Object.ReferenceEquals(item, other))
        {
          neighbors.Add(other);
        }
      }

      return neighbors;
    }

    public void Add(T item)
    {
      this.spatialObjects.Add(item);
    }

    public void Clear()
    {
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
  }
}
