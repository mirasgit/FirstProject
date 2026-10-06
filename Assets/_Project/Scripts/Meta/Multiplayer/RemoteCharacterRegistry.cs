using FirstProject.Core.Characters;
using System;

namespace FirstProject.Meta.Multiplayer
{
    public class RemoteCharacterRegistry 
    {
        public event Action<Character> RemoteCharacterSpawned;
        public Character CurrentRemoteCharacter { get; private set; }   

        public void Register(Character character)
        {
            CurrentRemoteCharacter = character;
            RemoteCharacterSpawned?.Invoke(character);
        }

        public void Clear()
        {
            CurrentRemoteCharacter = null;
        }
    }
}
