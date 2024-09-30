using UnityEngine;

public class WaterCanvasScaler : MonoBehaviour
{
    [SerializeField] private RectTransform _canvas;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    [ContextMenu("Update")]
    private void Update()
    {
        if (!_cam) _cam = Camera.main; // fallback for context menu item
        _canvas.sizeDelta = new Vector2(_cam.orthographicSize * 2 * _cam.aspect, _cam.orthographicSize * 2);
    }

    private void OnValidate()
    {
        if (_canvas == null)
        {
            _canvas = GetComponent<RectTransform>();
        }
    }
}