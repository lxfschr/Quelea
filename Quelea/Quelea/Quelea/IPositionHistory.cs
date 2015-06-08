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
  public interface IPositionHistory
  {
    int Count { get; }
    void Add(Point3d position, bool wrapped);
    Point3d Get(int i);
    Point3d[] ToArray();
    List<Point3d> ToList();
    DataTree<Point3d> ToTree();
    List<IGH_Goo> ToGooList();
    GH_Structure<IGH_Goo> ToStructure();
  }
}
