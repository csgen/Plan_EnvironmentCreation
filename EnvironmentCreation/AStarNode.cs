using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentCreation
{
    internal class AStarNode
    {
        public int x;
        public int y;
        public AStarNode parent;
        public int gScore;
        public int hScore;
        public int fScore
        {
            get
            {
                return gScore + hScore;
            }
        }
        public AStarNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            parent = null;
            gScore = 0;
            hScore = 0;
        }
    }
}
