using TMPro;
using UnityEngine;

public class DataText : MonoBehaviour
{
    public enum Value
    {
        BaseHealth,
        Money,
    }

    [SerializeField] private Value _value;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private string _prefix;
    [SerializeField] private string _suffix;
    
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
                _text.text = $"{_prefix}{GameManager.Instance.health}{_suffix}";
                break;
            case Value.Money:
                _text.text = $"{_prefix}{GameManager.Instance.Money}{_suffix}";
                break;
        }
    }
}