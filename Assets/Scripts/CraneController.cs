using GameMath.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameMath.Crane
{
    public class CraneController : MonoBehaviour
    {
        [SerializeField] private HoldableButton _craneLeft;
        [SerializeField] private HoldableButton _craneRight;
        [SerializeField] private Slider _trolleyControl;
        [SerializeField] private Slider _cableControl;
        [SerializeField] private Crane _crane;
        [SerializeField] private Trolley _trolley;
        [SerializeField] private Hook _hook;

        void Awake()
        {
            _craneLeft.OnIsHeldChanged.AddListener((isHeld) => { Debug.Log($"Crane left {(isHeld ? "started" : "stopped")} holding"); });
            _craneRight.OnIsHeldChanged.AddListener((isHeld) => { Debug.Log($"Crane right {(isHeld ? "started" : "stopped")} holding"); });
            _cableControl.onValueChanged.AddListener((value) => { Debug.Log($"Cable slider at: {value}"); });
            _trolleyControl.onValueChanged.AddListener(value => { Debug.Log($"Trolley slider at: {value}"); });
        }
    }
}
