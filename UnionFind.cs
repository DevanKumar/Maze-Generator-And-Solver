using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteStructuresAE2
{
    internal class UnionFind<T>
    {
        public int[] Parent { get; private set; }
        public UnionFind(int size)
        {
            Parent = new int[size];
            for (int i = 0; i < Parent.Length; i++)
            {
                Parent[i] = i;
            }
        }
        public int Find(int p)
        {
            if (Parent[p] == p)
            {
                return p;
            }
            Parent[p] = Find(Parent[p]);
            return Parent[p];
        }
        public void Union(int p, int q)
        {
            int pParent = Find(p);
            int qParent = Find(q);
            Parent[pParent] = qParent;
        }
        public bool Connected(int p, int q) => Parent[p] == Parent[q] ? true : false;
    }
}
