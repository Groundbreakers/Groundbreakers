namespace Assets.Scripts
{
    using Sirenix.OdinInspector;

    using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField]
        [AssetList]
        [PreviewField(70, ObjectFieldAlignment.Center)]
        private Sprite[] assets;

        private void OnEnable()
        {
            this.PickRandomSprite();
        }

        [Button("Get New")]
        private void PickRandomSprite()
        {
            if (this.assets.Length == 0)
            {
                Debug.LogError("Attempt to PickRandomSprite with no assets");
                return;
            }

            var sprite = this.GetComponent<SpriteRenderer>();
            sprite.sprite = this.assets[Random.Range(0, this.assets.Length)];
        }
    }
}