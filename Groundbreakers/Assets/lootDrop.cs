using Assets.Scripts;
using UnityEngine;

public class lootDrop : MonoBehaviour
{
    public int lootValue;

    private CrystalCounter CrystalCounter;

    private float distance;

    private HP HP;

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
    }

    /// <summary>
    /// Free all the loots when GameOver or Exiting stage
    /// </summary>
    private void freeLoot() {
        if (this.HP.healthPoint <= 0 || BattleManager.GameState == GameStates.Exiting)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Alternative for built in MouseOnDown()
    /// </summary>
    private void OnMouseClick() {
        int layerMask = (LayerMask.GetMask("loot"));
      
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 mousePos = GetWorldPositionOnPlane(mouseScreenPos, Input.mousePosition.z);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, layerMask);

            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
                this.CrystalCounter.SetCrystals(this.lootValue);
            }
        }
    }

    /// <summary>
    /// Convert screen position to world position (perspective main camera)
    /// </summary>

    private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    // Start is called before the first frame update
    void Start() {
        this.CrystalCounter = GameObject.Find("CrystalCounter").GetComponent<CrystalCounter>();
        this.HP = GameObject.Find("HPCounter").GetComponent<HP>();

        this.spawnPointY = this.transform.position.y;
        this.distance = this.transform.position.y - this.spawnPointY;
        this.reachedTop = false;
    }

    // Update is called once per frame
    void Update() {
        this.OnMouseClick();
        this.dropLoot();
        this.freeLoot();
    }
    
}
