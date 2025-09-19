using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyAction")]
public class EnemyAction : ScriptableObject
{
    public string actionName;
    public SkillData skill;

    public void Execute(Character caster, Character chosenTarget)
    {
        if (skill == null || skill.effects.Length == 0) return;

        foreach (var effect in skill.effects)
        {
            int finalValue = GetOverrideValue(effect);

            Character actualTarget = TargetResolver.Resolve(caster, chosenTarget, effect);
            if (actualTarget == null)
            {
                Debug.LogWarning($"{caster.name} dùng {skill.skillName} nhưng không có target hợp lệ cho {effect.name}");
                return; // huỷ toàn bộ action
            }

            if (effect is ValueEffect ve)
            {
                int backup = ve.value;
                ve.value = finalValue;
                ve.Apply(caster, actualTarget);
                ve.value = backup;
            }
            else
            {
                effect.Apply(caster, actualTarget);
            }
        }

        if (skill.cooldown > 0)
        {
            caster.SetCooldown(skill, skill.cooldown);
        }
    }

    private int GetOverrideValue(EffectData effect)
    {
        if (skill.overrides != null)
        {
            foreach (var o in skill.overrides)
            {
                if (o.effect == effect)
                    return o.overrideAmount;
            }
        }
        return effect.GetBaseValue();
    }
}
