using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class SkillDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text skillName;
    public Image iconImage;
    public TMP_Text manaText;
    public TMP_Text cooldownText;

    [Header("Skill Icons")]
    public IconSkillIntent intentIcons;
    public Image intentIconImage;
    public TMP_Text valueText;

    public Button infoButton;
    public Button skillButton;

    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    private SkillData currentSkill;
    private PlayableCharacter owner;

    private void Start()
    {
        if (infoButton != null)
        {
            infoButton.onClick.AddListener(ToggleTooltip);
        }
        tooltipPanel.SetActive(false);
    }

    public void Setup(SkillData skill, PlayableCharacter character)
    {
        currentSkill = skill;
        owner = character;

        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(OnSkillClicked);

        iconImage.sprite = skill.img;

        if (intentIcons != null && intentIconImage != null)
        {
            var typeIcon = intentIcons.GetIcon(skill.type);
            intentIconImage.sprite = typeIcon != null ? typeIcon : null;
        }

        manaText.text = $"{skill.manaCost}";
        cooldownText.text = $"{skill.cooldown}";
        valueText.text = $"{GetDisplayValue()}"; // hiển thị giá trị trực tiếp

        // Gán tooltip bằng description
        if (tooltipText != null)
            tooltipText.text = currentSkill.description;
        skillName.text = currentSkill.skillName;
    }

    private void OnSkillClicked()
    {
        SkillSelectionManager.Instance.SelectSkill(owner, currentSkill);
        HighlightSelected();
    }

    private void HighlightSelected()
    {
        transform.localScale = Vector3.one; 
        transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);

        //transform.DOLocalMoveY(transform.localPosition.y + 15f, 0.2f);
    }

    public void ResetHighlight()
    {
        transform.localScale = Vector3.one;
    }

    private void ToggleTooltip()
    {
        tooltipPanel.SetActive(!tooltipPanel.activeSelf);
    }

    private int GetDisplayValue()
    {
        if (currentSkill.effects != null && currentSkill.effects.Length > 0)
        {
            var effect = currentSkill.effects[0]; // lấy effect đầu tiên để hiển thị
            foreach (var o in currentSkill.overrides)
            {
                if (o.effect == effect)
                    return o.overrideAmount;
            }
            return effect.GetBaseValue();
        }
        return 0;
    }

}
