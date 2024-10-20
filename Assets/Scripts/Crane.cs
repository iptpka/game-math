using UnityEngine;
using UnityEngine.Events;

namespace GameMath.Crane
{
    public class Crane : MonoBehaviour
    {
        [SerializeField] private float _swingSpeed = 15f;
        [SerializeField] private Vector3 _craneForward;
        private float _swingDuration;
        private float _swingStartTime;
        private bool _isSwinging = false;
        private Quaternion _swingStart;
        private Quaternion _swingTarget;
        public UnityEvent ReachedTarget;

        private void Awake() => ReachedTarget ??= new();

        void Update()
        {
            if (!_isSwinging) return;
            var t = (Time.time - _swingStartTime) / _swingDuration;
            if (t >= 1)
            {
                _isSwinging = false;
                transform.rotation = _swingTarget;
                ReachedTarget.Invoke();
                return;
            }
            var blend = Mathf.SmoothStep(0, 1, t);
            blend *= blend;
            transform.rotation = Quaternion.Lerp(_swingStart, _swingTarget, blend);
        }

        public void SetSwingTarget(Vector3 targetPosition)
        {
            var forward = transform.rotation * _craneForward;
            var angle = Vector3.SignedAngle(forward, Vector3.ProjectOnPlane(targetPosition - transform.position, Vector3.up), transform.up);
            _swingTarget = transform.rotation * Quaternion.AngleAxis(angle, transform.up);
            _swingStart = transform.rotation;
            _swingStartTime = Time.time;
            float angleRatio = (Mathf.Abs((Mathf.Abs(angle) - 180) / 180));
            angleRatio *= angleRatio * angleRatio * angleRatio * 5f;
            _swingDuration = (Mathf.Abs(angle) / _swingSpeed) * (1 + angleRatio);
            _isSwinging = true;
        }
    }
}
