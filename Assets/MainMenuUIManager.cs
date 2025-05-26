using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private Image daggerSprite;
    [SerializeField] private Sprite[] daggerSprites;
    [SerializeField] private float floatDistance = 20f;
    [SerializeField] private float floatDuration = 2f;
    [SerializeField] private float spriteChangeDuration = 3f;
    [SerializeField] private float fadeDuration = 0.5f;
    
    private Vector2 originalPosition;
    private int currentSpriteIndex = 0;
    
    private void Start()
    {
        originalPosition = daggerSprite.rectTransform.anchoredPosition;
        
        if (daggerSprites.Length > 0) {
            daggerSprite.sprite = daggerSprites[0];
        }
        
        StartFloatingAnimation();
        StartCoroutine(ChangeSpriteRoutine());
    }
    
    private void StartFloatingAnimation()
    {
        daggerSprite.rectTransform.DOAnchorPosY(originalPosition.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    private IEnumerator ChangeSpriteRoutine()
    {
        while (true) {
            yield return new WaitForSeconds(spriteChangeDuration);
            
            // Fade out
            daggerSprite.DOFade(0, fadeDuration).OnComplete(() => {
                currentSpriteIndex = (currentSpriteIndex + 1) % daggerSprites.Length;
                daggerSprite.sprite = daggerSprites[currentSpriteIndex];
                
                daggerSprite.DOFade(1, fadeDuration);
            });
            
            yield return new WaitForSeconds(fadeDuration * 2);
        }
    }
    
    private void OnDestroy()
    {
        DOTween.Kill(daggerSprite.rectTransform);
    }
}