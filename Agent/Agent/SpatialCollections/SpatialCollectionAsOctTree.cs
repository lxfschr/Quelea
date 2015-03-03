//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Rhino.Geometry;

//namespace Agent
//{

//  public class SpatialCollectionAsOctTree<T> : ISpatialCollection<T> where T : IAgent
//  {
//    private IList<T> spatialObjects;
//    private PointOctree<T> octTree;

//    public SpatialCollectionAsOctTree()
//    {
//      this.spatialObjects = new List<T>();
//      this.octTree = new PointOctree<T>(100, new Vector3(0, 0, 0), 1); // DK: these values matter!
//    }

//    public SpatialCollectionAsOctTree(float initialWorldSize, Vector3 initialWorldPos, float minNodeSize)
//    {
//      this.spatialObjects = new List<T>();
//      this.octTree = new PointOctree<T>(initialWorldSize, initialWorldPos, minNodeSize); // DK: these values matter!
//    }

//    public SpatialCollectionAsOctTree(Point3d min, Point3d max, int minNodeSize)
//    {
//      this.spatialObjects = new List<T>();
//      UpdateDatastructure(min, max, minNodeSize, new List<T>());
//    }

//    public SpatialCollectionAsOctTree(SpatialCollectionAsOctTree<T> collection)
//    {
//      this.spatialObjects = collection.spatialObjects;
//      this.octTree = collection.octTree;
//    }

//    public SpatialCollectionAsOctTree(ISpatialCollection<T> spatialCollection)
//    {
//      // TODO: Complete member initialization
//      this.spatialObjects = ((SpatialCollectionAsOctTree<T>)spatialCollection).spatialObjects;
//      this.octTree = ((SpatialCollectionAsOctTree<T>)spatialCollection).octTree;
//    }

//    public ISpatialCollection<T> GetNeighborsInSphere(T item, double r)
//    {
//      // ISpatialCollection<T> neighbors = new SpatialCollectionAsOctTree<T>();
//      IPosition position = (IPosition)item;
//      Point3d p3D = position.GetPoint3D();
//      Vector3 positionVec = new Vector3((float)p3D.X, (float)p3D.Y, (float)p3D.Z);
//      T[] neighborsArray = this.octTree.GetNeighborsInSphere(item, positionVec, (float)r); // DK: added "item" parameter
//      return new SpatialCollectionAsList<T>(neighborsArray);
//    }

//    public void Add(T item)
//    {
//      this.spatialObjects.Add(item);
//      Point3d p = ((IPosition)item).GetPoint3D();
//      this.octTree.Add(item, new Vector3((float)p.X, (float)p.Y, (float)p.Z)); // DK: added
//    }

//    public void Clear()
//    {
//      foreach (T item in this.spatialObjects) {
//        this.octTree.Remove(item);
//      }
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
//      get { return this.spatialObjects.IsReadOnly; }
//    }

//    public bool Remove(T item)
//    {
//      return this.spatialObjects.Remove(item) && this.octTree.Remove(item);
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
//      float initialWorldSize = (float)min.DistanceTo(max);
//      BoundingBox box = new BoundingBox(min, max);
//      float x = (float)box.Center.X;
//      float y = (float)box.Center.Y;
//      float z = (float)box.Center.Z;
//      Vector3 initialWorldPos = new Vector3(x, y, z);
//      this.octTree = new PointOctree<T>(initialWorldSize, initialWorldPos, minNodeSize); // DK: these values matter!
//    }
//  }
//}
