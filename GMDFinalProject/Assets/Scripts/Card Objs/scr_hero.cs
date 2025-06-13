using UnityEngine;

[CreateAssetMenu(fileName = "hero", menuName = "Scriptable Sheets/Hero")]
public class scr_hero : scr_card
{
    public int speed, upkeep;
    [System.Serializable]
    public enum weaponType
    {
        melee,
        ranged
    }
}
