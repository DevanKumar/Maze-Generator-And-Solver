using System.Collections.Generic;
using System.Linq;

namespace DiscreteStructuresAE2
{
    // Below is my implementation of the Union-Find (Disjoint Sets) data structure
    internal class UnionFind<T>
    {
        // ID is an array that is the size of the data
        // that contains the ID (index) for its parent
        public int[] ID { get; private set; }

        // Parent is a dictionary that takes the given
        // data values and correlates them to their index
        // in the ID array
        public Dictionary<T, int> Parent { get; private set; }

        // NumOfSets is the number of different sets within
        // the data structure
        public int NumOfSets { get; private set; }

        // Size keeps track of the size of each set within
        // the data structure
        private int[] Size;
        public UnionFind(IReadOnlyList<T> Data)
        {
            ID = new int[Data.Count];
            Size = new int[Data.Count];
            Parent = new Dictionary<T, int>();
            NumOfSets = Data.Count;
            for (int i = 0; i < ID.Length; i++)
            {
                ID[i] = i;
                Size[i] = 1;
                Parent.Add(Data[i], ID[i]);
            }
        }

        // Given a value Find will return the ID (index) of its parent
        public int Find(T p) => Find(Parent[p]);

        // Given an index of a child it will return the index of its parent
        public int Find(int p)
        {
            if (ID[p] == p)
            {
                return p;
            }
            return Find(ID[p]);
        }

        // Given two values Union will join their correlating sets together
        public void Union(T p, T q) => Union(Parent[p], Parent[q]);

        // Give two indexes (IDs) Union will join their respective sets together
        public void Union(int p, int q)
        {
            int pParent = Find(p);
            int qParent = Find(q);
            if(!Connected(pParent, qParent))
            {
                NumOfSets--;
            }
            if (Size[pParent] < Size[qParent])
            {
                ID[pParent] = qParent;
                Size[qParent] += Size[pParent];
            }
            else
            {
                ID[qParent] = pParent;
                Size[pParent] += Size[qParent];
            }
        }

        // Given two values Connected will return true or false depending on
        // whether or not the two values's correlating sets are connected (joined)
        public bool Connected(T p, T q) => Connected(Parent[p], Parent[q]);

        // Given two indexes (IDs) Connected will return true or false depending
        // on whether or not the respective sets are connected (joined)
        public bool Connected(int p, int q) => Find(ID[p]) == Find(ID[q]);

        // Give a value GetParent will return its correlating index for the ID array
        public T GetParent(T p) => Parent.Keys.ElementAt(Find(Parent[p]));
    }
}
