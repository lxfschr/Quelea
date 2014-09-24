using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  class EmitterParameter : GH_Param<EmitterType>
  {

    public EmitterParameter()
      : base(new EmitterDescription("Emitter", "Emit", "An Emitter", "Params", "Primitive"), GH_ParamAccess.item) { }

    public override Guid ComponentGuid
    {
      get { throw new NotImplementedException(); }
    }
  }
}
