namespace Assets.Scripts
{
    using DG.Tweening;

    using UnityEngine;

    /// <summary>
    ///     This component handles tile enter/exiting animation. Attaching this component to object
    ///     that you wish to falling and fade.
    /// </summary>
    public class EnterBehavior : MonoBehaviour
    {
        #region Inspector Values

        [SerializeField]
        private Vector3 offset;

        [SerializeField]
        [Range(0.5f, 5.0f)]
        private float enterDuration;

        #endregion

        #region Internal Fields

        private SpriteRenderer sprite;

        private Vector3 originalPosition;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            // Init transform
            this.originalPosition = this.transform.position;
            this.transform.Translate(this.offset);

            // Init sprite 
            this.sprite = this.GetComponent<SpriteRenderer>();
            this.sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        private void Start()
        {
            // Let DOTween handle the animation.
            this.transform.DOMove(this.originalPosition, this.enterDuration)
                    .SetEase(Ease.OutBack);

            this.sprite.DOFade(1.0f, this.enterDuration)
                    .SetEase(Ease.OutExpo);
        }

        #endregion
    }
}