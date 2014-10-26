using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  class AgentSystemParameter : GH_Param<AgentSystemType>
  {
    public AgentSystemParameter()
      : base(new AgentSystemDescription("Agent System", "System", "An Agent System", "Params", "Primitive"), GH_ParamAccess.item) { }

    public override Guid ComponentGuid
    {
      get { return System.Guid.NewGuid(); }
    }
  }
}
