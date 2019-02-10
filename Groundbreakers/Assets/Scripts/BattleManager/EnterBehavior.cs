namespace Assets.Scripts
{
    using DG.Tweening;
    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <summary>
    ///     This component handles tile enter/exiting animation. Attaching this component to object
    ///     that you wish to falling and fade. We are using DOTween to do the job for us, and it
    ///     is very cool.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnterBehavior : MonoBehaviour, IBattlePhaseHandler
    {
        #region Inspector Values

        /// <summary>
        /// The offset: it is the position of where you would like the object to travel from.
        /// </summary>
        [SerializeField]
        private Vector3 offset = new Vector3(0.0f, -4.0f);

        /// <summary>
        /// The duration of object traveling in seconds.
        /// </summary>
        [SerializeField]
        [Range(0.5f, 5.0f)]
        private float duration = 1.75f;

        /// <summary>
        /// The maximum random delay allowed in seconds.
        /// </summary>
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float maxDelay = 2.0f;

        #endregion

        #region Internal Fields

        private SpriteRenderer sprite;

        private Vector3 originalPos;

        private Vector3 targetPos;

        #endregion

        #region IBattlePhaseHandler

        public void OnTilesEntering()
        {
            this.StartEntering();
        }

        public void OnBattling()
        {
            throw new System.NotImplementedException();
        }

        public void OnTilesExiting()
        {
            this.StartExiting();
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            // Init transform
            this.originalPos = this.transform.position;
            this.transform.Translate(this.offset);
            this.targetPos = this.originalPos - this.offset;

            // Init sprite 
            this.sprite = this.GetComponent<SpriteRenderer>();
            this.sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        private void Start()
        {
            this.StartEntering();
        }

        private void Update()
        {
            if (Input.GetKeyDown("f"))
            {
                this.StartExiting();
            }
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Let DOTween handle the entering animation(i.e. transform and fade in).
        /// </summary>
        private void StartEntering()
        {
            var delay = Random.Range(0.0f, this.maxDelay);

            this.transform.DOMove(this.originalPos, this.duration)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);

            this.sprite.DOFade(1.0f, this.duration)
                .SetEase(Ease.OutExpo)
                .SetDelay(delay);
        }

        /// <summary>
        /// Let DOTween handle the exiting animation(i.e. transform and fade out).
        /// </summary>
        private void StartExiting()
        {
            var delay = Random.Range(0.0f, this.maxDelay);

            this.transform.DOMove(this.targetPos, this.duration)
                .SetEase(Ease.InBack)
                .SetDelay(delay);

            this.sprite.DOFade(0.0f, this.duration)
                .SetEase(Ease.InExpo)
                .SetDelay(delay);
        }

        #endregion
    }
}