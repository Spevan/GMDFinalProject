using UnityEngine;

[CreateAssetMenu(fileName = "ammo", menuName = "Scriptable Objects/scr_ammo")]
public class scr_ammo : ScriptableObject
{
    public string description;
    public int speed, damage, splashDmg;
    public Vector2 splashRng;
}
