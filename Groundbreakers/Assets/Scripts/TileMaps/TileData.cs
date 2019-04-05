namespace TileMaps
{
    using System.Collections;
    using System.Collections.Generic;

    using Sirenix.OdinInspector;
    using Sirenix.Utilities;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     This is a container of the tile information. It keeps track of which grid is which tile.
    ///     You can perform a swap tile command on this component to modify tiles.
    ///     Note this is a Data component, means this should contain only passive states.
    /// </summary>
    public class TileData : MonoBehaviour
    {
        [SerializeField]
        [InfoBox("Current Tile Data")]
        [TableMatrix]
        private Tiles[,] data = new Tiles[Dimension, Dimension];

        public static int Dimension { get; } = 8;

        /// <summary>
        ///     The get tile type at.
        /// </summary>
        /// <param name="x">
        ///     The x coordinate of the data. Range: [0, 7]
        /// </param>
        /// <param name="y">
        ///     The y coordinate of the data. Range: [0, 7]
        /// </param>
        /// <returns>
        ///     The <see cref="T:Assets.Scripts.Tiles" />. The enumeration that represent the tile's type.
        /// </returns>
        public Tiles GetTileTypeAt(int x, int y)
        {
            if (x < 0 || x >= Dimension || y < 0 || y >= Dimension)
            {
                return Tiles.OutOfBound;
            }

            return this.data[x, y];
        }

        public Tiles GetTileTypeAt(float x, float y)
        {
            return this.GetTileTypeAt((int)x, (int)y);
        }

        /// <summary>
        ///     Traversing all tile information.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<Tiles> TraverseTiles()
        {
            for (var x = 0; x < Dimension; x++)
            {
                for (var y = 0; y < Dimension; y++)
                {
                    yield return this.data[x, y];
                }
            }
        }
    }
}