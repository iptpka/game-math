using Unity.VisualScripting;
using UnityEngine;

namespace GameMath.Crane
{
    public class Crane : MonoBehaviour
    {
        [SerializeField] private float _maxSwingSpeed = 15f;
        [SerializeField] private float _swingAcceleration = 0.01f;
        [SerializeField] private float _swingDeceleration = 0.1f;
        [SerializeField] private float _swingStopThreshold = 0.015f;
        private bool _isSwinging = false;
        private bool _hasStopped = true;
        private int _swingDirection;
        private float _currentSwingSpeed = 0f;
        private float _lerpTime = 0f;

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
            _currentSwingSpeed = Mathf.Lerp(_currentSwingSpeed, _maxSwingSpeed * _swingDirection, _lerpTime * _swingAcceleration);
            _lerpTime += Time.deltaTime;
            if (Mathf.Approximately(_currentSwingSpeed, _maxSwingSpeed))
            {
                _currentSwingSpeed = _maxSwingSpeed;
                _lerpTime = 0f;
            }
        }

        void Decelerate()
        {
            if (Mathf.Abs(_currentSwingSpeed) < _swingStopThreshold)
            {
                _currentSwingSpeed = 0f;
                _hasStopped = true;
                _lerpTime = 0f;
                return;
            }
            _currentSwingSpeed = Mathf.Lerp(_currentSwingSpeed, 0f, _lerpTime * _swingDeceleration);
            _lerpTime += Time.deltaTime;
        }

        public void StartSwinging(int direction)
        {
            _isSwinging = true;
            _hasStopped = false;
            _swingDirection = direction;
        }

        public void StopSwinging()
        {
            _lerpTime = 0f;
            _isSwinging = false;
        }
    }
}
