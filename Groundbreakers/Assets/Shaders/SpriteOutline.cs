using Sirenix.OdinInspector;

using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

    private static readonly int Outline = Shader.PropertyToID("_Outline");

    [SerializeField]
    private Color color = Color.white;

    private SpriteRenderer spriteRenderer;

    private bool show;

    [Button]
    public void Show()
    {
        this.show = true;
    }

    [Button]
    public void Hide()
    {
        this.show = false;
    }

    private void OnEnable()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        this.Hide();
        this.UpdateOutline();
    }

    private void OnDisable()
    {
        this.Hide();
        this.UpdateOutline();
    }

    private void Update()
    {
        this.UpdateOutline();
    }

    private void UpdateOutline()
    {
        var mpb = new MaterialPropertyBlock();
        this.spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(Outline, this.show ? 1.0f : 0.0f);
        mpb.SetColor(OutlineColor, this.color);
        this.spriteRenderer.SetPropertyBlock(mpb);
    }
}