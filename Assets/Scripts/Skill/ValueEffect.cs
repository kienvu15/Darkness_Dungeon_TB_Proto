using UnityEngine;

public abstract class ValueEffect : EffectData
{
    public int value;

    public override void Apply(Character caster, Character target)
    {
        ApplyValue(caster, target, value);
    }

    public abstract void ApplyValue(Character caster, Character target, int finalValue);

    public override int GetBaseValue() => value;
}
