using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  public abstract class AbstractCommand<T>
  {
    protected IReciever reciever = null;

    protected AbstractCommand(IReciever reciever)
    {
        this.reciever = reciever;
    }

    public abstract int Execute(T args);
  }
}
