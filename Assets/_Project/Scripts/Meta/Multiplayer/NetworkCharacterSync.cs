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
                _photonView.RPC(nameof(ApplyEffectRPC), RpcTarget.Others, (int)effect.Type, effect.EffectData());
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
        private void ApplyEffectRPC(int effectTypeInt, float[] effectData)
        {
            if (_photonView.IsMine)
            {
                EffectType type = (EffectType)effectTypeInt;
                CharacterApplicableEffect effectToApply = null;


                switch (type)
                {
                    case EffectType.Poison:
                        effectToApply = new PoisonEffect(effectData[0], effectData[1], effectData[2]);
                        break;
                    case EffectType.Weakness:
                        effectToApply = new WeaknessEffect(effectData[0], effectData[1]);
                        break;
                    case EffectType.Stun:
                        effectToApply = new StunEffect(effectData[0]);
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
