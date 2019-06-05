namespace TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     An abstract mesh class that holds information regarding the
    /// </summary>
    public class NavigationMap : MonoBehaviour
    {
        private const int Dimension = 8;

        private readonly Node[,] map = new Node[Dimension, Dimension];

        private Tilemap tilemap;

        /// <summary>
        ///     The main A star search algorithm API.
        /// </summary>
        /// <param name="start">
        ///     The starting point.
        /// </param>
        /// <param name="end">
        ///     The ending(goal) point.
        /// </param>
        /// <param name="ignoreBlock">
        ///     The Temp.
        /// </param>
        /// <returns>
        ///     The <see cref="Vector3" />.
        /// </returns>
        public IEnumerable<Vector3> Search(Vector3 start, Vector3 end, bool ignoreBlock = false)
        {
            this.InitializeMap();

            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(this.GetNodeAt(start));

            while (open.Count > 0)
            {
                var current = open[0];

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
                    return ConstructFinalPath(current, start);
                }

                var adjacentPos = GetAdjacentPos(current);

                foreach (var pos in adjacentPos)
                {
                    var node = this.GetNodeAt(pos);
                    if (!node.CanPass)
                    {
                        // temp solution
                        if (ignoreBlock && node.Weight > 0.0f)
                        {
                        }
                        else
                        {
                            continue;
                        }
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

            return new List<Vector3>();
        }

        protected void OnEnable()
        {
            this.tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        }

        protected void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var center = new Vector3(3.5f, 3.25f, 0.0f);

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var status = this.tilemap.GetCachedTileStatusAt(i, j);

                    if (status == null)
                    {
                        return;
                    }

#if UNITY_EDITOR
                    Handles.Label(new Vector3(i, j), status.Weight.ToString("F2").Remove(0, 1));
#endif

                    if (status.IsOccupied)
                    {
                        Gizmos.color = new Color(1f, 1f, 0.0f, 0.2f);
                    }
                    else if (status.CanPass())
                    {
                        Gizmos.color = new Color(0f, 1f, 0.0f, 0.2f);
                    }
                    else
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.2f);
                    }

                    Gizmos.DrawCube(new Vector3(i, j), new Vector3(1.0f, 1.0f, 0.0f));
                }
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
        ///     Here defines the Heuristic function. Originally I was using Manhattan Distance
        ///     (i.e. Math.Abs(pos.x - goal.x) + Math.Abs(pos.y - goal.y);).
        /// </summary>
        /// <param name="pos">
        ///     The current position.
        /// </param>
        /// <param name="goal">
        ///     The end position.
        /// </param>
        /// <returns>
        ///     The <see cref="float" /> euclidean distance from pos to goal.
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

        private static IEnumerable<Vector3> ConstructFinalPath(Node current, Vector3 startPoint)
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

        private Node GetNodeAt(Vector3 pos)
        {
            var x = Mathf.CeilToInt(pos.x);
            var y = Mathf.CeilToInt(pos.y);

            return this.map[x, y];
        }

        private void InitializeMap()
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    this.map[i, j] = new Node(i, j);

                    var status = this.tilemap.GetCachedTileStatusAt(i, j);
                    this.map[i, j].CanPass = status.CanPass();
                    this.map[i, j].Weight = status.Weight;
                }
            }
        }

        /// <summary>
        ///     Internal Node class that holds f and g values.
        /// </summary>
        private class Node : IComparable<Node>
        {
            public Node(int x, int y)
            {
                this.Pos = new Vector3(x, y);
            }

            public bool CanPass { get; set; }

            public float F { get; set; }

            public float G { get; set; }

            public float Weight { get; set; }

            public Node Parent { get; set; }

            public Vector3 Pos { get; }

            public int CompareTo(Node other)
            {
                return this.F.CompareTo(other.F);
            }
        }
    }
}