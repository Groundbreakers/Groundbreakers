namespace Groundbreakers
{
    using UnityEngine;

    public class BattleManager : MonoBehaviour
    {
        private const int Columns = 8;

        private const int Rows = 8;

        public Sprite sprite;

        public Sprite[] sprites;

        public GameObject[] tiles;

        private Transform boardHolder;

        public enum Tiles
        {
            Stone,

            Sand,

            Grass
        }

        /// <summary>
        ///     Start is called before the first frame update
        /// </summary>
        public void Start()
        {
            this.Initialize();
        }

        /// <summary>
        ///     TUpdate is called once per frame
        /// </summary>
        public void Update()
        {
        }

        private void Initialize()
        {
            this.boardHolder = new GameObject("Map").transform;

            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    var instance = new GameObject();
                    var spriteRenderer = instance.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = this.sprite;

                    instance.transform.SetPositionAndRotation(new Vector3(i, j, 0f), Quaternion.identity);
                    instance.transform.SetParent(this.boardHolder);
                }
            }

            this.boardHolder.SetPositionAndRotation(new Vector3(-Columns / 2f, -Rows / 2f), Quaternion.identity);
        }
    }
}