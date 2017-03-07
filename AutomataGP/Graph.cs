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
        public int initialState;

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
            StringBuilder node_final_str = new StringBuilder();
            StringBuilder edge_str = new StringBuilder();

            node_init_str.Append("{ \n node[shape = point, width = 0]\n");
            node_str.Append("{ \n node[shape = circle]\n");
            node_final_str.Append("{ \n node[shape = doublecircle]\n");

            sb.Append("\n");
            int init_state_counter = 0;

            foreach (Vertex v in vertices)
            {
                if (v.isInitial)
                {
                    node_init_str.Append("I" + init_state_counter + "\n");
                    edge_str.Append("I" + init_state_counter + "->" + v.GetName() + "\n");
                    init_state_counter++;
                }
                if (v.isFinal)
                {
                    node_final_str.Append(v.GetName() + "\n");
                }
                else
                {
                    node_str.Append(v.GetName() + "\n");
                }
                foreach (Edge edge in v.outgoing)
                {
                    edge_str.Append(edge.from.GetName() + "->" + edge.to.GetName() + " [label=" + edge.key + "]" + "\n");
                }
            }

            node_init_str.Append("}\n");
            node_str.Append("}\n");
            node_final_str.Append("}\n");

            sb.Append(node_init_str.ToString());
            sb.Append(node_str.ToString());
            sb.Append(node_final_str.ToString());
            sb.Append(edge_str.ToString());

            sb.Append("}\n");
            return sb.ToString();
        }

        public bool acceptString(string s)
        {
            return acceptString(s, 0, initialState);
        }

        public bool acceptString(string s,int n,int curState)
        {
            if (n == s.Length)
            {
                return At(curState).isFinal;
            }

            char c = s[n];
            bool b = false;

            foreach(Edge e in At(curState).outgoing)
            {   
                if(e.key == c)
                {
                    b = b || acceptString(s, n+1, e.to.no);
                }
            }

            return b;

        }

        public static Graph ConvertToDFA(Graph g)
        {
            Graph h = new Graph(0);

            List<char> alphabet = new List<char>() { 'a', 'b' };
            List<ComplexVertex> cstates = new List<ComplexVertex>();

            //Create all Combinations of NDFA States to create DFA States
            double count = Math.Pow(2, g.vertices.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                List<Vertex> state = new List<Vertex>();
                string str = Convert.ToString(i, 2).PadLeft(g.vertices.Count, '0');
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        state.Add(g.At(j));
                    }
                }
                cstates.Add(new ComplexVertex(state));
            }

            //Convert Complex Vertices to Simple Vertices
            foreach (ComplexVertex cv in cstates)
                h.vertices.Add(cv.toVertex());

            //Create Edges
            for (int cvi = 0; cvi < cstates.Count; cvi++)
                foreach (char alpha in alphabet)
                {
                    ComplexVertex target = cstates[cvi].onTrasition(alpha);
                    for (int cvicomp = 0; cvicomp < cstates.Count; cvicomp++)
                    {
                        if (cstates[cvicomp].Equals(target))
                        {
                            h.At(cvi).addOutEdge(alpha, h.At(cvicomp));
                        }
                    }
                }


            return h;
        }

    }
}
