using UnityEngine;
using FirstProject.Projectiles;
using FirstProject.CharacterEffect;
namespace FirstProject.Characters.Attack
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected LayerMask _whatIsTarget;
        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] protected float _attackSpeed = 1f;

        protected CharacterStats _stats;
        protected CharacterFacing _facing;
        protected CharacterAnimator _characterAnimator;
        protected CharacterDeath _death;
        protected CharacterEffects _effects;
        protected ProjectileRegistry _projectileRegistry;
        protected const float TO_PERCENT_MULTIPLIER = 100f;
        private bool _isAllowedToFight;
        protected float _lastAttackTime;

        public void Initialize(
            CharacterStats stats,
            CharacterFacing facing,
            CharacterAnimator characterAnimator,
            CharacterDeath death,
            CharacterEffects effects, ProjectileRegistry projectileRegistry)
        {
            _stats = stats;
            _facing = facing;
            _characterAnimator = characterAnimator;
            _death = death;
            _effects = effects;
            _projectileRegistry = projectileRegistry;
        }

        protected virtual void Update()
        {
            HandleAttack();
        }

        public void AllowToFight()
        {
            _isAllowedToFight = true;
        }

        public void DisallowToFight()
        {
            _isAllowedToFight = false;
        }

        protected bool CanAttack()
        {
            return !_death.IsDead && _isAllowedToFight && !_effects.HasEffect(EffectType.Stun);
        }

        private void PlayAttack()
        {
            _characterAnimator.PlayAttack();
        }

        protected void SetAttackSpeed(float speed)
        {
            _characterAnimator.SetAttackSpeed(speed);
        }

        protected virtual void HandleAttack()
        {
            if (!CanAttack())
            {
                return;
            }

            if (Time.time < _lastAttackTime + _attackCooldown)
            {
                return;
            }

            _lastAttackTime = Time.time;

            SetAttackSpeed(_attackSpeed);
            PlayAttack();
        }
    }
}