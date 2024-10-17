using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMath.Crane
{
    public class CraneController : MonoBehaviour
    {
        [SerializeField] private Crane _crane;
        [SerializeField] private Trolley _trolley;
        [SerializeField] private Hook _hook;

        public void SetNewTarget(PointerEventData eventData)
        {
            if (!eventData.pointerPress.TryGetComponent(out Hookable target))
                return;
            var targetPosition = target.transform.position;
            _crane.SetTarget(targetPosition);

        }

        void OnCraneInputStart(int inputDirection)
        {
            _crane.StartSwinging(inputDirection);
        }

        void OnCraneInputStop()
        {
            _crane.StopSwinging();
        }

        void OnTrolleySliderValueChanged(float newValue)
        {
            _trolley.SetNewDollyTarget(newValue);
        }

        void OnCableSliderValueChanged(float newValue)
        {
            _hook.SetHeightTarget(newValue);
        }
    }
}
