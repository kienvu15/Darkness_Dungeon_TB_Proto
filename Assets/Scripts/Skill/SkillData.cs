using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Attack,
    Defense,
    Buff,
    Debuff,
    Special
}

[CreateAssetMenu(menuName = "RPG/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Basic Info")]
    public string skillName;
    public Sprite img;
    [TextArea] public string description;

    [Header("Skill Type")]
    public SkillType type;

    [Header("Overrides (optional)")]
    public List<OverrideValue> overrides = new List<OverrideValue>();

    [Header("Skill Properties")]
    public int manaCost;
    public int cooldown;

    [Header("Effects")]
    public EffectData[] effects;

    public void Apply(Character caster, Character chosenTarget)
    {
        // 1. Check mana trước
        if (!caster.UseMana(manaCost))
        {
            Debug.Log($"{caster.name} không đủ mana để dùng {skillName}");
            return;
        }

        // 2. Resolve target cho tất cả effect
        Character[] resolvedTargets = new Character[effects.Length];
        for (int i = 0; i < effects.Length; i++)
        {
            resolvedTargets[i] = TargetResolver.Resolve(caster, chosenTarget, effects[i]);
            if (resolvedTargets[i] == null)
            {
                Debug.LogWarning($"{skillName} bị hủy: target không hợp lệ cho {effects[i].name}");
                return; // hủy toàn bộ skill, không mất mana, không cooldown
            }
        }

        // 3. Apply effect khi target hợp lệ
        for (int i = 0; i < effects.Length; i++)
        {
            var effect = effects[i];
            var resolvedTarget = resolvedTargets[i];
            int valueToUse = GetOverrideValue(effect);

            if (effect is ValueEffect ve)
            {
                int backup = ve.value;
                ve.value = valueToUse;
                ve.Apply(caster, resolvedTarget);
                ve.value = backup;
            }
            else
            {
                effect.Apply(caster, resolvedTarget);
            }
        }

        // 4. Set cooldown cuối cùng
        caster.SetCooldown(this, cooldown);
    }

    private int GetOverrideValue(EffectData effect)
    {
        foreach (var o in overrides)
            if (o.effect == effect)
                return o.overrideAmount;

        return effect.GetBaseValue(); // fallback về giá trị gốc
    }
}
