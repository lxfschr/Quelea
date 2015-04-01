using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Agent
{
  public class AgentInfo : GH_AssemblyInfo
  {
    public override string Name
    {
      get
      {
        return "Agent";
      }
    }
    public override Bitmap Icon
    {
      get
      {
        //Return a 24x24 pixel bitmap to represent this GHA library.
        return Properties.Resources.icon_quelea;
      }
    }
    public override string Description
    {
      get
      {
        //Return a short string describing the purpose of this GHA library.
        return "";
      }
    }
    public override Guid Id
    {
      get
      {
        return new Guid("8e2ef6d7-ac04-4dad-a95d-effdddc8a47b");
      }
    }

    public override string AuthorName
    {
      get
      {
        //Return a string identifying you or your company.
        return "Alex Fischer";
      }
    }
    public override string AuthorContact
    {
      get
      {
        //Return a string representing your preferred contact details.
        return "";
      }
    }
  }
}
