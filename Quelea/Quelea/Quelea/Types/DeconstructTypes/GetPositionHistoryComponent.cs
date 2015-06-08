using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
  public class GetPositionHistoryComponent : AbstractDeconstructTypeComponent
  {
    private List<IQuelea> particles;
    private DataTree<Point3d> outTree;
    /// <summary>
    /// Initializes a new instance of the GetPositionHistoryComponent class.
    /// </summary>
    public GetPositionHistoryComponent()
      : base("Get Particle Position History", "GetPosHist",
             "Gets the position history of anything that inherits from Particle.", RS.icon_getPositionHistory, "76687f32-a550-493f-8262-89dcaf80825c")
    {
      particles = new List<IQuelea>();
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter(RS.particleName + " " + RS.queleaName, RS.particleNickname + RS.queleaNickname, RS.particleDescription, GH_ParamAccess.list);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddPointParameter(RS.positionHistoryName, RS.positionHistoryNickname, RS.positionHistoryDescription, GH_ParamAccess.tree);
      //pManager.AddPointParameter(RS.positionHistoryName, RS.positionHistoryNickname, RS.positionHistoryDescription, GH_ParamAccess.list);
      //pManager.AddPointParameter(RS.positionHistoryName, RS.positionHistoryNickname, RS.positionHistoryDescription, GH_ParamAccess.tree);
    }

    protected override bool GetInputs(IGH_DataAccess da)
    {
      particles = new List<IQuelea>();
      if (!da.GetDataList(nextInputIndex++, particles)) return false;
      return true;
    }

    protected override void SetOutputs(IGH_DataAccess da)
    {
      outTree = new DataTree<Point3d>();
      GH_Path trunk = new GH_Path(0); // {0}
      GH_Path branch = new GH_Path(); // {}\
      GH_Path limb = new GH_Path();

      // then add six branches...
      for (int i = 0; i < particles.Count; i++)
      {
        IQuelea particle = particles[i];
        branch = trunk.AppendElement(i);
        DataTree<Point3d> particlePositionHistoryTree = particle.Position3DHistory.ToTree();
        // ...with 4 items each
        for (int j = 0; j < particlePositionHistoryTree.BranchCount; j++)
        {
          limb = branch.AppendElement(j);
          outTree.AddRange(particlePositionHistoryTree.Branch(j), limb);
          
        }
      }
      da.SetDataTree(nextOutputIndex++, outTree);
    }
  }
}