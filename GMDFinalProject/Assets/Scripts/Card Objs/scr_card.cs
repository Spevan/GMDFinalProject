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
    public costTypes[] costType;
    public int[] cost;
    public int power, health, range;
    public string description;
    public float maxCooldown;
    [System.Serializable]
    public enum statusTypes
    {
        none,
        swift,
        frugal,
        perceptive,
        strong,
        fortified,
        exhausted,
        expensive,
        blind,
        weak,
        brittle
    }
    [SerializeField]
    private statusTypes[] status;
    public int[] statusAmnt;
}