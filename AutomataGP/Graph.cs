using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataGP
{
    class Graph
    {

        public int size;
        public List<Vertex> vertices;

        public Graph(int p)
        {
            size = p;
            vertices = new List<Vertex>();
            for (int i = 0; i < p; i++)
            {
                vertices.Add(new Vertex(i));
            }
        }

        public Vertex At(int i)
        {
            return vertices[i];
        }

        public string toDOT()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("digraph G { \n");
            sb.Append("rankdir=LR\nsize=\"8,5\"\n node [shape = circle]\n");

            sb.Append("\n");

            foreach (Vertex v in vertices)
            {
                foreach (Edge edge in v.outgoing)
                {
                    sb.Append("S" + edge.from.no + "->" + "S" + edge.to.no + " [label=" + edge.key + "]" + "\n");
                }
            }
            sb.Append("}\n");
            return sb.ToString();
        }


    }
}
