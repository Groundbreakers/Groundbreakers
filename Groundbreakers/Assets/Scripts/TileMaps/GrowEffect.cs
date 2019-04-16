namespace TileMaps
{
    using DG.Tweening;

    using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
    public class GrowEffect : MonoBehaviour
    {
        private SpriteRenderer sprite;

        private void OnDisable()
        {
            Destroy(this);
        }

        private void OnEnable()
        {
            var delay = Random.Range(0.0f, 2.0f);

            this.sprite = this.GetComponent<SpriteRenderer>();
            this.sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            this.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);

            // Fade
            this.sprite.DOFade(1.0f, 0.5f).SetEase(Ease.InOutSine).SetDelay(delay);

            this.transform.DOScaleY(1.0f, 0.5f).SetEase(Ease.InOutSine).SetDelay(delay);
        }
    }
}