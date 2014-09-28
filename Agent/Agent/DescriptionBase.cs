using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;

namespace Agent
{
  public abstract class DescriptionBase : IGH_InstanceDescription
  {
    private string name, nickname, category, subcategory, description;
    private Guid guid;
    public DescriptionBase(string name, string nickname, string description, string category, string subcategory)
    {
      this.name = name;
      this.nickname = nickname;
      this.description = description;
      this.category = category;
      this.subcategory = subcategory;
      this.guid = System.Guid.NewGuid();
    }

    public string Category
    {
      get
      {
        return this.category;
      }
      set
      {
        this.category = value;
      }
    }

    public string Description
    {
      get
      {
        return this.description;
      }
      set
      {
        this.description = value;
      }
    }

    public bool HasCategory
    {
      get { return (this.category != null); }
    }

    public bool HasSubCategory
    {
      get { return (this.subcategory != null); }
    }

    public string InstanceDescription
    {
      get { return this.description; }
    }

    public Guid InstanceGuid
    {
      get { return this.guid; }
    }

    public IEnumerable<string> Keywords
    {
      get { return null; }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = value;
      }
    }

    public void NewInstanceGuid(Guid UUID)
    {
      this.guid = UUID;
    }

    public void NewInstanceGuid()
    {
      throw new NotImplementedException();
    }

    public string NickName
    {
      get
      {
        return this.nickname;
      }
      set
      {
        this.nickname = value;
      }
    }

    public string SubCategory
    {
      get
      {
        return this.subcategory;
      }
      set
      {
        this.subcategory = value;
      }
    }

    public bool Read(GH_IO.Serialization.GH_IReader reader)
    {
      throw new NotImplementedException();
    }

    public bool Write(GH_IO.Serialization.GH_IWriter writer)
    {
      throw new NotImplementedException();
    }
  }
}
