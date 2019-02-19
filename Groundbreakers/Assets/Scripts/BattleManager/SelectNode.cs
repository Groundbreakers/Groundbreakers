using System;

using Assets.Scripts;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class SelectNode : MonoBehaviour
{
    public int characterOnTop;

    private Boolean isSelected;

    #region Inspector Values

    [SerializeField]
    private bool canDeploy;

    [SerializeField]
    private Sprite canDeployIcon;

    [SerializeField]
    private Sprite canNotDeployIcon;

    [SerializeField]
    private Sprite occupiedIcon;

    #endregion

    #region Internal fields

    /// <summary>
    /// The sprite renderer of the Child of this game object. (used for hover icon)
    /// </summary>
    private SpriteRenderer rend;

    /// <summary>
    /// Making this canvas a field then we don't need to call the expensive function
    /// 'GameObject.Find' every time.
    /// </summary>
    private GameObject deployPanel;

    private LineRenderer lineRenderer;

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
        this.deployPanel = GameObject.Find("DeployPanel");

        var components = this.GetComponentsInChildren<SpriteRenderer>();
        this.rend = components[1];

        this.characterOnTop = -1;
        this.rend.sprite = this.canDeployIcon;
        this.rend.enabled = false;

        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.InitializeLineRenderer();
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Deploy deploy = this.deployPanel.GetComponent<Deploy>();
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
            this.rend.sprite = this.canNotDeployIcon;
            this.rend.enabled = true;
            return;
        }

        this.MouseInput();

        if (!this.isSelected)
        { 
            this.rend.sprite = this.occupiedIcon;
            this.rend.enabled = true;
            if (this.IsOccupied())
            {
                this.lineRenderer.enabled = true;
            }
        }
    }

    public void MouseInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            foreach (Transform child in GameObject.Find("TileMap").transform)
            {
                SelectNode selectNode = child.gameObject.GetComponent<SelectNode>();
                if (selectNode != null)
                {
                    selectNode.isSelected = false;
                    selectNode.OnMouseExit();
                }
            }
            this.rend.sprite = this.canDeployIcon;
            this.rend.enabled = true;
            this.isSelected = true;
            Deploy deploy = this.deployPanel.GetComponent<Deploy>();
            deploy.SetNode(this.gameObject);
        }
    }

    public void OnMouseExit()
    {
        if (!this.isSelected)
        {
            this.rend.enabled = false;
        }

        this.lineRenderer.enabled = false;
    }

    #endregion

    #region Internal Functions

    private bool IsOccupied()
    {
        return this.characterOnTop != -1;
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
        //var character = GameObject.Find("CharacterList").transform.GetChild(this.characterOnTop);
        //var radius = character.GetComponent<CircleCollider2D>().radius;

        //this.CreatePoints(radius, radius, segments);
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
