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
    public int conditionAmnt;
    public float rangePerLvl = 0.5f, speedPerLvl = 0.5f, cooldownPerLevel = 0.1f, fireDmgTick = 0.5f;
    public int healthPerLvl = 10, powerPerLvl = 5, burnDuration = 2, entangledDuration = 3;
    public GameObject conditionOrigin;

    public scr_condition(conditionTypes condition, int amnt)
    {
        conditionType = condition;
        conditionAmnt = amnt;
    }

    public scr_condition(conditionTypes condition, int amnt, GameObject origin)
    {
        conditionType = condition;
        conditionAmnt = amnt;
        conditionOrigin = origin;
    }
}
