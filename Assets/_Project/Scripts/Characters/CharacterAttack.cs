using UnityEngine;
using Zenject;
using FirstProject.CharacterEffect;
using FirstProject.Configs;
using FirstProject.MatchupConfigs;
namespace FirstProject.Characters
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected LayerMask _whatIsTarget;
        [SerializeField] protected EffectType _effectType;
        protected int _applyProbabilityPercent;
        protected float _attackCooldown;
        protected float _attackSpeed;

        protected RemoteConfigService _configService;
        protected CharacterStats _stats;
        protected CharacterIdentity _identity;
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
            CharacterIdentity identity,
            CharacterFacing facing,
            CharacterAnimator characterAnimator,
            CharacterDeath death,
            CharacterEffects effects, RemoteConfigService configService)
        {
            _stats = stats;
            _identity = identity;
            _facing = facing;
            _characterAnimator = characterAnimator;
            _death = death;
            _effects = effects;
            _configService = configService;

            var config = _configService.GetCharacterConfig(_identity.MyClass);

            _attackCooldown = config.AttackCooldown;
            _attackSpeed = config.AttackSpeed;
            _applyProbabilityPercent = config.EffectProbabilityInPercent;
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
            if (_configService == null || Random.value > _applyProbabilityPercent / TO_PERCENT_MULTIPLIER)
            {
                return null;
            }

            var effectsData = _configService.Data.Effects;

            switch (_effectType)
            {
                case EffectType.Poison:
                    return new PoisonEffect(effectsData.SlightPoison.Duration, effectsData.SlightPoison.Interval, effectsData.SlightPoison.TickDamage);

                case EffectType.Weakness:
                    return new WeaknessEffect(effectsData.SlightWeakness.Duration, effectsData.SlightWeakness.Coefficient);

                case EffectType.Stun:
                    float stunDuration = _identity.MyClass == CharacterClass.Warrior? effectsData.MeleeStunDuration: effectsData.RangedStunDuration;
                    return new StunEffect(stunDuration);

                default: 
                    return null;
            }
        }
    }
}