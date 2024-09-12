using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameMath.UI
{
    public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool isPointerDown;

        public bool IsHeldDown => isPointerDown;

        public UnityEvent<bool> OnIsHeldChanged;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
            OnIsHeldChanged.Invoke(IsHeldDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerDown = false;
            OnIsHeldChanged.Invoke(IsHeldDown);
        }
    }
}