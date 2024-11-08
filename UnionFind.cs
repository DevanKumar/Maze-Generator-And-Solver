using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteStructuresAE2
{
    internal class UnionFind<T>
    {
        public int[] ID { get; private set; }
        public Dictionary<T, int> Parent { get; private set; }
        private int[] Size;
        public UnionFind(List<T> Data)
        {
            ID = new int[Data.Count];
            Size = new int[Data.Count];
            Parent = new Dictionary<T, int>();

            for (int i = 0; i < ID.Length; i++)
            {
                ID[i] = i;
                Size[i] = 1;
                Parent.Add(Data[i], ID[i]);
            }
        }
        public int Find(int p)
        {
            if (ID[p] == p)
            {
                return p;
            }
            return Find(ID[p]);
        }
        public int Find(T p) => Find(Parent[p]);
        public void Union(int p, int q)
        {
            int pParent = Find(p);
            int qParent = Find(q);
            if (Size[pParent] < qParent)
            {
                ID[pParent] = qParent;
                Size[pParent] += Size[qParent];
            }
            else{
                ID[qParent] = p;
                Size[qParent] += Size[pParent];
            }
        }
        public void Union(T p, T q) => Union(Parent[p], Parent[q]);
        public bool Connected(int p, int q) => ID[p] == ID[q];
        public bool Connected(T p, T q) => Connected(Parent[p], Parent[q]);
        public T GetParent(T p) => Parent.Keys.ElementAt(Find(Parent[p]));
    }
}
