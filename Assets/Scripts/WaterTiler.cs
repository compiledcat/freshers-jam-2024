using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class WaterTiler : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [ShowInInspector] [ReadOnly] private float _textureAspect;

    private Camera _cam;

    [SerializeField] private float _speed = 0.05f;
    [SerializeField] private float _scaleFactor = 1;

    private void Awake()
    {
        _cam = Camera.main;
        _textureAspect = _rawImage.texture.width / (float)_rawImage.texture.height;
    }

    private void Update()
    {
        // offset over time
        var offset = _rawImage.uvRect.position;
        offset.x += Time.deltaTime * _speed;
        offset.y += Time.deltaTime * _speed;

        // ensure texture isn't stretched
        var aspect = _cam.aspect / _textureAspect;

        _rawImage.uvRect = new Rect(
            offset,
            new Vector2(_cam.orthographicSize * aspect, _cam.orthographicSize) * _scaleFactor
        );
    }

    private void OnValidate()
    {
        if (_rawImage == null)
        {
            _rawImage = GetComponent<RawImage>();
        }
    }
}