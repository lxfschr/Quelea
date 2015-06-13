using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Quelea
{
  public class PositionHistoryAsDataTree : IPositionHistory
  {
    private DataTree<Point3d> tree;
    private int size;
    private int nextPathIndex;

    public PositionHistoryAsDataTree(int size)
    {
      this.tree = new DataTree<Point3d>();
      this.size = size;
      this.nextPathIndex = 0;
    }
    public int Count { get { return tree.DataCount; } }
    public void Add(Point3d position, bool wrapped)
    {
      if (Count >= size)
      {
        tree.Branch(0).RemoveAt(0);
          
        if (tree.Branch(0).Count == 0)
        {
          tree.RemovePath(tree.Path(0));
        }
      }
      if (wrapped)
      {
        nextPathIndex++;
        tree.Add(position, new GH_Path(nextPathIndex));
        
      }
      else
      {
        tree.Add(position, new GH_Path(nextPathIndex));
      }
    }
    public void Add(Point3d position)
    {
      tree.Add(position);
    }

    public Point3d Get(int i)
    {
      throw new NotImplementedException();
    }

    public Point3d[] ToArray()
    {
      throw new NotImplementedException();
    }

    public List<Point3d> ToList()
    {
      return tree.AllData();
    }

    public DataTree<Point3d> ToTree()
    {
      return this.tree;
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
