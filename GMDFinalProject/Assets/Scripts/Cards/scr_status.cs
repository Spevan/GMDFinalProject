using System;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class scr_status
{
    [System.Serializable]
    public enum statusTypes
    {
        [Description("Target moves at a faster speed.")]
        Swift,
        [Description("")]
        Frugal,
        [Description("")]
        Perceptive,
        [Description("")]
        Strong,
        [Description("")]
        Fortified,
        [Description("")]
        Recyclable,
        [Description("")]
        Thief,
        Productive,
        Healing,
        Vampiric,
        Sleepy,
        Greedy,
        Blinding,
        Crushing,
        Heated,
        Frigid,
        [Description("Target’s attacks apply the Entangled condition.")]
        Tangled,
        Miraculous,
        Paralyzer,
        Conspiracist,
        Pacifist,
        Grippy
    }
    public statusTypes statusType;
    public int statusAmnt;
    [NonSerialized] public float rangePerLvl = 0.5f, speedPerLvl = 0.5f;
    [NonSerialized] public int healthPerLvl = 10, powerPerLvl = 5;
}
