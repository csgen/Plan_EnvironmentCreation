using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentCreation
{
    internal class Node
    {
        Vector2 m_position;
        public Vector2 position => m_position;
        public Node parent;

        //角色到该节点的实际距离
        int m_g;
        public int g
        {
            get => m_g;//等同于get{return m_g}
            set
            {
                m_g = value;
                m_f = m_g + m_h;
            }
        }
        //该节点到目的地的估价距离
        int m_h;
        public int h
        {
            get => m_h;
            set
            {
                m_h = value;
                m_f = m_g + m_h;
            }
        }
        int m_f;
        public int f => m_f;

        public Node(Vector2 pos,Node parent,int g, int h)
        {
            m_position = pos;
            this.parent = parent;
            m_g = g;
            m_h = h;
            m_f = m_g + m_h;
        }
    }
}
