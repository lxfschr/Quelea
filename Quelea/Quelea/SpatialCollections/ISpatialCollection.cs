using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Quelea
{
  public interface ISpatialCollection<T> : ICollection<T>
  {
    ISpatialCollection<T> GetNeighborsInSphere(T item, double r);
    IEnumerable<T> SpatialObjects { get; }

    void UpdateDatastructure(Point3d min, Point3d max, int minNodeSize, IList<T> spatialObjects);
  }
}
