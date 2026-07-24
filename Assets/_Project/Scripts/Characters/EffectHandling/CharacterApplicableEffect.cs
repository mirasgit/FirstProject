using System.Collections;

namespace FirstProject.CharacterEffect
{
    public abstract class CharacterApplicableEffect 
    {
        public EffectType Type { get; private set; }
        public string Name { get; private set; }

        public CharacterApplicableEffect(EffectType type, string name)
        {
            Type = type;
            Name = name;
        }

        public abstract IEnumerator Run(CharacterEffectContext context);

        public virtual void CancelEffect(CharacterEffectContext context)
        {

        }
    }
}