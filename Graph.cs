using System.Collections.Generic;

namespace DiscreteStructuresAE2
{
    // Below is my implementation of a Directed Graph that also has an
    // implementation for Dijkstra's path finding algorithm
    internal class Graph<T>
    {
        // vertices is a list of all vertices in the graph
        private List<Vertex<T>> vertices;

        // edges is a list off all edges in the graph
        private List<Edge<T>> edges;
        public IReadOnlyList<Vertex<T>> Vertices => vertices;
        public IReadOnlyList<Edge<T>> Edges => edges;
        public int VertexCount => vertices.Count;

        public Graph()
        {
            vertices = new List<Vertex<T>>();
            edges = new List<Edge<T>>();
        }

        // given a value, Find will return the correlating Vertex and if none
        // exists it will return null
        public Vertex<T> Find(T value)
        {
            foreach (Vertex<T> currVertex in vertices)
            {
                if (currVertex.Value.Equals(value))
                {
                    return currVertex;
                }
            }
            return null;
        }

        // given a start and end vertex, GetEdge will find and return the edge
        // that connects the two, if none exists it will return null
        public Edge<T> GetEdge(Vertex<T> start, Vertex<T> end)
        {
            if (start == null || end == null)
            {
                return null;
            }
            for (int i = 0; i < start.NeighborCount; i++)
            {
                if (start.Neighbors[i].EndingPoint == end)
                {
                    return start.Neighbors[i];
                }
            }
            return null;
        }

        // AddVertex will add the given vertex to the graph
        public void AddVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.NeighborCount > 0 || vertices.Contains(vertex))
            {
                return;
            }
            vertices.Add(vertex);
        }
        
        // RemoveVertex will remove the given vertex from the graph, severing all
        // connections to it in the process
        public bool RemoveVertex(Vertex<T> vertex)
        {
            if (!vertices.Contains(vertex))
            {
                return false;
            }
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].EndingPoint == vertex)
                {
                    edges[i].StartingPoint.Neighbors.Remove(edges[i]);
                    edges.Remove(edges[i]);
                }
            }
            vertices.Remove(vertex);
            return true;
        }

        // Given a start vertex, end vertex, and distance, AddEdge will make a
        // directed edge connecting the two vertices
        public bool AddEdge(Vertex<T> start, Vertex<T> end, float distance)
        {
            if (start == null || end == null || !vertices.Contains(start) || !vertices.Contains(end))
            {
                return false;
            }
            var x = new Edge<T>(start, end, distance);
            edges.Add(x);
            start.Neighbors.Add(x);
            return true;
        }

        // Given a start and an end vertex, RemoveEdge severes the edge
        // connecting the two.
        public bool RemoveEdge(Vertex<T> start, Vertex<T> end)
        {
            if (start == null || end == null)
            {
                return false;
            }
            var x = GetEdge(start, end);
            if (x == null)
            {
                return false;
            }
            start.Neighbors.Remove(x);
            edges.Remove(x);
            return true;
        }

        // Below is my implementation of Dijkstra's path finding algorithm
        // which will find the shortest path connecting the given start and
        // end vertices
        public List<Vertex<T>> Dijkstra(Vertex<T> start, Vertex<T> end)
        {
            if (start == end)
            {
                return new List<Vertex<T>>();
            }
            var info = new Dictionary<Vertex<T>, (double distance, Vertex<T> previous, bool visited)>();
            var queue = new PriorityQueue<Vertex<T>, double>();
            Vertex<T> curr;
            for (int i = 0; i < vertices.Count; i++)
            {
                info.Add(vertices[i], (double.PositiveInfinity, null, false));
            }
            info[start] = (0, null, false);
            queue.Enqueue(start, 0);

            while (queue.Count > 0 && !info[end].visited)
            {
                curr = queue.Dequeue();
                foreach (var currNeighbor in curr.Neighbors)
                {
                    if (info[currNeighbor.EndingPoint].distance > currNeighbor.Distance + info[curr].distance)
                    {
                        info[currNeighbor.EndingPoint] = (currNeighbor.Distance + info[curr].distance, curr, false);
                    }
                    if (!info[currNeighbor.EndingPoint].visited)
                    {
                        queue.Enqueue(currNeighbor.EndingPoint, info[currNeighbor.EndingPoint].distance);
                    }
                }
                info[curr] = (info[curr].distance, info[curr].previous, true);
            }
            if (!info[end].visited)
            {
                return null;
            }
            return EndOfDijkstra(start, end, info);
        }

        // Below is a helper function for my implementation of Dijkstra's Algorithm
        // that converts the path generated by the algorithm into a list of vertices
        private List<Vertex<T>> EndOfDijkstra(Vertex<T> start, Vertex<T> end, Dictionary<Vertex<T>, (double distance, Vertex<T> previous, bool visited)> info)
        {
            var curr = end;
            var stack = new Stack<Vertex<T>>();
            var list = new List<Vertex<T>>();
            while (curr != null)
            {
                stack.Push(curr);
                curr = info[curr].previous;
            }
            while (stack.Count > 0)
            {   
                list.Add(stack.Pop());
            }
            return list;
        }
    }
}