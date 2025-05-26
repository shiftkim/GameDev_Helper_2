using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    
    private Vector3 originalScale;
    private Button button;
    
    private void Awake()
    {
        originalScale = transform.localScale;
        button = GetComponent<Button>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            transform.DOScale(originalScale * hoverScale, scaleDuration).SetEase(scaleEase);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            transform.DOScale(originalScale, scaleDuration).SetEase(scaleEase);
        }
    }
    
    private void OnDisable()
    {
        transform.localScale = originalScale;
    }
    
    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}