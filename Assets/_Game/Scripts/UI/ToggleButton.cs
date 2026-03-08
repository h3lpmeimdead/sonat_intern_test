using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Image imageHandler;

    [SerializeField] private RectTransform posOn;
    [SerializeField] private RectTransform posOff;
    
    public void OnInit(bool value)
    {
        RectTransform posToCome = value ? posOn : posOff;
        
        imageHandler.color = value ? Color.green : Color.red;
        imageHandler.rectTransform.anchoredPosition = posToCome.anchoredPosition;
    }

    public void Toggle(bool value)
    {
        RectTransform posToCome = value ? posOn : posOff;
        
        imageHandler.color = value ? Color.green : Color.red;
        
        imageHandler.rectTransform.DOAnchorPos(posToCome.anchoredPosition, 0.2f).SetEase(Ease.OutCirc);
    }
}