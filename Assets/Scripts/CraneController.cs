using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMath.Crane
{
    public class CraneController : MonoBehaviour
    {
        [SerializeField] private Crane _crane;
        [SerializeField] private Trolley _trolley;
        [SerializeField] private Hook _hook;
        [SerializeField] private Transform _concrete;
        float _concreteStartingHeight;
        Vector3 _target;
        bool _operating = false;

        private void Awake()
        {
            _concreteStartingHeight = _concrete.position.y;
        }

        public void SetNewTarget(PointerEventData eventData)
        {
            if (_operating || !eventData.pointerPress.TryGetComponent(out Hookable target))
                return;
            _operating = true;
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
            _hook.Connected.AddListener(OnHookConnected);
        }

        void OnHookConnected()
        {
            _hook.Connected.RemoveAllListeners();
            _hook.HoistUp();
            _hook.Lifted.AddListener(OnItemLifted);
        }

        void OnItemLifted()
        {
            _hook.Lifted.RemoveAllListeners();
            _hook.Disconnect();
            var concretePosition = RandomReachablePosition();
            var concreteRotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
            _concrete.SetPositionAndRotation(concretePosition, concreteRotation);
            _operating = false;
        }

        Vector3 RandomReachablePosition()
        {
            var cranePosition = _crane.transform.position;
            var minRadius = Vector3.ProjectOnPlane(_trolley.NearLimit - cranePosition, Vector3.up).magnitude;
            var maxRadius = Vector3.ProjectOnPlane(_trolley.FarLimit - cranePosition, Vector3.up).magnitude;
            var radius = Random.Range(minRadius, maxRadius);
            var angle = Random.Range(0, 360);
            return cranePosition + new Vector3(radius * Mathf.Cos(angle), _concreteStartingHeight, radius * Mathf.Sin(angle));
        }
    }
}
