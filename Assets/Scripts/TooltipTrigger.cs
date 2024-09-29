using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Multiline] public string text;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.SetText(text);
        Tooltip.Instance.Appear();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.Disappear();
    }
}