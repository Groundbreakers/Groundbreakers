using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeployBar : MonoBehaviour
{
    public Image deployBar;
    public Image background;
    public characterAttributes character;
    private float max;
    private float current;

    private float DEFAULT_DEPLOY_TIME = 2.0f;

    void Start()
    {
        this.Reset();
    }

    public void Reset()
    {
        this.deployBar = this.GetComponent<Image>();
        this.background = this.transform.parent.gameObject.GetComponent<Image>();
        this.character = this.transform.parent.parent.parent.gameObject.GetComponent<characterAttributes>();
        this.deployBar.color = new Color(this.deployBar.color.r, this.deployBar.color.g, this.deployBar.color.b, 1f);
        this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 1f);

        this.current = 0;
        this.max = this.DEFAULT_DEPLOY_TIME / (this.character.MOB * .5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.current <= this.max)
        {
            this.current += Time.deltaTime;
            this.deployBar.fillAmount = this.current / this.max;
        }
        else
        {
            this.Hide();
        }
    }

    public void Hide()
    {
        this.deployBar.color = new Color(this.deployBar.color.r, this.deployBar.color.g, this.deployBar.color.b, 0f);
        this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 0f);
    }
}
