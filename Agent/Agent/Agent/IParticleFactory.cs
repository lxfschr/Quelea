using Rhino.Geometry;

namespace Agent
{
  public interface IParticleFactory
  {
    IParticle MakeParticle(IParticle p, Point3d emittionPt, Point3d refEmittionPt);
  }
}
