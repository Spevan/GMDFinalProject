using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Scriptable Sheets/Card")]
[System.Serializable]
public class scr_card : ScriptableObject
{
    [System.Serializable]
    public class status
    {
        [System.Serializable]
        public enum statusTypes
        {
            swift,
            frugal,
            perceptive,
            strong,
            fortified,
            recyclable,
            thief,
            productive,
            healing,
            vampiric,
            sleepy,
            greedy,
            blinding,
            crushing,
            heated,
            frigid,
            tangled,
            miraculous,
            paralyzer,
            conspiracist,
            pacifist,
            grippy
        }
        public statusTypes statusType;
        public int statusAmnt;
    }

    public int card_id;
    public bool loaded;
    public GameObject unit;
    public int cost, power, health;
    public string description;
    public float range, maxCooldown;
    public status[] statuses;
}