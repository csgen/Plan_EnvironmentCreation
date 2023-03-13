using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentCreation
{
    internal class MCurve
    {
        public List<Curve> curves { get; set; }

        public MCurve(List<Curve> curves)
        {
            this.curves = curves;
        }
        public Curve MultiIntersection()
        {
            Curve finalCrv = this.curves[0];
            for (int i = 1; i < this.curves.Count; i++)
            {
                //curves[i] = crv[i];
                Curve[] curves = Curve.CreateBooleanIntersection(finalCrv, this.curves[i], 0.001);
                if (curves.Length < 2)
                {
                    finalCrv = curves[0];
                }
            }
            return finalCrv;
        }
    }
}
