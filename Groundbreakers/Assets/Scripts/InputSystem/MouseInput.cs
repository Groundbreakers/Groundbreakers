namespace InputSystem
{
    using System.Collections.Generic;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.EventSystems;

    using Assert = UnityEngine.Assertions.Assert;

    public class MouseInput : MonoBehaviour
    {
        private Camera mainCamera;

        private List<GameObject> buffer = new List<GameObject>();

        private TileController tileController;

        private void OnEnable()
        {
            Assert.IsNotNull(Camera.main);

            this.mainCamera = Camera.main;
            this.tileController = GameObject.FindObjectOfType<TileController>();
        }

        private void Update()
        {
            var hit = Physics2D.Raycast(
                this.mainCamera.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero);

            if (hit)
            {
                var target = hit.collider.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    this.tileController.SelectTile(target);

                    //ExecuteEvents.Execute<ITileSelectMessageTarget>(
                    //    target,
                    //    null,
                    //    (t, data) => t.Select());
                }
            }
        }
    }
}