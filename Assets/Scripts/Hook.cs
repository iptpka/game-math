using GameMath.Util;
using UnityEngine;
using UnityEngine.Video;

namespace GameMath.Crane
{
    public class Hook : Parentable
    {
        [SerializeField] private Transform _crane;
        [SerializeField] private float _verticalSpeed = 5f;
        [SerializeField] private float _lowerLimit = 5f;
        [SerializeField] private float _followingSpeed = 5f;
        [SerializeField] private float _lengthDelay = 0.5f;
        private float _upperLimit;
        private float _targetHeight;
        private bool _isHeightChanging = false;
        private bool _isConnected = false;
        private Hookable _hooked;
        public float LengthPercent => (_upperLimit - transform.position.y)
                                    / Mathf.Abs(_upperLimit - _lowerLimit);

        void Awake()
        {
            var trolleyBounds = _parent.GetComponent<Renderer>().bounds;
            _upperLimit = trolleyBounds.center.y - trolleyBounds.extents.y - 1f;
            _targetHeight = transform.position.y;
        }

        float GetCurrentFollowingSpeed()
        {
            // Naïve following speed delay from "longer cable"
            // -> the smaller transform y is, the slower it follows the parent
            return _followingSpeed - (_followingSpeed * LengthPercent * _lengthDelay);
        }

        override protected void LateUpdate()
        {
            var targetPosition = GetTargetPosition();
            targetPosition.y = transform.position.y;
            var followingSpeed = GetCurrentFollowingSpeed();
            // Lerped smooth following of parent for fake inertia
            var delayedPosition = Vector3.Lerp(transform.position, targetPosition,
                                                followingSpeed * Time.deltaTime);
            // Vector from this to the trolley
            var trolleyDirection = _parent.position - delayedPosition;
            // For aligning the hook 'forward' which faces to the right when looking from the crane
            var forward = Vector3.Cross(delayedPosition - _crane.position, trolleyDirection) * -1;
            // Aligns transform up towards trolley
            var lookAtRotation = Quaternion.LookRotation(forward, trolleyDirection);
            if (_isHeightChanging && !(_isConnected && _targetHeight < transform.position.y && !_hooked.CanMoveDown))
            {
                delayedPosition.y = Mathf.Lerp(delayedPosition.y, _targetHeight,
                           _verticalSpeed * Time.deltaTime);
                if (Mathf.Approximately(delayedPosition.y, _targetHeight))
                    _isHeightChanging = false;
            }
            transform.SetPositionAndRotation(delayedPosition, lookAtRotation);
        }

        public void SetHeightTarget(float targetHeightPercent)
        {
            if (targetHeightPercent > 1 || targetHeightPercent < 0) return;
            _targetHeight = ((_upperLimit - _lowerLimit) * (1 - targetHeightPercent)) + _lowerLimit;
            _isHeightChanging = true;
        }

        public bool Connect(Hookable hooked)
        {
            if (_isConnected) return false;
            _isConnected = true;
            _hooked = hooked;
            GetComponent<Collider>().enabled = false;
            return true;
        }

        public void Disconnect()
        {
            _hooked = null;
            _isConnected = false;
            GetComponent<Collider>().enabled = true;
        }
    }
}
