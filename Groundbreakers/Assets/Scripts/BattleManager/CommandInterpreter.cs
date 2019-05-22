namespace Assets.Scripts
{
    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class CommandInterpreter : MonoBehaviour
    {
        [SerializeField]
        [Required("TileController is necessary for this component.")]
        private TileController controller;


        [Button]
        public void ExecuteTileSwap()
        {
            this.controller.Begin();
        }

        [Button]
        private void Debug()
        {
            this.controller.SelectTile(new Vector3(0.0f, 0.0f));
            this.controller.SelectTile(new Vector3(5.0f, 5.0f));
        }

        private void OnEnable()
        {
            Assert.IsNotNull(this.controller, "please attach the controller to this field.");

            this.controller.ClearSelected();
        }

        private void Update()
        {
            if (Input.GetKeyDown("s"))
            {
                this.ExecuteTileSwap();
            }
        }
    }
}