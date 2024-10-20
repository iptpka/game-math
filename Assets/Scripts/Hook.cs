using GameMath.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GameMath.Crane
{
    public class Hook : Parentable
    {
        [SerializeField] private Transform _crane;
        [SerializeField] private float _hoistingSpeed = 5f;
        [SerializeField] private float _lowerLimit = 5f;
        [SerializeField] private float _followingSpeed = 5f;
        [SerializeField] private float _lengthDelay = 0.5f;
        private float _upperLimit;
        private float _targetHeight;
        private float _hoistStartHeight;
        private float _hoistStartTime;
        private float _hoistDuration;
        private bool _isHoisting = false;
        private bool _isConnected = false;
        private Hookable _connectedItem;
        public float LengthPercent => (_upperLimit - transform.position.y)
                                    / Mathf.Abs(_upperLimit - _lowerLimit);

        public UnityEvent Connected;
        public UnityEvent Lifted;
        void Awake()
        {
            var trolleyBounds = _parent.GetComponent<Renderer>().bounds;
            var hookBounds = GetComponent<Renderer>().bounds;
            _upperLimit = trolleyBounds.center.y - trolleyBounds.extents.y - hookBounds.extents.y;
            _targetHeight = transform.position.y;
            Connected ??= new();
            Lifted ??= new();
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
            var followingBlend = 1 - Mathf.Pow(0.5f, GetCurrentFollowingSpeed() * Time.deltaTime);
            targetPosition = Vector3.Slerp(transform.position, targetPosition, followingBlend);
            // Vector from this to the trolley
            var trolleyDirection = _parent.position - targetPosition;
            // For aligning the hook 'forward' which faces to the right when looking from the crane
            var forward = Vector3.Cross(targetPosition - _crane.position, trolleyDirection) * -1;
            // Aligns transform up towards trolley
            var lookAtRotation = Quaternion.LookRotation(forward, trolleyDirection);
            if (_isHoisting)
            {
                var t = (Time.time - _hoistStartTime) / _hoistDuration;
                if (t >= 1)
                {
                    _isHoisting = false;
                    if (_isConnected)
                    {
                        Lifted.Invoke();
                    }
                }
                else
                {
                    var blend = Mathf.SmoothStep(0, 1, t);
                    targetPosition.y = Mathf.Lerp(_hoistStartHeight, _targetHeight, blend);
                }

            }
            transform.SetPositionAndRotation(targetPosition, lookAtRotation);
        }

        public void SetTargetHeight(Vector3 targetHeight)
        {
            SetTargetHeight(targetHeight.y);
        }

        public void SetTargetHeight(float targetHeight)
        {
            _targetHeight = targetHeight;
            _isHoisting = true;
            _hoistStartHeight = transform.position.y;
            _hoistStartTime = Time.time;
            _hoistDuration = Mathf.Abs(_targetHeight - _hoistStartHeight) / _hoistingSpeed;
        }

        public bool Connect(Hookable hooked)
        {
            if (_isConnected) return false;
            _isConnected = true;
            _connectedItem = hooked;
            GetComponent<Collider>().enabled = false;
            SetTargetHeight(_upperLimit);
            Connected.Invoke();
            return true;
        }

        public void Disconnect()
        {
            _connectedItem = null;
            _isConnected = false;
            GetComponent<Collider>().enabled = true;
        }
    }
}
