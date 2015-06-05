using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SpatialCollectionType : GH_Goo<Object>
  {
    private readonly ISpatialCollection<IQuelea> quelea;
    private List<Point3d> wrappedPositions;

    public SpatialCollectionType()
    {
      quelea = new SpatialCollectionAsBinLattice<IQuelea>();
    }

    public SpatialCollectionType(ISpatialCollection<IQuelea> quelea)
    {
      this.quelea = quelea;
    }

    public SpatialCollectionType(SpatialCollectionType spatialCollection)
    {
      quelea = new SpatialCollectionAsBinLattice<IQuelea>(spatialCollection.quelea);
    }

    public ISpatialCollection<IQuelea> Quelea
    {
      get
      {
        return quelea;
      }
    }

    public List<Point3d> WrappedPositions
    {
      get
      {
        return wrappedPositions;
      }
      set
      {
        wrappedPositions = value;
      }
    }

    public override IGH_Goo Duplicate()
    {
      return new SpatialCollectionType(this);
    }

    public override bool IsValid
    {
      get { return true; }
    }

    public override string ToString()
    {
      return quelea.ToString();
    }

    public override string TypeDescription
    {
      get { return RS.queleaNetworkDescription; }
    }

    public override string TypeName
    {
      get { return RS.queleaNetworkName; }
    }
  }
}
