using UnityEngine;

public class CharacterState : MonoBehaviour
{
    protected readonly Character Character;
    protected readonly CharacterStateMachine StateMachine;

    protected CharacterState(Character character, CharacterStateMachine stateMachine)
    {
        Character = character;
        StateMachine = stateMachine;
    }
    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

}
