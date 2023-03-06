using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentCreation
{
    internal class AStar
    {
        public static List<AStarNode> FindPath(int[,] map, AStarNode start, AStarNode end)
        {
            List<AStarNode> openSet = new List<AStarNode>();
            openSet.Add(start);

            List<AStarNode> closedSet = new List<AStarNode>();

            while (openSet.Count > 0)
            {
                AStarNode current = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fScore < current.fScore || (openSet[i].fScore == current.fScore && openSet[i].hScore < current.hScore))
                    {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current.x == end.x && current.y == end.y)
                {
                    return ReconstructPath(current);
                }

                foreach (AStarNode neighbor in GetNeighbors(map, current))
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int tentativeGScore = current.gScore + 1;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGScore >= neighbor.gScore)
                    {
                        continue;
                    }

                    neighbor.parent = current;
                    neighbor.gScore = tentativeGScore;
                    neighbor.hScore = CalculateHScore(neighbor, end);
                }
            }

            // No path found
            return null;
        }

        private static List<AStarNode> GetNeighbors(int[,] map, AStarNode node)
        {
            List<AStarNode> neighbors = new List<AStarNode>();

            if (node.x > 0 && map[node.x - 1, node.y] == 0)
            {
                neighbors.Add(new AStarNode(node.x - 1, node.y));
            }

            if (node.x < map.GetLength(0) - 1 && map[node.x + 1, node.y] == 0)
            {
                neighbors.Add(new AStarNode(node.x + 1, node.y));
            }

            if (node.y > 0 && map[node.x, node.y - 1] == 0)
            {
                neighbors.Add(new AStarNode(node.x, node.y - 1));
            }

            if (node.y < map.GetLength(1) - 1 && map[node.x, node.y + 1] == 0)
            {
                neighbors.Add(new AStarNode(node.x, node.y + 1));
            }

            return neighbors;
        }

        private static int CalculateHScore(AStarNode node, AStarNode end)
        {
            return Math.Abs(node.x - end.x) + Math.Abs(node.y - end.y);
        }

        private static List<AStarNode> ReconstructPath(AStarNode endNode)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode current = endNode;
            while (current != null)
            {
                path.Add(current);
                current = current.parent;
            }
            path.Reverse();
            return path;
        }
    }
}
