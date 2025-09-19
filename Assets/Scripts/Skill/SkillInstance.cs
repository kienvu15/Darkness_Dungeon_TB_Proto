using UnityEngine;

/// <summary>
/// Runtime instance của skill để lưu cooldown riêng cho từng character
/// </summary>
[System.Serializable]
public class SkillInstance
{
    public SkillData data;
    private int cooldownRemaining = 0;

    public SkillInstance(SkillData skill)
    {
        data = skill;
        cooldownRemaining = 0;
    }

    public bool IsOnCooldown() => cooldownRemaining > 0;

    public void TickCooldown()
    {
        if (cooldownRemaining > 0)
            cooldownRemaining--;
    }

    /// <summary>
    /// Apply skill lên target, giảm mana và bắt đầu cooldown
    /// </summary>
    public void Apply(Character caster, Character target)
    {
        if (IsOnCooldown())
        {
            Debug.LogWarning($"{caster.name} không thể dùng skill {data.skillName}, đang cooldown!");
            return;
        }

        if (!caster.UseMana(data.manaCost))
        {
            Debug.LogWarning($"{caster.name} không đủ mana để dùng {data.skillName}!");
            return;
        }

        foreach (var effect in data.effects)
        {
            effect.Apply(caster, target);
        }

        cooldownRemaining = data.cooldown;
        Debug.Log($"{caster.name} dùng skill {data.skillName} → cooldown {data.cooldown}");
    }
}
