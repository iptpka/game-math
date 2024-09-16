using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Hook : Parentable
    {
        [SerializeField] private Transform _crane;
        [SerializeField] private float _liftingSpeed = 5f;
        [SerializeField] private float _lowerLimit = 5f;
        [SerializeField] private float _followingSpeed = 5f;
        [SerializeField] private float _followDelayMultiplier = 1.5f;
        private float _upperLimit;
        private float _targetHeight;
        private bool _isHeightChanging = true;

        private void Start()
        {
            _upperLimit = _targetHeight = transform.position.y;
        }

        override protected void LateUpdate()
        {
            var targetPosition = GetTargetPosition();
            targetPosition.y = transform.position.y;
            // Naïve following speed delay from "longer cable"
            // -> the smaller transform y is, the slower it follows the parent
            float lengthDelay = (Mathf.Abs(_upperLimit - transform.position.y - _lowerLimit) * _followDelayMultiplier)
                                    / (_upperLimit - _lowerLimit);
            float followingSpeed = _followingSpeed - (_followingSpeed * (lengthDelay));
            // Lerped smooth following of parent for fake inertia
            var delayedPosition = Vector3.Lerp(transform.position, targetPosition,
                                                followingSpeed * Time.deltaTime);
            var delayedRotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(),
                                                  (_followingSpeed * (5)) * Time.deltaTime);
            // Vector from this to the trolley
            var trolleyDirection = _parent.position - transform.position;
            // For aligning the hook 'forward' which faces to the right when looking from the crane
            var forward = Vector3.Cross(transform.position - _crane.position, trolleyDirection) * -1;
            // Aligns transform up towards trolley
            delayedRotation.SetLookRotation(forward, trolleyDirection);
            if (_isHeightChanging)
            {
                var delayedHeight = new Vector3(delayedPosition.x,
                                                _targetHeight,
                                                delayedPosition.z);
                delayedPosition = Vector3.Lerp(delayedPosition,
                                               delayedHeight,
                                               _liftingSpeed * Time.deltaTime);
                if (Mathf.Approximately(Vector3.Distance(delayedPosition, transform.position), 0))
                {
                    _isHeightChanging = false;
                }
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
