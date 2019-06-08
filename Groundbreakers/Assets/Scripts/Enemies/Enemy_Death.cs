using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Animator anim;

    public GameObject loot;

    private readonly int lootDropProbability = 15;

    private static readonly int Direction = Animator.StringToHash("Direction");

    public void setDirection(int i)
    {
        this.anim.SetInteger(Direction, i);
        this.createLoot(); // drop loot
    }

    private void createLoot()
    {
        var probability = Random.Range(0, 100);

        if (probability <= this.lootDropProbability)
        {
            var temp = Instantiate(this.loot, this.transform.position, this.transform.rotation);
        }
    }
}