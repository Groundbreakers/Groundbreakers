using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject selectCircle;
    public GameObject icon1;
    public GameObject icon2;
    public GameObject icon3;
    public GameObject icon4;
    public GameObject icon5;
    private int characterIndex;

    private float rotateSpeed = 10.0F;

    // Update is called once per frame
    void Update()
    {
        this.selectCircle.transform.Rotate(0.0F, 0.0F, -Time.deltaTime * this.rotateSpeed);
    }

    public void SelectC1()
    {
        this.characterIndex = 0;
        this.selectCircle.transform.position = this.icon1.transform.position;
    }

    public void SelectC2()
    {
        this.characterIndex = 1;
        this.selectCircle.transform.position = this.icon2.transform.position;
    }

    public void SelectC3()
    {
        this.characterIndex = 2;
        this.selectCircle.transform.position = this.icon3.transform.position;
    }

    public void SelectC4()
    {
        this.characterIndex = 3;
        this.selectCircle.transform.position = this.icon4.transform.position;
    }

    public void SelectC5()
    {
        this.characterIndex = 4;
        this.selectCircle.transform.position = this.icon5.transform.position;
    }

    public int GetCharacterIndex()
    {
        return this.characterIndex;
    }
}
