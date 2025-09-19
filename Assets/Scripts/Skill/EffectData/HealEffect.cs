using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "RPG/Effects/HealEffect")]
public class HealEffect : ValueEffect
{
    public override void ApplyValue(Character caster, Character target, int finalValue)
    {
        if (target == null || target.isDead) return;

        target.Heal(finalValue);
        Debug.Log($"{caster.name} hồi {finalValue} máu cho {target.name}");
    }
}
