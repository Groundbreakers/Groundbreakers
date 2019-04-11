namespace TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using UnityEngine;

    public class NavigationMap : MonoBehaviour
    {
        private const int Dimension = 8;

        private Tilemap tilemap;

        private Node[,] map = new Node[Dimension, Dimension];

        private static IEnumerable<Vector3> GetAdjacentPos(Node node)
        {
            var x = node.Pos.x;
            var y = node.Pos.y;

            var proposed = new List<Vector3>
                               {
                                   new Vector3(x + 1.0f, y),
                                   new Vector3(x - 1.0f, y),
                                   new Vector3(x, y + 1.0f),
                                   new Vector3(x, y - 1.0f)
                               };

            return proposed.Where(IsValid);
        }

        private static float GetHeuristic(Vector3 pos, Vector3 goal)
        {
            return Math.Abs(pos.x - goal.x) + Math.Abs(pos.y - goal.y);
        }

        private static bool IsValid(Vector3 position)
        {
            var x = position.x;
            var y = position.y;

            return !(x < 0 || x >= Dimension || y < 0 || y >= Dimension);
        }

        private void InitializeMap()
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    this.map[i, j] = new Node(i, j);
                }
            }
        }

        [Button]
        public void Search()
        {
            this.InitializeMap();

            var start = new Vector3(0, 0);
            var end = new Vector3(7, 7);

            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(this.GetNodeAt(start));

            var current = open[0];
            while (open.Count > 0)
            {
                current = open[0];

                for (var i = 1; i < open.Count; i++)
                {
                    // Loop through the open list starting from the second object
                    if (open[i].F < current.F)
                    {
                        current = open[i]; // Set the current node to that object
                    }
                }

                open.Remove(current);
                closed.Add(current);

                if (current.Pos == end)
                {
                    break;
                }

                var adjacentPos = GetAdjacentPos(current);

                foreach (var pos in adjacentPos)
                {
                    // if contained in closed set
                    if (closed.Exists(n => n.Pos == pos))
                    {
                        continue;
                    }

                    var node = this.GetNodeAt(pos);
                    var f = current.G + 1;

                    if (open.All(n => n.Pos != pos))
                    {
                        // not in open set
                        open.Add(node);
                    }
                    else if (f >= node.G)
                    {
                        continue;
                    }

                    node.Parent = current;
                    node.G = f;
                    node.F = f + GetHeuristic(pos, end);
                }
            }

            var finalPath = new List<Vector3>();

            while (current != null && current.Pos != start)
            {
                finalPath.Add(current.Pos);
                current = current.Parent;
            }

            finalPath.Reverse();

            foreach (var node in finalPath)
            {
                Debug.Log(node);
            }
        }

        private void OnEnable()
        {
            this.tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        }

        private Node GetNodeAt(Vector3 pos)
        {
            var x = (int)pos.x;
            var y = (int)pos.y;

            return this.map[x, y];
        }

        private class Node : IComparable<Node>
        {
            public float F;

            public float G;

            public Node Parent;

            public Vector3 Pos;

            public Node(Vector2 pos)
            {
                this.Pos = pos;
                this.F = 0;
                this.G = 0;
            }

            public Node(int x, int y)
            {
                this.Pos = new Vector3(x, y);
                this.F = 0;
                this.G = 0;
            }

            public int CompareTo(Node other)
            {
                return this.F.CompareTo(other.F);
            }

            public override string ToString()
            {
                return $"{this.Pos}: {this.F}\n";
            }
        }
    }
}