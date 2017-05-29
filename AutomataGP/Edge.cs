using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataGP
{
    class Edge
    {
        public Vertex from { get; set; }
        public Vertex to { get; set; }
        public char key { get; set; }

        public char npda_pop { get; set; }
        public string npda_push { get; set; }
        public bool isPDA { get; set; }

        public Edge(char key, Vertex from, Vertex to)
        {
            this.key = key;
            this.from = from;
            this.to = to;
            isPDA = false;
        }

        public Edge(char key, Vertex from, Vertex to,char npda_pop,string npda_push)
        {
            this.key = key;
            this.from = from;
            this.to = to;
            this.npda_pop = npda_pop;
            this.npda_push = npda_push;
            isPDA = true;
        }

        public String print()
        {
            return from.no + "----" + key + "---->" + to.no;
        }
    }
}
