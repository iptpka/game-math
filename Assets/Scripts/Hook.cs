using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Hook : Parentable
    {
        [SerializeField] private Transform _crane;
        [SerializeField] private float _liftingSpeed;
        [SerializeField] private float _loweringSpeed;
        [SerializeField] private float _lowerLimit;
        private float _upperLimit;
        private float _targetHeight;
        private bool _isHeightChanging = false;

        private void Start()
        {
            _upperLimit = _targetHeight = transform.position.y;
        }

        override protected void LateUpdate()
        {
            var delayedPosition = Vector3.Lerp(transform.position, GetTargetPosition(), 5f * Time.deltaTime);
            var delayedRotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(), 5f * Time.deltaTime);
            var trolleyDirection = _parent.position - transform.position;
            //For aligning the hook 'forward' which faces to the right when looking from the crane
            var forward = Vector3.Cross(transform.position - _crane.position, trolleyDirection) * -1;
            delayedRotation.SetLookRotation(forward, trolleyDirection);
            if (_isHeightChanging)
            {
                var delayedHeight = new Vector3(delayedPosition.x,
                                                _targetHeight,
                                                delayedPosition.z);
                delayedPosition = Vector3.Lerp(delayedPosition,
                                               delayedHeight,
                                               _liftingSpeed);
            }
            transform.SetPositionAndRotation(delayedPosition, delayedRotation);
        }

        public void SetNewHeightTarget(float targetHeightPercent)
        {
            if (targetHeightPercent > 1 || targetHeightPercent < 0) return;
            _targetHeight = ((_upperLimit - _lowerLimit) * targetHeightPercent) + _lowerLimit;
            _isHeightChanging = true;
        }
    }
}
