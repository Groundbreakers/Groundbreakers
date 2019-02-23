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
    public int slot;
    public String description;

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
    public Boolean cleaveAE;
    public Boolean whirwindAE;
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
    private GameObject tooltip;
    private Transform parent;

    private Boolean isEquipped;

    public Boolean isDragable = true;

    // Start is called before the first frame update
    void Start()
    {
        this.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        this.characterManager = GameObject.Find("CharactersPanel").GetComponent<CharacterManager>();
        this.inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        this.tooltip = this.transform.GetChild(0).gameObject;
        this.button.onClick.AddListener(this.HandleTooltip);
        this.parent = this.transform.parent;
    }

    public void HandleTooltip()
    {
        // Reference the character tooltip
        GameObject tooltipTitle = this.tooltip.transform.GetChild(0).gameObject;
        Text titleText = tooltipTitle.GetComponent<Text>();
        titleText.text = this.title;

        // Rarity affects text color
        switch (this.rarity)
        {
            case 0:
                titleText.color = Color.black;
                break;
            case 1:
                titleText.color = new Color(0.0f, 0.4f, 1.0f);
                break;
            case 2:
                titleText.color = new Color(1.0f, 0.5f, 0.0f);
                break;
            default:
                titleText.color = Color.black;
                break;
        }

        // Update the descriptionText
        GameObject tooltipDescription = this.tooltip.transform.GetChild(1).gameObject;
        Text descriptionText = tooltipDescription.GetComponent<Text>();
        descriptionText.text = this.description;

        this.tooltip.SetActive(!this.tooltip.activeSelf);
    }

    public int[] GetModuleAttributes()
    {
        int[] attributes = { this.POW, this.ROF, this.RNG, this.MOB, this.AMP,
                               Convert.ToInt32(this.burstAE),
                               Convert.ToInt32(this.ricochetAE),
                               Convert.ToInt32(this.laserAE),
                               Convert.ToInt32(this.splashAE),
                               Convert.ToInt32(this.pierceAE),
                               Convert.ToInt32(this.traceAE),
                               Convert.ToInt32(this.cleaveAE),
                               Convert.ToInt32(this.whirwindAE),
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
