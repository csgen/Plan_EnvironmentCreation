using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EnvironmentCreation
{
    internal class GridCreation
    {
        public Point3d startPoint { get; set; }
        public Point3d endPoint { get; set; }
        public int resolution { get; set; }
        public Rectangle3d[,] Grids { get; set; }//格子
        public int[,] EnvValue { get; set; }//阵列中的0和1，代表是否能通过
        public Point3d[,] Points { get; set; }//点阵
        public int[] StartNodeId { get; set; }
        public int[] EndNodeId { get; set; }
        public Point3d startLocation { get; set; }
        public Point3d endLocation { get; set; }
        public int worldLength;
        public int worldWidth;
        List<Curve> Obstacles { get; set; }
        public GridCreation(Point3d startPoint, Point3d endPoint, int resolution, List<Curve> Obstacles)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.resolution = resolution;
            this.Obstacles = Obstacles;
            startLocation = startPoint;
            endLocation = endPoint;


            //Rectangle3d rect = new Rectangle3d(Plane.WorldXY, this.pt1, this.pt2);
            Box box = new Box(GetObstacleBox(Obstacles));
            //Box box2 = new Box(Plane.WorldXY, new Point3d[] { this.startPoint, this.endPoint });
            box.Union(this.startPoint);
            box.Union(this.endPoint);
            Plane boxScalePlane = Plane.WorldXY;
            boxScalePlane.Origin = box.Center;
            box.Transform(Transform.Scale(boxScalePlane, 2, 2, 1));
            Interval domainX = box.X;
            Interval domainY = box.Y;
            double length = domainX.Length;
            double width = domainY.Length;
            double distance = this.startPoint.DistanceTo(this.endPoint);
            double DPI = distance / this.resolution;
            worldLength = Math.Max((int)((length - length % DPI) / DPI), 4);
            worldWidth = Math.Max((int)((width - width % DPI) / DPI), 4);
            List<Interval> segX = DivideDomain(domainX, worldLength);
            List<Interval> segY = DivideDomain(domainY, worldWidth);
            worldLength += 6;
            worldWidth += 6;
            Grids = new Rectangle3d[worldLength, worldWidth];
            Points = new Point3d[worldLength, worldWidth];
            EnvValue = new int[worldLength, worldWidth];
            List<int> startList = new List<int>();
            List<int> endList = new List<int>();
            for(int x = 0; x < worldLength; x++)
            {
                for(int y=0;y<worldWidth;y++)
                {
                    Grids[x, y] = new Rectangle3d(Plane.WorldXY, segX[x], segY[y]);
                    Points[x, y] = Grids[x, y].Center;
                    int isStart = (int)Grids[x, y].Contains(this.startPoint);
                    startList.Add(isStart);
                    int isEnd = (int)Grids[x, y].Contains(this.endPoint);
                    endList.Add(isEnd);
                    if (isStart != 2)
                        StartNodeId = new int[] { x, y };
                    //if (isStart==1)
                    //    StartNodeId = new int[] { x, y };
                    if (isEnd != 2)
                        EndNodeId = new int[] { x, y };
                    //if (isEnd==1)
                    //    EndNodeId = new int[] { x, y };
                }
            }
            //this.startLocation = Points[StartNodeId[0], StartNodeId[1]];
            //this.endLocation = Points[EndNodeId[0], EndNodeId[1]];
            AdjustGrids();
            for (int x = 0; x < worldLength; x++)
            {
                for (int y = 0; y < worldWidth; y++)
                {
                    Points[x, y] = Grids[x, y].Center;
                    if (IsObstacle(Grids[x, y], this.Obstacles, DPI / 100))
                        EnvValue[x, y] = 0;
                    else EnvValue[x, y] = 1;
                }
            }

            //return box;
        }
        private void AdjustGrids()
        {
            Vector3d moveGridtoSPt = this.startLocation - this.Grids[this.StartNodeId[0], this.StartNodeId[1]].Center;
            for(int x = 0; x < worldLength; x++)
            {
                for(int y = 0; y < worldWidth; y++)
                {
                    Grids[x, y].Transform(Transform.Translation(moveGridtoSPt));
                }
            }
            Point3d endGridCenter = this.Grids[this.EndNodeId[0], this.EndNodeId[1]].Center;
            double scaleX;
            double scaleY;
            if (Math.Abs((this.startLocation.X - endGridCenter.X)) < 0.0001)
            {
                scaleX = 1;
            }
            else
            {
                scaleX = Math.Abs(this.startLocation.X - this.endLocation.X) / Math.Abs(this.startLocation.X - endGridCenter.X);
            }
            if (Math.Abs((this.startLocation.Y - endGridCenter.Y)) < 0.0001)
            {
                scaleY = 1;
            }
            else
            {
                scaleY = Math.Abs(this.startLocation.Y - this.endLocation.Y) / Math.Abs(this.startLocation.Y - endGridCenter.Y);
            }
            Plane startPtPlane = Plane.WorldXY;
            startPtPlane.Origin = startLocation;
            for(int x = 0; x < worldLength; x++)
            {
                for(int y = 0; y < worldWidth; y++)
                {
                    Grids[x, y].Transform(Transform.Scale(startPtPlane, scaleX, scaleY, 1));
                }
            }
        }
        private static bool IsObstacle(Rectangle3d grid,List<Curve> obstacles,double tolerence)
        {
            bool isObstacle = false;
            if(obstacles.Count==0) isObstacle = false;
            Curve mycurve = grid.ToNurbsCurve();
            
            foreach(Curve obstacle in obstacles)
            {
                CurveIntersections intersect = Intersection.CurveCurve(mycurve, obstacle, tolerence, 0);
                if (intersect.Count > 0) isObstacle = true;
                if ((int)obstacle.Contains(grid.Center) == 1) isObstacle = true;
            }
            return isObstacle;
        }
        private List<Interval> DivideDomain(Interval domain, int res)//res为分段数量，分段后向前后各延展2个subdomain
        {
            double t0 = domain.T0;
            double t1 = domain.T1;
            double l = (domain.Length / (double)res);
            List<Interval> subdomains = new List<Interval>();
            subdomains.Add(new Interval(t0 - l * 3, t0 - l * 2));
            subdomains.Add(new Interval(t0 - l * 2, t0 - l));
            subdomains.Add(new Interval(t0 - l, t0));
            for (int i = 0; i < res; i++)
            {
                subdomains.Add(new Interval(t0 + i * l, t0 + (i + 1) * l));
            }
            subdomains.Add(new Interval(t1, t1 + l));
            subdomains.Add(new Interval(t1 + l, t1 + l * 2));
            subdomains.Add(new Interval(t1 + l * 2, t1 + l * 3));
            return subdomains;
        }
        private BoundingBox GetObstacleBox(List<Curve> obstacles)
        {
            BoundingBox bBox = BoundingBox.Unset;
            foreach(Curve obstacle in obstacles)
            {
                BoundingBox bbObj = obstacle.GetBoundingBox(true);
                bBox.Union(bbObj);
            }
            return bBox;
        }
        //public Rectangle3d GridPoint()
        //{
        //Rectangle3d rect = new Rectangle3d(Plane.WorldXY, this.pt1, this.pt2);
        //double xD = Math.Abs(pt1.X - pt2.X);
        //double yD = Math.Abs(pt1.Y - pt2.Y);
        //double maxD = Math.Max(xD, yD);
        //double minD = Math.Min(xD, yD);
        //List<Point3d> ptGrid = new List<Point3d>();
        //double x = maxD / this.dpi;

        //for(int i = 0; i < this.dpi; i++)
        //{
        //    for(int j = 0; j < minD / (maxD / this.dpi); j++)
        //    {
        //        if (xD < yD)
        //        {

        //        }
        //    }
        //}

        //return rect;
        //}
    }
}
