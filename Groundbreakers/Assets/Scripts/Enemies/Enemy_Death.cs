using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Animator anim;

    public GameObject loot;

    private float lootDrop_probability = 15f;

    public void setDirection(int i) {
        anim.SetInteger("Direction", i);
        createLoot(); // drop loot
    }

    private void createLoot() {

        float probability = Random.Range(0f, 100f);

        if (probability <= lootDrop_probability)
        {
            GameObject temp = (GameObject)Instantiate(loot, transform.position, transform.rotation);
            //temp.transform.parent = GameObject.Find("Loot").transform;
        }
    }
}
