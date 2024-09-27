using PrimeTween;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void OnValidate()
    {
        if (_text == null)
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void Awake()
    {
        Instance = this;
        
        _canvasGroup.alpha = 0;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    private void Update()
    {
        // Set pivot based on quadrant of mouse
        var rectTransform = (RectTransform)transform;
        var pivot = Input.mousePosition.x < Screen.width / 2f
            ? Input.mousePosition.y < Screen.height / 2f
                ? new Vector2(0, 0)
                : new Vector2(0, 1)
            : Input.mousePosition.y < Screen.height / 2f
                ? new Vector2(1, 0)
                : new Vector2(1, 1);

        rectTransform.pivot = pivot;
        transform.position = Input.mousePosition;
    }

    public void Appear()
    {
        Tween.Alpha(_canvasGroup, 1, 0.2f, Ease.InOutCubic);
    }

    public void Disappear()
    {
        Tween.Alpha(_canvasGroup, 0, 0.2f, Ease.InOutCubic);
    }
}