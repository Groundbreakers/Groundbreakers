namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class TileBlock : MonoBehaviour
    {
        #region Internal Variables

        private const float TempOffset = 7.0f;

        private Rigidbody2D rb2D;

        private Vector3 originalPosition;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.rb2D = this.GetComponent<Rigidbody2D>();
            this.rb2D.gravityScale = 0f;
        }

        private void Start()
        {
            // Saving starting position
            this.originalPosition = this.transform.position;

            this.transform.SetPositionAndRotation(
                new Vector3(this.originalPosition.x, this.originalPosition.y - TempOffset),
                Quaternion.identity);

            this.StartCoroutine(this.StartDropping());
        }

        private void FixedUpdate()
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

        #region Internal Functions

        private IEnumerator StartDropping()
        {
            var time = Random.Range(0.0f, 5.0f);

            yield return new WaitForSeconds(time);

            this.rb2D.gravityScale = -0.3f;
        }

        #endregion
    }
}