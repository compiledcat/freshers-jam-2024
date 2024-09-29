using UnityEngine;
using UnityEngine.UI;

public class WaterTiler : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }
    
    private void Update()
    {
        _rawImage.rectTransform.sizeDelta = new Vector2(_cam.orthographicSize * 2 * _cam.aspect, _cam.orthographicSize * 2);
        
        // offset over time
        var offset = _rawImage.uvRect.position;
        offset.x += Time.deltaTime * 0.1f;
        offset.y += Time.deltaTime * 0.1f;
        _rawImage.uvRect = new Rect(offset, _rawImage.uvRect.size);
    }
    
    private void OnValidate()
    {
        if (_rawImage == null)
        {
            _rawImage = GetComponent<RawImage>();
        }
    }
}