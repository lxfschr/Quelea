using System.Drawing;
using Grasshopper.Kernel;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class LoadImage : AbstractComponent
  {
    private string filepath, previousFilepath;
    private Bitmap bitmap;
    private bool reload;
    public LoadImage()
      : base("Load Image", "Image", "Loads a bitmap image from a filepath.", RS.pluginCategoryName, RS.utilitySubcategoryName, null, "352571b8-3f04-47cf-bdb5-729a9da1145b")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddTextParameter("Filepath", "F", "The full path to the image file you want to load.",
        GH_ParamAccess.item);
      pManager.AddBooleanParameter("Reload?", "R",
        "If true, reloads the bitmap from the source file. Use a boolean button and set it to true when you have modified the original file. Otherwise, this component only reloads the file when the filepath has changed.",
        GH_ParamAccess.item, false);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Bitmap Image", "I", "A bitmap image of the file.", GH_ParamAccess.item);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      if (!da.GetData(nextInputIndex++, ref filepath)) return false;
      if (!da.GetData(nextInputIndex++, ref reload)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      if (!filepath.Equals(previousFilepath) || reload)
      {
        bitmap = new Bitmap(filepath);
      }
      previousFilepath = filepath;
      da.SetData(nextOutputIndex++, bitmap);
    }
  }
}
