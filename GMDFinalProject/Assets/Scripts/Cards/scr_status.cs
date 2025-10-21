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
        [Description("Target costs less to summon.")]
        Frugal,
        [Description("Target’s range is increased.")]
        Perceptive,
        [Description("Target’s power is increased.")]
        Strong,
        [Description("Target’s maximum health is increased.")]
        Fortified,
        [Description("Target restores water equal to its cost to its owners water resource.")]
        Recyclable,
        [Description("Target applies the Leaking condition to enemies and returns the water stolen to this target's owner.")]
        Thief,
        [Description("Target produces more water based on its level.")]
        Productive,
        [Description("Target heals a set amount of health based on level.")]
        Healing,
        [Description("Target heals health based on level and power.")]
        Vampiric,
        [Description("Target’s attacks apply the Exhausted condition.")]
        Sleepy,
        [Description("Target’s attacks apply the Expensive condition.")]
        Greedy,
        [Description("Target’s attacks apply the Blind condition.")]
        Blinding,
        [Description("Target’s attacks apply the Weak condition.")]
        Crushing,
        [Description("Target’s attacks apply the Burnt condition.")]
        Heated,
        [Description("Target’s attacks apply the Frozen condition.")]
        Frigid,
        [Description("Target’s attacks apply the Entangled condition.")]
        Tangled,
        [Description("Target’s attacks apply the Resurrected condition.")]
        Miraculous,
        [Description("Target’s attacks apply the Stunned condition.")]
        Paralyzer,
        [Description("Target’s attacks apply the Mutinied condition.")]
        Conspiracist,
        [Description("Target’s attacks apply the Compromised condition.")]
        Pacifist,
        [Description("Target’s attacks apply the Grappled condition.")]
        Grippy
    }
    public statusTypes statusType;
    public int statusAmnt;
    [NonSerialized] public float rangePerLvl = 0.5f, speedPerLvl = 0.5f;
    [NonSerialized] public int healthPerLvl = 10, powerPerLvl = 5;
}
