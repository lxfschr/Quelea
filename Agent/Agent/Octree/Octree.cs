using System;
using System.Collections;
using Tools.Vector;

namespace OctreeSearch
{

    /// <summary> The Octree lets you organize objects in a grid, that redefines
    /// itself and focuses more gridding when more objects appear in a
    /// certain area.
    /// </summary>
    [Serializable]
    public class Octree : IOctree
    {

        protected internal OctreeNode top;

        public Octree()
            : this(1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 20, OctreeNode.NoMinSize)
        {
        }

        public Octree(float xMax, float xMin, float yMax, float yMin, float zMax, float zMin, int maxItems)
            : this(xMax, xMin, yMax, yMin, zMax, zMin, maxItems, OctreeNode.NoMinSize)
        {
        }

        public Octree(int up, int left, int down, int right, int front, int back, int maxItems)
            : this((float)up, (float)left, (float)down, (float)right, (float)front, (float)back, maxItems, OctreeNode.DefaultMinSize)
        {
        }

        public Octree(float xMax, float xMin, float yMax, float yMin, float zMax, float zMin, int maxItems, float minSize)
        {
            top = new OctreeNode(xMax, xMin, yMax, yMin, zMax, zMin, maxItems, minSize);
        }

        #region Add Node

        /// <summary> Add a object into the tree at a location.
        /// </summary>
        /// <param name="x">up-down location in Octree Grid</param>
        /// <param name="y">left-right location in Octree Grid</param>
        /// <param name="z">front-back location in Octree Grid</param>
        /// <returns> true if the insertion worked. </returns>
        public bool AddNode(float x, float y, float z, object obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, int obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, uint obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, short obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, long obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, float obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, double obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(float x, float y, float z, bool obj)
        {
            return top.AddNode(x, y, z, obj);
        }

        public bool AddNode(Vector3F vector, object obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, int obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, uint obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, short obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, long obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, float obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, double obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3F vector, bool obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }


        public bool AddNode(double x, double y, double z, object obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, int obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, uint obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, short obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, long obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, float obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, double obj)
        {
            return top.AddNode(x, y, z, obj);
        }
        public bool AddNode(double x, double y, double z, bool obj)
        {
            return top.AddNode(x, y, z, obj);
        }

        public bool AddNode(Vector3D vector, object obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, int obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, uint obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, short obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, long obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, float obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, double obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }
        public bool AddNode(Vector3D vector, bool obj)
        {
            return top.AddNode(vector.X, vector.Y, vector.Z, obj);
        }

        #endregion

        #region Remove Node
        /// <summary> Remove a object out of the tree at a location. </summary>
        /// <param name="x">up-down location in Octree Grid (x, y)</param>
        /// <param name="y">left-right location in Octree Grid (y, x)</param>
        /// <returns> the object removed, null if the object not found.
        /// </returns>
        public object RemoveNode(float x, float y, float z, object obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, int obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, uint obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, short obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, long obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, float obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, double obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(float x, float y, float z, bool obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }

        public object RemoveNode(Vector3F vector, object obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, int obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, uint obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, short obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, long obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, float obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, double obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3F vector, bool obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }

        public object RemoveNode(double x, double y, double z, object obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, int obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, uint obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, short obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, long obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, float obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, double obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }
        public object RemoveNode(double x, double y, double z, bool obj)
        {
            return top.RemoveNode(x, y, z, obj);
        }

        public object RemoveNode(Vector3D vector, object obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, int obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, uint obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, short obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, long obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, float obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, double obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        public object RemoveNode(Vector3D vector, bool obj)
        {
            return top.RemoveNode(vector.X, vector.Y, vector.Z, obj);
        }
        #endregion

        #region Get Node

        /// <summary> Get an object closest to a x/y. </summary>
        /// <param name="x">up-down location in Octree Grid (x, y)</param>
        /// <param name="y">left-right location in Octree Grid (y, x)</param>
        /// <returns> the object that was found.</returns>
        public object GetNode(float x, float y, float z)
        {
            return top.GetNode(x, y, z);
        }
        public object GetNode(Vector3F vector)
        {
            return top.GetNode(vector.X, vector.Y, vector.Z);
        }
        public object GetNode(double x, double y, double z)
        {
            return top.GetNode(x, y, z);
        }
        public object GetNode(Vector3D vector)
        {
            return top.GetNode(vector.X, vector.Y, vector.Z);
        }

        /// <summary> Get an object closest to a x/y, within a maximum distance.
        /// 
        /// </summary>
        /// <param name="x">up-down location in Octree Grid (x, y)
        /// </param>
        /// <param name="y">left-right location in Octree Grid (y, x)
        /// </param>
        /// <param name="shortestDistance">the maximum distance to get a hit, in
        /// decimal degrees.
        /// </param>
        /// <returns> the object that was found, null if nothing is within
        /// the maximum distance.
        /// </returns>
        public object GetNode(float x, float y, float z, double shortestDistance)
        {
            return top.GetNode(x, y, z, shortestDistance);
        }
        public object GetNode(Vector3F vector, double shortestDistance)
        {
            return top.GetNode(vector.X, vector.Y, vector.Z, shortestDistance);
        }
        public object GetNode(double x, double y, double z, double shortestDistance)
        {
            return top.GetNode(x, y, z, shortestDistance);
        }
        public object GetNode(Vector3D vector, double shortestDistance)
        {
            return top.GetNode(vector.X, vector.Y, vector.Z, shortestDistance);
        }

        /// <summary> Get all the objects within a bounding box.
        /// 
        /// </summary>
        /// <param name="Top">top location in Octree Grid (x, y)
        /// </param>
        /// <param name="Left">left location in Octree Grid (y, x)
        /// </param>
        /// <param name="Bottom">lower location in Octree Grid (x, y)
        /// </param>
        /// <param name="Right">right location in Octree Grid (y, x)
        /// </param>
        /// <returns> ArrayList of objects.
        /// </returns>
        public ArrayList GetNode(float xMax, float xMin, float yMax, float yMin, float zMax, float zMin)
        {
            return GetNode(xMax, xMin, yMax, yMin, zMax, zMin, ArrayList.Synchronized(new ArrayList(100)));
        }
        public ArrayList GetNode(double xMax, double xMin, double yMax, double yMin, double zMax, double zMin)
        {
            return GetNode(xMax, xMin, yMax, yMin, zMax, zMin, ArrayList.Synchronized(new ArrayList()));
        }

        /// <summary> Get all the objects within a bounding box, and return the
        /// objects within a given Vector.
        /// 
        /// </summary>
        /// <param name="Top">top location in Octree Grid (x, y)
        /// </param>
        /// <param name="Left">left location in Octree Grid (y, x)
        /// </param>
        /// <param name="Bottom">lower location in Octree Grid (x, y)
        /// </param>
        /// <param name="Right">right location in Octree Grid (y, x)
        /// </param>
        /// <param name="vector">a vector to add objects to.
        /// </param>
        /// <returns> ArrayList of objects.
        /// </returns>
        public ArrayList GetNode(float xMax, float xMin, float yMax, float yMin, float zMax, float zMin, ArrayList nodes)
        {
            if (nodes == null)
                nodes = ArrayList.Synchronized(new ArrayList(10));
            
            if (xMin > xMax || (Math.Abs(xMin - xMax) < 1e-6))
                return top.GetNode(xMax, xMin, yMax, yMin, zMax, zMin, top.GetNode(xMax, 0, yMax, yMin, zMax, zMin, nodes));
            else
                return top.GetNode(xMax, xMin, yMax, yMin, zMax, zMin, nodes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xMax"></param>
        /// <param name="xMin"></param>
        /// <param name="yMax"></param>
        /// <param name="yMin"></param>
        /// <param name="zMax"></param>
        /// <param name="zMin"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public ArrayList GetNode(double xMax, double xMin, double yMax, double yMin, double zMax, double zMin, ArrayList nodes)
        {
            if (nodes == null)
                nodes = ArrayList.Synchronized(new ArrayList(10));

            if (xMin > xMax || (Math.Abs(xMin - xMax) < 1e-6))
                return top.GetNode(xMax, xMin, yMax, yMin, zMax, zMin, top.GetNode(xMax, 0, yMax, yMin, zMax, zMin, nodes));
            else
                return top.GetNode(xMax, xMin, yMax, yMin, zMax, zMin, nodes);
        }
        
        #endregion

        #region Get Nodes
                /// <summary> Get an object closest to a x/y, within a maximum distance.
        /// 
        /// </summary>
        /// <param name="x">up-down location in Octree Grid (x, y)
        /// </param>
        /// <param name="y">left-right location in Octree Grid (y, x)
        /// </param>
        /// <param name="withinDistance">the maximum distance to get a hit, in
        /// decimal degrees.
        /// </param>
        /// <returns> the objects that were found  within the maximum radius.
        /// </returns>
        public ArrayList GetNodes(float x, float y, float z, double radius)
        {
            return top.GetNodes(x, y, z, radius);
        }
        public ArrayList GetNodes(Vector3F vector, double radius)
        {
            return top.GetNodes(vector.X, vector.Y, vector.Z, radius);
        }
        public ArrayList GetNodes(double x, double y, double z, double radius)
        {
            return top.GetNodes(x, y, z, radius);
        }
        public ArrayList GetNodes(Vector3D vector, double radius)
        {
            return top.GetNodes(vector.X, vector.Y, vector.Z, radius);
        }

        /// <summary> Get an object closest to a x/y, within a maximum distance./// </summary>
        /// <param name="x">up-down location in Octree Grid (x, y)</param>
        /// <param name="y">left-right location in Octree Grid (y, x)</param>
        /// <param name="withinDistance">the maximum distance to get a hit, in
        /// decimal degrees.</param>
        /// <returns> the objects that were found  within the maximum radius.</returns>
        public ArrayList GetNodes(float x, float y, float z, double minRadius, double maxRadius)
        {
            return top.GetNodes(x, y, z, minRadius, maxRadius);
        }
        public ArrayList GetNodes(Vector3F vector, double minRadius, double maxRadius)
        {
            return top.GetNodes(vector.X, vector.Y, vector.Z, minRadius, maxRadius);
        }
        public ArrayList GetNodes(double x, double y, double z, double minRadius, double maxRadius)
        {
            return top.GetNodes(x, y, z, minRadius, maxRadius);
        }
        public ArrayList GetNodes(Vector3D vector, double minRadius, double maxRadius)
        {
            return top.GetNodes(vector.X, vector.Y, vector.Z, minRadius, maxRadius);
        }

        #endregion

        /// <summary>Clear the tree. </summary>
        public void Clear()
        {
            top.Clear();
        }

    }
}