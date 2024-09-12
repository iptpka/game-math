using UnityEngine;

namespace GameMath.Util
{
    public class Parentable : MonoBehaviour
    {
        [SerializeField] protected Transform _parent;
        protected Vector3 _relativePosition;
        protected Quaternion _relativeRotation;

        void Awake()
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

