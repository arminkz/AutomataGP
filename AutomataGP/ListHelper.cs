using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataGP
{
    static class ListHelper
    {

        public static bool Compare(List<Vertex> l1, List<Vertex> l2)
        {
            var firstNotSecond = l1.Except(l2).ToList();
            var secondNotFirst = l2.Except(l1).ToList();
            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }

    }
}
