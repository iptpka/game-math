using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<PointerEventData> Clicked;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(eventData);
    }
}
