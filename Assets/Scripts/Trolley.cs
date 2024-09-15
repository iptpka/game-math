using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Trolley : Parentable
    {
        [SerializeField] private Transform _nearLimit;
        [SerializeField] private Transform _farLimit;
        [SerializeField] private float _dollySpeed;
        private float _dollyTarget = 1f;
        private bool _isDollying = false;

        void Update()
        {
            transform.SetPositionAndRotation(GetTargetPosition(), GetTargetRotation());
        }

        protected override void LateUpdate()
        {
            if (!_isDollying) return;
            var dollyTargetPosition = _nearLimit.position + ((_farLimit.position - _nearLimit.position)*_dollyTarget);
            var delayedPosition = Vector3.Lerp(transform.position, dollyTargetPosition, _dollySpeed * Time.deltaTime);
            transform.position = delayedPosition;
            UpdateRelativeTransform();
            if (Vector3.Distance(transform.position, dollyTargetPosition) < 0.001f) _isDollying = false;
        }

        public void SetNewDollyTarget(float targetPosition)
        {
            if (targetPosition > 1 || targetPosition < 0) return;
            _dollyTarget = targetPosition;
            _isDollying = true;
        }
    }
}
