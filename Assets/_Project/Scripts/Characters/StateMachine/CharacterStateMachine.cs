
public class CharacterStateMachine
{
    private CharacterState _currentState;
    public CharacterState CurrentState => _currentState;

    public void Initialize(CharacterState state)
    {
        _currentState = state;
        _currentState.Enter();
    }

    public void ChangeState(CharacterState newState)
    {
        if(_currentState == newState) { return; }

        _currentState.Exit(); 
        
        _currentState = newState;

        _currentState.Enter();
    }
}