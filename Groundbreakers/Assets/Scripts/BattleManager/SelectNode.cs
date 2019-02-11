using System;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TileBlock))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class SelectNode : MonoBehaviour
{
    public int characterOnTop;

    #region Internal fields

    /// <summary>
    /// The sprite renderer of the Child of this game object. (used for hover icon)
    /// </summary>
    private SpriteRenderer rend;

    /// <summary>
    /// Making this canvas a field then we don't need to call the expensive function
    /// 'GameObject.Find' every time.
    /// </summary>
    private GameObject canvas;

    /// <summary>
    /// Keeps a reference to the TileBlock script in this game object.
    /// </summary>
    private TileBlock tileBlock;

    private LineRenderer lineRenderer;

    private bool canDeploy = true;

    #endregion

    #region Public Functions

    public void SetCanDeploy(bool value)
    {
        this.canDeploy = value;
    }

    #endregion

    #region Unity Callbacks

    public void Start()
    {
        this.canvas = GameObject.Find("Canvas");
        this.tileBlock = this.GetComponent<TileBlock>();

        var components = this.GetComponentsInChildren<SpriteRenderer>();
        this.rend = components[1];

        this.characterOnTop = 0;
        this.rend.sprite = this.tileBlock.CanDeployIcon;
        this.rend.enabled = false;

        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.InitializeLineRenderer();
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Deploy deploy = this.canvas.GetComponent<Deploy>();
            Status status = this.canvas.GetComponent<Status>();
            deploy.Close();
            status.Close();
        }
    }

    private void FixedUpdate()
    {
        // well, just don't rotate if it is a cross
        if (!this.canDeploy)
        {
            return;
        }

        this.rend.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f));
    }

    public void OnMouseOver()
    {
        // Clearly, do nothing when the battleManager is not in the battle state
        if (BattleManager.GameState != BattleManager.Stages.Combating)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!this.canDeploy)
        {
            this.rend.sprite = this.tileBlock.CanNotDeployIcon;
            this.rend.enabled = true;
            return;
        }

        this.MouseInput();

        if (this.IsOccupied())
        {
            this.rend.sprite = this.tileBlock.OccupiedIcon;
            this.rend.enabled = true;
            this.lineRenderer.enabled = true;
            return;
        }


        this.rend.sprite = this.rend.sprite = this.tileBlock.CanDeployIcon;
        this.rend.enabled = true;
    }

    public void MouseInput()
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

    public void OnMouseExit()
    {
        this.rend.enabled = false;
        this.lineRenderer.enabled = false;
    }

    #endregion

    #region Internal Functions

    private bool IsOccupied()
    {
        return this.characterOnTop != 0;
    }

    private void InitializeLineRenderer()
    {
        int segments = 25;
        this.lineRenderer.positionCount = segments + 1;
        this.lineRenderer.useWorldSpace = false;
        this.lineRenderer.enabled = false;

        // Trying to get the radius of the collider box
        // To be honest, I don't like this solution, but it works, and it does not break
        // others code.
        var character = GameObject.Find("CharacterList").transform.GetChild(this.characterOnTop + 1);
        var radius = character.GetComponent<CircleCollider2D>().radius;

        this.CreatePoints(radius, radius, segments);
    }

    private void CreatePoints(float xradius, float yradius, int segments)
    {
        float x;
        float y;
        float z = this.transform.position.z - 1;

        float angle = 20f;

        for (int i = 0; i < segments + 1; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            this.lineRenderer.SetPosition(i, new Vector3(x, y, z));

            angle += 360f / segments;
        }
    }

    #endregion
}
