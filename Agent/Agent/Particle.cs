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
        public double mass = 1;

        public Particle(Vector3d l)
        {
            location = l;
            acceleration = new Vector3d(0, 0, 0);
            //this.acceleration = new Vector3d(0, 0, -0.05);
            //this.velocity = new Vector3d(0, 0, -0.1);
            Random random = new Random();
            double min = -0.5;
            double max = 0.5;
            double x = random.NextDouble() * (max - min) + min;
            double y = random.NextDouble() * (max - min) + min;
            double z = random.NextDouble() * (max - min) + min;
            velocity = new Vector3d(x, y, z);
            this.lifespan = 30.0;
        }


        public void update()
        {
            velocity = Vector3d.Add(velocity, acceleration);
            location = Vector3d.Add(location, velocity);
            acceleration = Vector3d.Multiply(acceleration, 0);
            lifespan -= 1.0;

        }

        public void applyForce(Vector3d force)
        {
            Vector3d f = force;
            f = Vector3d.Divide(f, mass);
            acceleration = Vector3d.Add(acceleration, f);
        }

        public Boolean isDead()
        {
            return (lifespan <= 0.0);
        }

        public void run()
        {
            update();
        }
    }
}
