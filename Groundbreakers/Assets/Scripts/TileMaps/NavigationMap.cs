namespace TileMaps
{
    using UnityEngine;

    public class NavigationMap : MonoBehaviour
    {
        private Node[,] nodes = new Node[TileData.Dimension, TileData.Dimension];

        private struct Node
        {
            /// <summary>
            ///     The weight value. Different tiles should have different values.
            /// </summary>
            private float weight;

            /// <summary>
            ///     Holding the state if this node is active (not traveling).
            /// </summary>
            private bool active;

            /// <summary>
            ///     Holding the state if this node is occupied by any character or enemy.
            /// </summary>
            private bool occupied;
        }
    }
}