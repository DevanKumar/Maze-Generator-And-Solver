using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace DiscreteStructuresAE2
{
    // The GenerateButton class inherits the Sprite class and is used to
    // create the button in the bottom right corner of the visualizer
    // that generates and solves a maze
    internal class GenerateButton : Sprite
    {
        public List<Vertex<Point>> DijkstraPath { get; private set; }
        public Graph<Point> MazePaths { get; private set; }
        public UnionFind<Vertex<Point>> UnionFind { get; private set; }
        public List<Rectangle> Walls { get; private set; }
        private Color OrigColor;
        public GenerateButton(Texture2D texture, Vector2 position, Color color, Vector2 scale) : base(texture, position, color, scale, 0)
        {
            OrigColor = color;
        }

        // Clicked generates and solves a maze, and updates
        // InputManager.Generated to true, letting the program
        // know that a maze has been solved and generated.
        public void Clicked(Tile[,] tiles, Vector2 tileSize)
        {
            if (InputManager.CurrentMouseState.LeftButton == ButtonState.Pressed && Hitbox.Contains(InputManager.MousePosition()))
            {
                Color = Color.Black;
                InputManager.Generated = true;
                GenerateMaze(tiles, tileSize);
            }
            else
            {
                Color = OrigColor;
            }
        }

        // GenerateMaze creates a Graph "algorithmGraph" that represents a blank
        // maze with no walls (the whole grid is connected to its adjacent vertex),
        // and another Graph "MazePaths" that will represent the paths that can be traveled
        // through in the maze but starts off with no edges, and a UnionFind that contains
        // every single node in the Grid which each start as their own set. It then randomly
        // connects neighboring nodes that are part of different sets until there is only
        // one set left in the UnionFind (all sets have been unioned together), removes
        // the edge in "algorithmGraph" that correlate to each union ands it to "MazePaths".
        // The remaining edges in "algorithmGraph" represent where a wall should be drawn,
        // so a list of rectangles (Walls) is generated. Then finally Dijkstra's Algorithm
        // is ran on "MazePaths" and the generated path is saved to "DijkstraPath".
        private void GenerateMaze(Tile[,] tiles, Vector2 tileSize)
        {
            MazePaths = new Graph<Point>();
            Graph<Point> algorithmGraph = new Graph<Point>();
            DijkstraPath = new List<Vertex<Point>>();
            Walls = new List<Rectangle>();
            Dictionary<Point, Vertex<Point>> tileVerticies = new Dictionary<Point, Vertex<Point>>();
            foreach (var tile in tiles)
            {
                Vertex<Point> algorithmVertex = new Vertex<Point>(tile.GetPosition((int)tileSize.X, (int)tileSize.Y));
                tileVerticies.Add(algorithmVertex.Value, algorithmVertex);
                algorithmGraph.AddVertex(algorithmVertex);
                Vertex<Point> mazeVertex = new Vertex<Point>(tile.GetPosition((int)tileSize.X, (int)tileSize.Y));
                MazePaths.AddVertex(mazeVertex);
            }
            UnionFind = new UnionFind<Vertex<Point>>(algorithmGraph.Vertices);
            foreach ((Point point, Vertex<Point> vertex) in tileVerticies)
            {
                foreach (var neighbor in GenerateNeighbors(tiles, point))
                {
                    algorithmGraph.AddEdge(vertex, tileVerticies[neighbor], 1);
                }
            }
            Random gen = new Random();
            while (UnionFind.NumOfSets > 1)
            {
                int edgeToUnion = gen.Next(0, algorithmGraph.Edges.Count);
                Vertex<Point> firstNode = algorithmGraph.Edges[edgeToUnion].StartingPoint;
                Vertex<Point> secondNode = algorithmGraph.Edges[edgeToUnion].EndingPoint;
                if(!UnionFind.Connected(firstNode, secondNode))
                {
                    UnionFind.Union(UnionFind.GetParent(firstNode), UnionFind.GetParent(secondNode));
                    algorithmGraph.RemoveEdge(firstNode, secondNode);
                    algorithmGraph.RemoveEdge(secondNode, firstNode);
                    Vertex<Point> mazeFirstNode = MazePaths.Find(firstNode.Value);
                    Vertex<Point> mazeSecondNode = MazePaths.Find(secondNode.Value);
                    MazePaths.AddEdge(mazeFirstNode, mazeSecondNode, 1);
                    MazePaths.AddEdge(mazeSecondNode, mazeFirstNode, 1);
                }
            }
            foreach (Edge<Point> edge in algorithmGraph.Edges)
            {
                if (edge.StartingPoint.Value.X == edge.EndingPoint.Value.X + 1)
                {
                    Tile currTile = tiles[edge.StartingPoint.Value.X, edge.StartingPoint.Value.Y];
                    Rectangle wall = new Rectangle((int)currTile.Position.X, (int)currTile.Position.Y, 2, (int)currTile.Scale.Y);
                    Walls.Add(wall);
                }
                else if (edge.StartingPoint.Value.X == edge.EndingPoint.Value.X - 1)
                {
                    Tile currTile = tiles[edge.StartingPoint.Value.X, edge.StartingPoint.Value.Y];
                    Rectangle wall = new Rectangle((int)currTile.Position.X + (int)currTile.Scale.X, (int)currTile.Position.Y, 2, (int)currTile.Scale.Y);
                    Walls.Add(wall);
                }
                else if (edge.StartingPoint.Value.Y == edge.EndingPoint.Value.Y + 1)
                {
                    Tile currTile = tiles[edge.StartingPoint.Value.X, edge.StartingPoint.Value.Y];
                    Rectangle wall = new Rectangle((int)currTile.Position.X, (int)currTile.Position.Y, (int)currTile.Scale.X, 2);
                    Walls.Add(wall);
                }
                else
                {
                    Tile currTile = tiles[edge.StartingPoint.Value.X, edge.StartingPoint.Value.Y];
                    Rectangle wall = new Rectangle((int)currTile.Position.X, (int)currTile.Position.Y + (int)currTile.Scale.Y, (int)currTile.Scale.X, 2);
                    Walls.Add(wall);
                }
            }
            Vertex<Point> startNode = MazePaths.Find(new Point((int)InputManager.StartNode.Position.X, (int)InputManager.StartNode.Position.Y));
            Vertex<Point> endNode = MazePaths.Find(new Point((int)InputManager.EndNode.Position.X, (int)InputManager.EndNode.Position.Y));
            DijkstraPath = MazePaths.Dijkstra(startNode, endNode);
            if (DijkstraPath == null)
            {
                GenerateMaze(tiles, tileSize);
            }
        }

        // Helper function for GenerateMaze that generates a list of points that
        // correlate to the indices of a vertex's adjacent (neighboring) nodes in
        // the Grid's Tiles[,] 2D array
        private IEnumerable<Point> GenerateNeighbors(Tile[,] tiles, Point p)
        {
            List<Point> possibleNeighbors = new List<Point>{ new Point(p.X - 1, p.Y),
                new Point(p.X, p.Y - 1), new Point(p.X, p.Y + 1), new Point(p.X + 1, p.Y) };
            List<Point> neighbors = new List<Point>();
            foreach (var currPossibility in possibleNeighbors)
            {
                if (tiles.GetLength(0) > currPossibility.X && currPossibility.X >= 0 && tiles.GetLength(1) > currPossibility.Y && currPossibility.Y >= 0)
                {
                    neighbors.Add(currPossibility);
                }
            }
            return neighbors;
        }
    }
}
