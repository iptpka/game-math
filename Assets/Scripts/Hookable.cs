using UnityEngine;

namespace GameMath.Crane
{
    public class Hookable : MonoBehaviour
    {
        [SerializeField] LayerMask _groundLayer;
        private Hook _hook;
        private Transform _targetArea;
        private Vector3 _relativePosition;
        private Quaternion _relativeRotation;
        private float _height;
        private bool _canBeHooked = true;
        private bool _isHooked = false;
        private bool _canBeReleased = false;
        public bool CanMoveDown => DistanceToGround() > _height;

        void Start()
        {
            _height = DistanceToGround();
        }

        private float DistanceToGround()
        {
            if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit, 100f, _groundLayer))
            {
                return hit.distance;
            }
            return -1;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!_canBeHooked || _isHooked || !other.TryGetComponent(out Hook hook) || !hook.Connect(this)) return;
            _hook = hook;
            _isHooked = true;
            _relativePosition = _hook.transform.InverseTransformPoint(transform.position);
            _relativeRotation = Quaternion.Inverse(_hook.transform.rotation) * transform.rotation;
        }

        protected Vector3 GetTargetPosition()
        {
            return _hook.transform.TransformPoint(_relativePosition);
        }

        protected Quaternion GetTargetRotation()
        {
            return _hook.transform.rotation * _relativeRotation;
        }

        protected virtual void LateUpdate()
        {
            if (!_isHooked) return;
            if (_canBeReleased && DistanceToGround() <= _height)
            {
                Disconnect();
                return;
            }
            transform.SetPositionAndRotation(GetTargetPosition(), GetTargetRotation());
        }

        protected void Disconnect()
        {
            transform.position = new Vector3(_targetArea.position.x,
                                             _targetArea.position.y + _height,
                                             _targetArea.position.z);
            _isHooked = false;
            _canBeHooked = false;
            _targetArea = null;
            _hook.Disconnect();
        }

        public void SetReleaseTarget(Transform targetArea)
        {
            if (!_isHooked) return;
            _canBeReleased = true;
            _targetArea = targetArea;
        }

        public void RemoveReleaseTarget()
        {
            _canBeReleased = false;
            _targetArea = null;
        }
    }
}
