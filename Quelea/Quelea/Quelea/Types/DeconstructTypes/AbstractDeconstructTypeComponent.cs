using System.Drawing;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractDeconstructTypeComponent : AbstractTypeComponent
  {
    protected AbstractDeconstructTypeComponent(string name, string nickname, string description, 
                                             Bitmap icon, string componentGuid)
      : base(name, nickname, description, icon, componentGuid)
    {
    }
    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.tertiary; }
    }
  }
}
