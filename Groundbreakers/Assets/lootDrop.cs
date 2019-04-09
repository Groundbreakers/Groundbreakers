using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lootDrop : MonoBehaviour
{
    
    private float spawnPointY;
    private float distance ;
    private bool reachedTop;
    private CrystalCounter CrystalCounter;
    public GameObject loot;

    // Start is called before the first frame update
    void Start()
    {
        this.spawnPointY = this.transform.position.y;
        distance = this.transform.position.y - spawnPointY;
        reachedTop = false;
        CrystalCounter = GameObject.Find("CrystalCounter").GetComponent<CrystalCounter>();
    }

    // Update is called once per frame
    void Update() {
        dropLoot();
    }

    private void dropLoot() {

       
        if (this.reachedTop == false && this.distance <= 0.5f)
        {
            this.transform.Translate(Vector2.up * 2 * Time.deltaTime, Space.World);
            distance = this.transform.position.y - spawnPointY;

            if (this.distance >= 0.5f)
            {
                this.reachedTop = true;
            }

        }else if (this.reachedTop == true && this.distance >= 0f)
        {
            this.transform.Translate(Vector2.down * 2 * Time.deltaTime, Space.World);
            distance = this.transform.position.y - spawnPointY;

        }



    }

    void OnMouseDown()
    {
       this.CrystalCounter.SetCrystals(100);
       Destroy(gameObject);
       
    }

    // drop loot when an enemy is killed
    public void createLoot()
    {

        GameObject temp = Instantiate(loot, transform.position, transform.rotation);

    }

}
