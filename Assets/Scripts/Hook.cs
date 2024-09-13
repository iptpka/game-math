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
            //For aligning the hook 'forward' which faces to the right when looking from the crane
            var forward = Vector3.Cross(transform.position - _crane.position, trolleyDirection); 
            delayedRotation.SetLookRotation(forward, trolleyDirection);
            transform.SetPositionAndRotation(delayedPosition, delayedRotation);
        }
    }
}
