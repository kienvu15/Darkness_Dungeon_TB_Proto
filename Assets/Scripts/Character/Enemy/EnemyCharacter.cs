using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EnemyCharacter : Character
{
    [Header("UI References")]
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public Slider manaSlider;
    public TextMeshProUGUI manaText;
    public GameObject Shield;
    public TextMeshProUGUI shieldText;

    [Header("Enemy Settings")]
    public List<EnemyAction> actions;      // list tất cả action có thể dùng
    public EnemyIntentUI intentUI;

    [Header("AI Settings")]
    public bool useRandomAction = true;    // true = random, false = sequential
    private int currentSequentialIndex = 0;

    private EnemyAction nextAction;
    private Character nextTarget;

    // cooldown map: SkillData -> remaining turns
    private Dictionary<SkillData, int> skillCooldowns = new Dictionary<SkillData, int>();

    private void Start()
    {
        PeekNextAction(); // chuẩn bị action cho lượt đầu tiên
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        // Giảm cooldown tất cả skill
        UpdateCooldowns();

        Debug.Log($"--- {name}'s Turn (ENEMY) ---");

        if (actions.Count == 0) return;

        if (nextAction == null)
        {
            Debug.LogWarning($"{name} không còn action nào hợp lệ!");
            StartCoroutine(EndTurnAfterDelay());
            return;
        }

        // chọn target hợp lệ
        List<PlayerCharacter> validTargets = new List<PlayerCharacter>(FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None));
        validTargets.RemoveAll(t => t.isDead);
        if (validTargets.Count == 0) return;

        nextTarget = validTargets[Random.Range(0, validTargets.Count)];

        // cập nhật intent UI
        if (intentUI != null)
            intentUI.ShowIntent(nextAction);

        // thực hiện action
        ExecuteCurrentIntent();
    }

    public void PeekNextAction()
    {
        nextAction = PickNextAction();
        if (nextAction == null) return;

        List<PlayerCharacter> validTargets = new List<PlayerCharacter>(FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None));
        validTargets.RemoveAll(t => t.isDead);
        if (validTargets.Count == 0) return;

        nextTarget = validTargets[Random.Range(0, validTargets.Count)];

        if (intentUI != null)
            intentUI.ShowIntent(nextAction); // chỉ show action hợp lệ
    }



    private void UpdateCooldowns()
    {
        List<SkillData> keys = new List<SkillData>(skillCooldowns.Keys);
        foreach (var key in keys)
        {
            if (skillCooldowns[key] > 0)
                skillCooldowns[key]--;
        }
    }

    private EnemyAction PickNextAction()
    {
        if (useRandomAction)
        {
            // Random until find one not on cooldown
            List<EnemyAction> validActions = actions.FindAll(a => !IsOnCooldown(a));
            if (validActions.Count == 0) return null;
            return validActions[Random.Range(0, validActions.Count)];
        }
        else
        {
            // Sequential: bỏ qua action đang cooldown
            int startingIndex = currentSequentialIndex;
            do
            {
                EnemyAction action = actions[currentSequentialIndex];
                currentSequentialIndex = (currentSequentialIndex + 1) % actions.Count;

                if (!IsOnCooldown(action))
                    return action;

            } while (currentSequentialIndex != startingIndex);

            return null;
        }
    }


    private bool IsOnCooldown(EnemyAction action)
    {
        if (action == null || action.skill == null) return true;
        return skillCooldowns.ContainsKey(action.skill) && skillCooldowns[action.skill] > 0;
    }

    private void ApplyCooldown(EnemyAction action)
    {
        if (action == null || action.skill == null) return;
        if (action.skill.cooldown > 0)
            skillCooldowns[action.skill] = action.skill.cooldown;
    }

    private void ExecuteCurrentIntent()
    {
        if (nextAction != null && nextTarget != null)
        {
            Debug.Log($"{name} thực hiện {nextAction.skill.skillName} lên {nextTarget.name}");
            nextAction.Execute(this, nextTarget);

            // set cooldown
            ApplyCooldown(nextAction);
        }

        StartCoroutine(EndTurnAfterDelay());
    }

    private System.Collections.IEnumerator EndTurnAfterDelay()
    {
        PeekNextAction(); // chuẩn bị action cho lượt sau
        yield return new WaitForSeconds(1f);
        //turnMarker.SetActive(false);
        TurnManager.Instance.EndTurn();
    }

    private void Update()
    {
        hpSlider.value = currentHP;
        hpText.text = currentHP + " / " + baseStats.maxHP;

        manaSlider.value = currentMana;
        manaText.text = currentMana + " / " + baseStats.maxMana;

        shieldText.text = currentShield.ToString();
        Shield.SetActive(currentShield > 0);
    }

    public EnemyAction GetNextAction() => nextAction;
    public Character GetNextTarget() => nextTarget;
}
