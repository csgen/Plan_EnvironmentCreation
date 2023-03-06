using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DebugTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Random rd = new Random();
            byte[,] mapRef = new byte[10, 10]
            {
                {1,1,1,1,1,1,1,1,1,0},
                {1,1,0,1,1,1,1,1,1,1},
                {1,1,0,0,1,1,1,1,1,0},
                {1,1,0,0,1,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,0},
            };
            //byte[,] mapRef2 = new byte[10, 10];

            //for(int i = 0; i < 10; i++)
            //{
            //    for(int j = 0; j < 10; j++)
            //    {
            //        mapRef2[i, j] = (byte)rd.Next(0, 2);
            //    }
            //}
            //mapRef2[1, 1] = 1;
            //mapRef2[9, 9] = 1;

            PathFind pf = new PathFind();
            List<Fgh> list = pf.AStarSearch(mapRef, 1, 10, 10, new int[] { 8, 6 }, new int[] { 1, 1 }, true);
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].x + " " + list[i].y);
            }
            //for (int i = 0; i < 100; i++)
            //{
            //    for (int j = 0; j < 100; j++)
            //    {
            //        Console.Write(mapRef2[i, j] + " ");
            //    }
            //    Console.Write("\n");
            //}
            //Console.Write(1+"\n");
            //Console.Write(2);
            //Console.WriteLine(5.5%2.3);
        }
    }
}
