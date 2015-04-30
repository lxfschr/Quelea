using System.Drawing;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public abstract class AbstractTypeComponent : AbstractComponent
  {
    protected AbstractTypeComponent(string name, string nickname, string description, 
                                             Bitmap icon, string componentGuid)
      : base(name, nickname, description, RS.pluginCategoryName, RS.pluginSubcategoryName, icon, componentGuid)
    {
    }
  }
}
