namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.Events;

    public class ShakeBehavior : MonoBehaviour
    {
        [SerializeField]
        private float dampingSpeed = 1.0f;

        private Vector3 initialPosition;

        [SerializeField]
        private UnityEvent invokeMethod;

        [SerializeField]
        private float shakeDuration;

        [SerializeField]
        private float shakeMagnitude = 0.7f;

        public void Awake()
        {
            this.invokeMethod.Invoke();
        }

        public void FixedUpdate()
        {
            if (this.shakeDuration > 0)
            {
                this.transform.localPosition = this.initialPosition + Random.insideUnitSphere * this.shakeMagnitude;

                this.shakeDuration -= Time.deltaTime * this.dampingSpeed;
            }
            else
            {
                this.shakeDuration = 0f;
                this.transform.localPosition = this.initialPosition;
            }
        }

        public void OnEnable()
        {
            this.initialPosition = this.transform.localPosition;
        }

        public void TriggerShake()
        {
            this.shakeDuration = 2.0f;
        }
    }
}