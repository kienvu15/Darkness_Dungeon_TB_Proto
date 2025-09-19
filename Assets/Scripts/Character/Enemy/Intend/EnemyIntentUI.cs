using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyIntentUI : MonoBehaviour
{
    [Header("References")]
    public GameObject intentPanel;
    public Image intentImage;
    public TMP_Text valueText;
    public IconSkillIntent iconDatabase;

    public void ShowIntent(EnemyAction action)
    {
        if (action == null || action.skill == null)
        {
            intentPanel.SetActive(false);
            return;
        }

        intentPanel.SetActive(true);

        // Lấy type từ SkillData
        SkillType type = action.skill.type;

        // Lấy icon từ database
        Sprite icon = iconDatabase.GetIcon(type);

        if (icon != null)
        {
            intentImage.sprite = icon;
            intentImage.enabled = true;
            Debug.Log($"Intent icon set: {type} -> {icon.name}");
        }
        else
        {
            intentImage.enabled = false;
            Debug.LogWarning($"Không tìm thấy icon cho type {type}");
        }

        // Lấy effect đầu tiên
        EffectData mainEffect = action.skill.effects[0];

        // Lấy override nếu có
        int displayValue = mainEffect.GetBaseValue();
        if (action.skill.overrides != null)
        {
            foreach (var o in action.skill.overrides)
            {
                if (o.effect == mainEffect)
                {
                    displayValue = o.overrideAmount;
                    break;
                }
            }
        }

        valueText.text = displayValue != 0 ? displayValue.ToString() : "";
    }


}
