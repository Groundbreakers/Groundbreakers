using System.Linq;

using Assets.Enemies.Scripts;

using TileMaps;

using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Blockade))]
public class Booooom : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float boooomRadius = 2.0f;

    [SerializeField]
    [Range(0.0f, 200.0f)]
    private float boooomDamage = 100.0f;

    private Blockade blockade;

    // Start is called before the first frame update
    private void OnEnable()
    {
        this.blockade = this.GetComponent<Blockade>();

        Assert.IsNotNull(this.blockade);
    }

    private void OnDisable()
    {
        // Nothing happen if not dead
        if (this.blockade && this.blockade.GetHitPoint() > 0.0f)
        {
            return;
        }

        Debug.Log($"BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM{this.blockade.GetHitPoint()}");


        // Perform range explosion
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        var pos = this.transform.position;

        var targets = enemies.Where(
            e => Vector3.Distance(pos, e.transform.position) <= this.boooomRadius);

        foreach (var target in targets)
        {
            target.GetComponent<Enemy_Generic>().DamageEnemy(
                (int)this.boooomDamage, 
                0, 
                1.0f, 
                false, 
                false);
        }
    }
}