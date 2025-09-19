using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [Header("Base Stats")]
    public int maxHP = 100;

    public int armor = 0;  

    public int shield = 0;

    public int speed = 10;      

    [Header("Mana / Resource")]
    public int maxMana = 50;
    public int manaRegen = 5;   // hồi bao nhiêu khi end turn
}
