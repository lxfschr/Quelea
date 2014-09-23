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
        //public List<EmitterType> emitters;
        public EmitterType[] emitters;
        int timestep;

        public ParticleSystem()
        {
            particles = new List<Particle>();
            //emitters = new List<EmitterType>();
            //emitters = new EmitterType[1];
            timestep = 0;
        }

        public EmitterType[] Emitters
        {
            set
            {
                this.emitters = value;
            }
        }

        public void applyForce(Vector3d f)
        {
            foreach (Particle p in particles)
            {
                p.applyForce(f);
            }
        }

        public void addParticle(EmitterType emitter)
        {
            Vector3d emittionPt = emitter.emit();
            particles.Add(new Particle(emittionPt));
        }

        public void addParticle(Vector3d emittionPt)
        {
            particles.Add(new Particle(emittionPt));
        }

        public void run()
        {
            foreach(EmitterType emitter in emitters)
            {
                if (emitter.ContinuousFlow && (timestep % emitter.CreationRate == 0))
                {
                    if((emitter.NumAgents == 0) || (this.particles.Count < emitter.NumAgents))
                    {
                        addParticle(emitter);
                    }
                }
            }
            
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Particle p = particles[i] as Particle;
                p.run();
                if (p.isDead())
                {
                    particles.Remove(p);
                }
            }
            timestep++;
        }
    }

}
