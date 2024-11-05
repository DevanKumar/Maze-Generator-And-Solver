using DiscreteStructuresAE2;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteStructuresAE2
{
    internal class Graph<T>
    {
        private List<Vertex<T>> vertices;
        private List<Edge<T>> edges;
        public IReadOnlyList<Vertex<T>> Vertices => vertices;
        public IReadOnlyList<Edge<T>> Edges => edges;
        public int VertexCount => vertices.Count;

        public Graph()
        {
            vertices = new List<Vertex<T>>();
            edges = new List<Edge<T>>();
        }
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
        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null || b == null)
            {
                return null;
            }
            for (int i = 0; i < a.NeighborCount; i++)
            {
                if (a.Neighbors[i].EndingPoint == b)
                {
                    return a.Neighbors[i];
                }
            }
            return null;
        }
        public void AddVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.NeighborCount > 0 || vertices.Contains(vertex))
            {
                return;
            }
            vertices.Add(vertex);
        }
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
        public bool AddEdge(Vertex<T> a, Vertex<T> b, float distance)
        {
            if (a == null || b == null || !vertices.Contains(a) || !vertices.Contains(b))
            {
                return false;
            }
            var x = new Edge<T>(a, b, distance);
            edges.Add(x);
            a.Neighbors.Add(x);
            return true;
        }
        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            var x = GetEdge(a, b);
            if (x == null)
            {
                return false;
            }
            a.Neighbors.Remove(x);
            edges.Remove(x);
            return true;
        }
        public List<Vertex<T>> Dijkstra(Vertex<T> start, Vertex<T> end) // enqueued doesn't seem to be doing anything
        {
            if (start == end)
            {
                return new List<Vertex<T>>();
            }
            var info = new Dictionary<Vertex<T>, (double distance, Vertex<T> previous, bool visited)>();
            var enqueued = new HashSet<Vertex<T>>();
            var queue = new PriorityQueue<Vertex<T>, double>();
            Vertex<T> curr;
            for (int i = 0; i < vertices.Count; i++)
            {
                info.Add(vertices[i], (double.PositiveInfinity, null, false));
            }
            info[start] = (0, null, false);
            queue.Enqueue(start, 0);
            enqueued.Add(start);

            while (queue.Count > 0 && !info[end].visited)
            {
                curr = queue.Dequeue();
                enqueued.Remove(curr);
                foreach (var currNeighbor in curr.Neighbors)
                {
                    if (info[currNeighbor.EndingPoint].distance > currNeighbor.Distance + info[curr].distance)
                    {
                        info[currNeighbor.EndingPoint] = (currNeighbor.Distance + info[curr].distance, curr, false);
                    }
                    if (!info[currNeighbor.EndingPoint].visited && !enqueued.Contains(currNeighbor.EndingPoint))
                    {
                        queue.Enqueue(currNeighbor.EndingPoint, info[currNeighbor.EndingPoint].distance);
                        enqueued.Add(currNeighbor.EndingPoint);
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