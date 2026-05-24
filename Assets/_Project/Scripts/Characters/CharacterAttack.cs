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
        protected Random _random = new Random();
        private void Awake()
        {
            _character = GetComponent<Character>();
        }
        public void InitializeProjectileRegistry(ProjectileRegistry projectileRegistry)
        {
            _projectileRegistry = projectileRegistry;
        }
    }
}