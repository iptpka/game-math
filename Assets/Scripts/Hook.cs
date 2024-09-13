using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Hook : Parentable
    {
        [SerializeField] private Transform _crane;
        override protected void LateUpdate()
        {
            var delayedPosition = Vector3.Lerp(transform.position, GetTargetPosition(), 5f * Time.deltaTime);
            var delayedRotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(), 5f * Time.deltaTime);
            var trolleyDirection = _parent.position - transform.position;
            var forward = transform.position - _crane.position;
            delayedRotation.SetLookRotation(Vector3.Cross(forward, trolleyDirection), trolleyDirection);
            transform.SetPositionAndRotation(delayedPosition, delayedRotation);
        }
    }
}
