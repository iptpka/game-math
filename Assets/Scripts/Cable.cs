using UnityEngine;

namespace GameMath.Crane
{
    public class Cable : MonoBehaviour
    {
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        private LineRenderer _lineRenderer;
        private Vector3 _relativeToStart;
        private Vector3 _relativeToEnd;


        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            Vector3[] positions = new Vector3[2];
            _lineRenderer.GetPositions(positions);
            _relativeToStart = _start.InverseTransformPoint(positions[0]);
            _relativeToEnd = _end.InverseTransformPoint(positions[1]);
        }

        void LateUpdate()
        {
            _lineRenderer.SetPositions(new[] { _start.TransformPoint(_relativeToStart), _end.TransformPoint(_relativeToEnd) });
        }
    }
}
