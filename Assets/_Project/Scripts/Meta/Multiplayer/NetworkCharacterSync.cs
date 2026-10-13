using FirstProject.Core.Characters;
using FirstProject.Core.Characters.Effects;
using FirstProject.MatchupConfigs;
using FirstProject.Meta.Configs;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace FirstProject.Meta.Multiplayer
{
    public class NetworkCharacterSync : MonoBehaviour
    {
        private Character _character;
        private PhotonView _photonView;
        private RangedAttack _rangedAttack;
        private bool _isRanger;
        private IRemoteConfigService _remoteConfigService;

        [Inject]
        public void Construct(IRemoteConfigService configService)
        {
            _remoteConfigService = configService;
        }

        private void Awake()
        {
            _character = GetComponent<Character>();
            _photonView = GetComponent<PhotonView>();
            if (TryGetComponent(out RangedAttack rangedAttack))
            {
                _isRanger = true;
                _rangedAttack = rangedAttack;
            }
        }

        private void Start()
        {
            _character.DamageTaken += OnDamageTaken;
            _character.EffectApplied += OnEffectApplied;

            if (_isRanger)
            {
                _rangedAttack.RangerAttacked += OnRangerAttacked;
            }
        }

        private void OnDestroy()
        {
            if (_character != null)
            {
                _character.DamageTaken -= OnDamageTaken;
                _character.EffectApplied -= OnEffectApplied;

                if (_isRanger)
                {
                    _rangedAttack.RangerAttacked -= OnRangerAttacked;
                }
            }
        }

        private void OnRangerAttacked()
        {
            if (_photonView.IsMine)
            {
                _photonView.RPC(nameof(SpawnProjectileRPC), RpcTarget.Others);
            }
        }

        private void OnEffectApplied(CharacterApplicableEffect effect)
        {
            if (!_photonView.IsMine)
            {
                _photonView.RPC(nameof(ApplyEffectRPC), RpcTarget.Others, (int)effect.Type);
            }
        }

        private void OnDamageTaken(float finalDamage)
        {
            if (!_photonView.IsMine)
            {
                _photonView.RPC(nameof(ApplyDamageRPC), RpcTarget.Others, finalDamage);
            }
        }

        [PunRPC]
        private void ApplyEffectRPC(int effectTypeInt)
        {
            if (_photonView.IsMine)
            {
                EffectType type = (EffectType)effectTypeInt;
                CharacterApplicableEffect effectToApply = null;

                var effectsData = _remoteConfigService.Data.Effects;

                switch (type)
                {
                    case EffectType.Poison:
                        effectToApply = new PoisonEffect(effectsData.SlightPoison.Duration, effectsData.SlightPoison.Interval, effectsData.SlightPoison.TickDamage);
                        break;
                    case EffectType.Weakness:
                        effectToApply = new WeaknessEffect(effectsData.SlightWeakness.Duration, effectsData.SlightWeakness.Coefficient);
                        break;
                    case EffectType.Stun:
                        float stunDuration = _character.GetComponent<CharacterIdentity>().MyClass == CharacterClass.Warrior ? effectsData.MeleeStunDuration : effectsData.RangedStunDuration;
                        effectToApply = new StunEffect(stunDuration);
                        break;
                }

                if (effectToApply != null)
                {
                    _character.ApplyEffect(effectToApply);
                }
            }
        }

        [PunRPC]
        private void SpawnProjectileRPC()
        {
            _rangedAttack.SpawnVisualProjectile();
        }

        [PunRPC]
        private void ApplyDamageRPC(float damage)
        {
            if (_photonView.IsMine)
            {
                _character.TakeDamage(damage, CharacterClass.None);
            }
        }
    }
}
