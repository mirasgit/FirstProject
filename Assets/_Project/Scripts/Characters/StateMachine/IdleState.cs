

public class IdleState : CharacterState
{
    protected IdleState(Character character, CharacterStateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        Character.ResetCharacterState();
        Character.CharAnimator._anim.SetTrigger("Idle");
    }
}
