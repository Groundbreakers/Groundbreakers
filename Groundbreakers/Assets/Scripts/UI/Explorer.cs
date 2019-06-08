using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explorer : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController scholar;
    public RuntimeAnimatorController trickster;
    public RuntimeAnimatorController gladiator;
    public RuntimeAnimatorController scavenger;
    public RuntimeAnimatorController agent;
    public Text text;

    public int profession;

    public GameObject popup;

    // Start is called before the first frame update
    void Start()
    {
        this.profession = Random.Range(0, 5);
        this.UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100.0f);
        this.popup.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void LeftButton()
    {
        this.profession--;
        this.UpdateSprite();
    }

    public void RightButton()
    {
        this.profession++;
        this.UpdateSprite();
    }

    public void UpdateSprite()
    {
        this.profession = this.mod(this.profession, 5);
        switch (this.profession)
        {
            case 0:
                this.animator.runtimeAnimatorController = this.scholar;
                this.text.text = "Scholar";
                break;
            case 1:
                this.animator.runtimeAnimatorController = this.trickster;
                this.text.text = "Trickster";
                break;
            case 2:
                this.animator.runtimeAnimatorController = this.gladiator;
                this.text.text = "Gladiator";
                break;
            case 3:
                this.animator.runtimeAnimatorController = this.scavenger;
                this.text.text = "Scavenger";
                break;
            case 4:
                this.animator.runtimeAnimatorController = this.agent;
                this.text.text = "Agent";
                break;
        }
    }

    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void ShowPopup()
    {
        this.popup.SetActive(true);
    }

    public void HidePopup()
    {
        this.popup.SetActive(false);
    }
}
