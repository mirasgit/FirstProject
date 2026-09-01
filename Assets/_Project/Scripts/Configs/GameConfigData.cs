using System;
using FirstProject.MatchupConfigs;

namespace FirstProject.Configs
{
    [Serializable]
    public class GameConfigData
    {
        public AdsSettings AdsConfig;
        public MatchupSettings[] Matchups;
        public UpgradeSettings Upgrades;
        public EffectsSettings Effects;
        public CharacterSettings[] Characters;
        public FloatingTextSettings FloatingTextSettings;
        public BattleSettings BattleSettings;
    }

    [Serializable]
    public class BattleSettings
    {
        public int WinReward;
    }

    [Serializable]
    public class ProjectileSettings
    {
        public int LifeSpanInSeconds;
        public int MoveSpeed;
    }

    [Serializable]
    public class FloatingTextSettings
    {
        public int LifeSpanInSeconds;
        public int MoveSpeed;
    }

    [Serializable]
    public class CharacterSettings
    {
        public CharacterClass ClassType;
        public float BaseHealth;
        public float BaseDamage;
        public float AttackCooldown;
        public float AttackSpeed;
        public int EffectProbabilityInPercent;
        public float MeleeAttackRadius;
        public float MoveSpeed;

        public ProjectileSettings Projectile;
    }

    [Serializable]
    public class AdsSettings
    {
        public int InterstitialInterval;
        public int RewardedAdReward;
    }

    [Serializable]
    public class MatchupSettings
    {
        public CharacterClass Attacker;
        public CharacterClass Defender;
        public float DamageMultiplier;
    }

    [Serializable]
    public class UpgradeSettings
    {
        public int BaseHealthCost;
        public int HealthCostStep;
        public int BaseDamageCost;
        public int DamageCostStep;
        public int BaseAttackSpeedCost;
        public int AttackSpeedCostStep;
        public float HealthMultiplierPerPurchase;
        public float DamageMultiplierPerPurchase;
        public float AttackSpeedMultiplierPerPurchase;
        public float DefaultMultiplier;
    }

    [Serializable]
    public class EffectsSettings
    {
        public float MeleeStunDuration;
        public float RangedStunDuration;
        public PoisonSettings SlightPoison;
        public WeaknessSettings SlightWeakness;
    }

    [Serializable]
    public class PoisonSettings
    {
        public float Duration;
        public float Interval;
        public float TickDamage;
    }

    [Serializable]
    public class WeaknessSettings
    {
        public float Duration;
        public float Coefficient;
    }
}