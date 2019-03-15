namespace Assets.Scripts
{
    using UnityEngine;

    public static class BattleUtility
    {
        private static GameMap map;

        /// <summary>
        /// Must be called in BattleManager.Initialize. Setting up some references to be referred
        /// later.
        /// </summary>
        public static void Initialize()
        {
            map = GameObject.Find("TileMap").GetComponent<GameMap>();
        }

        public static void SetTileCanDeploy(GameObject parent, bool canDeploy)
        {
            if (!parent)
            {
                Debug.LogWarning("Attempt to access tiles of parent");
                return;
            }

            parent.GetComponent<SelectNode>().SetCanDeploy(canDeploy);
        }

        public static void SetTileCanDeploy(Vector3 position, bool canDeploy)
        {
            var block = map.GetTileAt(position);

            if (!block)
            {
                Debug.LogWarning("Attempt to access tiles at (" + position.x + ", " + position.y + ")");
                return;
            }

            block.GetComponent<SelectNode>().SetCanDeploy(canDeploy);
        }

        public static void GetRandomTile()
        {
            
        }
    }
}