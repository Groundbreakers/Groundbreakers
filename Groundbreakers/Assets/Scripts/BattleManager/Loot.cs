using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

/// <summary>
/// Constructor for loots
/// </summary>

public class Loot : MonoBehaviour
{
    public int lootValue;
    protected CrystalCounter CrystalCounter;
    protected HP HP;


    void Awake()
    {
        this.CrystalCounter = GameObject.Find("CrystalCounter").GetComponent<CrystalCounter>();
        this.HP = GameObject.Find("HPCounter").GetComponent<HP>();
    }
    

    /// <summary>
    /// Alternative for built in MouseOnDown()
    /// </summary>
    protected virtual void _OnMouseClick()
    {
       if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 mousePos = GetWorldPositionOnPlane(mouseScreenPos, Input.mousePosition.z);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                int lootValue = hit.collider.gameObject.GetComponent<lootDrop>().lootValue;
                Destroy(hit.collider.gameObject);
                this.CrystalCounter.SetCrystals(lootValue);
            }
        }
    }
   
 

    /// <summary>
    /// Free all the loots when GameOver or Exiting stage
    /// </summary>
    protected void freeLoot()
    {
        if (HP.healthPoint <= 0 || BattleManager.GameState == GameStates.Exiting)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Convert screen position to world position (perspective main camera)
    /// </summary>
    ///
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    void Update()
    {
        if (GameObject.Find("loot(Clone)") != null)
        {
            lootDrop lootDrop = GameObject.Find("loot(Clone)").GetComponent<lootDrop>();
            lootDrop._OnMouseClick();
        }
    }

}
