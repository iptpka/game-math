using GameMath.Util;
using UnityEngine;
using UnityEngine.Events;

namespace GameMath.Crane
{
    public class Trolley : Parentable
    {
        [SerializeField] private Transform _nearLimit;
        [SerializeField] private Transform _farLimit;
        [SerializeField] private float _dollySpeed;
        private float _dollyDuration;
        private float _dollyStartTime;
        private Vector3 _dollyStart;
        private Vector3 _dollyTarget;
        private bool _isDollying = false;
        public UnityEvent ReachedTarget;

        private void Awake() => ReachedTarget ??= new();

        void Update()
        {
            transform.SetPositionAndRotation(GetTargetPosition(), GetTargetRotation());
        }

        protected override void LateUpdate()
        {
            if (!_isDollying) return;
            var t = (Time.time - _dollyStartTime) / _dollyDuration;
            if (t >= 1)
            {
                _isDollying = false;
                transform.position = _dollyTarget;
                ReachedTarget.Invoke();
                return;
            }
            var blend = Mathf.SmoothStep(0, 1, t);
            blend *= blend;
            transform.position = Vector3.Lerp(_dollyStart, _dollyTarget, blend);
            UpdateRelativeTransform();
        }

        public void SetDollyTarget(Vector3 targetPosition)
        {
            _dollyTarget = new (targetPosition.x, transform.position.y, targetPosition.z);
            _dollyStartTime = Time.time;
            _dollyStart = transform.position;
            _dollyDuration = Vector3.Distance(_dollyStart, _dollyTarget) / _dollySpeed;
            _isDollying = true;
        }
    }
}
