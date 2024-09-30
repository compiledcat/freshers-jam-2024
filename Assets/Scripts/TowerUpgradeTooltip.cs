using System;
using System.Collections.Generic;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class UpgradeSet
{
    public Button PurchaseButton;
    public Image Bar;
    public TextMeshProUGUI CostText;
    public int Level;

    public UnityEvent OnUpgrade = new();
}

public class TowerUpgradeTooltip : MonoBehaviour
{
    [ReadOnly] public Tower Tower;

    [SerializeField] private CanvasGroup _canvasGroup;
    private Camera _cam;

    [SerializeField] private TextMeshProUGUI _nameText;

    [SerializeField] private Button _firstModeButton;
    [SerializeField] private Button _lastModeButton;
    [SerializeField] private Button _strongModeButton;
    [SerializeField] private Button _closeModeButton;

    private TextMeshProUGUI _firstModeText;
    private TextMeshProUGUI _lastModeText;
    private TextMeshProUGUI _strongModeText;
    private TextMeshProUGUI _closeModeText;

    [SerializeField] private Button _sellButton;

    public UpgradeSet DamageUpgrade;
    public UpgradeSet RangeUpgrade;
    public UpgradeSet SpeedUpgrade;

    public List<Sprite> SpriteLevels = new();

    private const int CostPerLevel = 5;
    private const int MaxLevel = 6;

    private void OnValidate()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void Awake()
    {
        _cam = Camera.main;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        transform.localScale = Vector3.zero;

        var upgrades = new List<UpgradeSet> { DamageUpgrade, RangeUpgrade, SpeedUpgrade };
        foreach (var upgrade in upgrades)
        {
            upgrade.PurchaseButton.onClick.AddListener(() =>
            {
                var upgradeCost = CostPerLevel * upgrade.Level;
                if (upgrade.Level >= MaxLevel || GameManager.Instance.Money < upgradeCost)
                {
                    return;
                }

                GameManager.Instance.Money -= CostPerLevel * upgrade.Level;
                Tower.InvestedValue += upgradeCost;

                upgrade.Level++;
                upgrade.Bar.sprite = SpriteLevels[upgrade.Level - 1];

                var newUpgradeCost = CostPerLevel * upgrade.Level;
                upgrade.CostText.text = upgrade.Level == MaxLevel ? "MAX" : $"{newUpgradeCost}";

                upgrade.OnUpgrade.Invoke();
            });
        }

        // cleaner ways of doing this, i know
        _firstModeText = _firstModeButton.GetComponentInChildren<TextMeshProUGUI>();
        _lastModeText = _lastModeButton.GetComponentInChildren<TextMeshProUGUI>();
        _strongModeText = _strongModeButton.GetComponentInChildren<TextMeshProUGUI>();
        _closeModeText = _closeModeButton.GetComponentInChildren<TextMeshProUGUI>();

        _firstModeButton.onClick.AddListener(() =>
        {
            Tower.SetTargetingMode(ShootingPriority.First);
            _firstModeText.fontStyle = FontStyles.Bold;
            _lastModeText.fontStyle = FontStyles.Normal;
            _strongModeText.fontStyle = FontStyles.Normal;
            _closeModeText.fontStyle = FontStyles.Normal;
        });

        _lastModeButton.onClick.AddListener(() =>
        {
            Tower.SetTargetingMode(ShootingPriority.Last);
            _firstModeText.fontStyle = FontStyles.Normal;
            _lastModeText.fontStyle = FontStyles.Bold;
            _strongModeText.fontStyle = FontStyles.Normal;
            _closeModeText.fontStyle = FontStyles.Normal;
        });

        _strongModeButton.onClick.AddListener(() =>
        {
            Tower.SetTargetingMode(ShootingPriority.Strong);
            _firstModeText.fontStyle = FontStyles.Normal;
            _lastModeText.fontStyle = FontStyles.Normal;
            _strongModeText.fontStyle = FontStyles.Bold;
            _closeModeText.fontStyle = FontStyles.Normal;
        });

        _closeModeButton.onClick.AddListener(() =>
        {
            Tower.SetTargetingMode(ShootingPriority.Close);
            _firstModeText.fontStyle = FontStyles.Normal;
            _lastModeText.fontStyle = FontStyles.Normal;
            _strongModeText.fontStyle = FontStyles.Normal;
            _closeModeText.fontStyle = FontStyles.Bold;
        });

        _sellButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Money += Mathf.FloorToInt(Tower.InvestedValue / 2f);
            Tween.Scale(Tower.transform, Vector3.zero, 0.2f, Ease.InBack).OnComplete(() => Destroy(Tower.gameObject));
            Destroy(gameObject);
        });
    }

    private void Start()
    {
        // Set pivot based on screen position of tower
        var rectTransform = (RectTransform)transform;

        var towerPos = _cam.WorldToScreenPoint(Tower.transform.position);
        var pivot = towerPos.x < Screen.width / 2f
            ? towerPos.y < Screen.height / 2f
                ? new Vector2(0, 0)
                : new Vector2(0, 1)
            : towerPos.y < Screen.height / 2f
                ? new Vector2(1, 0)
                : new Vector2(1, 1);

        rectTransform.pivot = pivot;
        transform.position = towerPos;

        _nameText.text = $"{Tower.name}  <size=70%>Tower</size>";
    }

    public void Appear()
    {
        if (_canvasGroup.blocksRaycasts) return; // already open
        
        Tween.Alpha(_canvasGroup, 1, 0.2f, Ease.InOutCubic);
        Tween.Scale(transform, Vector3.one, 0.2f, Ease.OutBack);
        _canvasGroup.blocksRaycasts = true;

        var allTooltips = FindObjectsByType<TowerUpgradeTooltip>(FindObjectsSortMode.None);
        foreach (var tooltip in allTooltips)
        {
            if (tooltip != this)
            {
                tooltip.Disappear();
            }
        }
    }

    public void Disappear()
    {
        if (!_canvasGroup.blocksRaycasts) return; // already closed
        
        Tween.Alpha(_canvasGroup, 0, 0.2f, Ease.InOutCubic);
        Tween.Scale(transform, Vector3.zero, 0.2f, Ease.InBack);
        _canvasGroup.blocksRaycasts = false;
    }
}