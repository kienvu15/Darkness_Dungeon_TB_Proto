using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public static SkillPanel Instance;

    [Header("References")]
    public Transform skillContainer; // Grid chứa 4 skill display
    public GameObject skillDisplayPrefab;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowSkills(PlayableCharacter character)
    {
        // clear cũ
        foreach (Transform child in skillContainer)
        {
            Destroy(child.gameObject);
        }

        // spawn mới
        foreach (var skill in character.skills)
        {
            var go = Instantiate(skillDisplayPrefab, skillContainer);
            var display = go.GetComponent<SkillDisplay>();
            display.Setup(skill, character);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
