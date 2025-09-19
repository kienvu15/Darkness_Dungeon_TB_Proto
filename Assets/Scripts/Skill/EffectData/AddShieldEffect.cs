using UnityEngine;

[CreateAssetMenu(fileName ="AddShieldEffect" ,menuName = "RPG/Effects/AddShield")]
public class AddShieldEffect : ValueEffect
{
    public override void ApplyValue(Character caster, Character target, int finalValue)
    {
        if (target == null || target.isDead) return;

        target.AddShield(finalValue);
        Debug.Log($"{caster.name} cast {name}: {target.name} nhận {finalValue} shield");
    }
}
