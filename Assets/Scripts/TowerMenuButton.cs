using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerMenuButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Image Image;
    public Button Button;

    public Tower TowerPrefab;

    [SerializeField] private TooltipTrigger _tooltipTrigger;

    private Camera _cam;

    private Vector3 _initialDragPos;

    private void Start()
    {
        _cam = Camera.main;
        
        _tooltipTrigger.text = $"{TowerPrefab.name}  <size=70%>Tower (${TowerPrefab.cost})</size>\n\n" +
                               $"Range: {TowerPrefab.shootingRange}m\n" +
                               $"Cooldown: {TowerPrefab.GetComputedCooldownTime()}s\n" +
                               $"Damage: {TowerPrefab.projectileDamage}";
    }

    private void OnValidate()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
        }

        if (Button == null)
        {
            Button = GetComponent<Button>();
        }
        
        if (_tooltipTrigger == null)
        {
            _tooltipTrigger = GetComponent<TooltipTrigger>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _initialDragPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = _initialDragPos;

        Image.raycastTarget = false;
        var raycastHit = Physics2D.Raycast(_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        Image.raycastTarget = true;

        if (raycastHit && raycastHit.transform.TryGetComponent<TowerSpot>(out var towerSpot))
        {
            if (towerSpot.Tower != null) { return; }
            if (GameManager.Instance.Money < TowerPrefab.cost) { return; }
            if (TowerPrefab.towerPlacementType != towerSpot.towerPlacementType) { return; }

            GameManager.Instance.Money -= TowerPrefab.cost;

            var tower = Instantiate(TowerPrefab, towerSpot.transform);
            towerSpot.Tower = tower;
        }
    }
}