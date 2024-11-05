using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteStructuresAE2
{
    internal class Vertex<T>
    {
        public T Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }
        public int NeighborCount => Neighbors.Count;
        public Vertex(T Value) 
        {
            this.Value = Value;
            Neighbors = new List<Edge<T>>();
        }
    }
}
