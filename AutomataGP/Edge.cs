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
        public string key { get; set; }

        public Edge(string key, Vertex from, Vertex to)
        {
            this.key = key;
            this.from = from;
            this.to = to;
        }

        public String print()
        {
            return from.no + "----" + key + "---->" + to.no;
        }
    }
}
