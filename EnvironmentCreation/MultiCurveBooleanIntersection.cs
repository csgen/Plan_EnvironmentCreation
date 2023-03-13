using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace EnvironmentCreation
{
    public class MultiCurveBooleanIntersection : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MultiCurveBooleanIntersection class.
        /// </summary>
        public MultiCurveBooleanIntersection()
          : base("MultiCurveBooleanIntersection", "MCBI",
            "create boolean intersection for multiple curves",
            "MyComponent", "Plane")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "Input curve", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "Output curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> curves = new List<Curve>();
            DA.GetDataList(0, curves);
            MCurve mc = new MCurve(curves);
            DA.SetData(0, mc.MultiIntersection());
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("94DD4F62-2E97-4A4B-A1F2-B1A68197F25E"); }
        }
    }
}