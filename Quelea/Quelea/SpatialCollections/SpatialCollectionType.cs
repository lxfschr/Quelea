using System;
using Grasshopper.Kernel.Types;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class SpatialCollectionType : GH_Goo<Object>
  {
    private readonly ISpatialCollection<IQuelea> quelea;

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
