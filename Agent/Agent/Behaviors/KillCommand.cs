using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  public class KillCommand : AbstractCommand
  {
    public KillCommand(IReciever reciever)
      : base(reciever)
    {
    }

    public override int Execute()
    {
      reciever.SetAction(ACTIO_LIST.SUBTRACT);
      return reciever.GetResult();
    }

    
  }
}
