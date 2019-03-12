using UnityEngine;

public class Cetus_Charge_Shot : MonoBehaviour
{
    public float shotSpeed;

    private Vector3 dir;

    // Destroy after going offscreen, and do damage
    private void OnBecameInvisible()
    {
        var canvas = GameObject.Find("HPCounter");
        var hp = canvas.GetComponent<HP>();
        Debug.Log(3);
        hp.UpdateHealth(-3);
        Destroy(this.gameObject);
    }

    // Stun characters for 3 seconds on collision.
    private void OnTriggerEnter2D(Collider2D hitTarget)
    {
        if (hitTarget.gameObject.tag == "Player")
        {
            // hitTarget.gameObject.GetComponent<characterAttack>().StunCharacter(3);
        }
    }

    private void PickDirection()
    {
        var diceroll = Random.Range(0, 3);
        if (diceroll == 0)
        {
            this.dir = Vector3.right;
        }
        else if (diceroll == 1)
        {
            this.dir = Vector3.down;
        }
        else
        {
            this.dir = Vector3.left;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.PickDirection();
    }

    private void Update()
    {
        this.transform.Translate(this.dir.normalized * this.shotSpeed * Time.deltaTime, Space.World);
        this.transform.Rotate(Vector3.forward, 720 * Time.deltaTime);
    }
}