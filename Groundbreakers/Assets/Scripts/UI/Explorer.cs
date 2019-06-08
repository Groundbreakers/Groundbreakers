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
    public Text description;

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
                this.description.text = "Ranged.\nHigh damage, good against multiple enemies but bad against armored enemies.";
                break;
            case 1:
                this.animator.runtimeAnimatorController = this.trickster;
                this.text.text = "Trickster";
                this.description.text = "Ranged.\nModerate damage, single target focused and can slow enemies at level 5.";
                break;
            case 2:
                this.animator.runtimeAnimatorController = this.gladiator;
                this.text.text = "Gladiator";
                this.description.text = "Melee.\nModerate damage, good at crowd control and against armored enemies.";
                break;
            case 3:
                this.animator.runtimeAnimatorController = this.scavenger;
                this.text.text = "Scavenger";
                this.description.text = "Melee.\nModerate damage, good against multiple and armored enemies and bosses.";
                break;
            case 4:
                this.animator.runtimeAnimatorController = this.agent;
                this.text.text = "Agent";
                this.description.text = "Ranged.\nHigh damage, amazing against multiple enemies but bad against armored enemies.";
                break;
        }
    }

    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
