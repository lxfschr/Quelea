using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  class AgentSystemDescription : DescriptionBase
  {
    public AgentSystemDescription(string name, string nickname, string description, string category, string subcategory)
      : base(name, nickname, description, category, subcategory)
    {
    }
  }
}
