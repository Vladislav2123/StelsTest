using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterMovement Movement { get; private set; }
    public CharacterAnimationHandler AnimationHandler { get; private set; }

    protected virtual void Awake()
    {
        Movement = GetComponent<CharacterMovement>();
        AnimationHandler = GetComponent<CharacterAnimationHandler>();
    }
}
