using UnityEngine;
using FirstProject.CharacterEffect;
using FirstProject.Characters.Attack;
using FirstProject.Common;

namespace FirstProject.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Character : MonoBehaviour
    {

        public CharacterStats Stats { get; private set; }
        public CharacterEffects Effects { get; private set; }
        public CharacterAnimator CharAnimator { get; private set; }
        public CharacterAttack Attack { get; private set; }
        public CharacterFacing Facing { get; private set; }
        public CharacterDeath Death { get; private set; }
        public bool BattleStarted { get; private set; } = false;
        [field: SerializeField] public bool IsDead => Death.IsDead;

        protected void Awake()
        {
            Stats = GetComponent<CharacterStats>();
            Effects = GetComponent<CharacterEffects>();
            CharAnimator = GetComponent<CharacterAnimator>();
            Attack = GetComponent<CharacterAttack>();
            Facing = GetComponent<CharacterFacing>();
            Death = GetComponent<CharacterDeath>();
        }
        public void InitializeEffects()
        {
            Effects.Initialize(this);
        }
        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            Stats.TakeDamage(damage);

            if (Stats.CurrentHealth <= CharacterConstants.AliveHealthThreshold || IsDead)
            {
                Die();
            }
        }

        public void ApplyEffect(CharacterApplicableEffect effect)
        {
            if (IsDead) return;

            Effects.Apply(effect);

        }

        public bool CanAttack()
        {
            return !IsDead && BattleStarted && !Effects.HasEffect(EffectType.Stun);
        }
        public void StartBattle()
        {
            BattleStarted = true;

        }
        public void ResetCharacterState()
        {
            Stats.ResetCharacterStats();
            //Effects.ResetEffects();
            BattleStarted = false;
        }
        private void Die()
        {
            Death.Die();
        }

        #region Facing
        public void SetFacingRight(bool facingRight)
        {
            Facing.SetFacingRight(facingRight);
        }
        public int FacingDirection() => Facing.FacingDirection;
        #endregion
    }
}