using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GH_IO.Types;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;

namespace Quelea
{
  public class PositionHistoryAsGhStructure : IPositionHistory
  {
    private GH_Structure<IGH_Goo> structure;
    private int size;
    private int nextPathIndex;

    public PositionHistoryAsGhStructure(int size)
    {
      this.structure = new GH_Structure<IGH_Goo>();
      this.size = size;
      this.nextPathIndex = 0;
    }
    public int Count { get { return structure.DataCount; } }
    public void Add(Point3d position, bool wrapped)
    {
      if (Count >= size)
      {
        structure.RemoveData(structure.get_FirstItem(false));
        if(structure.get_Branch(0).Count == 0)
        {
          structure.RemovePath(new GH_Path(0));
        }
      }
      IGH_Goo pt = new GH_Point(position);
      if (wrapped)
      {
        
        structure.Append(pt, new GH_Path(nextPathIndex));
        nextPathIndex++;
      }
      else
      {
        structure.Append(pt, new GH_Path(nextPathIndex));
      }
    }
    public void Add(Point3d position)
    {
      IGH_Goo pt = new GH_Point(position);
      structure.Append(pt, new GH_Path(nextPathIndex));
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
      throw new NotImplementedException();
    }

    public List<IGH_Goo> ToGooList()
    {
      return structure.ToList();
    }

    public DataTree<Point3d> ToTree()
    {
      throw new NotImplementedException();
    }

    public GH_Structure<IGH_Goo> ToStructure()
    {
      return this.structure;
    } 
  }
}
