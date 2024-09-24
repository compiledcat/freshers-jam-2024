using TMPro;
using UnityEngine;

public class DataText : MonoBehaviour
{
    public enum Value
    {
        BaseHealth
    }

    [SerializeField] private Value _value;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnValidate()
    {
        if (!_text)
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
    }

    private void LateUpdate()
    {
        switch (_value)
        {
            case Value.BaseHealth:
                _text.text = $"{GameManager.Instance.Health}";
                break;
        }
    }
}