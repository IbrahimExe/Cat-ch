using UnityEngine;
using UnityEngine.UI;

public class TurnIndicatorVisual : MonoBehaviour
{
    [SerializeField]
    private Image vignetteImage;
    [SerializeField]
    private float vignetteAlphaActive = 0.3f;
    [SerializeField]
    private float vignetteAlphaInactive = 0.6f;
    
    [SerializeField]
    private SpriteRenderer mouseRenderer;
    [SerializeField]
    private SpriteRenderer catRenderer;
    [SerializeField]
    private float activeOpacity = 1f;
    [SerializeField]
    private float inactiveOpacity = 0.5f;
    
    [SerializeField]
    private GameObject mouseHighlight;
    [SerializeField]
    private GameObject catHighlight;
    
    private void Update()
    {
        if (GameManager.Instance == null)
            return;
        
        bool isMouseTurn = GameManager.Instance.IsMouseTurn;
        
        UpdateVignette(isMouseTurn);
        UpdateOpacity(isMouseTurn);
        UpdateHighlight(isMouseTurn);
    }
    
    private void UpdateVignette(bool isMouseTurn)
    {
        if (vignetteImage == null)
            return;
        
        Color color = vignetteImage.color;
        color.a = isMouseTurn ? vignetteAlphaActive : vignetteAlphaInactive;
        vignetteImage.color = color;
    }
    
    private void UpdateOpacity(bool isMouseTurn)
    {
        if (mouseRenderer != null)
        {
            Color mouseColor = mouseRenderer.color;
            mouseColor.a = isMouseTurn ? activeOpacity : inactiveOpacity;
            mouseRenderer.color = mouseColor;
        }
        
        if (catRenderer != null)
        {
            Color catColor = catRenderer.color;
            catColor.a = isMouseTurn ? inactiveOpacity : activeOpacity;
            catRenderer.color = catColor;
        }
    }
    
    private void UpdateHighlight(bool isMouseTurn)
    {
        if (mouseHighlight != null)
            mouseHighlight.SetActive(isMouseTurn);
        
        if (catHighlight != null)
            catHighlight.SetActive(!isMouseTurn);
    }
}
