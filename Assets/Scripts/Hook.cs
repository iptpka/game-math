using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Hook : Parentable
    {
        override protected void LateUpdate()
        {
            var delayedPosition = Vector3.Lerp(transform.position, GetTargetPosition(), 5f * Time.deltaTime);
            var delayedRotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(), 5f * Time.deltaTime);
            transform.SetPositionAndRotation(delayedPosition, delayedRotation);
        }
    }
}
