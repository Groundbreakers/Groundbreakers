using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class lootDrop : MonoBehaviour
{
    
    private float spawnPointY;
    private float distance ;
    private bool reachedTop;
    private CrystalCounter CrystalCounter;
    private HP HP;




    // Start is called before the first frame update
    void Start()
    {
        this.spawnPointY = this.transform.position.y;
        distance = this.transform.position.y - spawnPointY;
        reachedTop = false;
        CrystalCounter = GameObject.Find("CrystalCounter").GetComponent<CrystalCounter>();
        HP = GameObject.Find("HPCounter").GetComponent<HP>();

    }

    // Update is called once per frame
    void Update() {

        dropLoot();
        freeLoot();

        int layerMask = (LayerMask.GetMask("loot"));
    
       // Debug.Log(layerMask);

        if (Input.GetMouseButtonDown(0))
        {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 45));
                Vector2 mousePos2D = new Vector2(mousePos.x , mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, layerMask);
               // Debug.DrawRay(mousePos2D, new Vector2(0,1000), Color.red,1000);

          //  if(hit == null) Debug.Log("zz");
          //  else Debug.Log("jj");

              
            if (hit.collider != null)
                {
               Destroy(hit.collider.gameObject);
               CrystalCounter.SetCrystals(10000);




            }
           



        }
         
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

    private void freeLoot() {
        if (this.HP.healthPoint <= 0)
        {

            Destroy(this.gameObject);
        }
    }



}
