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
        private int[] Size;
        public UnionFind(int size)
        {
            Parent = new int[size];
            Size = new int[size];
            for (int i = 0; i < Parent.Length; i++)
            {
                Parent[i] = i;
                Size[i] = 1;
            }
        }
        public int Find(int p)
        {
            if (Parent[p] == p)
            {
                return p;
            }
            return Find(Parent[p]);
        }
        public void Union(int p, int q)
        {
            int pParent = Find(p);
            int qParent = Find(q);
            if (Size[pParent] < qParent)
            {
                Parent[pParent] = qParent;
                Size[pParent] += Size[qParent];
            }
            else{
                Parent[qParent] = p;
                Size[qParent] += Size[pParent];
            }
        }
        public bool Connected(int p, int q) => Parent[p] == Parent[q] ? true : false;
    }
}
