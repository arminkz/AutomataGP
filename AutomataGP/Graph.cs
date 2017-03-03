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
            sb.Append("rankdir=LR\nsize=\"8,5\"");

            StringBuilder node_str = new StringBuilder();
            StringBuilder node_init_str = new StringBuilder();
            StringBuilder edge_str = new StringBuilder();

            node_str.Append("{ \n node[shape = circle]\n");
            node_init_str.Append("{ \n node[shape = doublecircle]\n");

            sb.Append("\n");

            foreach (Vertex v in vertices)
            {
                if (v.isInitial)
                {
                    node_init_str.Append("S" + v.no + "\n");
                }
                else
                {
                    node_str.Append("S" + v.no + "\n");
                }
                foreach (Edge edge in v.outgoing)
                {
                    edge_str.Append("S" + edge.from.no + "->" + "S" + edge.to.no + " [label=" + edge.key + "]" + "\n");
                }
            }

            node_str.Append("}\n");
            node_init_str.Append("}\n");

            sb.Append(node_init_str.ToString());
            sb.Append(node_str.ToString());
            sb.Append(edge_str.ToString());

            sb.Append("}\n");
            return sb.ToString();
        }


    }
}
