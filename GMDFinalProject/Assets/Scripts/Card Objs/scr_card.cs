using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Scriptable Sheets/card")]
public class scr_card : ScriptableObject
{
    public GameObject unit;
    public int cost, power, health, range;
    public string description;
    public float maxCooldown;
    [System.Serializable]
    public enum statusTypes
    {
        none,
        poison,
        munitions,
        ironclad
    }
    [SerializeField]
    private statusTypes status;
    public int statusAmnt;
    [System.Serializable]
    public enum costTypes
    {
        none,
        water,
        steel
    }
    [SerializeField]
    private costTypes costType;
    public scr_player player;
}