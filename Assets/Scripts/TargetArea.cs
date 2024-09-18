using GameMath.Crane;
using UnityEngine;

public class TargetArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hookable item)) return;
        item.SetReleaseTarget(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Hookable item)) return;
        item.RemoveReleaseTarget();
    }
}
