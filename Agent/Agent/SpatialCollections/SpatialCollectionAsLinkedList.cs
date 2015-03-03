//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Rhino.Geometry;

//namespace Agent
//{

//  public class SpatialCollectionAsLinkedList<T> : ISpatialCollection<T> where T : IAgent
//  {
//    private LinkedList<T> spatialObjects;

//    public SpatialCollectionAsLinkedList() {
//      this.spatialObjects = new LinkedList<T>();
//    }

//    public SpatialCollectionAsLinkedList(SpatialCollectionAsLinkedList<T> collection)
//    {
//      this.spatialObjects = collection.spatialObjects;
//    }

//    public SpatialCollectionAsLinkedList(T[] array)
//    {
//      this.spatialObjects = new LinkedList<T>(array);
//    }

//    public SpatialCollectionAsLinkedList(ISpatialCollection<T> spatialCollection)
//    {
//      // TODO: Complete member initialization
//      this.spatialObjects = ((SpatialCollectionAsLinkedList<T>)spatialCollection).spatialObjects;
//    }
    
//    public ISpatialCollection<T> GetNeighborsInSphere(T item, double r)
//    {
//      ISpatialCollection<T> neighbors = new SpatialCollectionAsLinkedList<T>();
//      IPosition position = (IPosition)item;
//      foreach (T other in this.spatialObjects) {
//        // DK: changed this:
//        // IPosition otherPosition = (IPosition)other;
//        // double d = position.getPoint3d().DistanceTo(otherPosition.getPoint3d());
//        // if (d < r && !Object.ReferenceEquals(item, other))
//        // {
//        //   neighbors.Add(other);
//        // }
//        // to this:
//        if (!Object.ReferenceEquals(item, other)) {
//            Point3d p1 = position.GetPoint3D();
//            Point3d p2 = ((IPosition)other).GetPoint3D();
//            double dSquared = (Math.Pow(p1.X - p2.X, 2) +
//                               Math.Pow(p1.Y - p2.Y, 2) + 
//                               Math.Pow(p1.Z - p2.Z, 2));
//            if (dSquared < r*r)
//            {
//              neighbors.Add(other);
//            }
//        }
//      }

//      return neighbors;
//    }

//    public void Add(T item)
//    {
//      this.spatialObjects.AddLast(item);
//    }

//    public void Clear()
//    {
//      this.spatialObjects.Clear();
//    }

//    public bool Contains(T item)
//    {
//      return this.spatialObjects.Contains(item);
//    }

//    public void CopyTo(T[] array, int arrayIndex)
//    {
//      this.spatialObjects.CopyTo(array, arrayIndex);
//    }

//    public int Count
//    {
//      get { return this.spatialObjects.Count; }
//    }

//    public bool IsReadOnly
//    {
//      get { return false; }
//    }

//    public bool Remove(T item)
//    {
//      return this.spatialObjects.Remove(item);
//    }

//    public IEnumerator<T> GetEnumerator()
//    {
//      return this.spatialObjects.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//      return this.spatialObjects.GetEnumerator();
//    }


//    public IEnumerable<T> SpatialObjects
//    {
//      get { return this.spatialObjects; }
//    }


//    public void UpdateDatastructure(Point3d min, Point3d max, int minNodeSize, IList<T> spatialObjects)
//    {
//      return;
//    }
//  }
//}
