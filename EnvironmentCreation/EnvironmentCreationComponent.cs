using EnvironmentCreation.Properties;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.Versioning;

namespace EnvironmentCreation
{
    public class EnvironmentCreationComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public EnvironmentCreationComponent()
          : base("PathCreation", "PC",
            "自动创建路径",
            "MyComponent", "Plane")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("StartPoint", "S", "", GH_ParamAccess.item);
            pManager.AddPointParameter("EndPoint", "E", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("Obstacle", "O", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Resolution", "R", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Orthogonal", "Orth","", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("OutputPath", "OP", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("PathGrid", "PG", "", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d startPt = new Point3d(0, 0, 0);
            Point3d endPt = new Point3d(0, 1, 0);
            List<Curve> obstacle = new List<Curve>();
            int resolution = 0;
            bool ortho = true;
            DA.GetData(0,ref startPt);
            DA.GetData(1, ref endPt);
            DA.GetDataList(2, obstacle);
            DA.GetData(3, ref resolution);
            DA.GetData(4, ref ortho);

            Polyline path = null;
            List<Curve> testGrid = new List<Curve>();
            if (obstacle.Count != 0)
            {
                PathSolver pathS = new PathSolver(startPt, endPt, obstacle, resolution, ortho);
                path = pathS.PathRhinoSolver();
                GridCreation gctest = new GridCreation(startPt, endPt, resolution, obstacle);
                for (int x = 0; x < gctest.Grids.GetLength(0); x++)
                {
                    for (int y = 0; y < gctest.Grids.GetLength(1); y++)
                        testGrid.Add(gctest.Grids[x, y].ToNurbsCurve());
                }
            }
            if (path != null)
            {
                DA.SetData(0, path);
                DA.SetDataList(1, testGrid);
            }
            //DA.SetDataList(0,testGrid);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.Path2;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("5e559695-1425-4684-9cc5-096991fb14bb");
    }
}