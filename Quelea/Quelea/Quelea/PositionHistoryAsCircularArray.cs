using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Quelea.Quelea
{
  class PositionHistoryAsCircularArray : IPositionHistory
  {
    private CircularArray<Point3d> positionHistory;
    private List<int> wrapIndices;
    private int size;

    public PositionHistoryAsCircularArray(int size)
    {
      this.size = size;
      positionHistory = new CircularArray<Point3d>(this.size);
      wrapIndices = new List<int>(this.size);
    }
    public int Count { get { return positionHistory.Count; } }

    public void Add(Point3d position)
    {
      positionHistory.Add(position);

    }
    public void Add(Point3d position, bool wrapped)
    {
      if (wrapped)
      {
        wrapIndices.Add(Count);
      }
      Add(position);
    }

    public Point3d Get(int i)
    {
      return positionHistory.Get(i);
    }

    public Point3d[] ToArray()
    {
      return positionHistory.ToArray();
    }

    public List<Point3d> ToList()
    {
      return positionHistory.ToList();
    }

    public DataTree<Point3d> ToTree()
    {
      throw new NotImplementedException();
    }

    public List<IGH_Goo> ToGooList()
    {
      throw new NotImplementedException();
    }

    public GH_Structure<IGH_Goo> ToStructure()
    {
      throw new NotImplementedException();
    }
  }
}
