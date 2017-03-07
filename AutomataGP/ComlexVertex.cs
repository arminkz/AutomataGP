using System.Collections.Generic;

namespace AutomataGP
{
    class ComplexVertex
    {
        public List<Vertex> sub;

        public ComplexVertex(List<Vertex> sub)
        {
            this.sub = sub;
        }

        public bool Equals(ComplexVertex cv)
        {
            //sub.Sort();
            //cv.sub.Sort();

            bool isEqual = true;

            if(sub.Count == cv.sub.Count)
            {
                /*for(int i = 0; i < sub.Count; i++)
                {
                    if (!sub[i].Equals(cv.sub[i])) return false;
                }
                return true;*/
                return ListHelper.Compare(sub, cv.sub);
            }
            return false;
        }

        //Returns Vertex version of ComplexVertex (without Edges)
        public Vertex toVertex()
        {
            int no = 0;
            string name = "S"; //[
            bool isFinal = false;
            for (int i = 0; i < sub.Count; i++)
            {
                no += sub[i].no + 1;
                name += sub[i].no;
                if (i != sub.Count - 1) name += "_"; else name += ""; //]
                isFinal = isFinal || sub[i].isFinal;
            }
            Vertex v = new Vertex(no);
            v.name = name;
            v.isFinal = isFinal;
            v.isInitial = (sub.Count == 1 && sub[0].isInitial);
            return v;
        }

        public ComplexVertex onTrasition(char a)
        {
            List<Vertex> ot = new List<Vertex>();

            foreach(Vertex v in sub)
            {
                foreach (Edge e in v.outgoing)
                {
                    if (e.key == a)
                    {
                        if (!ot.Contains(e.to)) ot.Add(e.to);
                    }
                }
            }       
            return new ComplexVertex(ot);
        }

    }
}