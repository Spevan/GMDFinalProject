using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "card", menuName = "Scriptable Sheets/Card")]
[System.Serializable]
public class scr_card : ScriptableObject
{
    public enum rarityType
    {
        common,
        uncommon,
        rare,
        legendary
    }
    public rarityType rarity;
    public enum cardType
    {
        orcland,
        hartwood,
        haevana,
        steelridge
    }
    public cardType type;
    public int card_id, count;
    public bool loaded;
    public GameObject unit;
    public int cost, power, health;
    public string description;
    public float maxCooldown;
    public List<scr_status> statuses = new List<scr_status>();
}