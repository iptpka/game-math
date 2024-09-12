using GameMath.Util;
using UnityEngine;

namespace GameMath.Crane
{
    public class Trolley : Parentable
    {
        [SerializeField] private Transform _nearLimit;
        [SerializeField] private Transform _farLimit;
    }
}
