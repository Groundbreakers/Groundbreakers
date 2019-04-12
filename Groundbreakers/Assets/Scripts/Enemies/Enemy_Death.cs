using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Animator anim;
    // private lootDrop lootDrop;
    public GameObject loot;
 
    // Start is called before the first frame update
    void Start()
    {
       // lootDrop = this.lootDrop.GetComponent<lootDrop>();
    }


    public void setDirection(int i)
    {
        anim.SetInteger("Direction", i);
        createLoot();
    }

    public void createLoot()
    {

        GameObject temp = (GameObject) Instantiate(loot, transform.position, transform.rotation);
       

    }


}
