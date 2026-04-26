using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator _anim { get; private set; }

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }
}
