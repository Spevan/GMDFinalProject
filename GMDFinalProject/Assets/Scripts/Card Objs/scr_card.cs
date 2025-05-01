using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Scriptable Sheets/card")]
public class scr_card : ScriptableObject
{
    public GameObject unit;
    public int cost, power, health, range;
    public string description;
    public float maxCooldown;
    public enum statusType
    {
        poison,
        munitions,
        ironclad
    }
    public int statusAmnt;
    public enum costType
    {
        water,
        steel
    }
    public scr_player player;
}