namespace Assets.Scripts
{
    using Assets.Script;

    using DG.Tweening;

    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    public class Destructible : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 10)]
        private int durability;

        private void OnDisable()
        {
            Debug.Log("OnDisable");
            var parent = this.transform.parent.gameObject;

            BattleUtility.SetTileCanDeploy(parent, true);
        }

        private void OnEnable()
        {
            var parent = this.transform.parent.gameObject;
            BattleUtility.SetTileCanDeploy(parent, false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                this.durability--;
                Destroy(other.gameObject);

                if (this.durability <= 0)
                {
                    Destroy(this.gameObject);
                }

                this.transform.DOShakePosition(1.0f, 0.3f);
            }
        }
    }
}