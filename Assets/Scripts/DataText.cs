﻿using TMPro;
using UnityEngine;

public class DataText : MonoBehaviour
{
    public enum Value
    {
        BaseHealth,
        Money,
        WaveNumber
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
        var value = _value switch
        {
            Value.BaseHealth => GameManager.Instance.health,
            Value.Money => GameManager.Instance.Money,
            Value.WaveNumber => RoundManager.Instance.currentWave + 1, // todo why are we 0 indexing waves
            _ => 0
        };

        _text.text = $"{_prefix}{value}{_suffix}";
    }
}