using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{
  public interface ISpatialCollection<T> : ICollection<T>
  {
    ISpatialCollection<T> getNeighborsInSphere(T item, double r);
    IEnumerable<T> SpatialObjects { get; }

    void updateDatastructure(Point3d min, Point3d max, int minNodeSize, IList<T> spatialObjects);
  }
}
