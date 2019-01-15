using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Tilemap))]
    public class TerrainSystem : MonoBehaviour
    {
        #region Inspector Variables

        /// <summary>
        /// This is a factor affecting the Perlin noise function
        /// </summary>
        [SerializeField]
        [Range(0.1f, 2.0f)]
        private float scale = 1.0f;

        /// <summary>
        /// Pick a temporary sprite that to render as each tile.
        /// </summary>
        [SerializeField]
        private Sprite sprite;

        #endregion

        #region Internal Variables

        private const uint Dimension = 8;


        private Tilemap tilemap;

        /// <summary>
        /// The parent transform that hold all sub sprite for tiles.
        /// </summary>
        private Transform boardHolder;

        /// <summary>
        /// The 2D array that stores the height map information, each height is a float ranged from 0f to 1f.
        /// </summary>
        private float[,] data;

        #endregion

        #region Unity Callbacks

        public void Start()
        {
            this.data = new float[Dimension, Dimension];
            this.GenerateHeightMap();
            this.SetupDebuggingSprites();
        }

        #endregion

        #region Internal functions

        /// <summary>
        /// Using default Perlin noise function to generate a height map
        /// </summary>
        private void GenerateHeightMap()
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var xCoord = (float)i / (float)Dimension * this.scale;
                    var yCoord = (float)j / (float)Dimension * this.scale;

                    this.data[i, j] = Mathf.PerlinNoise(xCoord, yCoord);
                }
            }
        }

        /// <summary>
        /// Mapping the noise data to the sprite color for debugging. 
        /// </summary>
        private void SetupDebuggingSprites()
        {
            this.boardHolder = new GameObject("Map").transform;

            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    // Creating the instance with color
                    var instance = new GameObject();
                    var spriteRenderer = instance.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = this.sprite;
                    var sample = this.data[i, j];
                    spriteRenderer.color = new Color(sample, sample, sample);

                    // Locating the instance
                    instance.transform.SetPositionAndRotation(new Vector3(i, j, 0f), Quaternion.identity);
                    instance.transform.SetParent(this.boardHolder);
                }
            }

            // Locating the tile map at the center of the screen.
            this.boardHolder.SetPositionAndRotation(
                new Vector3(-Dimension / 2f, -Dimension / 2f),
                Quaternion.identity);
        }

        #endregion
    }
}