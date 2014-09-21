using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Agent
{
    public class PointEmitterType : GH_Goo<Point3d>
    {

        private Point3d pt;
        bool continuousFlow;
        int creationRate;
        int numAgents;
        
        // Default Constructor. Defaults to continuous flow, creating a new Agent every timestep.
        public PointEmitterType()
        {
            this.pt = Point3d.Origin;
            this.continuousFlow = true;
            this.creationRate = 1;
            this.numAgents = 10;
        }

        // Constructor with initial values.
        public PointEmitterType(Point3d pt, bool continuousFlow, int creationRate, int numAgents)
        {
            this.pt = pt;
            this.continuousFlow = continuousFlow;
            this.creationRate = creationRate;
            this.numAgents = numAgents;
        }

        // Copy Constructor
        public PointEmitterType(PointEmitterType ptEmitType)
        {
            this.pt = ptEmitType.pt;
            this.continuousFlow = ptEmitType.continuousFlow;
            this.creationRate = ptEmitType.creationRate;
            this.numAgents = ptEmitType.numAgents;
        }

        public override IGH_Goo Duplicate()
        {
            return new PointEmitterType(this);
        }

        public override bool IsValid
        {
            get
            {
                return (this.pt.IsValid && this.creationRate > 0 && this.numAgents >= 0);
            }

        }

        public override string ToString()
        {
            string origin = "Origin Point: " + pt.ToString() + "\n";
            string continuousFlow = "ContinuousFlow: " + this.continuousFlow.ToString() + "\n";
            string creationRate = "Creation Rate: " + this.creationRate.ToString() + "\n";
            string numAgents = "Number of Agents: " + this.numAgents.ToString() + "\n";
            return origin + continuousFlow + creationRate + numAgents;
        }

        public override string TypeDescription
        {
            get { return "A Point Emitter"; }
        }

        public override string TypeName
        {
            get { return "PointEmitterType"; }
        }

    }
}
