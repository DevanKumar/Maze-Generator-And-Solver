using System.Collections.Generic;

namespace DiscreteStructuresAE2
{
    // Vertex class for my graph implementation, stores its value along
    // with all of its edge connections.
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
