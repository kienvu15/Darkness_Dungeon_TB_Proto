using UnityEngine;

[CreateAssetMenu(fileName = "AttackEffect", menuName = "RPG/Effects/DealDamage")]
public class DealDamageEffect : ValueEffect
{
    public override void ApplyValue(Character caster, Character target, int finalValue)
    {
        if (target == null || target.isDead) return;

        target.TakeDamage(finalValue);
        Debug.Log($"{caster.name} gây {finalValue} damage lên {target.name}");
    }
}
