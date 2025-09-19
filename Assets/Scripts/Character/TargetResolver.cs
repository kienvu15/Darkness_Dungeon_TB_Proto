using UnityEngine;
using System.Linq;

public static class TargetResolver
{
    public static Character Resolve(Character caster, Character chosenTarget, EffectData effect)
    {
        switch (effect.targetType)
        {
            case EffectTargetType.Self:
                return caster;

            case EffectTargetType.Ally:
                if (chosenTarget != null &&
                    chosenTarget.teamID == caster.teamID &&
                    !chosenTarget.isDead)
                    return chosenTarget;

                return FindAlly(caster);

            case EffectTargetType.Enemy:
                if (chosenTarget != null &&
                    chosenTarget.teamID != caster.teamID &&
                    !chosenTarget.isDead)
                    return chosenTarget;

                return FindEnemy(caster);
        }
        return null;
    }

    private static Character FindAlly(Character caster)
    {
        var all = GameObject.FindObjectsByType<Character>(FindObjectsSortMode.None);
        return all.FirstOrDefault(c =>
            c != caster &&
            c.teamID == caster.teamID &&
            !c.isDead);
    }

    private static Character FindEnemy(Character caster)
    {
        var all = GameObject.FindObjectsByType<Character>(FindObjectsSortMode.None);
        return all.FirstOrDefault(c =>
            c.teamID != caster.teamID &&
            !c.isDead);
    }
}
