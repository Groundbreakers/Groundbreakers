namespace Core
{
    using System.Collections.Generic;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class InputSystem : MonoBehaviour
    {
        private Camera mainCamera;

        private List<GameObject> buffer = new List<GameObject>();

        private TileController tileController;


        [Required]
        [SerializeField]
        private LayerMask tileLayer;

        [ShowInInspector]
        private GameObject currentHovered;

        private void OnEnable()
        {
            Assert.IsNotNull(Camera.main);

            this.mainCamera = Camera.main;
            this.tileController = FindObjectOfType<TileController>();
        }

        private void Update()
        {
            var hit = Physics2D.Raycast(
                this.mainCamera.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero,
                100,
                this.tileLayer);

            if (hit)
            {
                var target = hit.collider.gameObject;

                if (this.currentHovered != target)
                {
                    if (this.currentHovered != null)
                    {
                        this.currentHovered.transform.localScale = new Vector3(1.0f, 1.0f);
                    }

                    this.currentHovered = target;

                    target.transform.localScale = new Vector3(2.0f, 2.0f);
                }


                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("1");

                    // this.tileController.SelectTile(target);

                    //ExecuteEvents.Execute<ITileSelectMessageTarget>(
                    //    target,
                    //    null,
                    //    (t, data) => t.Select());
                }
            }
            else
            {
                if (this.currentHovered != null)
                {
                    this.currentHovered.transform.localScale = new Vector3(1.0f, 1.0f);
                    this.currentHovered = null;
                }
            }
        }
    }
}