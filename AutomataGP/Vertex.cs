using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataGP
{
    class Vertex
    {
        public int no;

        public List<Edge> outgoing;
        public List<Edge> incoming;

        public bool isInitial = false;
        public bool isFinal = false;

        public Vertex(int no)
        {
            this.no = no;
            outgoing = new List<Edge>();
            incoming = new List<Edge>();
        }

        public void addOutEdge(char key, Vertex to)
        {
            Edge e = new Edge(key, this, to);
            outgoing.Add(e);
            to.incoming.Add(e);
        }

        public void removeOutEdge(Edge e)
        {
            //Edge e = new Edge(key,this,to);
            e.to.incoming.Remove(e);
            outgoing.Remove(e);
        }

        public void addInEdge(char key, Vertex from)
        {
            Edge e = new Edge(key, from, this);
            incoming.Add(e);
            from.outgoing.Add(e);
        }

        public string getAdj()
        {
            string str = "";
            foreach (Edge e in outgoing)
            {
                str += "(" + e.to.no + "," + e.key + ") ";
            }
            return str;
        }

    }
}
