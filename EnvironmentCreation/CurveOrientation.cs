using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Render.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentCreation
{
    internal class CurveOrientation
    {
        public Curve curve { get; set; }

        public CurveOrientation(Curve curve)
        {
            this.curve = curve;
        }
        
        public Plane GetOrientPlane()
        {
            Point3d centerPt = AreaMassProperties.Compute(curve).Centroid;
            Vector3d x = Vector3d.XAxis;
            Vector3d y = Vector3d.YAxis;

            Line line1 = new Line(centerPt, Vector3d.XAxis, 1);
            CurveIntersections crvInt1 = Intersection.CurveLine(curve, line1, 0.01, 0.01);
            double length = crvInt1[0].PointB.DistanceTo(crvInt1[1].PointB);
            double n = 30;
            
            for(int i = 1; i < n; i++)
            {
                double tempRotate = i * Math.PI / n;
                Vector3d xtemp = Vector3d.XAxis;
                Vector3d ytemp = Vector3d.YAxis;
                xtemp.Rotate(tempRotate, Vector3d.ZAxis);
                ytemp.Rotate(tempRotate, Vector3d.ZAxis);
                
                Line line = new Line(centerPt,xtemp,1);
                CurveIntersections crvInt = Intersection.CurveLine(curve, line, 0.01, 0.01);
                Point3d startPt = crvInt[0].PointB;
                Point3d endPt = crvInt[1].PointB;
                if (startPt.DistanceTo(endPt) <= length)
                {
                    length = startPt.DistanceTo(endPt);
                    x = xtemp;
                    y = ytemp;
                }
            }
            Plane orientPlane = new Plane(Point3d.Origin, x, y);
            return orientPlane;
        }
    }
}
