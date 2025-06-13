using UnityEngine;

[CreateAssetMenu(fileName = "ammo", menuName = "Scriptable Sheets/Ammo")]
public class scr_ammo : ScriptableObject
{
    public string description;
    public int speed, damage, splashDmg;
    public Vector2 splashRng;
}
