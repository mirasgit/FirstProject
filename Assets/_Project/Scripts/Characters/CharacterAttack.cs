using UnityEngine;
using FirstProject.Projectiles;
using FirstProject.CharacterEffect;
using FirstProject.Battle;

namespace FirstProject.Characters.Attack
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected LayerMask _whatIsTarget;
        [SerializeField] protected float _attackCooldown = 1f;

        protected CharacterStats _stats;
        protected CharacterFacing _facing;
        protected CharacterAnimator _charAnimator;
        protected CharacterDeath _death;
        protected CharacterEffects _effects;
        protected ProjectileRegistry _projectileRegistry;

        private bool _isAllowedToFight;
        private float _lastAttackTime;

        public void Initialize(
            CharacterStats stats,
            CharacterFacing facing,
            CharacterAnimator characterAnimator,
            CharacterDeath death,
            CharacterEffects effects, ProjectileRegistry projectileRegistry)
        {
            _stats = stats;
            _facing = facing;
            _charAnimator = characterAnimator;
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
            _charAnimator.PlayAttack();
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
            PlayAttack();
        }
    }
}