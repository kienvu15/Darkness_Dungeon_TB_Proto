using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Targetable : MonoBehaviour, IPointerClickHandler
{
    public Character targetCharacter;   // assign trong Inspector hoặc runtime

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SkillSelectionManager.Instance != null && targetCharacter != null)
        {
            SkillSelectionManager.Instance.SelectTarget(targetCharacter);
        }
    }
}
