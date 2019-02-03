using System;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.EventSystems;

public class SelectNode : MonoBehaviour
{
    #region Inspector values

    public Color hoverColor;

    public Color errorColor;

    #endregion

    public int characterOnTop;

    #region Internal fields

    private Color startColor;

    private SpriteRenderer rend;

    /// <summary>
    /// Making this canvas a field then we don't need to call the expensive function
    /// 'GameObject.Find' every time.
    /// </summary>
    private GameObject canvas;

    private bool canDeploy = true;

    #endregion

    #region Public Functions

    public void SetCanDeploy(bool value)
    {
        this.canDeploy = value;
    }

    #endregion

    #region Unity Callbacks

    void Start()
    {
        this.rend = this.GetComponent<SpriteRenderer>();
        this.startColor = this.rend.color;
        this.canvas = GameObject.Find("Canvas");
        this.characterOnTop = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Deploy deploy = this.canvas.GetComponent<Deploy>();
            Status status = this.canvas.GetComponent<Status>();
            deploy.Close();
            status.Close();
        }
    }
    
    void OnMouseOver()
    {
        // Clearly, do nothing when the battleManager is not in the battle state
        if (BattleManager.GameState != BattleManager.Stages.Combating)
        {
            return;
        }

        if (!this.canDeploy)
        {
            this.rend.color = this.errorColor;
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        this.rend.color = this.hoverColor;
        this.MouseInput();
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonUp(0) && this.characterOnTop == 0)
        {
            Deploy deploy = this.canvas.GetComponent<Deploy>();
            deploy.Toggle(this.gameObject);
        }
        else if (Input.GetMouseButtonUp(0) && this.characterOnTop != 0)
        {
            Status status = this.canvas.GetComponent<Status>();
            status.Toggle(this.gameObject);
        }
    }

    void OnMouseExit()
    {
        this.rend.color = this.startColor;
    }

    #endregion
}
