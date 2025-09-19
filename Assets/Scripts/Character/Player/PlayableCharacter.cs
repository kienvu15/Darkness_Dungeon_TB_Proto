using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayableCharacter : PlayerCharacter
{
    [Header("UI References")]
    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    public Slider manaSlider;
    public TextMeshProUGUI manaText;

    public GameObject Shield;
    public TextMeshProUGUI shieldText;

    private void Start()
    {
        // Khởi tạo UI
        hpSlider.maxValue = baseStats.maxHP;
        hpSlider.value = currentHP;

        manaSlider.maxValue = baseStats.maxMana;
        manaSlider.value = currentMana;
    
        
    }

    private void Update()
    {
        // HP
        hpSlider.value = currentHP;
        hpText.text = currentHP + " / " + baseStats.maxHP;

        // Mana
        manaSlider.value = currentMana;
        manaText.text = currentMana + " / " + baseStats.maxMana;

        // Shield
        shieldText.text = currentShield.ToString();
        Shield.SetActive(currentShield > 0);
    }


    public override void OnTurnStart()
    {
        base.OnTurnStart();

        // Bật panel skill cho nhân vật này
        SkillPanel.Instance.ShowSkills(this);
    }
}
