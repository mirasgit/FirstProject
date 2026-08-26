using UnityEngine;
using Zenject;
using FirstProject.CharacterEffect;
using FirstProject.CharacterEffect.Configs;

namespace FirstProject.Characters
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected LayerMask _whatIsTarget;
        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] protected float _attackSpeed = 1f;
        [SerializeField] protected EffectConfig _effectConfig;
        [SerializeField, Range(0, 100)] protected int _applyProbabilityPercent;

        protected CharacterStats _stats;
        protected CharacterFacing _facing;
        protected CharacterAnimator _characterAnimator;
        protected CharacterDeath _death;
        protected CharacterEffects _effects;
        protected const float TO_PERCENT_MULTIPLIER = 100f;
        private bool _isAllowedToFight;
        protected float _lastAttackTime;

        [Inject]
        public void Construct(
            CharacterStats stats,
            CharacterFacing facing,
            CharacterAnimator characterAnimator,
            CharacterDeath death,
            CharacterEffects effects)
        {
            _stats = stats;
            _facing = facing;
            _characterAnimator = characterAnimator;
            _death = death;
            _effects = effects;
        }

        protected virtual void Update()
        {
            HandleAttack();
        }

        public void ApplyMultiplier(float multiplier)
        {
            _attackSpeed = _attackSpeed + (_attackSpeed * multiplier);
            SetAttackSpeed(_attackSpeed);
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

            float effectiveCoolDown = _attackCooldown / _attackSpeed;

            if (Time.time < _lastAttackTime + effectiveCoolDown)
            {
                return;
            }

            _lastAttackTime = Time.time;

            SetAttackSpeed(_attackSpeed);
            PlayAttack();
        }

        protected CharacterApplicableEffect TryGetEffect()
        {
            if (_effectConfig != null && Random.value <= _applyProbabilityPercent / TO_PERCENT_MULTIPLIER)
            {
                return _effectConfig.CreateEffect();
            }

            return null;
        }
    }
}