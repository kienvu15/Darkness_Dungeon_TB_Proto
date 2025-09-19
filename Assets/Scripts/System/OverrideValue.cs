using UnityEngine;

[System.Serializable]
public class OverrideValue
{
    public EffectData effect;   // Tham chiếu effect gốc
    public int overrideAmount;  // Giá trị ghi đè (vd: damage/heal/shield)

    public int GetValue()
    {
        return overrideAmount > 0 ? overrideAmount : effect.GetBaseValue();
    }
}
