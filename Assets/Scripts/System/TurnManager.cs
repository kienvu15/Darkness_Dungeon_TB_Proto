using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Character> turnOrder = new List<Character>();
    private int currentIndex = 0;
    private Character Current => turnOrder[currentIndex];

    public static TurnManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // tránh trùng singleton
    }

    private void Start()
    {
        turnOrder = FindObjectsByType<Character>(FindObjectsSortMode.None).ToList();

        if (turnOrder.Count > 0)
        {
            StartTurn();
        }
    }

    private void StartTurn()
    {
        Debug.Log($"--- {Current.name}'s Turn ---");

        // bật marker cho Current
        foreach (var c in turnOrder)
        {
            if (c.turnMarker != null)
                c.turnMarker.SetActive(c == Current);
        }

        Current.OnTurnStart();
    }


    public void EndTurn()
    {
        Debug.Log($"{Current.name} đã kết thúc lượt!");
        Current.RegenMana();

        // Tắt marker của Current
        if (Current.turnMarker != null)
            Current.turnMarker.SetActive(false);

        // chuyển lượt
        currentIndex = (currentIndex + 1) % turnOrder.Count;
        StartTurn();
    }
}
