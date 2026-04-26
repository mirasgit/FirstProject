using UnityEngine;
using Random = System.Random;
public class CharacterAttack : MonoBehaviour
{
    [SerializeField] protected Character _character;
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] protected LayerMask _whatIsTarget;

    protected Random _random = new Random();
    private void Awake()
    {
        _character = GetComponent<Character>();
    }

}
