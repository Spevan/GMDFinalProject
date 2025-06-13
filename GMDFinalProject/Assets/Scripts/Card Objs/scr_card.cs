using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Scriptable Sheets/Card")]
public class scr_card : ScriptableObject
{
    public GameObject unit;
    [System.Serializable]
    public enum costTypes
    {
        none,
        water,
        steel
    }
    [SerializeField]
    public costTypes costType;
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
}