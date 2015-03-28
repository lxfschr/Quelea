using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Agent
{
  public abstract class AbstractParticleFactory
  {
    public abstract IParticle MakeParticle(IParticle settings, Point3d emittionPt, Point3d refEmittionPt);
  }
}
