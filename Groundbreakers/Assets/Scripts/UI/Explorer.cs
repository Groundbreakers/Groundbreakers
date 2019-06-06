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

    // Start is called before the first frame update
    void Start()
    {
        this.animator.runtimeAnimatorController = this.scholar;
        this.profession = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        this.profession = this.mod(this.profession, 4);
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
        }
    }

    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
