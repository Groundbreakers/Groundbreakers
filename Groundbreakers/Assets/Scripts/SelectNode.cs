// Currently supports 2 characters, can always add more

using System;

using UnityEngine;
using UnityEngine.EventSystems;

public class SelectNode : MonoBehaviour
{
    public Color hoverColor;

    private SpriteRenderer rend;

    private Color startColor;

    public int characterOnTop;

    void Start()
    {
        this.rend = GetComponent<SpriteRenderer>();
        this.startColor = rend.color;

        this.characterOnTop = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            GameObject canvas = GameObject.Find("Canvas");
            Deploy deploy = canvas.GetComponent<Deploy>();
            Status status = canvas.GetComponent<Status>();
            deploy.Close();
            status.Close();
        }
    }
    
    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
            this.rend.color = this.hoverColor;
            this.MouseInput();
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonUp(0) && this.characterOnTop == 0)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Deploy deploy = canvas.GetComponent<Deploy>();
            deploy.Toggle(this.gameObject);
        }
        else if (Input.GetMouseButtonUp(0) && this.characterOnTop != 0)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Status status = canvas.GetComponent<Status>();
            status.Toggle(this.gameObject);
        }
    }

    void OnMouseExit()
    {
        this.rend.color = this.startColor;
    }
}
