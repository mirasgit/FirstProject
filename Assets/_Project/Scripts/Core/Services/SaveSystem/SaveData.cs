using System;

namespace FirstProject.Core.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public int Coins;
        public int HealthLevel;
        public int DamageLevel;
        public int AttackSpeedLevel;
        public bool IsAdsRemoved;
        public long LastSaveTimeTicks;
    }

}