using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterAnimationData : ScriptableObject
{
    [SerializeField] private SpriteLibrary _characterLibrary;

    [SerializeField] private AnimatorController _animatorController;
}
