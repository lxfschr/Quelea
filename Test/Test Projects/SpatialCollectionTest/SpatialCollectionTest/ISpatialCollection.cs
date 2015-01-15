using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  public interface ISpatialCollection<T> : ICollection<T>
  {
    ISpatialCollection<T> getNeighborsInSphere(T item, double r);
  }
}
