namespace Assets.Scripts
{
    using UnityEngine;
    using Random = UnityEngine.Random;

    [RequireComponent(typeof(SpriteRenderer))]
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] sprites;

        private void OnEnable()
        {
            var sprite = this.GetComponent<SpriteRenderer>();
            sprite.sprite = this.PickRandomSprite();
        }

        private Sprite PickRandomSprite()
        {
            return this.sprites[Random.Range(0, this.sprites.Length)];
        }
    }
}