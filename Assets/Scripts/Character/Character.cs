using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int teamID;

    [Header("Stats")]
    public CharacterStats baseStats = new CharacterStats();

    [Header("Runtime Values")]
    public int currentHP;
    public int currentMana;
    public int currentShield;

    public bool isDead = false;

    [Header("Turn UI")]
    public GameObject turnMarker;

    private void Awake()
    {
        // khởi tạo
        currentHP = baseStats.maxHP;
        currentMana = baseStats.maxMana;
        currentShield = 0;
    }

    void Start()
    {
        if (turnMarker != null)
            turnMarker.SetActive(false);
    }

    #region HP / Damage
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int remainingDamage = damage;

        // 1. Trừ vào shield trước
        if (currentShield > 0)
        {
            int absorbed = Mathf.Min(currentShield, remainingDamage);
            currentShield -= absorbed;
            remainingDamage -= absorbed;
            Debug.Log($"{name}'s shield absorbed {absorbed} damage!");
        }

        // 2. Nếu còn damage thì trừ vào máu (có armor)
        if (remainingDamage > 0)
        {
            int finalDamage = Mathf.Max(0, remainingDamage - baseStats.armor);
            currentHP -= finalDamage;
            Debug.Log($"{name} took {finalDamage} damage!");
        }

        // 3. Check chết
        if (currentHP <= 0)
        {
            Die();
        }
    }


    public void Heal(int amount)
    {
        if (isDead) return;
        currentHP = Mathf.Min(currentHP + amount, baseStats.maxHP);
        Debug.Log($"{name} healed {amount} HP!");
    }
    #endregion

    public void AddShield(int amount)
    {
        currentShield += amount;
        Debug.Log($"{name} gained {amount} armor!");
        // Note: Shield duration and removal logic should be handled elsewhere
    }

    #region Mana
    public bool UseMana(int cost)
    {
        if (currentMana < cost) return false;
        currentMana -= cost;
        return true;
    }

    public void RegenMana()
    {
        currentMana = Mathf.Min(currentMana + baseStats.manaRegen, baseStats.maxMana);
    }
    #endregion

    private void Die()
    {
        isDead = true;
        Debug.Log($"{name} has died.");
        // TODO: Animation, remove from turn order, v.v.
    }


    // thêm vào trong Character class
    protected Dictionary<SkillData, int> cooldownDict = new Dictionary<SkillData, int>();

    public virtual void OnTurnStart()
    {
        TickCooldowns();
    }

    public bool IsOnCooldown(SkillData skill)
    {
        return cooldownDict.ContainsKey(skill) && cooldownDict[skill] > 0;
    }

    public void SetCooldown(SkillData skill, int turns)
    {
        cooldownDict[skill] = turns;
    }

    public void TickCooldowns()
    {
        List<SkillData> keys = new List<SkillData>(cooldownDict.Keys);
        foreach (var k in keys)
        {
            if (cooldownDict[k] > 0) cooldownDict[k]--;
        }
    }

}

