using UnityEngine;
using Random = System.Random;
using FirstProject.Projectiles;

namespace FirstProject.Characters.Attack
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] protected Character _character;
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected LayerMask _whatIsTarget;

        protected ProjectileRegistry _projectileRegistry { get; private set; }
        protected Random _random = new();
        private void Awake()
        {
            _character = GetComponent<Character>();
        }
        protected virtual void Update()
        {
            HandleAttack();
        }
        protected virtual void HandleAttack()
        {
            if (_character.BattleStarted)
            {
                if (_character.CanAttack() && !_character.IsDead)
                {
                    _character.PlayAttack();
                }
            }
        }
        public void InitializeProjectileRegistry(ProjectileRegistry projectileRegistry)
        {
            _projectileRegistry = projectileRegistry;
        }
    }
}