using UnityEngine;

[CreateAssetMenu(menuName = "Game/IconSkillIntent")]
public class IconSkillIntent : ScriptableObject
{
    [System.Serializable]
    public class IntentIconEntry
    {
        public SkillType type;
        public Sprite icon;
    }

    public IntentIconEntry[] icons;

    public Sprite GetIcon(SkillType type)
    {
        foreach (var entry in icons)
        {
            if (entry.type == type)
                return entry.icon;
        }
        return null;
    }
}
