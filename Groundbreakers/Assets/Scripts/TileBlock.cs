namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using UnityEngine;

    using Random = UnityEngine.Random;

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TileBlock : MonoBehaviour, IBattlePhaseHandler
    {
        // 8 * 8 = 64
        private const uint TotalBlocks = GameMap.Dimension * GameMap.Dimension; 

        private static uint blocksReady = 0;

        #region Inspector Variables

        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float enterDuration = 2.5f;

        #endregion

        #region Internal Variables

        private const float TempOffset = 10.0f;

        private Rigidbody2D rb2D;

        private SpriteRenderer sprite;

        private Vector3 originalPosition;

        private bool stabled = false;

        #endregion

        #region Unity Callbacks

        public void OnEnable()
        {
            // Setup event listener
            BattleManager.StartListening("test", this.OnTilesEntering);

            // Initialize field
            this.rb2D = this.GetComponent<Rigidbody2D>();
            this.rb2D.gravityScale = 0f;

            this.sprite = this.GetComponent<SpriteRenderer>();
        }

        public void OnDisable()
        {
            BattleManager.StopListening("test", this.OnTilesEntering);
        }

        public void Start()
        {
            // Saving starting position
            this.originalPosition = this.transform.position;

            this.transform.SetPositionAndRotation(
                new Vector3(this.originalPosition.x, this.originalPosition.y - TempOffset),
                Quaternion.identity);
        }

        public void FixedUpdate()
        {
            if (!this.stabled && this.CheckTileReachDestination())
            {
                this.stabled = true;
                blocksReady++;

                // Check if all tiles are ready and emit event.
                if (blocksReady == TotalBlocks)
                {
                    Debug.Log("All block ready");
                    BattleManager.TriggerEvent("block ready");
                }
            }
        }

        #endregion

        #region IBattlePhaseHandler

        public void OnTilesEntering()
        {
            this.StartCoroutine(this.StartDropping());
        }

        public void OnBattling()
        {
            throw new System.NotImplementedException();
        }

        public void OnTilesExiting()
        {
            // var rate = Random.Range(0.1f, 1.0f);
            this.StartCoroutine(this.SelfDestroy());
        }

        #endregion

        #region Internal Functions

        private bool CheckTileReachDestination()
        {
            var delta = Mathf.Abs(this.gameObject.transform.position.y - this.originalPosition.y);

            // TODO: add bouncing effect
            if (delta < 0.1f)
            {
                this.rb2D.gravityScale = 0f;
                this.rb2D.velocity = Vector3.zero;
                this.transform.SetPositionAndRotation(this.originalPosition, Quaternion.identity);

                return true;
            }

            return false;
        }

        private IEnumerator StartDropping()
        {
            var time = Random.Range(0.0f, this.enterDuration);
            var rate = 0.4f;

            yield return new WaitForSeconds(time);

            this.rb2D.gravityScale = -0.4f;

            // Handle fade in
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * rate)
            {
                var alpha = Mathf.SmoothStep(0.0f, 1.0f, i);

                var originColor = this.sprite.color;
                this.sprite.color = new Color(originColor.r, originColor.g, originColor.b, alpha);

                yield return null;
            }
        }

        private IEnumerator SelfDestroy(float rate = 0.4f)
        {
            var time = Random.Range(0.0f, this.enterDuration);

            yield return new WaitForSeconds(time);

            this.rb2D.gravityScale = -0.4f;

            // Handle fade out
            for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime * rate)
            {
                var alpha = Mathf.SmoothStep(0.0f, 1.0f, i);

                this.sprite.color = new Color(1.0f, 1.0f, 1.0f, alpha);

                yield return null;
            }

            Destroy(this.gameObject);
        }

        #endregion
    }
}