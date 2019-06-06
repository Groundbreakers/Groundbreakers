using UnityEngine;
using Assets.Scripts;

public class lootDrop : Loot
{

    private float distance;

    private bool reachedTop;

    private float spawnPointY;
    

    /// <summary>
    /// PlaceHolder for loot drop animation
    /// </summary>
    private void dropLoot() {

         if (this.reachedTop == false && this.distance <= 0.5f)
         {
              this.transform.Translate(Vector2.up * 2 * Time.deltaTime, Space.World);
              this.distance = this.transform.position.y - this.spawnPointY;

              if (this.distance >= 0.5f)
              {
                  this.reachedTop = true;
              }
         }
         else if (this.reachedTop == true && this.distance >= 0f)
         {
             this.transform.Translate(Vector2.down * 2 * Time.deltaTime, Space.World);
             this.distance = this.transform.position.y - this.spawnPointY;
         }
          
        //this.transform.Translate(Random.insideUnitCircle.normalized * 2 * Time.deltaTime, Space.World);

    }


    protected override void _OnMouseClick()
    {
        int layerMask = (LayerMask.GetMask("loot"));

        if (Input.GetMouseButtonDown(0) && GameObject.Find("SwapButton").GetComponent<ButtonPressed>().Pressed != true)
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 mousePos = GetWorldPositionOnPlane(mouseScreenPos, Input.mousePosition.z);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, layerMask);

            if (hit.collider != null)
            {
                int lootValue = hit.collider.gameObject.GetComponent<lootDrop>().lootValue;
                Destroy(hit.collider.gameObject);
                this.CrystalCounter.SetCrystals(lootValue);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        this.spawnPointY = this.transform.position.y;
        this.distance = this.transform.position.y - this.spawnPointY;
        this.reachedTop = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        this.dropLoot();
        this.freeLoot();
    }
}
