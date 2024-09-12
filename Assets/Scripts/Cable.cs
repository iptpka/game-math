using UnityEngine;

namespace GameMath.Crane
{
    public class Cable : MonoBehaviour
    {
        [SerializeField] private Transform _trolley;
        [SerializeField] private Transform _hook;
        private LineRenderer _lineRenderer;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void LateUpdate()
        {
            _lineRenderer.SetPositions(new[] { _trolley.position, _hook.position });
        }
    }
}
