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
                    var node = this.GetNodeAt(pos);
                    if (!node.CanPass) 
                    {
                        // temp solution
                        continue;
                    }

                    // if contained in closed set
                    if (closed.Exists(n => n.Pos == pos))
                    {
                        continue;
                    }

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

            var finalPath = this.ConstructFinalPath(current, start);

            foreach (var pos in finalPath)
            {
                var block = this.tilemap.GetTileBlockAt(pos);
                block.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

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

        /// <summary>
        /// Here defines the Heuristic function. Originally I was using Manhattan Distance 
        /// (i.e. Math.Abs(pos.x - goal.x) + Math.Abs(pos.y - goal.y);).
        /// </summary>
        /// <param name="pos">
        /// The current position.
        /// </param>
        /// <param name="goal">
        /// The end position.
        /// </param>
        /// <returns>
        /// The <see cref="float"/> euclidean distance from pos to goal.
        /// </returns>
        private static float GetHeuristic(Vector3 pos, Vector3 goal)
        {
            return Vector3.Distance(pos, goal);
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

                    var status = this.tilemap.GetTileStatusAt(i, j);
                    this.map[i, j].CanPass = status.CanPass();
                }
            }
        }

        private IEnumerable<Vector3> ConstructFinalPath(Node current, Vector3 startPoint)
        {
            var finalPath = new List<Vector3>();

            while (current != null && current.Pos != startPoint)
            {
                finalPath.Add(current.Pos);
                current = current.Parent;
            }

            finalPath.Add(startPoint);
            finalPath.Reverse();

            return finalPath;
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

        /// <summary>
        /// Internal Node class that holds f and g values.
        /// </summary>
        private class Node : IComparable<Node>
        {
            public float F;

            public float G;

            public Node Parent;

            public bool CanPass;

            public Vector3 Pos;

            public Node(int x, int y)
            {
                this.Pos = new Vector3(x, y);
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