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
            _craneLeft.OnIsHeldChanged.AddListener((isHeld) 
                => { if (isHeld) OnCraneInputStart(-1); else OnCraneInputStop(); });
            _craneRight.OnIsHeldChanged.AddListener((isHeld)
                => { if (isHeld) OnCraneInputStart(1); else OnCraneInputStop(); });
            _cableControl.onValueChanged.AddListener((value) 
                => OnCableSliderValueChanged(value));
            _trolleyControl.onValueChanged.AddListener(value 
                => OnTrolleySliderValueChanged(value));
        }

        private void Start()
        {
            _cableControl.value = _hook.LengthPercent;
        }

        void OnCraneInputStart(int inputDirection)
        {
            _crane.StartSwinging(inputDirection);
        }

        void OnCraneInputStop()
        {
            _crane.StopSwinging();
        }

        void OnTrolleySliderValueChanged(float newValue)
        {
            _trolley.SetNewDollyTarget(newValue);
        }

        void OnCableSliderValueChanged(float newValue)
        {
            _hook.SetHeightTarget(newValue);
        }
    }
}
