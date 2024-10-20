using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMath.Crane
{
    public class CraneController : MonoBehaviour
    {
        [SerializeField] private Crane _crane;
        [SerializeField] private Trolley _trolley;
        [SerializeField] private Hook _hook;
        Vector3 _target;
        public void SetNewTarget(PointerEventData eventData)
        {
            if (!eventData.pointerPress.TryGetComponent(out Hookable target))
                return;
            _target = target.transform.position;
            _crane.SetSwingTarget(_target);
            _crane.ReachedTarget.AddListener(OnCraneReachedTarget);
        }

        void OnCraneReachedTarget()
        {
            _crane.ReachedTarget.RemoveAllListeners();
            _trolley.SetDollyTarget(_target);
            _trolley.ReachedTarget.AddListener(OnTrolleyReachedTarget);
        }

        void OnTrolleyReachedTarget()
        {
            _trolley.ReachedTarget.RemoveAllListeners();
            _hook.SetTargetHeight(_target);
        }
    }
}
