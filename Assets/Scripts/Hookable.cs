using UnityEngine;

namespace GameMath.Crane
{
    public class Hookable : MonoBehaviour
    {
        [SerializeField] LayerMask _groundLayer;
        private Hook _hook;
        private Vector3 _relativePosition;
        private Quaternion _relativeRotation;
        private float _height;
        private bool _canBeHooked = true;
        private bool _isHooked = false;
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
            transform.SetPositionAndRotation(GetTargetPosition(), GetTargetRotation());
        }

        protected void Disconnect()
        {
            _isHooked = false;
        }
    }
}
