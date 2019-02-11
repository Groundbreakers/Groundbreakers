namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.Events;

    public class ShakeBehavior : MonoBehaviour
    {
        #region Inspector Field

        [SerializeField]
        private float shakeDuration = 0f;

        [SerializeField]
        private float shakeMagnitude = 0.7f;

        [SerializeField]
        private float dampingSpeed = 1.0f;

        [SerializeField]
        private UnityEvent invokeMethod;

        #endregion  

        private Vector3 initialPosition;

        public void TriggerShake()
        {
            this.shakeDuration = 2.0f;
        }

        #region Unity Callbacks

        public void Awake()
        {
            this.invokeMethod.Invoke();
        }

        public void OnEnable()
        {
            this.initialPosition = this.transform.localPosition;
        }

        public void FixedUpdate()
        {
            if (this.shakeDuration > 0)
            {
                this.transform.localPosition = this.initialPosition
                                               + (Random.insideUnitSphere * this.shakeMagnitude);

                this.shakeDuration -= Time.deltaTime * this.dampingSpeed;
            }
            else
            {
                this.shakeDuration = 0f;
                this.transform.localPosition = this.initialPosition;
            }
        }

        #endregion
    }
}