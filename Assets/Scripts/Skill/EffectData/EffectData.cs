using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    [TextArea] public string description;
    public EffectTargetType targetType;
    public SkillType actionType;
    public abstract void Apply(Character caster, Character target);

    public virtual int GetBaseValue()
    {
        return 0; // mặc định, subclass override
    }

    public virtual Character GetTarget(Character caster, Character chosenTarget)
    {
        return TargetResolver.Resolve(caster, chosenTarget, this);
    }

    public virtual bool IsValidTarget(Character caster, Character target)
    {
        return target != null;
    }

}
