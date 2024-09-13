using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Trolley : Parentable
    {
        [SerializeField] private Transform _nearLimit;
        [SerializeField] private Transform _farLimit;
        private float _dollyTarget = 1f;
        private bool _dollying = false;

        void Update()
        {
            transform.SetPositionAndRotation(GetTargetPosition(), GetTargetRotation());
        }

        protected override void LateUpdate()
        {
            if (!_dollying) return;
            var dollyTargetPosition = _nearLimit.position + ((_farLimit.position - _nearLimit.position)*_dollyTarget);
            var delayedPosition = Vector3.Lerp(transform.position, dollyTargetPosition, 5f * Time.deltaTime);
            transform.position = delayedPosition;
            UpdateRelativeTransform();
            if (Vector3.Distance(transform.position, dollyTargetPosition) < 0.001f) _dollying = false;
        }

        public void SetNewDollyTarget(float targetPosition)
        {
            if (targetPosition > 1 || targetPosition < 0) return;
            _dollyTarget = targetPosition;
            _dollying = true;
        }
    }
}
