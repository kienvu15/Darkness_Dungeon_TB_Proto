using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [Header("Skills")]
    public List<SkillData> skills;

    //public bool CanUseSkill(int index)
    //{
    //    if (index < 0 || index >= skills.Count) return false;
    //    SkillData skill = skills[index];
    //    return currentMana >= skill.manaCost && !IsOnCooldown(skill);
    //}

}
