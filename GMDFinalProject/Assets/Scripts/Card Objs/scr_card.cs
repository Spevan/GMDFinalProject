using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "card", menuName = "Scriptable Sheets/Card")]
[System.Serializable]
public class scr_card : ScriptableObject
{
    public Sprite typeRarity;
    public enum rarityType
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }
    public rarityType rarity;
    public enum cardType
    {
        Orcland,
        Hartwood,
        Haevana,
        Steelridge
    }
    public cardType type;
    public int /*card_id,*/ count;
    public bool loaded;
    public GameObject unit;
    public int cost, power, health;
    public string description;
    public float maxCooldown;
    public List<scr_status> statuses = new List<scr_status>();
}