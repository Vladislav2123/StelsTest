using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    private CharacterMovement _movement;

    private const string MOVING_BOOL = "IsMoving";
    private const string DEFEAT_TRIGGER = "Defeat";

    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        _movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        HandleMovingAnimation();
    }

    private void HandleMovingAnimation()
    {
        Animator.SetBool(MOVING_BOOL, _movement.IsMoving);
    }

    public void PlayDefeatAnimation()
    {
        Animator.SetTrigger(DEFEAT_TRIGGER);
    }
}
