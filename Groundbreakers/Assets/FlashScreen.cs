using DG.Tweening;

using UnityEngine;

public class FlashScreen : MonoBehaviour
{
    /// <summary>
    ///     The start screen flash.
    /// </summary>
    /// <example>
    ///     FindObjectOfType<FlashScreen>().StartScreenFlash(Color.red, 0.25f);
    /// </example>
    /// <param name="color">
    ///     The color.
    /// </param>
    /// <param name="power">
    ///     The power.
    /// </param>
    public void StartScreenFlash(Color color, float power)
    {
        var sprite = this.GetComponent<SpriteRenderer>();

        sprite.color = color;

        var value = Mathf.Clamp(power, 0.0f, 1.0f);

        var sequence = DOTween.Sequence();
        sequence.Append(sprite.DOFade(value, 0.3f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true));
        sequence.Append(sprite.DOFade(0.0f, 0.3f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true));

        sequence.SetUpdate(true);
    }
}