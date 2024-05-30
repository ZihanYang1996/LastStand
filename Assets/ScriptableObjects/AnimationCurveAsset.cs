using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewAnimationCurve", menuName = "AnimationCurve Asset", order = 0)]
    public class AnimationCurveAsset : ScriptableObject
    {
        public AnimationCurve curve;
    }
}