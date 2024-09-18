using UnityEngine;

namespace GameMath.Util
{
    public class Parentable : MonoBehaviour
    {
        [SerializeField] protected Transform _parent;
        [SerializeField] protected Vector3 _parentOffset;
        protected Vector3 _relativePosition;
        protected Quaternion _relativeRotation;
        protected Vector3 ParentPosition => _parent.position + _parentOffset;

        void Start()
        {
            UpdateRelativeTransform();
        }

        protected void UpdateRelativeTransform()
        {
            _relativePosition = _parent.InverseTransformPoint(transform.position);
            _relativeRotation = Quaternion.Inverse(_parent.rotation) * transform.rotation;
        }

        protected Vector3 GetTargetPosition()
        {
            return _parent.TransformPoint(_relativePosition);
        }

        protected Quaternion GetTargetRotation()
        {
            return _parent.rotation * _relativeRotation;
        }

        protected virtual void LateUpdate()
        {
            transform.SetPositionAndRotation(GetTargetPosition(), GetTargetRotation());
        }
    }
}

