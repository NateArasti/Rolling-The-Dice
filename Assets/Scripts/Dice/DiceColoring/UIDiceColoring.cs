using UnityEngine;
using UnityEngine.UI;

public class UIDiceColoring : MonoBehaviour
{
    private static UIDiceColoring _instance;

    [Header("UI Coloring")]
    [SerializeField] private Image _farUISide;
    [SerializeField] private Image _topUISide;
    [SerializeField] private Image _leftUISide;
    [SerializeField] private Image _closeUISide;
    [SerializeField] private Image _rightUISide;
    [SerializeField] private Image _downUISide;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public static void SetUIColoring(
        Sprite farSprite,
        Sprite topSprite,
        Sprite leftSprite,
        Sprite closeSprite,
        Sprite rightSprite,
        Sprite downSprite
    )
    {
        _instance._farUISide.sprite = farSprite;
        _instance._topUISide.sprite = topSprite;
        _instance._leftUISide.sprite = leftSprite;
        _instance._closeUISide.sprite = closeSprite;
        _instance._rightUISide.sprite = rightSprite;
        _instance._downUISide.sprite = downSprite;
    }

    public void SetUIEditColoring(
        Sprite farSprite,
        Sprite topSprite,
        Sprite leftSprite,
        Sprite closeSprite,
        Sprite rightSprite,
        Sprite downSprite
    )
    {
        _farUISide.sprite = farSprite;
        _topUISide.sprite = topSprite;
        _leftUISide.sprite = leftSprite;
        _closeUISide.sprite = closeSprite;
        _rightUISide.sprite = rightSprite;
        _downUISide.sprite = downSprite;
    }
}
