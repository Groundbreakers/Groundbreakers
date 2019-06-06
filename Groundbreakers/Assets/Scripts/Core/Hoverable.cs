namespace Core
{
    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    ///     Attach this to tiles that you want to be hover able.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(TileStatus))]
    public class Hoverable : MonoBehaviour
    {
        private static readonly int TurnOn = Shader.PropertyToID("_TurnOn");

        private SpriteRenderer rend;

        private TileStatus status;

        private bool isHovered;

        private bool isDirty;

        public void Hover()
        {
            this.isHovered = true;
            this.isDirty = true;
        }

        public void Unhover()
        {
            this.isHovered = false;
            this.isDirty = true;
        }

        private void OnEnable()
        {
            this.rend = this.GetComponent<SpriteRenderer>();
            this.status = this.GetComponent<TileStatus>();
        }

        private void Update()
        {
            if (this.isDirty)
            {
                this.SetHighLight(this.isHovered);
            }
        }

        private void SetHighLight(bool on)
        {
            // TMP Fast FIX
            if (this.status.GetTileType() == Tiles.HighGround)
            {
                var trans = this.transform;

                Assert.AreNotEqual(trans.childCount, 0);

                var child = trans.GetChild(0);
                child.GetComponent<SpriteRenderer>().material.SetFloat(
                    TurnOn, 
                    on ? 1.0f : 0.0f);
            }
            else
            {
                this.rend.material.SetFloat(
                    TurnOn,
                    on ? 1.0f : 0.0f);
            }
        }
    }
}