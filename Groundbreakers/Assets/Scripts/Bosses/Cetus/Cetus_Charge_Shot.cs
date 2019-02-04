using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cetus_Charge_Shot : MonoBehaviour
{
    private Vector3 dir;
    public float shotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PickDirection();
    }

    void Update()
    {
        this.transform.Translate(this.dir.normalized * this.shotSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward, 720 * Time.deltaTime);
    }

    private void PickDirection()
    {
        int diceroll = Random.Range(0, 3);
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

    // Stun characters for 3 seconds on collision.
    void OnTriggerEnter2D(Collider2D hitTarget)
    {
        if (hitTarget.gameObject.tag == "Player")
        {
            //hitTarget.gameObject.GetComponent<characterAttack>().StunCharacter(3);
        }
    }

    // Destroy after going offscreen, and do damage
    void OnBecameInvisible()
    {
        GameObject canvas = GameObject.Find("Canvas");
        HP hp = canvas.GetComponent<HP>();
        Debug.Log(3);
        hp.healthPoint -= 3;
        Destroy(gameObject);
    }
}
