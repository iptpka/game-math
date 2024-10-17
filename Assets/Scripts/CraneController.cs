using GameMath.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMath.Crane
{
    public class CraneController : MonoBehaviour
    {
        [SerializeField] private Crane _crane;
        [SerializeField] private Trolley _trolley;
        [SerializeField] private Hook _hook;

        public void SetNewTarget(BaseEventData eventData)
        {
            if (!eventData.selectedObject.TryGetComponent(out Hookable target))
                return;


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
