using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteStructuresAE2
{
    internal class Edge<T>
    {
        public Vertex<T> StartingPoint {  get; set; }
        public Vertex<T> EndingPoint { get; set; }
        public float Distance { get; set; }
        public Edge(Vertex<T> StartingPoint, Vertex<T> EndingPoint, float Distance)
        {
            this.StartingPoint = StartingPoint;
            this.EndingPoint = EndingPoint;
            this.Distance = Distance;
        }
    }
}
