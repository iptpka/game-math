using UnityEngine;
using UnityEngine.Events;

namespace GameMath.Crane
{
    public class Crane : MonoBehaviour
    {
        [SerializeField] private float _maxSwingSpeed = 15f;
        [SerializeField] private float _swingAcceleration = 1f;
        [SerializeField] private float _swingDeceleration = 2f;
        [SerializeField] private float _swingStopThreshold = 0.015f;
        private bool _isSwinging = false;
        private bool _hasStopped = true;
        private int _swingDirection;
        private float _currentSwingSpeed = 0f;
        private Quaternion _targetRotation;

        public UnityEvent ReachedTarget;

        private void Awake() => ReachedTarget ??= new();

        void Update()
        {
            if (!_isSwinging && _hasStopped) return;
            if (!_isSwinging && !_hasStopped) Decelerate();
            else if (_currentSwingSpeed < _maxSwingSpeed) Accelerate();
            var rotation = Quaternion.AngleAxis(_currentSwingSpeed * Time.deltaTime, transform.up);
            transform.rotation *= rotation;
        }

        void Accelerate()
        {
            var blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * _swingAcceleration);
            _currentSwingSpeed = Mathf.Lerp(_currentSwingSpeed, _maxSwingSpeed * _swingDirection, blend);
            if (Mathf.Approximately(_currentSwingSpeed, _maxSwingSpeed))
            {
                _currentSwingSpeed = _maxSwingSpeed;
            }
        }

        void Decelerate()
        {
            if (Mathf.Abs(_currentSwingSpeed) < _swingStopThreshold)
            {
                _currentSwingSpeed = 0f;
                _hasStopped = true;
                return;
            }

            var blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * _swingDeceleration);
            _currentSwingSpeed = Mathf.Lerp(_currentSwingSpeed, 0f, blend);

        }

        public void SetTarget(Vector3 position)
        {
            if (_isSwinging)
            {
                StopSwinging();
            }
            else
            {
                StartSwinging(1);
            }
        }

        public void StartSwinging(int direction)
        {
            _isSwinging = true;
            _hasStopped = false;
            _swingDirection = direction;
        }

        public void StopSwinging()
        {
            _isSwinging = false;
            ReachedTarget.Invoke();
        }
    }
}
