using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleGeneric : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Title and description
    public Button button;
    public string title;
    public int rarity;
    public String[] description = new string[4];
    public Text titleText;
    public Text rarityText;
    public Text[] descriptionText = new Text[4];

    // Basic Attributes
    public int POW;
    public int ROF;
    public int RNG;
    public int MOB;
    public int AMP;

    // Attack Effects
    public Boolean burstAE;
    public Boolean ricochetAE;
    public Boolean laserAE;
    public Boolean splashAE;
    public Boolean pierceAE;
    public Boolean traceAE;
    public Boolean whirlwindAE;
    public Boolean reachAE;

    // Status Effects
    public Boolean slowSE;
    public Boolean stunSE;
    public Boolean burnSE;
    public Boolean markSE;
    public Boolean purgeSE;
    public Boolean breakSE;
    public Boolean blightSE;
    public Boolean netSE;

    private Canvas canvas;
    private CharacterManager characterManager;
    private Inventory inventory;
    public GameObject tooltip;
    public Image icon;
    private Transform parent;

    private Boolean isEquipped;

    public Boolean isDragable = true;

    // Start is called before the first frame update
    void Start()
    {
        this.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        this.characterManager = Resources.FindObjectsOfTypeAll<CharacterManager>()[0];
        this.inventory = Resources.FindObjectsOfTypeAll<Inventory>()[0];

        // Update the title
        this.titleText.text = this.title;

        // Update the rarity text
        switch (this.rarity)
        {
            case 0:
                this.rarityText.text = "Common";
                this.rarityText.color = Color.black;
                break;
            case 1:
                this.rarityText.text = "Modified";
                this.rarityText.color = new Color(0.0f, 0.4f, 1.0f);
                break;
            case 2:
                this.rarityText.text = "Ideal";
                this.rarityText.color = new Color(0.5f, 0.0f, 0.5f);
                break;
            case 3:
                this.rarityText.text = "Groundbreaking";
                this.rarityText.color = new Color(1.0f, 0.5f, 0.0f);
                break;
            default:
                this.rarityText.color = Color.black;
                break;
        }

        // Update the descriptionText 1-4
        this.descriptionText[0].text = this.description[0];
        if (this.description[1] != "")
        {
            this.descriptionText[1].text = this.description[1];
            this.descriptionText[1].gameObject.SetActive(true);
        }
        if (this.description[2] != "")
        {
            this.descriptionText[2].text = this.description[2];
            this.descriptionText[2].gameObject.SetActive(true);
        }
        if (this.description[3] != "")
        {
            this.descriptionText[3].text = this.description[3];
            this.descriptionText[3].gameObject.SetActive(true);
        }

        this.button.onClick.AddListener(this.ToggleTooltip);
        this.parent = this.transform.parent;
     }

    public void ToggleTooltip()
    {
        this.tooltip.SetActive(!this.tooltip.activeSelf);
    }

    public int[] GetModuleAttributes()
    {
        int[] attributes = 
            {
                               this.POW, this.ROF, this.RNG, this.MOB, this.AMP,
                               Convert.ToInt32(this.burstAE),
                               Convert.ToInt32(this.ricochetAE),
                               Convert.ToInt32(this.laserAE),
                               Convert.ToInt32(this.splashAE),
                               Convert.ToInt32(this.pierceAE),
                               Convert.ToInt32(this.traceAE),
                               Convert.ToInt32(this.whirlwindAE),
                               Convert.ToInt32(this.reachAE),

                               Convert.ToInt32(this.slowSE),
                               Convert.ToInt32(this.stunSE),
                               Convert.ToInt32(this.burnSE),
                               Convert.ToInt32(this.markSE),
                               Convert.ToInt32(this.purgeSE),
                               Convert.ToInt32(this.breakSE),
                               Convert.ToInt32(this.blightSE),
                               Convert.ToInt32(this.netSE)
            };
        return attributes;
    }

    public void NewParent(Transform transform)
    {
        this.tooltip.SetActive(false);
        this.transform.SetParent(transform);
        this.parent = transform;
        this.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        this.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        this.inventory.UpdateInventory();
        this.characterManager.UpdatePanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!this.isDragable)
        {
            eventData.pointerDrag = null;
        }
        else
        {
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
            this.transform.parent = this.canvas.transform;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform, Input.mousePosition, this.canvas.worldCamera, out pos);
        this.transform.position = this.canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.parent = this.parent;
        this.transform.localPosition = Vector3.zero;
    }
}
