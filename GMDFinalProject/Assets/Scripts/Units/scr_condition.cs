using UnityEngine;

public class scr_condition
{
    [System.Serializable]
    public enum conditionTypes
    {
        exhausted,
        expensive,
        blind,
        weak, 
        brittle,
        leaking,
        burnt,
        frozen,
        entangled,
        resurrected,
        stunned,
        mutinied,
        compromised,
        grapped
    }
    public conditionTypes conditionType;
    public int conditonAmnt;
    public float rangePerLvl = 0.5f, speedPerLvl = 0.5f, cooldownPerLevel = 0.1f;
    public int healthPerLvl = 10, powerPerLvl = 5;
}
