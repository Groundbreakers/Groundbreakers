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

        private Camera mainCamera;

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
            Assert.IsNotNull(Camera.main);
            Assert.IsNotNull(this.controller, "please attach the controller to this field.");

            this.mainCamera = Camera.main;

            this.controller.ClearSelected();
        }

        private void Update()
        {
            if (Input.GetKeyDown("s"))
            {
                this.ExecuteTileSwap();
            }

            var hit = Physics2D.Raycast(
                this.mainCamera.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);

            if (hit)
            {
                var target = hit.collider.gameObject;

                if (target.CompareTag("Tile"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        this.controller.SelectTile(target);

                        //ExecuteEvents.Execute<ITileSelectMessageTarget>(
                        //    target,
                        //    null,
                        //    (t, data) => t.Select());
                    }
                }


            }

        }
    }
}