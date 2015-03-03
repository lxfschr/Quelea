using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{
  public interface ISpatialCollection<T> : ICollection<T> where T : IAgent 
  {
    ISpatialCollection<T> GetNeighborsInSphere(T item, double r);
    IEnumerable<T> SpatialObjects { get; }

    void UpdateDatastructure(Point3d min, Point3d max, int minNodeSize, IList<T> spatialObjects);
  }
}
