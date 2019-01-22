namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class TileBlock : MonoBehaviour, IBattlePhaseHandler
    {
        #region Inspector Variables

        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float enterDuration = 2.5f;

        #endregion

        #region Internal Variables

        private const float TempOffset = 7.0f;

        private Rigidbody2D rb2D;

        private Vector3 originalPosition;

        #endregion

        #region Unity Callbacks

        public void OnEnable()
        {
            this.rb2D = this.GetComponent<Rigidbody2D>();
            this.rb2D.gravityScale = 0f;
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
            var delta = Mathf.Abs(this.gameObject.transform.position.y - this.originalPosition.y);

            // TODO: add bouncing effect
            if (delta < 0.1f)
            {
                this.rb2D.gravityScale = 0f;
                this.rb2D.velocity = Vector3.zero;
                this.transform.SetPositionAndRotation(this.originalPosition, Quaternion.identity);
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
            throw new System.NotImplementedException();
        }

        #endregion

        #region Internal Functions

        private IEnumerator StartDropping()
        {
            var time = Random.Range(0.0f, this.enterDuration);

            yield return new WaitForSeconds(time);

            this.rb2D.gravityScale = -0.3f;
        }

        #endregion
    }
}