namespace TileMaps
{
    using System.Collections.Generic;

    using UnityEngine;

    public interface ITerrainData
    {
        /// <summary>
        ///     The get tile type at this position.
        /// </summary>
        /// <param name="x">
        ///     The x coordinate of the data. Range: [0, 7]
        /// </param>
        /// <param name="y">
        ///     The y coordinate of the data. Range: [0, 7]
        /// </param>
        /// <returns>
        ///     The <see cref="Tiles" />. The enumeration that represent the tile's type.
        /// </returns>
        Tiles GetTileTypeAt(float x, float y);

        /// <summary>
        ///     This function should be called by GameMap.
        ///     For generators: A new sets of data is generated each time you call this Function.
        ///     For custom Terrains: Should Load the right level from our database.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Evaluate and return the spawn locations.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Vector3> GetSpawnLocations();
    }
}