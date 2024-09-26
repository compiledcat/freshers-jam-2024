using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    private static List<Tower> _towers = new();
    [SerializeField] private TowerMenuButton _towerMenuButtonPrefab;

    private void Awake()
    {
        _towers = Resources.LoadAll<Tower>("Towers").ToList();

        foreach (var tower in _towers)
        {
            var menuButton = Instantiate(_towerMenuButtonPrefab, transform);
            
            menuButton.Image.sprite = tower.GetComponent<SpriteRenderer>().sprite;
            menuButton.Image.color = tower.GetComponent<SpriteRenderer>().color;
            menuButton.TowerPrefab = tower;
            
            menuButton.Button.onClick.AddListener(() => Debug.Log(tower));
        }
    }
}