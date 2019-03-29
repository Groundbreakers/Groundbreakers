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
    #region Inspector Values

    [SerializeField]
    private int characterOnTop;

    [SerializeField]
    private bool canDeploy;

    [SerializeField]
    private bool deploying;

    [SerializeField]
    private int deployingCharacter = -1;

    [SerializeField]
    private Sprite canDeployIcon;

    [SerializeField]
    private Sprite canNotDeployIcon;

    [SerializeField]
    private Sprite occupiedIcon;

    [SerializeField]
    private Deploy deploy;

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
    //private GameObject deployPanel;

    private LineRenderer lineRenderer;

    #endregion

    #region Public Functions

    public void SetCanDeploy(bool value)
    {
        this.canDeploy = value;
    }

    #endregion

    #region Public Function

    public void SetCharacterIndex(int id)
    {
        this.characterOnTop = id;
    }

    public int GetCharacterIndex()
    {
        return this.characterOnTop;
    }

    public void EnableDeploy(int index)
    {
        this.deploying = true;
        this.deployingCharacter = index;
    }

    public void DisableDeploy()
    {
        this.deploying = false;
        this.deployingCharacter = -1;
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        this.deploy = GameObject.Find("DeployPanel").GetComponent<Deploy>();
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.lineRenderer.enabled = false;

        var components = this.GetComponentsInChildren<SpriteRenderer>();
        this.rend = components[1];

        this.characterOnTop = -1;
        this.rend.sprite = this.canDeployIcon;
        this.rend.enabled = false;
    }

    private void Update()
    {
        if (BattleManager.GameState != GameStates.Combating)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        // well, just don't rotate if it is a cross
        if (!this.canDeploy)
        {
            return;
        }

        if (this.IsOccupied() && this.deploying)
        {
            this.rend.transform.rotation = Quaternion.identity;
            return;
        }

        this.rend.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f));
    }

    private void OnMouseOver()
    {
        // Clearly, do nothing when the battleManager is not in the battle state
        if (BattleManager.GameState != GameStates.Combating)
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

        if (this.IsOccupied())
        {
            this.ResetLineRenderer();
            this.lineRenderer.enabled = true;
        }

        if (this.deploying)
        {
            if (this.IsOccupied())
            {
                this.rend.sprite = this.canNotDeployIcon;
                this.rend.enabled = true;
                return;
            }
            else
            {
                this.rend.sprite = this.canDeployIcon;
                this.rend.enabled = true;
                return;
            }
        }

        if (this.characterOnTop != -1)
        {
            this.deploy.Highlight(this.characterOnTop);
        }

        this.rend.sprite = this.occupiedIcon;
        this.rend.enabled = true;
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (this.IsOccupied())
            {
                return;
            }
            if (this.deploying && this.deployingCharacter != -1)
            {
                this.deploy.SetTargetPos(this.deployingCharacter, this.gameObject);
                this.deploy.DeployCharacter(this.deployingCharacter);
                this.deploy.DisableDeploy(this.deployingCharacter);
            }
        }
    }

    private void OnMouseExit()
    {
        this.rend.enabled = false;
        this.lineRenderer.enabled = false;

        this.deploy.DisableHighlight();
    }

    #endregion

    #region Internal Functions

    private bool IsOccupied()
    {
        return this.characterOnTop != -1;
    }

    private void ResetLineRenderer()
    {
        this.lineRenderer.positionCount = 0;

        const int Segments = 50;
        this.lineRenderer.positionCount = Segments + 1;
        this.lineRenderer.useWorldSpace = false;
        this.lineRenderer.enabled = false;

        // Trying to get the radius of the collider box
        // To be honest, I don't like this solution, but it works, and it does not break
        // others code.
        var character = GameObject.Find("CharacterList").transform.GetChild(this.characterOnTop);
        var radius = character.GetComponent<CircleCollider2D>().radius;

        this.CreatePoints(radius, radius, Segments);
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
