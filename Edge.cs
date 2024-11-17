namespace DiscreteStructuresAE2
{
    // Edge class for my graph implementation which stores its starting and
    // end vertices, and its weight (distance)
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
