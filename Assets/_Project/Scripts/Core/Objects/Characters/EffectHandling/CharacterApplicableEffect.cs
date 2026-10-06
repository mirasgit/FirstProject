using System.Collections;

namespace FirstProject.Core.Characters.Effects
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

        public virtual float[] EffectData()
        {
            float duration = 0f;
            float coefficient = 0f;
            float damage = 0f;

            return new float[] { duration, coefficient, damage};
        }

        public abstract IEnumerator Run(CharacterEffectContext context);

        public virtual void CancelEffect(CharacterEffectContext context)
        {

        }
    }
}