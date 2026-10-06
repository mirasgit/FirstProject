using FirstProject.Core.Characters;
using FirstProject.Core.Characters.Effects;
using FirstProject.UI.Characters;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace FirstProject.Meta.Multiplayer
{
    public class NetworkCharacterInitializer : MonoBehaviour, IPunInstantiateMagicCallback
    {
        RemoteCharacterRegistry _characterRegistry;
        private Character _character;

        [Inject]
        public void Construct(RemoteCharacterRegistry characterRegistry)
        {
            _characterRegistry = characterRegistry;
        }

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] data = info.photonView.InstantiationData;

            if (data != null && data.Length > 0)
            {
                bool isFacingRight = (bool)data[0];
                float healthMultiplier = (float)data[1];
                float damageMultiplier = (float)data[2];
                float attackSpeedMultiplier = (float)data[3];

                _character.SetFacingRight(isFacingRight);

                _character.ApplyUpgrades(healthMultiplier, damageMultiplier, attackSpeedMultiplier);
            }   

            if (!info.photonView.IsMine)
            {
                CharacterView view = GetComponentInChildren<CharacterView>(true);
                CharacterPresenter presenter = new CharacterPresenter(view, _character);
                presenter.Subscribe();

                if (TryGetComponent(out CharacterMovement movement)) 
                {
                    movement.enabled = false;
                }
                if (TryGetComponent(out CharacterAttack attack))
                {
                    attack.enabled = false;
                }
                if (TryGetComponent(out Rigidbody2D rigidbody))
                {
                    rigidbody.isKinematic = true;
                }

                if (TryGetComponent(out CharacterEffects effects))
                {
                    effects.IsEffectProcessingEnabled = false;
                }

                _characterRegistry.Register(_character);
            }
        }
    }
}
