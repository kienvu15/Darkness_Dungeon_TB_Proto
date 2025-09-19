using UnityEngine;

public class SkillSelectionManager : MonoBehaviour
{
    public static SkillSelectionManager Instance;

    private Character currentCaster;
    public SkillData pendingSkill;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Gọi khi player bấm vào 1 skill
    public void SelectSkill(Character caster, SkillData skill)
    {
        if (caster.IsOnCooldown(skill))
        {
            Debug.Log("Skill đang cooldown!");
            return;
        }

        if (caster.currentMana < skill.manaCost)
        {
            Debug.Log("Không đủ mana!");
            return;
        }

        currentCaster = caster;
        pendingSkill = skill;

        Debug.Log($"{caster.name} đã chọn skill {skill.skillName}, chọn target...");
        // TODO: bật UI chọn target
    }

    // Gọi khi player click vào 1 target
    public void SelectTarget(Character target)
    {
        if (currentCaster == null || pendingSkill == null)
        {
            Debug.LogWarning("Chưa chọn skill!");
            return;
        }

        // Kiểm tra hợp lệ
        foreach (var effect in pendingSkill.effects)
        {
            if (!effect.IsValidTarget(currentCaster, target))
            {
                Debug.Log("Target không hợp lệ cho skill này!");
                return;
            }
        }

        // Apply skill
        pendingSkill.Apply(currentCaster, target);

        Debug.Log($"{currentCaster.name} đã hành động → chờ 1s rồi end turn...");

        // Reset
        //StartCoroutine(EndTurnAfterDelay());

        // Reset
        currentCaster = null;
        pendingSkill = null;
    }

    private System.Collections.IEnumerator EndTurnAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        TurnManager.Instance.EndTurn();
    }

    public void CancelSelection()
    {
        currentCaster = null;
        pendingSkill = null;
        Debug.Log("Huỷ chọn skill");
    }
}
