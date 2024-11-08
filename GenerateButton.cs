﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteStructuresAE2
{
    internal class GenerateButton : Sprite
    {
        public List<Vertex<Point>> DijkstraPath { get; private set; }
        public Graph<Point> Graph { get; private set; }
        public UnionFind<Vertex<Point>> UnionFind { get; private set; }
        public List<Rectangle> Walls { get; private set; }
        private Color OrigColor;
        public GenerateButton(Texture2D texture, Vector2 position, Color color, Vector2 scale) : base(texture, position, color, scale, 0)
        {
            OrigColor = color;
        }

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
        private void GenerateMaze(Tile[,] tiles, Vector2 tileSize)
        {
            Graph = new Graph<Point>();
            DijkstraPath = new List<Vertex<Point>>();
            Walls = new List<Rectangle>();
            var tileVerticies = new Dictionary<Point, Vertex<Point>>();
            foreach (var tile in tiles)
            {
                Vertex<Point> currVertex = new Vertex<Point>(tile.GetPosition((int)tileSize.X, (int)tileSize.Y));
                tileVerticies.Add(currVertex.Value, currVertex);
                Graph.AddVertex(currVertex);
            }
            UnionFind = new UnionFind<Vertex<Point>>(Graph.Vertices);
            Vertex<Point> startNode = Graph.Find(new Point((int)InputManager.StartNode.Position.X, (int)InputManager.StartNode.Position.Y));
            Vertex<Point> endNode = Graph.Find(new Point((int)InputManager.EndNode.Position.X, (int)InputManager.EndNode.Position.Y));
            Random gen = new Random();
            while (!UnionFind.Connected(startNode, endNode))
            {
                int firstNode = gen.Next(0, Graph.Vertices.Count);
                int secondNode = gen.Next(0, Graph.Vertices.Count);
                UnionFind.Union(firstNode, secondNode);
            }
            foreach ((Point point, Vertex<Point> vertex) in tileVerticies)
            {
                foreach (var neighbor in GenerateNeighbors(tiles, point))
                {
                    if (UnionFind.Connected(vertex, tileVerticies[neighbor]))
                    {
                        Graph.AddEdge(vertex, tileVerticies[neighbor], 1);
                    }
                    else if (neighbor.X == point.X - 1)
                    {
                        Tile currTile = tiles[point.X, point.Y];
                        Rectangle wall = new Rectangle((int)currTile.Position.X, (int)currTile.Position.Y, 2, (int)currTile.Scale.Y);
                        Walls.Add(wall);
                    }
                    else if (neighbor.X == point.X + 1)
                    {
                        Tile currTile = tiles[point.X, point.Y];
                        Rectangle wall = new Rectangle((int)currTile.Position.X + (int)currTile.Scale.X, (int)currTile.Position.Y, 2, (int)currTile.Scale.Y);
                        Walls.Add(wall);
                    }
                    else if (neighbor.Y == point.Y - 1)
                    {
                        Tile currTile = tiles[point.X, point.Y];
                        Rectangle wall = new Rectangle((int)currTile.Position.X, (int)currTile.Position.Y, (int)currTile.Scale.X, 2);
                        Walls.Add(wall);
                    }
                    else
                    {
                        Tile currTile = tiles[point.X, point.Y];
                        Rectangle wall = new Rectangle((int)currTile.Position.X, (int)currTile.Position.Y + (int)currTile.Scale.Y, (int)currTile.Scale.X, 2);
                        Walls.Add(wall);
                    }
                }
            }
            DijkstraPath = Graph.Dijkstra(startNode, endNode);
            if (DijkstraPath == null)
            {
                GenerateMaze(tiles, tileSize);
            }
        }
    }
}
