using UnityEngine;

[System.Serializable]
public class scr_status
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
