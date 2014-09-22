using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
    class ParticleSystem
    {
        public List<Particle> particles;
        public EmitterType emitter;

        public ParticleSystem()
        {
            particles = new List<Particle>();
        }

        public void applyForce(Vector3d f)
        {
            foreach (Particle p in particles)
            {
                p.applyForce(f);
            }
        }

        public void addParticle()
        {
            Vector3d emittionPt = emitter.emit();
            particles.Add(new Particle(emittionPt));
        }

        public void run()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Particle p = particles[i] as Particle;
                p.run();
                if (p.isDead())
                {
                    particles.Remove(p);
                }
            }
        }
    }

}
