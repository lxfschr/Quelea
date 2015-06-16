using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Quelea
{
  class PositionHistoryAsCircularArray : IPositionHistory
  {
    private WrappingCircularArray<Point3d> positionHistory;
    private List<int> wrapIndices;
    private int size;

    public PositionHistoryAsCircularArray(int size)
    {
      this.size = size;
      positionHistory = new WrappingCircularArray<Point3d>(this.size);
      wrapIndices = new List<int>(this.size);
    }
    public int Count { get { return positionHistory.Count; } }

    public void Add(Point3d position)
    {
      positionHistory.Add(position);
    }
    public void Add(Point3d position, bool wrapped)
    {
      positionHistory.Add(position, wrapped);
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
      return positionHistory.ToTree();
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
