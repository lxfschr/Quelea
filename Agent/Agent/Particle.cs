using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
    class Particle
    {
        public Vector3d location = new Vector3d(0, 0, 0);
        public Vector3d velocity = new Vector3d(0, 0, 0);
        public Vector3d acceleration = new Vector3d(0, 0, 0);
        public double lifespan;

        public Particle(Vector3d l)
        {
            this.location = l;
            //acceleration = new Vector3d(0, 0, 0);
            this.acceleration = new Vector3d(0, 0, -0.05);
            this.velocity = new Vector3d(0, 0, -0.1);
            //Random random = new Random();
            //velocity = new Vector3d(random.Next(-1, 1), random.Next(-2, 0), random.Next(-1, 0));
            this.lifespan = 30.0;
        }

        public void update()
        {
            velocity = Vector3d.Add(velocity, acceleration);
            location = Vector3d.Add(location, velocity);
            lifespan -= 1.0;

        }

        public Boolean isDead()
        {
            if (lifespan <= 0.0) return true;
            else return false;
        }

        public void run()
        {
            update();
            //display();
        }
    }
}
