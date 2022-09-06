using DG.Tweening;
using TMPro;
using UnityEngine;

public class RemainingFires : MonoBehaviour
{
    public TextMeshProUGUI RemainingFiresText;
    public Color NormalColor;
    public Color CompletedColor;
    public Color WarningColor;

    void Start()
    {
        SetTextColor(NormalColor);
    }

    public void SetRemainingFires(int amount)
    {
        RemainingFiresText.text = amount.ToString();
        if(amount == 0)
        {
            SetTextColor(CompletedColor);
        }
    }

    public void WarningFire()
    {
        SetTextColor(NormalColor);
        RemainingFiresText
            .DOColor(WarningColor, 0.5f)
            .SetLoops(6, LoopType.Yoyo)
            .SetEase(Ease.InQuad);
    }
    
    private void SetTextColor(Color color)
    {
        RemainingFiresText.color = color;
    }
}
