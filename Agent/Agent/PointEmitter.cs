using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Agent
{
    public class PointEmitter : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PointEmitter()
            : base("Point Emitter", "pEmit",
                "Emit agents from a point.",
                "Agent", "Emitters")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
            pManager.AddPointParameter("Point", "P", "Base point for emitter", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddBooleanParameter("Continuous Flow", "C", "If true, Agents will be emitted every Rth timestep.\n If false, N Agents will be emitted once.", GH_ParamAccess.item, true);
            pManager.AddIntegerParameter("Creation Rate", "R", "Rate at which new Agents are created. Every Rth timestep.", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Number of Agents", "N", "The number of Agents", GH_ParamAccess.item, 10);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            //pManager.AddPointParameter("Point Emitter", "E", "Point Emitter", GH_ParamAccess.item);
            pManager.AddGenericParameter("Point Emitter", "E", "Point Emitter", GH_ParamAccess.item);

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            Point3d pt = new Point3d();
            bool continuousFlow = true;
            int creationRate = 1;
            int numAgents = 0;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref pt)) return;
            if (!DA.GetData(1, ref continuousFlow)) return;
            if (!DA.GetData(2, ref creationRate)) return;
            if (!DA.GetData(3, ref numAgents)) return;

            // We should now validate the data and warn the user if invalid data is supplied.
            if (creationRate <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Creation rate must be greater than 0.");
                return;
            }
            if (numAgents < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number of Agents must be greater than or equal to 0.");
                return;
            }

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            PointEmitterType emitterPt = createEmitter(pt, continuousFlow, creationRate, numAgents);

            // Finally assign the spiral to the output parameter.
            DA.SetData(0, emitterPt);
        }

        private PointEmitterType createEmitter(Point3d pt, bool continuousFlow, int creationRate, int numAgents)
        {
            //return pt;
            return new PointEmitterType(pt, continuousFlow, creationRate, numAgents);
            //return new PointEmitterType();
            //return null;
        }

        private Curve CreateSpiral(Plane plane, double r0, double r1, Int32 turns)
        {
            Line l0 = new Line(plane.Origin + r0 * plane.XAxis, plane.Origin + r1 * plane.XAxis);
            Line l1 = new Line(plane.Origin - r0 * plane.XAxis, plane.Origin - r1 * plane.XAxis);

            Point3d[] p0;
            Point3d[] p1;

            l0.ToNurbsCurve().DivideByCount(turns, true, out p0);
            l1.ToNurbsCurve().DivideByCount(turns, true, out p1);

            PolyCurve spiral = new PolyCurve();

            for (int i = 0; i < p0.Length - 1; i++)
            {
                Arc arc0 = new Arc(p0[i], plane.YAxis, p1[i + 1]);
                Arc arc1 = new Arc(p1[i + 1], -plane.YAxis, p0[i + 1]);

                spiral.Append(arc0);
                spiral.Append(arc1);
            }

            return spiral;
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                //return null;
                return Properties.Resources.ptEmitterIcon;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7de9eadc-3864-4bb0-9ee0-98b52466323a}"); }
        }
    }
}
